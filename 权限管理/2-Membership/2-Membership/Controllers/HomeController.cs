using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _2_Membership.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public void Register(string userName, string userPassword)
        {
            var user = Membership.CreateUser(userName, userPassword);
            if (user != null)//注册成功
            {
                FormsAuthentication.SetAuthCookie(user.UserName, true);
            }
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public void Login1(string userName, string userPassword)
        {
            if (Membership.ValidateUser(userName, userPassword))
            {
                FormsAuthentication.SetAuthCookie(userName, true);
            }
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }
        /// <summary>
        /// 登出
        /// </summary>
        public void Logout1()
        {
            FormsAuthentication.SignOut();
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public void ModifyPassword(string oldPassword, string newPassword)
        {
            var user = Membership.GetUser(User.Identity.Name);

            bool isOk = user.ChangePassword(oldPassword, newPassword);
            if (isOk)
                Response.Write("<script>alert('密码修改成功')</script>");
            else
                Response.Write("<script>alert('密码修改失败')</script>");
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleName"></param>
        public void AddRole(string roleName)
        {
            Roles.CreateRole(roleName);
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleName"></param>
        public void DelRole(string roleName)
        {
            Roles.DeleteRole(roleName);
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }
        /// <summary>
        /// 给当前用户添加角色
        /// </summary>
        /// <param name="roleName"></param>
        public void AddRoleByUser(string roleName)
        {
            Roles.AddUserToRole(User.Identity.Name, roleName);
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        /// <summary>
        /// 从角色中移除用户
        /// </summary>
        /// <param name="roleName"></param>
        public void DelRoleByUser(string roleName)
        {
            Roles.RemoveUserFromRole(User.Identity.Name, roleName);
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        [Authorize(Roles = "VIP")]
        public ActionResult VIP()
        {
            return View();
        }
    }
}