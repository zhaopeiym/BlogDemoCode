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

namespace Demo2
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
        string GetName();
    }

    public class UserService : IUserService
    {
        private IUser _user;
        public UserService(IUser user)
        {
            _user = user;
        }

        public string GetName()
        {
            return _user.GetName();
        }
    }

    /// <summary>
    /// 测试
    /// </summary>
    public class Tests1
    {
        [Fact]
        public void test1()
        {
            UserService user = new UserService(new User());
            var name = user.GetName();
            Assert.True(name == "农码一生");
        }

        [Fact]
        public void test2()
        {
            var typeUser = DI.Resolve("Demo2.User");
            UserService user = new UserService(typeUser);
            var name = user.GetName();
            Assert.True(name == "农码一生");
        }

        [Fact]
        public void test3()
        {
            var typeUser = DI.Resolve<IUser>();
            UserService user = new UserService(typeUser);
            var name = user.GetName();
            Assert.True(name == "农码一生");
           
        }

        [Fact]
        public void test3_1()
        {
            var user = DI.Resolve2<IUserService>();
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
    }

    public class DI
    {
        /// <summary>
        /// 通过反射 获取实例  并向上转成接口类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IUser Resolve(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return (IUser)assembly.CreateInstance(name);
        }

        /// <summary>
        /// 通过反射 获取“一个”实现了此接口的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(T))).FirstOrDefault();
            if (type == null)
            {
                throw new Exception("没有此接口的实现");
            }
            return (T)assembly.CreateInstance(type.ToString());//创建实例 转成接口
        }

        /// <summary>
        /// 通过反射 获取“一个”实现了此接口的实例
        /// 并且 实例中带有构造函数 （构造函数中又有注入）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve2<T>()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(T))).FirstOrDefault();
            if (type == null)
            {
                throw new Exception("没有此接口的实现");
            }

            ////接口 
            ////type.GetConstructors()[0] 获取第一个构造函数 
            ////GetParameters()[0]第一个参数 
            ////ParameterType.FullName 参数类型全名称 
            //var iuser = type.GetConstructors()[0].GetParameters()[0].ParameterType.FullName;
            //var typeuser = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(Type.GetType(iuser))).FirstOrDefault();
            //var user = assembly.CreateInstance(typeuser.ToString());

            var parameter = new List<object>();
            var constructorParameters = type.GetConstructors()[0].GetParameters();
            foreach (var constructorParameter in constructorParameters)
            {
                var tempType = assembly.GetTypes().Where(t => t.GetInterfaces()
                            .Contains(Type.GetType(constructorParameter.ParameterType.FullName)))
                            .FirstOrDefault();
                parameter.Add(assembly.CreateInstance(tempType.ToString()));
            }

            return (T)assembly.CreateInstance(type.ToString(), true, BindingFlags.Default, null, parameter.ToArray(), null, null);//true：不区分大小写            
        }
    }
}
