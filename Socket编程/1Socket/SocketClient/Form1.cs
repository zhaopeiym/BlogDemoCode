using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Text = "未连接";
        }

        private Socket socketClient;
        private void button1_Click(object sender, EventArgs e)
        {
            //1 创建Socket对象
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //2 连接服务端
            IPAddress ip = IPAddress.Parse(textBox1.Text);
            IPEndPoint ipEndPoint = new IPEndPoint(ip, int.Parse(textBox2.Text));
            socketClient.Connect(ipEndPoint);
            Text = "已经连接";
            //接收来自服务端的消息
            Task.Run(() => { Receive(socketClient); });
        }

        /// <summary>
        /// 接收来自服务端的消息
        /// </summary>
        /// <param name="socketClient"></param>
        void Receive(Socket socketClient)
        {
            byte[] data = new byte[1024 * 1024];
            while (true)
            {
                //读取客户端发送过来的数据
                int readLeng = socketClient.Receive(data, 0, data.Length, SocketFlags.None);
                if (readLeng == 0)//客户端断开连接
                {
                    txtMsg.Text += $"{socketClient.RemoteEndPoint}强行断开连接\r\n";
                    //socketClient.Shutdown(SocketShutdown.Both);
                    //socketClient.Close();
                    return;
                }
                txtMsg.Text += $"{socketClient.RemoteEndPoint}：{Encoding.UTF8.GetString(data, 0, readLeng)}\r\n";
            }
        }

        /// <summary>
        /// 发送消息到服务端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            socketClient.Send(Encoding.UTF8.GetBytes(textBox3.Text));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (socketClient.Connected)
                socketClient?.Shutdown(SocketShutdown.Both);
            socketClient?.Close();
        }
    }
}
