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
            while (i <= 5000)
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
                Url = "www.i.haojima.net"
            });
            db.SaveChanges();

            db.Blogs.First().Url = "www.haojima.net";
            db.SaveChanges();

            foreach (var item in db.Blogs.Take(10).ToList())
            {
                Console.WriteLine("查询到的博客id：" + item.BlogId);
            }
        }
    }
}
