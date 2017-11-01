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
using System.Security.Claims;

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
              .AddInMemoryApiResources(new List<ApiResource>{
                    new ApiResource("api1", "My API")
               })
              //添加Identity资源
              .AddInMemoryIdentityResources(new List<IdentityResource>{
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
              })
              //添加客户信息（也就是我们申请QQ第三方登录时，QQ给我们的APP ID、APP Key）
              //QQ中的APP ID对应我们这里的ClientId，APP Key对应我们这里的Secret
              //这里 生产应该用 IdentityServer4.EntityFramework 实现
              .AddInMemoryClients(new List<Client>{
                    new Client
                    {
                        ClientId = "client_mvc",//客户端ClientId需要这这里的一致
                        ClientName = "MVC Client",
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())  // 对应客户端的 options.ClientSecret
                        },
                        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,//GrantTypes.Implicit,//简化模式  //
                        RedirectUris = { "http://localhost:5002/signin-oidc" },
                        PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                        //设置客户端  ClientId 可以访问的资源
                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "api1"  //对应客户端的options.Scope.Add("api1");
                        },
                        AllowOfflineAccess = true  //对应客户端的 options.Scope.Add("offline_access");
                    }
              })
              //这里 生产 应该要用Identity.EntityFrameworkCore的IdentityDb来实现
              .AddTestUsers(new List<IdentityServer4.Test.TestUser>() {
                   new IdentityServer4.Test.TestUser(){
                        SubjectId = "1",
                        Username = "农码一生",
                        Password = "password",
                        Claims = new List<Claim>{
                            new Claim("name", "农码一生"), //这里的名字 可以显示在客户端
                            new Claim("website", "https://alice.com")
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
