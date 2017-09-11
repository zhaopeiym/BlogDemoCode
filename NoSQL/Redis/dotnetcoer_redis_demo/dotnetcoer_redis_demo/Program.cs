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
        //allowAdmin ： 当为true时 ，可以使用一些被认为危险的命令
        //connectRetry ：重试连接的次数
        //connectTimeout：超时时间
        //ssl={bool} ： 使用sll加密
        //syncTimeout={int} ： 异步超时时间

        static string RedisConnection = "192.168.2.135:6379,allowAdmin=true,password=fsm,defaultdatabase=10";
        static void Main(string[] args)
        {
            //[nuget StackExchange.Redis.StrongName]
            //ConnectionMultiplexer是线程安全的，且是昂贵的。所以我们应该尽量重用。            
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(RedisConnection);
            var db = connectionMultiplexer.GetDatabase();

            //Redis会很快速，所有的东西都在存储器里。但，让Redis闪耀的真正原因是其不同于其它解决方案的特殊数据结构。
            //如果key已经拥有一个值，则被覆盖
            //http://www.cnblogs.com/yanghua1012/p/5679183.html

            //字符串（Strings)
            var key = "keystring";
            db.StringSet(key, "你好", TimeSpan.FromSeconds(5));//写入字符串，并指定过期时间。对应reids的setex key 5或者(set key和expire key 5)命令 。
            db.StringAppend(key, " world"); //追加值
            db.StringIncrement("benny:click");//储存的数字值增一，对应 incr key 命令
            db.StringIncrement("benny:click");

            //散列（Hashes）
            db.HashSet("users:benny", "name", "农码一生");// 对应 hset key 命令
            db.HashSet("users:benny", "age", "27");
            db.HashSet("users:joy", new HashEntry[] { new HashEntry("name", "妹子啊"), new HashEntry("age", "18"), });//对应于 hmset key
            db.HashSet("users:joy", "sex", "女");

            //列表（Lists）
            db.ListLeftPush("list4", "list41"); //对应 lpush 命令
            db.ListLeftPush("list4", "list42");
            var ranges = db.ListRange("list4", 0);//获取列表所有值
            ranges = db.ListRange("list4", 0, 2);// 获取列表 0 到2 的值，对应 lrange list4 0 2命令
            ranges = db.Sort("list4", sortType: SortType.Alphabetic);//排序。对应 sort list4 limit 0 10 desc alpha命令                        

            //集合（Sets）
            db.SetAdd("list-a", "张三");//对应 sadd 命令
            db.SetAdd("list-a", "李四");
            db.SetAdd("list-b", "张三");
            db.SetAdd("list-b", new RedisValue[] { "王五", "郑六" });
            var hasValue = db.SetContains("list-a", "张三");//true，对应 sismember 命令
            hasValue = db.SetContains("list-a", "王五");//false
            var newSets = db.SetCombine(SetOperation.Union, new RedisKey[] { "list-a", "list-b" });//并集 
            newSets = db.SetCombine(SetOperation.Intersect, new RedisKey[] { "list-a", "list-b" });//交集
            newSets = db.SetCombine(SetOperation.Difference, new RedisKey[] { "list-a", "list-b" });//存在于集合list-a 却不存在于集合list-b中的值

            //分类集合（Sorted Sets）
            db.SortedSetAdd("list3", "list31", 1);
            db.SortedSetAdd("list3", "list33", 3);
            db.SortedSetAdd("list3", "list331", 4);
            db.SortedSetAdd("list3", "list33.1", 2);
            db.SortedSetAdd("list3", "list32", 2);
            var redisValues = db.SortedSetRangeByRank("list3", 0, 2, Order.Ascending, CommandFlags.None);//范围
            var length = db.SortedSetLength("list3");//获取长度
            var tempLength = db.SortedSetLengthByValue("list3", "list31", "list32");//获取RedisValue之间的长度
            var index = db.SortedSetRank("list3", "list331");//下标
            //db.SortedSetCombineAndStore(SetOperation.Union, "new...", "...", "...");//并集 
            //db.SortedSetCombineAndStore(SetOperation.Intersect, "new...", "...", "...");//交集
            //db.SortedSetCombineAndStore(SetOperation.Difference, "new...", "...", "...");//差异 not in

            //bit
            db.StringSetBit("bitkey", 0, true);
            db.StringSetBit("bitkey", 1, false);
            db.StringGetBit("bitkey", 0);
            var position = db.StringBitPosition("bitkey", false, 0, 1);

            //事务
            var tran = db.CreateTransaction();//创建一个事务
            if (db.KeyExists(key)) //判断是否存在key
            {
                //db.KeyDelete(key, CommandFlags.HighPriority);          //删除key
            }
            Console.WriteLine(db.KeyExists(key));
            //....
            bool committed = tran.Execute();// 提交执行事务

            //Lock(分布式锁)
            RedisValue token = Environment.MachineName;
            // 秒杀
            if (db.LockTake("id", token, TimeSpan.FromSeconds(10)))//10秒自动解锁
            {
                try
                {
                    //Thread.Sleep(5000);
                }
                finally
                {
                    db.LockRelease("id", token);
                }
            }

            #region 测试
            ////测试
            //for (int i = 1000000; i < 2000000; i++)
            //{
            //    db.StringSet("JsonConvert"+ i, "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）" +
            //        "ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）ASP.NET Core 快速入门（实战篇）");
            //} 
            #endregion

            #region 序列化后存储            
            Blog blog = new Blog { Id = 1, Title = "ASP.NET Core 快速入门（实战篇）", Content = "~~~~~~~~~~~~~" };
            //1、JsonConvert[nuget Newtonsoft.Json] 的方式序列化存储                
            db.StringSet("JsonConvert", JsonConvert.SerializeObject(blog));
            blog = JsonConvert.DeserializeObject<Blog>(db.StringGet("JsonConvert"));

            //2、BinaryFormatter 的方式序列化存储【注意：被序列化的类要标记特性 [Serializable]】
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, blog);
                db.StringSet("BinaryFormatter", memoryStream.ToArray());
            }
            using (var stream = new MemoryStream(db.StringGet("BinaryFormatter")))
            {
                blog = (Blog)new BinaryFormatter().Deserialize(stream);
            }

            //3、[nuget protobuf-net] 的方式序列化存储【注意：被序列化的类要标记特性 [ProtoContract]，字段要标记特性[ProtoMember(1)]】
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

            //1、JsonConvert[nuget Newtonsoft.Json] 的方式序列化
            stopwatch.Restart();//开始监视代码运行时间
            for (int i = 0; i < count; i++)
            {
                blog.Id = i;
                blog.Content = i.ToString();
                blog = JsonConvert.DeserializeObject<Blog>(JsonConvert.SerializeObject(blog));
            }
            stopwatch.Stop(); //  停止监视
            Console.WriteLine("Newtonsoft.Json:" + stopwatch.Elapsed.TotalMilliseconds + "毫秒");//总毫秒数

            //2、BinaryFormatter 的方式序列化
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

            //3、[nuget protobuf-net] 的方式序列化
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
