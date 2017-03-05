using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace web服务.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public async Task<string> Get(string str)
        {
            GetDataHelper sqlHelper = new GetDataHelper();
            switch (str)
            {
                //AsyncHelper.RunSync(
                case "异步处理"://                    
                    return await sqlHelper.GetDataAsync();
                case "同步处理"://
                    return sqlHelper.GetData();
            }
            return "参数不正确";           
        }       
    }
}
