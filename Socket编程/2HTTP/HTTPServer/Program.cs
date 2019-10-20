using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //1 创建Socket对象
            Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //2 绑定ip和端口
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ip, 80);
            socketServer.Bind(ipEndPoint);

            //3、开启侦听(等待客户机发出的连接),并设置最大客户端连接数为10
            socketServer.Listen(10);

            Console.WriteLine("服务已启动...");
            Console.WriteLine();
            Task.Run(() => { Accept(socketServer); });

            Console.ReadKey();
        }

        //4 阻塞等待客户端连接
        private static void Accept(Socket socketServer)
        {
            while (true)
            {
                //阻塞等待客户端连接
                Socket newSocket = socketServer.Accept();
                Task.Run(() => { Receive(newSocket); });
            }
        }

        //5 读取客户端发送过来的报文
        private static void Receive(Socket newSocket)
        {
            byte[] data = new byte[1024 * 1024];
            while (newSocket.Connected)
            {
                //读取客户端发送过来的数据
                int readLeng = newSocket.Receive(data, 0, data.Length, SocketFlags.None);
                if (readLeng == 0)//客户端断开连接
                {
                    //停止会话（禁用Socket上的发送和接收，该方法允许Socket对象一直等待，直到将内部缓冲区的数据发送完为止）
                    newSocket.Shutdown(SocketShutdown.Both);
                    //关闭连接
                    newSocket.Close();
                    return;
                }

                //读取客户端发来的请求报文
                var requst = Encoding.UTF8.GetString(data, 0, readLeng);
                Console.WriteLine("收到请求报文：");
                Console.WriteLine(requst);

                //解析请求报文的请求路径（可以解析请求路径、请求文件、文件类型）
                var requstFile = requst.Split("\r\n")[0].Split(" ")[1];

                //回复客户端响应报文
                Send(newSocket, requstFile);
            }
        }

        //6 回复客户端响应报文
        private static void Send(Socket newSocket, string requstFile)
        {
            //这里如果请求的根目录，默认显示Index.html
            if (requstFile == "/" ) requstFile = "/Index.html";

            var msg = File.ReadAllText(Directory.GetCurrentDirectory() + requstFile);
            //把消息内容转成字节数组后发送
            newSocket.Send(Encoding.UTF8.GetBytes(msg));
            Console.WriteLine("回复响应报文：");
            Console.WriteLine(msg);
            Console.WriteLine();

            //回复响应后马上关闭连接
            newSocket.Shutdown(SocketShutdown.Both);
            newSocket.Close();
        }
    }
}
