using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void Login(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                FormsAuthentication.SetAuthCookie(userName, true);
            }
            HttpContext.Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Response.Redirect(Request.UrlReferrer.LocalPath);
        }
    }
}