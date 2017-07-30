using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3_ASP.NET_Identity
{   
    public class MyUser : IdentityUser
    {
        public string MyName { get; set; }
    }

    public class MyRole : IdentityRole
    {    
    }
}