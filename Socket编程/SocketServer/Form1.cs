using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //https://www.cnblogs.com/Impulse/articles/9320539.html
            //如果有两个或多个线程操作某一控件的状态，则可能会迫使该控件进入一种不一致的状态。
            //还可能出现其他与线程相关的 bug，以及不同线程争用控件引起的死锁问题。 
            //正式环境应该使用委托(Invoke)的方式访问控件
            CheckForIllegalCrossThreadCalls = false;
        }
        private Socket socketServer;
        private Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();
        private void button1_Click(object sender, EventArgs e)
        {
            //1 创建Socket对象
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //2 绑定ip和端口
            IPAddress ip = IPAddress.Parse(textBox1.Text);
            IPEndPoint ipEndPoint = new IPEndPoint(ip, int.Parse(textBox2.Text));
            socketServer.Bind(ipEndPoint);

            //3、开启侦听(等待客户机发出的连接),并设置最大客户端连接数为10
            socketServer.Listen(10);
            txtMsg.Text = "启动服务\r\n";
            Text = "服务已启动";

            Task.Run(() => { Accept(socketServer); });
        }

        /// <summary>
        /// 客户端连接到服务端
        /// </summary>
        /// <param name="socket"></param>
        void Accept(Socket socket)
        {
            while (true)
            {
                //阻塞等待客户端连接
                Socket newSocket = socket.Accept();
                txtMsg.Text += $"{newSocket.RemoteEndPoint}上线了\r\n";
                listBox1.Items.Add(newSocket.RemoteEndPoint);
                dicSocket.Add(newSocket.RemoteEndPoint.ToString(), newSocket);
                Task.Run(() => { Receive(newSocket); });
            }
        }

        /// <summary>
        /// 接收客户端发送的消息
        /// </summary>
        /// <param name="newSocket"></param>
        void Receive(Socket newSocket)
        {
            byte[] data = new byte[1024 * 1024];
            while (newSocket.Connected)
            {
                //读取客户端发送过来的数据
                int readLeng = newSocket.Receive(data, 0, data.Length, SocketFlags.None);
                if (readLeng == 0)//客户端断开连接
                {
                    txtMsg.Text += $"{newSocket.RemoteEndPoint}下线了\r\n";
                    listBox1.Items.Remove(newSocket.RemoteEndPoint);
                    dicSocket.Remove(newSocket.RemoteEndPoint.ToString());
                    //停止会话（禁用Socket上的发送和接收，该方法允许Socket对象一直等待，直到将内部缓冲区的数据发送完为止）
                    newSocket.Shutdown(SocketShutdown.Both);
                    //关闭连接
                    newSocket.Close();
                    return;
                }
                txtMsg.Text += $"{newSocket.RemoteEndPoint}：{Encoding.UTF8.GetString(data, 0, readLeng)}\r\n";
            }
        }

        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox1.SelectedItems)
            {
                //把消息内容转成字节数组后发送
                dicSocket[item.ToString()].Send(Encoding.UTF8.GetBytes(txtContent.Text));
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var item in listBox1.Items)
            {
                dicSocket[item.ToString()].Shutdown(SocketShutdown.Both);
            }
        }
    }
}
