using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DemoNetCore.Controllers;
using System.Reflection;

namespace DemoNetCore
{

    public interface IApplicationService { }
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
            #region 自动注入
            Assembly assembly = Assembly.GetExecutingAssembly();
            var interfaces = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IApplicationService)))
                    .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".IApplicationService")))
                    .ToList();

            //自动注入标记了 IApplicationService接口的 接口
            foreach (var interfaceName in interfaces)
            {
                var obj = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (obj != null)
                    services.AddSingleton(interfaceName, obj);
            } 
            #endregion

            services.AddMvc();
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
