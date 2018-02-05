using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Demo
{
    public interface IDependency
    {

    }
    /// <summary>
    /// 开启事务属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionCallHandlerAttribute : Attribute
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }

        ///// <summary>
        ///// 事务范围
        ///// </summary>
        //public TransactionScopeOption ScopeOption { get; set; }

        ///// <summary>
        ///// 事务隔离级别
        ///// </summary>
        //public IsolationLevel IsolationLevel { get; set; }

        //public TransactionCallHandlerAttribute()
        //{
        //    Timeout = 60;
        //    ScopeOption = TransactionScopeOption.Required;
        //    IsolationLevel = IsolationLevel.ReadCommitted;
        //}
    }


    /// <summary>
    /// 事务 拦截器
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        ////可自行实现日志器，此处可忽略
        ///// <summary>
        ///// 日志记录器
        ///// </summary>
        //private static readonly ILog Logger = Log.GetLog(typeof(TransactionInterceptor));

        // 是否开发模式
        private bool isDev = false;
        public void Intercept(IInvocation invocation)
        {
            if (!isDev)
            {
                MethodInfo methodInfo = invocation.MethodInvocationTarget;
                if (methodInfo == null)
                {
                    methodInfo = invocation.Method;
                }

                TransactionCallHandlerAttribute transaction = methodInfo.GetCustomAttributes<TransactionCallHandlerAttribute>(true).FirstOrDefault();
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
            else
            {
                // 开发模式直接跳过拦截
                invocation.Proceed();
            }
        }
    }

    /// <summary>
    /// 应用程序IOC配置
    /// </summary>
    public class IocConfig
    {
        // 重写加载配置
        public void Load(ContainerBuilder builder)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            builder.RegisterType<TransactionInterceptor>();
            builder.RegisterAssemblyTypes(assembly)
                .Where(type => typeof(IDependency).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor));
        }
    }


    public class testClass2: IDependency
    {

        [TransactionCallHandler]
        public void AddArticle(string name)
        {

        }

        [Fact]
        public void test()
        {
            IocConfig iocConfig = new IocConfig();
            iocConfig.Load(new ContainerBuilder());

            AddArticle("");
        }
    }
}
