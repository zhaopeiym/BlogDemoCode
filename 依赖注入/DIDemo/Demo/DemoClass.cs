using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
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

    public class UserService
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

        public static IUser Resolve(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return (IUser)assembly.CreateInstance(name);
        }

        public static T Resolve<T>()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(T))).FirstOrDefault();
            if (type == null)
            {
                throw new Exception("没有此接口的实现");
            }
            return (T)assembly.CreateInstance(type.ToString());
        }
    }

    #region 例1
    //public interface IUser
    //{
    //    string GetName();
    //}
    //public class User : IUser
    //{
    //    public string GetName()
    //    {
    //        return "农码一生";
    //    }
    //}

    //public class User_Tests
    //{
    //    [Fact]
    //    public void GetName_Ok()
    //    {
    //        var container = new WindsorContainer();
    //        container.Register(Component.For<IUser>().ImplementedBy<User>().LifestyleTransient());
    //        var user = container.Resolve<IUser>();
    //        var name = user.GetName();
    //        Assert.True(name == "农码一生");
    //    }
    //}
    #endregion

    #region 例2
    //public interface ITransient { }
    //public interface IUser : ITransient
    //{
    //    string GetName();
    //}
    //public class User : IUser
    //{
    //    public string GetName()
    //    {
    //        return "农码一生";
    //    }
    //}

    //public class AssmInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        container.Register(Classes.FromThisAssembly()   //选择Assembly
    //            .IncludeNonPublicTypes()                    //约束Type
    //            .BasedOn<ITransient>()                      //约束Type
    //            .WithService.DefaultInterfaces()            //匹配类型
    //            .LifestyleTransient());                     //注册生命周期
    //    }
    //}


    //public class UserService : ITransient
    //{
    //    private readonly IUser _user;
    //    public UserService(IUser user)
    //    {
    //        _user = user;
    //    }

    //    public string GetUserName()
    //    {
    //        return "UserService:" + _user.GetName();
    //    }
    //}
    //public class User_Tests
    //{
    //    [Fact]
    //    public void GetName_IWindsorInstaller_Ok()
    //    {
    //        var container = new WindsorContainer();
    //        container.Install(FromAssembly.This());
    //        var user = container.Resolve<IUser>();
    //        var name = user.GetName();
    //        Assert.True(name == "农码一生");

    //        var userService = container.Resolve<UserService>();
    //        var str = userService.GetUserName();
    //        Assert.True(str == "UserService:农码一生");
    //    }
    //} 
    #endregion

}
