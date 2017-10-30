using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _1_Server.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public ConsentController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(string returnUrl)
        {
            var ScopesConsented = new string[] { "openid", "profile", "offline_access" };
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
