using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestDI
{
    /*
     1、什么是DI 什么是IOC DI和IOC的区别
     2、IOC容器
    */
    public interface IUser
    {
        string GetName();
    }
    public class User : IUser
    {
        public string GetName()
        {
            return "农码一生";
        }
    }

    public interface IUserService
    {
        IUser GetUser();
    }

    public class UserService : IUserService
    {
        private IUser _user;
        public UserService(IUser user)
        {
            _user = user;
        }

        public IUser GetUser()
        {
            return _user;
        }
    }

    /// <summary>
    /// 测试
    /// </summary>
    public class Tests1
    {
        [Fact]
        public void test0()
        {
            IUser user1 = new User();
            Assert.True(user1.GetName() == "农码一生");

            IUser user2 = DI.Resolve("TestDI.User");
            Assert.True(user2.GetName() == "农码一生");

            IUser user3 = DI.Resolve<IUser>();
            Assert.True(user3.GetName() == "农码一生");
        }

        [Fact]
        public void test2()
        {
            //通过注入获得IUserService的现实类实例
            var userService = DI.Resolve2<IUserService>();
            //获取User实例
            var user = userService.GetUser();
            //通过User实例获取用户名
            var name = user.GetName();
            Assert.True(name == "农码一生");
        }

        public interface IOrder
        {
        }

        [Fact]
        public void test4()
        {
            Assert.Throws<Exception>(() => { DI.Resolve<IOrder>(); });
        } 


        [Fact]
        public void test1_()
        {
            UserService user = new UserService(new User());
            var name = user.GetUser().GetName();
            Assert.True(name == "农码一生");
        }

        [Fact]
        public void test2_()
        {
            var typeUser = DI.Resolve("TestDI.User");
            UserService user = new UserService(typeUser);
            var name = user.GetUser().GetName();
            Assert.True(name == "农码一生");
        }

        [Fact]
        public void test3_()
        {
            var typeUser = DI.Resolve<IUser>();
            UserService user = new UserService(typeUser);
            var name = user.GetUser().GetName();
            Assert.True(name == "农码一生");
        }
    }

    public class DI
    {
        //通过反射 获取实例  并向上转成接口类型
        public static IUser Resolve(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();//获取当前代码的程序集
            return (IUser)assembly.CreateInstance(name);//这里写死了，创建实例后强转IUser
        }

        //通过反射 获取“一个”实现了此接口的实例
        public static T Resolve<T>()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            //获取“第一个”实现了此接口的实例
            var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(T))).FirstOrDefault();
            if (type == null)
                throw new Exception("没有此接口的实现");
            return (T)assembly.CreateInstance(type.ToString());//创建实例 转成接口类型
        }

        /// <summary>
        /// 通过反射 获取“一个”实现了此接口的实例
        /// 并且 实例中带有构造函数 （构造函数中又有注入）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve2<T>()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();//获取当前代码的程序集
            //获取“第一个”实现了此接口的实例（UserService）
            var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(T))).FirstOrDefault();
            if (type == null)          
                throw new Exception("没有此接口的实现");
            
            var parameter = new List<object>();
            //type.GetConstructors()[0]获取第一个构造函数 GetParameters的所有参数（IUser接口）
            var constructorParameters = type.GetConstructors()[0].GetParameters();
            foreach (var constructorParameter in constructorParameters)
            {
                //获取实现了（IUser）这个接口类型（User）
                var tempType = assembly.GetTypes().Where(t => t.GetInterfaces()
                            .Contains(Type.GetType(constructorParameter.ParameterType.FullName)))
                            .FirstOrDefault();
                //并实例化成对象（也就是User实例） 添加到一个集合里面 供最上面（UserService）的注入提供参数 
                parameter.Add(assembly.CreateInstance(tempType.ToString()));
            }
            //创建实例，并传入需要的参数 【public UserService(IUser user)】
            return (T)assembly.CreateInstance(type.ToString(), true, BindingFlags.Default, null, parameter.ToArray(), null, null);//true：不区分大小写            
        }

        #region 伪代码
        ////每次访问都是新的实例(通过obj1_1、obj1_2可以体现)
        //public static T Transient<T>()
        //{
        //    //var obj1_1 = assembly.CreateInstance(name);
        //    //var obj2 = assembly.CreateInstance(obj1_1,...)
        //    //var obj1_2 = assembly.CreateInstance(name);
        //    //var obj3 = assembly.CreateInstance(obj1_2,...)
        //    //var obj4 = assembly.CreateInstance(,...[obj2,obj3],...)
        //    //return (T)obj4;
        //}
        ////一次请求中唯一实例(通过obj1可以体现)
        //public static T Scoped<T>()
        //{
        //    //var obj1 = assembly.CreateInstance(name);
        //    //var obj2 = assembly.CreateInstance(obj1,...)
        //    //var obj3 = assembly.CreateInstance(obj1,...)
        //    //var obj4 = assembly.CreateInstance(,...[obj2,obj3],...)
        //    //return (T)obj4;
        //}
        ////全局单例（通过obj1 == null可以体现）
        //public static T Singleton<T>()
        //{
        //    //if(obj1 == null)
        //    //  obj1 = assembly.CreateInstance(name);
        //    //if(obj2 == null)
        //    //  obj2 = assembly.CreateInstance(obj1,...)
        //    //if(obj3 == null)
        //    //  obj3 = assembly.CreateInstance(obj1,...)
        //    //if(obj4 == null)
        //    //  obj4 = assembly.CreateInstance(,...[obj2,obj3],...)
        //    //return (T)obj4;
        //} 
        #endregion
    }
}
