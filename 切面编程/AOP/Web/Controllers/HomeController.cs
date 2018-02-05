using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{

    public class TestA
    {
        public DBContext order2 = null;
        public TestA(DBContext order)
        {
            order2 = order;
        }
    }
    public class HomeController : Controller
    {
        private DBContext order;
        private TestA testA;
        public HomeController(DBContext order, TestA testA)
        {
            this.order = order;
            this.testA = testA;
        }
        public ActionResult Index()
        {

            ViewBag.OrderHashCode = $"{order.GetHashCode() } --  {testA.order2.GetHashCode()}";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}