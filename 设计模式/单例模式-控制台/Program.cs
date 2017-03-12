using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 单例模式_控制台
{
    class Program
    {
        static void Main(string[] args)
        {
            //Singleton s1 = new Singleton();
            //Singleton s2 = new Singleton();
            //Console.WriteLine(s1.Equals(s2));
           
           // Console.WriteLine(ConfigInfo.singleton.Equals(ConfigInfo.singleton));
            Console.ReadKey();
        }

        static object objs = new object();
        public static void test()
        {
            var emailInfo = ConfigInfo.Instance;
            EmailSend(emailInfo.Email,emailInfo.EmailUser,emailInfo.EmailPass);
        }

        public static void EmailSend(string email,string user,string pass)
        {

        }
    }
}

public class ConfigInfo
{
    private static ConfigInfo singleton = null;
    private static object obj = new object();
    public static ConfigInfo Instance
    {
        get
        {
            if (singleton == null)
            {
                lock (obj)
                {
                    if (singleton == null)
                    {
                        singleton = new ConfigInfo(); 
                        //从配置文件读取并赋值
                        singleton.Email = "zhaopeiym@163.com";
                        singleton.EmailUser = "农码一生";
                        singleton.EmailPass = "***********";
                    }
                }
            }
            return singleton;
        }
    }

    public string Email { get; private set; }
    public string EmailUser { get; private set; }
    public string EmailPass { get; private set; }




    private ConfigInfo()//禁止初始化
    {
    }
}

//public class Singleton
//{
//    public static Singleton singleton = null;
//    private static object obj = new object(); 
//    public static Singleton Instance
//    {
//        get
//        {
//            if (singleton == null)
//            {
//                lock (obj)
//                {
//                    if (singleton == null)
//                    {
//                        singleton = new Singleton();
//                    }
//                }
//            }
//            return singleton;
//        } 
//    }
//    private Singleton()//禁止初始化
//    {
//    }
//}

//public class Singleton
//{
//    public static Singleton singleton = null;
//    private static object obj = new object();

//    public static Singleton GetSingleton()
//    {
//        if (singleton == null) //下面有锁了为什么还要判断，因为锁会阻塞线程。而singleton被实例化后这个判断永远为false，不在需要锁。
//        {
//            lock (obj)
//            {
//                //这里代码只可能存在一个线程同时到达
//                if (singleton == null)
//                {
//                    Thread.Sleep(1000);
//                    singleton = new Singleton();
//                } 
//            } 
//        }
//        return singleton;
//    }
//    private Singleton()//禁止初始化
//    {
//    }
//}

//public class Singleton
//{
//public static readonly Singleton singleton = new Singleton();//自读字段
//private Singleton()//禁止初始化
//{
//}
//} 

//public static class Singleton1
//{
//    //
//}
