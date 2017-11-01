using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Models;
using IdentityServer4;

namespace _1_Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services
              .AddIdentityServer()
              //设置临时签名凭据。
              .AddDeveloperSigningCredential()
              //添加api资源（也就是这些api是受认证中心管理，就像QQ中的QQ相册API）
              //.AddInMemoryApiResources(new List<ApiResource>{
              //      new ApiResource("api1", "My API")
              // })
              //添加Identity资源
              .AddInMemoryIdentityResources(new List<IdentityResource>{
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
              })
              //添加客户信息（也就是我们申请QQ第三方登录时，QQ给我们的APP ID、APP Key）
              //QQ中的APP ID对应我们这里的ClientId，APP Key对应我们这里的Secret
              .AddInMemoryClients(new List<Client>{
                new Client
                {
                    ClientId = "client_mvc",//客户端ClientId需要这这里的一致
                    AllowedGrantTypes = GrantTypes.Implicit,//简化模式
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
              });
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

            app.UseIdentityServer();
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
