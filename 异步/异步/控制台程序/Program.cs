using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 控制台程序
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            GetUlrString("https://github.com/").Wait();
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.ReadKey();
        }

        public async static Task<string> GetUlrString(string url)
        {
            using (HttpClient http = new HttpClient())
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                return await http.GetStringAsync(url);
            }
        }
    }


}
