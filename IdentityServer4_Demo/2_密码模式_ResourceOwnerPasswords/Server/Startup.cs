using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Server
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services
              .AddIdentityServer()
              //设置临时签名凭据。
              .AddDeveloperSigningCredential()   
              //添加api资源（也就是这些api是受认证中心管理，就像QQ中的QQ相册API）
              .AddInMemoryApiResources(new List<ApiResource>{
                    new ApiResource("api1", "My API")
               })
              //添加客户信息（也就是我们申请QQ第三方登录时，QQ给我们的APP ID、APP Key）
              //QQ中的APP ID对应我们这里的ClientId，APP Key对应我们这里的Secret
              .AddInMemoryClients(new List<Client>{
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,//密码模式
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                }
              })
              //测试用户
              .AddTestUsers(new List<TestUser>{
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "农码一生",
                        Password = "mima"
                    }
              });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            app.UseIdentityServer();            
        }
    }
}
