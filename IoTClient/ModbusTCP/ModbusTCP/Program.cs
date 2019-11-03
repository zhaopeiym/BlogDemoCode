using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ModbusTCP
{
    class Program
    {
        //Modbus服务端IP
        static string ip = "";
        //Modbus服务端端口
        static int port = 502;
        static void Main(string[] args)
        {
            //读取数据
            Read();

            //写入数据
            Write();
        }

        static void Read()
        {
            //1 创建Socket
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //2 建立连接
            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

            //3 获取命令（寄存器起始地址、站号、功能码、读取寄存器长度）
            byte[] command = GetReadCommand(4, 2, 3, 1);

            //4 发送命令
            socket.Send(command);

            //5 读取响应
            byte[] buffer1 = new byte[8];//先读取前面八个字节（Map报文头）
            socket.Receive(buffer1, 0, buffer1.Length, SocketFlags.None);

            //5.1 获取将要读取的数据长度
            int length = buffer1[4] * 256 + buffer1[5] - 2;//减2是因为这个长度数据包括了单元标识符和功能码，占两个字节

            //5.2 读取数据
            byte[] buffer2 = new byte[length];
            var readLength2 = socket.Receive(buffer2, 0, buffer2.Length, SocketFlags.None);

            byte[] buffer3 = new byte[readLength2 - 1];
            //5.3  过滤第一个字节（第一个字节代表数据的字节个数）
            Array.Copy(buffer2, 1, buffer3, 0, buffer3.Length);
            var buffer3Reverse = buffer3.Reverse().ToArray();
            var value = BitConverter.ToInt16(buffer3Reverse, 0);

            //6 关闭连接
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        static void Write()
        {
            //1 创建Socket
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //2 建立连接
            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

            //值的转换
            short value = 32;
            var values = BitConverter.GetBytes(value).Reverse().ToArray();

            //3 获取并发送命令（寄存器起始地址、站号、功能码）
            var command = GetWriteCommand(4, values, 2, 16);
            socket.Send(command);

            //4 关闭连接
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        #region 获取命令

        /// <summary>
        /// 获取读取命令
        /// </summary>
        /// <param name="address">寄存器起始地址</param>
        /// <param name="stationNumber">站号</param>
        /// <param name="functionCode">功能码</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        public static byte[] GetReadCommand(ushort address, byte stationNumber, byte functionCode, ushort length)
        {
            byte[] buffer = new byte[12];
            buffer[0] = 0x19;
            buffer[1] = 0xB2;//Client发出的检验信息
            buffer[2] = 0x00;
            buffer[3] = 0x00;//表示tcp/ip 的协议的modbus的协议
            buffer[4] = 0x00;
            buffer[5] = 0x06;//表示的是该字节以后的字节长度

            buffer[6] = stationNumber;  //站号
            buffer[7] = functionCode;   //功能码
            buffer[8] = BitConverter.GetBytes(address)[1];
            buffer[9] = BitConverter.GetBytes(address)[0];//寄存器地址
            buffer[10] = BitConverter.GetBytes(length)[1];
            buffer[11] = BitConverter.GetBytes(length)[0];//表示request 寄存器的长度(寄存器个数)
            return buffer;
        }

        /// <summary>
        /// 获取写入命令
        /// </summary>
        /// <param name="address">寄存器地址</param>
        /// <param name="values"></param>
        /// <param name="stationNumber">站号</param>
        /// <param name="functionCode">功能码</param>
        /// <returns></returns>
        public static byte[] GetWriteCommand(ushort address, byte[] values, byte stationNumber, byte functionCode)
        {
            byte[] buffer = new byte[13 + values.Length];
            buffer[0] = 0x19;
            buffer[1] = 0xB2;//检验信息，用来验证response是否串数据了           
            buffer[4] = BitConverter.GetBytes(7 + values.Length)[1];
            buffer[5] = BitConverter.GetBytes(7 + values.Length)[0];//表示的是header handle后面还有多长的字节

            buffer[6] = stationNumber; //站号
            buffer[7] = functionCode;  //功能码
            buffer[8] = BitConverter.GetBytes(address)[1];
            buffer[9] = BitConverter.GetBytes(address)[0];//寄存器地址
            buffer[10] = (byte)(values.Length / 2 / 256);
            buffer[11] = (byte)(values.Length / 2 % 256);//写寄存器数量(除2是两个字节一个寄存器，寄存器16位。除以256是byte最大存储255。)              
            buffer[12] = (byte)(values.Length);          //写字节的个数
            values.CopyTo(buffer, 13);                   //把目标值附加到数组后面
            return buffer;
        }
        #endregion
    }
}
