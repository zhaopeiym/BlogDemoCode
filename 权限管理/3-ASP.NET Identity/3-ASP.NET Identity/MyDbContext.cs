using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace _3_ASP.NET_Identity
{
    public class MyDbContext : IdentityDbContext<MyUser>
    {
        public MyDbContext() : base("DefaultConnection")
        {
        }
    }
}