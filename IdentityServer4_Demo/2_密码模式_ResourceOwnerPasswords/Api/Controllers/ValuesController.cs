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
        /// <summary>
        /// 这里可能是私密相册什么的啦（如 QQ相册、腾讯微博内容。。。）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>        
        [Authorize]
        public string Get(string str)
        {
            return str;
        }

        public string Get2(string str)
        {
            return str;
        }
    }
}
