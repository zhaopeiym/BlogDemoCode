using DemoLibrary;
using EFCore;
using Microsoft.AspNetCore.Mvc;

namespace DemoNetCore.Controllers
{
    public class HomeController : Controller
    {
        BloggingContext bloggingContext;
        DemoLibrary.TempDemo tempDemo;
        public HomeController(BloggingContext bloggingContext, DemoLibrary.TempDemo tempDemo)
        {
            this.bloggingContext = bloggingContext;
            this.tempDemo = tempDemo;
        }

        public IActionResult Index()
        {
            // 获取类库中的DbContext实例Code
            var code1 = tempDemo.GetDBHashCode();
            // 获取web启动项中DbContext实例Code
            var code2 = bloggingContext.GetHashCode();
            BloggingContext context = new BloggingContext();
            var code3 = context.GetHashCode();//获取方法内实例的Code

            var a = code1 == code2;
            var b = code1 == code3;

            return View();
        }
    }     
}
