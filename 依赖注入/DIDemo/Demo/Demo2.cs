using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoLibrary
{
    public class Demo2
    {
        public interface IUser
        {
            string GetName();
        }
        public class User : IUser
        {
            private string userName;
            public User()
            {
                userName = "农码一生";
            }

            public User(string userName)
            {
                this.userName = userName;
            }
            public string GetName()
            {
                return userName;
            }
        }

        public class User_Tests
        {
            [Fact]
            public void GetName_Ok()
            {
                var container = new WindsorContainer();
                //container.Register(Component.For<IUser>().ImplementedBy<User>().LifestyleTransient());
                //var user = container.Resolve<IUser>();
                //var name = user.GetName();
                //Assert.True(name == "农码一生");

                container.Register(Castle.MicroKernel.Registration.Component.For<IUser>()
                    .ImplementedBy<User>()
                    .DependsOn(dependency: Dependency.OnValue("userName", "benny")) //带参数
                    .LifestyleTransient());

                var user1 = container.Resolve<IUser>();
                var name1 = user1.GetName();
                Assert.True(name1 == "benny");

                IDictionary parameters = new Hashtable { { "userName", "农码" } };//带参数
                var user2 = container.Resolve<IUser>(parameters);
                var name2 = user2.GetName();
                Assert.True(name2 == "农码");

                //http://www.jianshu.com/p/d21c29334f78
            }
        }
    }
}
