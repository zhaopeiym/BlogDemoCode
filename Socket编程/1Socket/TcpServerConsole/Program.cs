using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerConsole
{
    class Program
    {
        static TcpListener tcpListener;
        static List<NetworkStream> networkStreams = new List<NetworkStream>();
        static void Main(string[] args)
        {

            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            tcpListener.Start(10);
            Console.WriteLine("启动服务（IP：127.0.0.1 端口：9999），等待客户端连接！");
            Task.Run(() => { Accept(); });

            while (true)
            {
                //群发
                var msg = Console.ReadLine();
                foreach (var item in networkStreams)
                {
                    item.Write(Encoding.UTF8.GetBytes(msg));
                }
            }
        }

        /// <summary>
        /// 等待客户端的连接
        /// </summary>
        static void Accept()
        {
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                NetworkStream networkStream = tcpClient.GetStream();
                Console.WriteLine($"{tcpClient.Client.RemoteEndPoint}上线");
                networkStreams.Add(networkStream);
                Task.Run(() => { Read(networkStream, tcpClient); });
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="networkStream"></param>
        static void Read(NetworkStream networkStream, TcpClient tcpClient)
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    //BinaryReader binaryReader = new BinaryReader(networkStream);
                    var readLen = networkStream.Read(buffer, 0, buffer.Length);
                    if (readLen == 0)
                    {
                        Console.WriteLine($"{tcpClient.Client.RemoteEndPoint}下线");
                        networkStreams.Remove(networkStream);
                        networkStream.Close();
                        tcpClient.Close();
                        return;
                    }
                    Console.WriteLine(tcpClient.Client.RemoteEndPoint + ":" + Encoding.UTF8.GetString(buffer, 0, readLen));
                }
                catch (Exception) { }
            }
        }
    }
}
