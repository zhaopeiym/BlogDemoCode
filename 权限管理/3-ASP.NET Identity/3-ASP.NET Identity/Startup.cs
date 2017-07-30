using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_3_ASP.NET_Identity.Startup))]
namespace _3_ASP.NET_Identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app); // 这里注意下，访问不到是Startup.cs 的命名空间和 Startup.Auth.cs 不一致。   
        }
    }
}