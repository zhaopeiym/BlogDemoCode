using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_Server.Models
{
    public class ConsentOutputModel
    {
        public string[] IdentityScopes { get;  set; }
        public string[] ResourceScopes { get;  set; }
        public string ReturnUrl { get;  set; }
    }
}
