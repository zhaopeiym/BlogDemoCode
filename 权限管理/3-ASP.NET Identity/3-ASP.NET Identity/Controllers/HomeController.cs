using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3_ASP.NET_Identity.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() : this(new UserManager<MyUser>(new UserStore<MyUser>(new MyDbContext())))
        {
        }
        public HomeController(UserManager<MyUser> userManager)
        {
            UserManager = userManager;
        }
        public UserManager<MyUser> UserManager { get; private set; }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
      
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public async Task Login(string userName, string userPassword)
        {
            // 1、查询用户
            var user = await UserManager.FindAsync(userName, userPassword);
            if (user != null)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                //2、获取identity 对象
                var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                //3、使用identity对象登录
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

                Response.Redirect(Request.UrlReferrer.LocalPath);//重定向到操作页
            }
            else
                Response.Write("<script>alert('登录失败')</script>");
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Logout()
        {
            AuthenticationManager.SignOut();
            Response.Redirect(Request.UrlReferrer.LocalPath);
        } 

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public async Task Register(string userName, string userPassword)
        {
            var user = new MyUser() { UserName = userName };
            var result = await UserManager.CreateAsync(user, userPassword);

            if (!result.Succeeded)
                Response.Write("<script>alert('注册失败')</script>");
            else
                Response.Redirect(Request.UrlReferrer.LocalPath);
        }
    }
}