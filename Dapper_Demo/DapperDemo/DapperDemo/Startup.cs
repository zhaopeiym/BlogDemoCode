using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using DapperDemo.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DapperDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            //benny
            services.AddMvc(t => { t.Filters.Add<ActionFilter>(); }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            return new AutofacServiceProvider(InitContainerBuilder(services));//第三方IOC接管 core内置DI容器 
        }

        /// <summary>
        /// 初始化 注入容器
        /// </summary>
        private IContainer InitContainerBuilder(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
            var builder = new ContainerBuilder();
            builder.RegisterType<UnitOfWorkIInterceptor>();
            //线程内唯一（也就是单个请求内唯一）
            builder.RegisterType<DBContext>().InstancePerLifetimeScope();            
            builder.RegisterType<UserManager>().InterceptedBy(typeof(UnitOfWorkIInterceptor)).EnableClassInterceptors();
            builder.Populate(services);
            return builder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
