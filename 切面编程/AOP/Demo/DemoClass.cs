using Castle.DynamicProxy;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Demo
{
    public class MyClass : IMyClass
    {
        public virtual void MyMethod()
        {
            Console.WriteLine("My Mehod");
        }
    }

    public class TestIntercept : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("111111111111");
            invocation.Proceed();
            Console.WriteLine("222");
        }
    }
    
    public class Test_tests
    {
        [Fact]
        public void tests()
        {
            var proxyGenerate = new ProxyGenerator();
            TestIntercept t = new TestIntercept(); 
            var pg = proxyGenerate.CreateClassProxy<MyClass>(t);
            pg.MyMethod(); 
        }
    } 
}
