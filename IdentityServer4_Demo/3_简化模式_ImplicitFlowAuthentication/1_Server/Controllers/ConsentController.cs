using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Web;
using _1_Server.Models;
using IdentityServer4.Stores;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _1_Server.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IResourceStore _resourceStore;
        private readonly IClientStore _clientStore;
        public ConsentController(IIdentityServerInteractionService interaction,
            IResourceStore resourceStore,
            IClientStore clientStore)
        {
            _interaction = interaction;
            _resourceStore = resourceStore;
            _clientStore = clientStore;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var vm = new ConsentOutputModel();
            //授权 身份信息
            vm.IdentityScopes = resources.IdentityResources.Select(x => x.Name).ToArray();
            //授权 资源（如相册什么的）
            vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => x.Name).ToArray();
            vm.ReturnUrl = returnUrl;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var returnUrl = model.ReturnUrl;
            var ScopesConsented = model.ScopesConsented;
            var grantedConsent = new ConsentResponse
            {
                RememberConsent = false,//下次授权自动同意
                ScopesConsented = ScopesConsented
            };
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _interaction.GrantConsentAsync(request, grantedConsent);

            return Redirect(returnUrl);//重定向到Client
        }



    }
}
