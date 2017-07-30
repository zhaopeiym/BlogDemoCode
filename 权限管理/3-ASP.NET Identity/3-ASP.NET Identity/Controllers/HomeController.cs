using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _3_ASP.NET_Identity.Controllers
{
    public class HomeController : Controller
    {

        private UserManager<MyUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public HomeController()
        {
            var db = new MyDbContext();
            //实例化用户操作类，AspNet.Identity 提供
            userManager = new UserManager<MyUser>(new UserStore<MyUser>(db));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            GetAllUsers();
            GetRoles();
            GetRolesByUser();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        //Microsoft.Owin.Host.SystemWeb 提供
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPassword">密码</param>
        /// <returns></returns>
        public async Task Login(string userName, string userPassword)
        {
            // 1、查询用户
            var user = await userManager.FindAsync(userName, userPassword);
            if (user != null)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                //2、获取identity 对象 (TODO:自己构造identity)
                //var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                //等效于上面一句代码
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));//添加一个 "证件单元" (用户名)
                var roleIds = user.Roles.Select(r => r.RoleId).ToList();
                var roles = roleManager.Roles.Where(t => roleIds.Contains(t.Id)).Select(t => t.Name).ToList();
                for (int i = 0; i < roles.Count; i++)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles[i]));//添加 "证件单元" （角色）
                }
                var identity = new ClaimsIdentity(claims, "ApplicationCookie");//实例一个 "身份"

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
            var result = await userManager.CreateAsync(user, userPassword);

            if (!result.Succeeded)
                Response.Write("<script>alert('注册失败')</script>");
            else
                Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        /// <summary>
        /// 获取所以用户
        /// </summary>
        /// <returns></returns>
        public void GetAllUsers()
        {
            ViewBag.Users = userManager.Users.Select(t => t.UserName).ToList();
        }

        //新建角色
        public void AddRole(string roleName)
        {
            var role = new IdentityRole() { Name = roleName };
            roleManager.Create(role);

            GetRoles();
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        //获取所有角色
        public void GetRoles()
        {
            ViewBag.Roles = roleManager.Roles.Select(t => t.Name).ToArray();
        }

        //删除角色
        public void DelRole(string roleName)
        {
            var role = roleManager.Roles.Where(t => t.Name == roleName).FirstOrDefault();
            if (role != null)
            {
                roleManager.Delete(role);
                GetRoles();//刷新
            }
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        //给用户新增角色
        public async Task AddUserToRole(string roleName)
        {
            var userId = userManager.FindByName(User.Identity.Name).Id;
            var identityResult = await userManager.AddToRoleAsync(userId, roleName);

            await AnewLoginAsync();//重新登录
            GetRolesByUser();
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        //获取所有角色
        public void GetRolesByUser()
        {
            HttpContextWrapper context = new HttpContextWrapper(System.Web.HttpContext.Current);
            var userId = context.User.Identity.GetUserId();
            ViewBag.RolesByUser = roleManager.Roles.Where(t => t.Users.Any(u => u.UserId == userId)).Select(t => t.Name).ToArray();
        }    

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <returns></returns>
        public async Task AnewLoginAsync()
        {
            var userId = userManager.FindByName(User.Identity.Name).Id;
            var user = userManager.Users.Where(t => t.Id == userId).First();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //2、获取identity 对象 (TODO:自己构造identity)
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            //3、使用identity对象登录 
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
        }

        //删除用户中的角色
        public async Task DelUserToRole(string roleName)
        {
            var userId = userManager.FindByName(User.Identity.Name).Id;
            var identityResult = await userManager.RemoveFromRoleAsync(userId, roleName);

            await AnewLoginAsync();//重新登录
            GetRolesByUser();
            Response.Redirect(Request.UrlReferrer.LocalPath);
        }

        [Authorize(Roles = "VIP")]
        public ActionResult VIP()
        {
            return View();
        }
    }
}