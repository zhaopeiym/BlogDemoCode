using Autofac;
using Autofac.Integration.Mvc;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Controllers;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitContainerBuilder();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void InitContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TestA>();
            builder.RegisterType<DBContext>().InstancePerLifetimeScope();

            //SingleInstance            //单例
            //InstancePerDependency     //每次实例化
            //InstancePerLifetimeScope  //线程内唯一

            //OnActivating  使用之前
            //OnActivated   实例化之后
            //OnRelease     释放之后

            //MVC控制器注入
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //MVC过滤器注入
            builder.RegisterFilterProvider();
            var Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}
