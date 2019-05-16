using Microsoft.AspNetCore.Mvc;

namespace DapperDemo.Controllers
{

    public class HomeController : Controller
    {
        private UserManager userManager;
        public HomeController(UserManager userManager)
        {
            this.userManager = userManager;
        }
        public virtual IActionResult Index()
        {
            userManager.AddUser();
            userManager.DelUser();

            return View();
        }
    }
}
