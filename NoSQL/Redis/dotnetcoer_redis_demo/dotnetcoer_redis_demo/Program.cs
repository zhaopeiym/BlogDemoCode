using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace dotnetcoer_redis_demo
{


    class Program
    {
        static string RedisConnection = "192.168.2.135:6379,allowAdmin=true,password=fsm2016redis,defaultdatabase=10";
        static void Main(string[] args)
        {
            //[nuget StackExchange.Redis.StrongName]
            //ConnectionMultiplexer是线程安全的 
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(RedisConnection);
            var db = connectionMultiplexer.GetDatabase();

            //如果key已经拥有一个值，则被覆盖
            var key = "keystring";
            db.StringSet(key, "你好，世界！");

            db.HashSet("list1", "name", "农码一生");
            db.HashSet("list1", "age", "27");

            db.SetAdd("list2", "list21");
            db.SetAdd("list2", "list22");

            db.SortedSetAdd("list3", "list31", DateTime.Now.Ticks);
            db.SortedSetAdd("list3", "list32", DateTime.Now.Ticks);

            db.ListLeftPush("list4", "list41");
            db.ListLeftPush("list4", "list42");

            Console.WriteLine(db.KeyExists(key));                  //判断是否存在key
            db.KeyDelete(key, CommandFlags.HighPriority);          //删除key
            Console.WriteLine(db.KeyExists(key));

            #region 序列化后存储
            Blog blog = new Blog { Id = 1, Title = "ASP.NET Core 快速入门（实战篇）", Content = "~~~~~~~~~~~~~" };
            //JsonConvert[nuget Newtonsoft.Json] 的方式序列化存储                
            db.StringSet("JsonConvert", JsonConvert.SerializeObject(blog));
            blog = JsonConvert.DeserializeObject<Blog>(db.StringGet("JsonConvert"));

            //BinaryFormatter 的方式序列化存储【注意：被序列化的类要标记特性 [Serializable]】
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, blog);
                db.StringSet("BinaryFormatter", memoryStream.ToArray());
            }
            using (var stream = new MemoryStream(db.StringGet("BinaryFormatter")))
            {
                blog = (Blog)new BinaryFormatter().Deserialize(stream);
            }

            //[nuget protobuf-net] 的方式序列化存储【注意：被序列化的类要标记特性 [ProtoContract]，字段要标记特性[ProtoMember(1)]】
            using (var memoryStream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(memoryStream, blog);
                byte[] byteArray = memoryStream.ToArray();
                db.StringSet("ProtoBuf", byteArray);
            }
            using (var memoryStream = new MemoryStream(db.StringGet("ProtoBuf")))
            {
                var obj = ProtoBuf.Serializer.Deserialize<Blog>(memoryStream);
            }
            #endregion

            #region 序列化 性能测试
            var count = 0;// 80000;
            Stopwatch stopwatch = new Stopwatch();

            //JsonConvert[nuget Newtonsoft.Json] 的方式序列化
            stopwatch.Restart();//开始监视代码运行时间
            for (int i = 0; i < count; i++)
            {
                blog.Id = i;
                blog.Content = i.ToString();
                blog = JsonConvert.DeserializeObject<Blog>(JsonConvert.SerializeObject(blog));
            }
            stopwatch.Stop(); //  停止监视
            Console.WriteLine("Newtonsoft.Json:" + stopwatch.Elapsed.TotalMilliseconds + "毫秒");//总毫秒数

            //BinaryFormatter 的方式序列化
            stopwatch.Restart();
            for (int i = 0; i < count; i++)
            {
                blog.Id = i;
                blog.Content = i.ToString();
                using (var stream = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(stream, blog);
                    blog = (Blog)new BinaryFormatter().Deserialize(new MemoryStream(stream.ToArray()));
                }
            }
            stopwatch.Stop();
            Console.WriteLine("BinaryFormatter:" + stopwatch.Elapsed.TotalMilliseconds + "毫秒");//总毫秒数

            //[nuget protobuf-net] 的方式序列化
            stopwatch.Restart();
            for (int i = 0; i < count; i++)
            {
                blog.Id = i;
                blog.Content = i.ToString();
                using (var memoryStream = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(memoryStream, blog);
                    byte[] byteArray = memoryStream.ToArray();
                    var obj = ProtoBuf.Serializer.Deserialize<Blog>(new MemoryStream(byteArray));
                }
            }
            stopwatch.Stop();
            Console.WriteLine("ProtoBuf:" + stopwatch.Elapsed.TotalMilliseconds + "毫秒");//总毫秒数 
            #endregion

            Console.ReadKey();
        }
    }
}
