using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4_SSO_Service.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }        
    }
}
