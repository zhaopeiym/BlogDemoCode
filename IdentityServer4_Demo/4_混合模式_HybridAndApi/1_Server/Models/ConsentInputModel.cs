using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_Server.Models
{
    public class ConsentInputModel
    {
        public string ReturnUrl { get; set; }
        public string[] ScopesConsented { get; set; }
    }
}
