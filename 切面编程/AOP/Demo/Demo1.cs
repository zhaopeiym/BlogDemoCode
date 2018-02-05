using Autofac;
using ClassLibrary1;
using Xunit;

namespace Demo
{
    public class Demo1
    {
        [Fact]
        public void Test1()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DBContext>();
        }
    }
}
