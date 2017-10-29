using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using IdentityServer4.Models;

namespace IdentityServer4_SSO_Service.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public ConsentController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        /// <summary>
        /// 回调，给客户端的信息
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string returnUrl)
        {
            var ScopesConsented = new string[] { "openid", "profile", "api1", "offline_access" };
            var grantedConsent = new ConsentResponse
            {
                RememberConsent = true,
                ScopesConsented = ScopesConsented
            };
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _interaction.GrantConsentAsync(request, grantedConsent);

            return Redirect(returnUrl);//重定向到Client
        }
    }
}