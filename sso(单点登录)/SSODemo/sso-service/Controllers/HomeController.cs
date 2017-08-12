using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sso_service.Controllers
{
    public class HomeController : Controller
    {
        public static Dictionary<string, Guid> TokenIds = new Dictionary<string, Guid>();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 验证是否登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Verification(string backUrl)
        {
            if (Session["user"] == null)
            {
                return Redirect("Login?backUrl="+backUrl);
            }

            Session["user"] = "已经登录";
            return Redirect(backUrl + "?tokenId=" + TokenIds[Session.SessionID]);
        }

        /// <summary>
        /// 验证TokenId是否有效
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool TokenIdIsValid(Guid tokenId)
        {
            return TokenIds.Any(t => t.Value == tokenId);
        }

        /// <summary>
        /// 返回登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passWord"></param>
        /// <param name="backUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public string Login(string name, string passWord, string backUrl)
        {
            if (true)//TODO：验证用户名密码登录
            {
                Session["user"] = "已经登录";
                TokenIds.Add(Session.SessionID, Guid.NewGuid());
            }
            else//验证失败重新登录
            {
                return "/Home/Login";
            }
            return backUrl + "?tokenId=" + TokenIds[Session.SessionID];//生成一个tokenId 发放到客户端
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {

            using (HttpClient http = new HttpClient())
            {
                await http.GetAsync("http://localhost:811/Home/ClearToken?tokenId=" + TokenIds[Session.SessionID]);
                await http.GetAsync("http://localhost:812/Home/ClearToken?tokenId=" + TokenIds[Session.SessionID]);
            }
            TokenIds.Remove(Session.SessionID);
            Session["user"] = null;
            return Redirect("Login");
        }
    }
}
