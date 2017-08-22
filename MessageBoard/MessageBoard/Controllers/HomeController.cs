using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _db;

        public HomeController(DataContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int page, int cont)
        {
            ViewBag.Messgs = await GetData(0, 30);
            return View();
        }


        public async Task<List<Message>> GetData(int page, int cont)
        {
            cont = cont == 0 || cont > 30 ? 30 : cont;

            var messgs = await _db.Messages.OrderByDescending(t => t.CreateTime).Skip(page).Take(cont).AsNoTracking().ToListAsync();
            return messgs;
        }

        [HttpPost]
        public async Task RecordMessges(string msg, string userName)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                var ip = GetUserIp();
                _db.Messages.Add(new Message()
                {
                    CreateTime = DateTime.Now,
                    IP = ip,
                    UserName = userName ?? "蒙面人",
                    Content = msg
                });
                await _db.SaveChangesAsync();
            }
            //Response.Redirect("/Home/Index");
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public string GetUserIp()
        {
            var ip = HttpContext.Request.Headers["X-real-ip"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip == "::1" ? "来自外星" : ip;
        }
    }
}
