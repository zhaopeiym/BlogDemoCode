using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ModbusTcpServer
{
    /// <summary>
    /// 为了减低代码量，便于理解，这里只能对short类型读写。
    /// 完整实现请参考 https://github.com/zhaopeiym/IoTClient/blob/master/IoTServer/Servers/ModBus/ModBusTcpServer.cs
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Start();
            Console.WriteLine("ModbusTcpServer 已开启");
            Console.Read();
        }

        //启动服务
        public void Start()
        {
            //1 创建Socket对象
            var socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //2 绑定ip和端口 
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 502);
            socketServer.Bind(ipEndPoint);

            //3、开启侦听(等待客户机发出的连接),并设置最大客户端连接数为10
            socketServer.Listen(10);

            Task.Run(() => { Accept(socketServer); });
        }

        //客户端连接到服务端
        void Accept(Socket socket)
        {
            while (true)
            {
                //阻塞等待客户端连接
                Socket newSocket = socket.Accept();
                Task.Run(() => { Receive(newSocket); });
            }
        }

        //接收客户端发送的消息
        void Receive(Socket newSocket)
        {
            while (newSocket.Connected)
            {
                byte[] requetData1 = new byte[8];
                //读取客户端发送报文 报文头
                int readLeng = newSocket.Receive(requetData1, 0, requetData1.Length, SocketFlags.None);
                byte[] requetData2 = new byte[requetData1[5] - 2];
                //读取客户端发送报文 报文数据
                readLeng = newSocket.Receive(requetData2, 0, requetData2.Length, SocketFlags.None);
                var requetData = requetData1.Concat(requetData2).ToArray();

                byte[] responseData1 = new byte[8];
                //复制请求报文中的报文头
                Buffer.BlockCopy(requetData, 0, responseData1, 0, responseData1.Length);
                //这里可以自己实现一个对象，用来存储客户端写入的数据（也可以用redis等实现数据的持久化）
                DataPersist data = new DataPersist("");

                //根据协议，报文的第八个字节是功能码（前面我们有说过 03：读保持寄存器  16：写多个寄存器）
                switch (requetData[7])
                {
                    //读保持寄存器
                    case 3:
                        {
                            var value = data.Read(requetData[9]);
                            short.TryParse(value, out short resultValue);
                            var bytes = BitConverter.GetBytes(resultValue);
                            //当前位置到最后的长度
                            responseData1[5] = (byte)(3 + bytes.Length);
                            byte[] responseData2 = new byte[3] { (byte)bytes.Length, bytes[1], bytes[0] };
                            var responseData = responseData1.Concat(responseData2).ToArray();
                            newSocket.Send(responseData);
                        }
                        break;
                    //写多个寄存器
                    case 16:
                        {
                            data.Write(requetData[9], requetData[requetData.Length - 1].ToString());
                            var responseData = new byte[12];
                            Buffer.BlockCopy(requetData, 0, responseData, 0, responseData.Length);
                            responseData[5] = 6;
                            newSocket.Send(responseData);
                        }
                        break;
                }
            }
        }
    }
}
