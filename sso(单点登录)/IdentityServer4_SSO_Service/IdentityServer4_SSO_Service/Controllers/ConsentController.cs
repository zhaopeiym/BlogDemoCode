using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using IdentityServer4.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer4_SSO_Service.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public ConsentController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

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

            return Redirect(returnUrl);
        }
    }
}