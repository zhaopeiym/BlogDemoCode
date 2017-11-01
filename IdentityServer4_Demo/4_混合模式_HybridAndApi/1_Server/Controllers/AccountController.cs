using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Events;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _1_Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public AccountController(IIdentityServerInteractionService interaction,
            IEventService events)
        {
            _interaction = interaction;
            _events = events;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {

            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string returnUrl)
        {
            returnUrl = HttpUtility.UrlDecode(returnUrl); //url解码                                                        
            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30))
            };
            // 验证登录信息，并登录
            await HttpContext.SignInAsync("1", userName, props);
            return Redirect(returnUrl);
        }

        /// <summary>
        /// client在登出的时候 会跳转到这个页面，然后跳转到client http://localhost:5002/signout-callback-oidc
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            await HttpContext.SignOutAsync();
            await _events.RaiseAsync(new UserLogoutSuccessEvent("1", "农码一生"));

            if (!string.IsNullOrWhiteSpace(logout?.PostLogoutRedirectUri))
            {
                return Redirect(logout?.PostLogoutRedirectUri);
            }
            return View("Login");
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            return View();
        }

    }
}
