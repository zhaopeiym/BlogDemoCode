using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;

namespace EFCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("是否主动释放DbContext（y/n）：  ");
            var yes = Console.ReadLine();
            Console.Write("请输入模拟并发量：  ");
            var number = Console.ReadLine();
            SemaphoreSlim _sem = new SemaphoreSlim(int.Parse(number));

            var i = 0;
            while (true)//或者设置总循环次数
            {
                Console.WriteLine("启动第" + i++ + "个线程：");

                _sem.Wait();

                #region Thread
                new Thread(() =>
                       {
                           if (yes == "y")
                           {
                               using (BloggingContext bloggingContext = new BloggingContext())
                               {
                                   DbOperation(bloggingContext);
                               }
                           }
                           else
                           {
                               BloggingContext bloggingContext = new BloggingContext();
                               DbOperation(bloggingContext);
                           }

                       }).Start();
                #endregion

                _sem.Release();
            }
        }

        //数据库的一些操作
        private static void DbOperation(BloggingContext db)
        {
            db.Blogs.Add(new Blog()
            {
                Rating = 1,
                #region Content
                Content = @"Hi-Blogs
嗨博客，基于ASP.NET Core2.0的跨平台的免费开源博客系统
演示地址：https://haojima.net/
意见和建议：https://github.com/zhaopeiym/Hi-Blogs/issues
使用到的相关平台、技术和工具
ASP.NET Core2.0 （底层框架）
CentOS7.3 （运行平台）
MySql5.6.37 （数据库，EF Core2.0+Pomelo.EntityFrameworkCore.MySql驱动）
nginx1.12.1 （代理）
Reids （缓存，StackExchange.Redis免费开源的.NET版的Reids客户端）
AngleSharp （Html解析组件）
Serilog （日志组件）
相关链接
开发日志、计划
学习资料
Linux视频
鸟哥的Linux
Docker入门
Redis命令参考
The Little Redis Book
StackExchange.Redis
ASP.NET Core
EF Core", 
                #endregion
                Url = "www.i.haojima.net"
            });
            db.SaveChanges();

            db.Blogs.First().Url = "www.haojima.net";
            db.SaveChanges();

            //为了验证实体跟踪是否会占用内存不是释放,而没有使用AsNoTracking
            var blogs = db.Blogs.Take(100).ToList();

            //正常项目，如果不是修改实体，需要添加AsNoTracking。
            //或者按需查询.Select(t=>new {}).ToList();
            //db.Blogs.Take(10).AsNoTracking().ToList();
            Console.WriteLine("数据操作执行成功~~");
        }
    }
}
