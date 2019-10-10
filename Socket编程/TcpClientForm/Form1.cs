using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClientForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private void button1_Click(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
            Text = "连接成功";
            networkStream = tcpClient.GetStream();
            //读取
            Task.Run(() => { Read(); });
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(textBox3.Text);
            if (networkStream.CanWrite)
            {
                //BinaryWriter binaryWriter = new BinaryWriter(networkStream);
                networkStream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        void Read()
        {
            while (networkStream.CanRead)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    var readLeng = networkStream.Read(buffer, 0, buffer.Length);
                    if (readLeng == 0)
                    {
                        txtMsg.AppendText("强行断开连接");
                        networkStream.Close();
                        return;
                    }
                    txtMsg.Text += $"{Encoding.UTF8.GetString(buffer)}\r\n";
                }
                catch (Exception) { }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            networkStream.Close();
            tcpClient?.Close();
        }
    }
}
