using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _1_Forms身份认证.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UserName = Session["userName"]?.ToString();           

            return View();
        }       

        public void Login1(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))  //为了方便演示，就不做真的验证了     
                Session["userName"] = userName;
            else
                Session["userName"] = null;
            Response.Redirect(Request.UrlReferrer.LocalPath);//重定向到原来页面
        }

        public void Logout1()
        {
            Session["userName"] = null;
            Response.Redirect(Request.UrlReferrer.LocalPath);//重定向到原来页面
        }

        public void Login2(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))  //为了方便演示，就不做真的验证了     
                FormsAuthentication.SetAuthCookie(userName, true); //登录
            Response.Redirect(Request.UrlReferrer.LocalPath);//重定向到原来页面
        }

        public void Logout2()
        {
            FormsAuthentication.SignOut();//登出
            Response.Redirect(Request.UrlReferrer.LocalPath);//重定向到原来页面
        }

        /// <summary>
        /// 扩展Forms认证
        /// </summary>
        /// <param name="userName"></param>
        public void Login3(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))  //为了方便演示，就不做真的验证了     
            {
                UserInfo user = new UserInfo()
                {
                    Name = userName,
                    LoginTime = DateTime.Now
                };
                //1、序列化要保存的用户信息
                var data = JsonConvert.SerializeObject(user);

                //2、创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, userName, DateTime.Now, DateTime.Now.AddDays(1), true, data);

                //3、加密保存
                string cookieValue = FormsAuthentication.Encrypt(ticket);

                // 4. 根据加密结果创建登录Cookie
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
                cookie.HttpOnly = true;
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.Path = FormsAuthentication.FormsCookiePath;

                // 5. 写登录Cookie
                Response.Cookies.Remove(cookie.Name);
                Response.Cookies.Add(cookie);
            }
            Response.Redirect(Request.UrlReferrer.LocalPath);//重定向到原来页面
        }

        public void Logout3()
        {
            FormsAuthentication.SignOut();//登出
            Response.Redirect(Request.UrlReferrer.LocalPath);//重定向到原来页面
        }

        [Authorize]
        public ActionResult LoginOk()
        {
            return View();
        }

        [MyAuthorize]
        public ActionResult LoginVIP()
        {
            return View();
        }
    }
}