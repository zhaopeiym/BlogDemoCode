using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulateHttpGet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //得到主机信息
            IPHostEntry ipInfo = Dns.GetHostEntry(new Uri(textBox1.Text).Host);
            //取得IPAddress[]
            IPAddress[] ipAddr = ipInfo.AddressList;
            //得到ip
            IPAddress ip = ipAddr[0];
            //组合出远程终结点
            IPEndPoint ipEndPoint = new IPEndPoint(ip, 80);
            //创建Socket 实例
            Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //尝试连接
            socketClient.Connect(ipEndPoint);
            //发送请求
            Send(socketClient);

            Task.Run(() =>
            {
                //接收服务器的响应 
                Receive(socketClient);
            }); 
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
                    textBox2.Text += $"{socketClient.RemoteEndPoint}强行断开连接\r\n";
                    return;
                }
                textBox2.AppendText($"{socketClient.RemoteEndPoint}：{Encoding.UTF8.GetString(data, 0, readLeng)}\r\n");
            }
        }

        /// <summary>
        /// 发送消息到服务端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Send(Socket socketClient)
        {
            var msg = $"GET / HTTP/1.1\r\nHost: {new Uri(textBox1.Text).Host}\r\n\r\n";
            socketClient.Send(Encoding.UTF8.GetBytes(msg));
        }
    }
}
