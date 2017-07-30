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
            ConfigureAuth(app);
        }
    }
}