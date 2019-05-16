using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Demo
{
    //[Intercept(typeof(TestIntercept))]
    public class MyClass
    {
        //[Fact]
        public virtual void MyMethod()
        {
            Test();
            Console.WriteLine("连接数据库，操作数据");
        }
        [Fact]
        public virtual void Test()
        {
            Console.WriteLine("连接数据库，操作数据");
        }
    }

    public class TestIntercept : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null) 
                methodInfo = invocation.Method; 

            FactAttribute transaction = methodInfo.GetCustomAttributes<FactAttribute>(true).FirstOrDefault();
            if (transaction != null)
            {
                //开启事务
                invocation.Proceed();
                //提交事务
            }
            else
            {
                // 没有事务时直接执行方法
                invocation.Proceed();
            }

        }
    }

    public class Test_tests
    {
        #region 注释
        //[Fact]
        //public void tests()
        //{
        //    var proxyGenerate = new ProxyGenerator();
        //    TestIntercept t = new TestIntercept();
        //    var pg = proxyGenerate.CreateClassProxy<MyClass>(t);
        //    pg.MyMethod();
        //} 
        #endregion

        #region 注释
        //[Fact]
        //public void Test()
        //{
        //    var obj = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name == "Test_tests").FirstOrDefault();
        //    var auutributes = ((TypeInfo)obj).DeclaredMethods.Where(t => t.Name == "tests")
        //        .SelectMany(t => t.CustomAttributes.Select(c => c.AttributeType.Name))
        //        .ToList();
        //} 
        #endregion

        [Fact]
        public void temp()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TestIntercept>();
            ////启用类代理拦截
            //builder.RegisterType<MyClass>().EnableClassInterceptors();
            ////启用接口代理拦截
            //builder.RegisterType<MyClass>().EnableInterfaceInterceptors();

            //动态注入拦截器CallLogger
            builder.RegisterType<MyClass>().InterceptedBy(typeof(TestIntercept)).EnableClassInterceptors();

            var obj = builder.Build().Resolve<MyClass>();
            obj.MyMethod();
            obj.Test();
        }
    }
}
