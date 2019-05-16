using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace DapperDemo
{
    /// <summary>
    /// nuget 安装Autofac.Extras.DynamicProxy
    /// </summary>
    public class UnitOfWorkIInterceptor : IInterceptor
    {
        private DBContext dBContext;
        public UnitOfWorkIInterceptor(DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
                methodInfo = invocation.Method;

            UnitOfWorkAttribute transaction = methodInfo.GetCustomAttributes<UnitOfWorkAttribute>(true).FirstOrDefault();
            //如果标记了 [UnitOfWork]，并且不在事务嵌套中。
            if (transaction != null && dBContext.Committed)
            {
                //开启事务
                dBContext.BeginTransaction();
                try
                {
                    //事务包裹 查询语句 
                    //https://github.com/mysql-net/MySqlConnector/issues/405
                    invocation.Proceed();
                    //提交事务
                    dBContext.CommitTransaction();
                }
                catch (Exception ex)
                {
                    //回滚
                    dBContext.RollBackTransaction();
                    throw;
                }
            }
            else
            {
                //如果没有标记[UnitOfWork]，直接执行方法
                invocation.Proceed();
            }
        }
    }
}
