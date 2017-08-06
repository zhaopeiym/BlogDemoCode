using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class WebSocket : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.HistoricalMessg = JsonConvert.SerializeObject(SocketHandler.historicalMessg);
            return View();
        }
    }
}
