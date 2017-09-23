using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DemoLibrary;
using Microsoft.EntityFrameworkCore;
using EFCore;

namespace DemoNetCore
{
    //当然，这三个接口应该被定义到外面。让他们类库也可以引用。
    //这样,在其他类库中也可以自动注入了

    // 瞬时（每次都重新实例）
    public interface ITransientDependency { }
    //一个请求内唯一（线程内唯一）
    public interface IScopedDependency { }
    //单例（全局唯一）
    public interface ISingletonDependency { }


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(TempDemo), typeof(TempDemo));

            //获取当前程序集
            var assemblyWeb = Assembly.GetExecutingAssembly();            
            //自动注入 当前程序集下的所有 标记了接口的接口和类型自动注入
            AutoInjection(services, assemblyWeb);

            //获取其他类库程序集
            //var assemblyApplication = ApplicationModule.GetAssembly();
            //自动注册其他类库
            //AutoInjection(services, assemblyApplication);

            services.AddMvc();
            services.AddDbContext<BloggingContext>(options => options.UseSqlServer("Server=(local);Database=EFCoreDemoDB2;Trusted_Connection=True;"));
        }

        /// <summary>
        /// 自动注入
        /// </summary>
        private void AutoInjection(IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes();

            #region ISingletonDependency
            //获取标记了ISingletonDependency接口的接口
            var singletonInterfaceDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(ISingletonDependency)))
                    .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".ISingletonDependency")))
                    .ToList();
            //自动注入标记了 ISingletonDependency接口的 接口
            foreach (var interfaceName in singletonInterfaceDependency)
            {
                var type = types.Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (type != null)
                    services.AddSingleton(interfaceName, type);
            } 
            

            //获取标记了ISingletonDependency接口的类
            var singletonTypeDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(ISingletonDependency)))
                    .ToList();
            //自动注入标记了 ISingletonDependency接口的 类
            foreach (var type in singletonTypeDependency)
            {
                services.AddSingleton(type, type);
            }
            #endregion

            #region ITransientDependency
            //获取标记了ITransientDependency接口的接口
            var transientInterfaceDependency = types
                   .Where(t => t.GetInterfaces().Contains(typeof(ITransientDependency)))
                   .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".ITransientDependency")))
                   .ToList();
            //自动注入标记了 ITransientDependency接口的 接口
            foreach (var interfaceName in transientInterfaceDependency)
            {
                var type = types.Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (type != null)
                    services.AddTransient(interfaceName, type);
            }
            //获取标记了ITransientDependency接口的类
            var transientTypeDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(ITransientDependency)))
                    .ToList();
            //自动注入标记了 ITransientDependency接口的 类
            foreach (var type in transientTypeDependency)
            {
                services.AddTransient(type, type);
            }
            #endregion

            #region IScopedDependency
            //获取标记了IScopedDependency接口的接口
            var scopedInterfaceDependency = types
                   .Where(t => t.GetInterfaces().Contains(typeof(IScopedDependency)))
                   .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".IScopedDependency")))
                   .ToList();
            //自动注入标记了 IScopedDependency接口的 接口
            foreach (var interfaceName in scopedInterfaceDependency)
            {
                var type = types.Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (type != null)
                    services.AddScoped(interfaceName, type);
            }


            //获取标记了IScopedDependency接口的类
            var scopedTypeDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(IScopedDependency)))
                    .ToList();
            //自动注入标记了 IScopedDependency接口的 类
            foreach (var type in scopedTypeDependency)
            {
                services.AddScoped(type, type);
            } 
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
