using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace web服务
{
    public class GetDataHelper
    {
        /// <summary>
        /// 同步方法获取数据
        /// </summary>
        /// <returns></returns>
        public string GetData()
        {
            var beginInfo = GetBeginThreadInfo();
            using (HttpClient http = new HttpClient())
            {
                http.GetStringAsync("https://github.com/").Wait();//注意：这里是同步阻塞
            }
            return beginInfo + GetEndThreadInfo();
        }

        /// <summary>
        /// 异步方法获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDataAsync()
        {
            var beginInfo = GetBeginThreadInfo();
            using (HttpClient http = new HttpClient())
            {
                await http.GetStringAsync("https://github.com/");//注意：这里是异步等待
            }
            return beginInfo + GetEndThreadInfo();
        }

        public string GetBeginThreadInfo()
        {
            int t1, t2, t3;
            ThreadPool.GetAvailableThreads(out t1, out t3);
            ThreadPool.GetMaxThreads(out t2, out t3);
            return string.Format("开始:{0:mm:ss,ffff} 线程Id:{1} Web线程数:{2}",
                                    DateTime.Now,
                                    Thread.CurrentThread.ManagedThreadId,                                  
                                    t2 - t1);
        }

        public string GetEndThreadInfo()
        {
            int t1, t2, t3;
            ThreadPool.GetAvailableThreads(out t1, out t3);
            ThreadPool.GetMaxThreads(out t2, out t3);
            return string.Format(" 结束:{0:mm:ss,ffff} 线程Id:{1} Web线程数:{2}",
                                    DateTime.Now,
                                    Thread.CurrentThread.ManagedThreadId,
                                    t2 - t1);
        }
    }
}