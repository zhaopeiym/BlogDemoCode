using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Services;
using System.Web;
using Microsoft.AspNetCore.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer4_SSO_Service.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interaction,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _interaction = interaction;
            _userManager = userManager;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<IActionResult> Register(string Email, string Password)
        {
            var user = new IdentityUser { UserName = Email, Email = Email };
            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = EmailConfirmationLink(Url, user.Id, code, Request.Scheme);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/");
            }
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password, string ReturnUrl = null)
        {
            var returnUrl = HttpUtility.UrlDecode(ReturnUrl); //url解码
            var result = await _signInManager.PasswordSignInAsync(Email, Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            return Redirect("/");
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            await _signInManager.SignOutAsync();
            if (!string.IsNullOrWhiteSpace(logout?.PostLogoutRedirectUri))
            {
                return Redirect(logout?.PostLogoutRedirectUri);
            }
            return View("Login");
        }

        private async Task<string> EmailConfirmationLink(IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return urlHelper.Action(
                action: nameof(result.Succeeded),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
