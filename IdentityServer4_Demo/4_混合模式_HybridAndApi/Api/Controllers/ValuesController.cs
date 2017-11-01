using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        [Authorize]
        [HttpGet]
        public string Get()
        {
            return "私密相册";
        }
    }
}
