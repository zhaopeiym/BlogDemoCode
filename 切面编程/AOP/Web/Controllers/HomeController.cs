using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ClassA
    {
        private readonly ClassB b;

        public ClassA(ClassB b)
        {
            this.b = b;
        }

        public void Show()
        {
            Console.WriteLine("I am ClassA's instance !");
        }
    }

    public class ClassB
    {
        public ClassA A { get; set; }

        public void Show()
        {
            Console.WriteLine("I am ClassB's instance !");
        }

    }

    public class ClassC
    {
        public string Name { get; set; }

        public ClassD D { get; set; }

        public void Show()
        {
            Console.WriteLine("I am ClassC's instance !" + Name);
        }
    }

    public class ClassD
    {
        public void Show()
        {
            Console.WriteLine("I am ClassD's instance !");
        }
    }


    public class TestA
    {
        public DBContext order2;
        public TestA(DBContext order2)
        {
            order2 = null;
        }
    }

    public class TestB: TestA
    {
        public TestB(DBContext order2):base(order2)
        {
        }
        public void test()
        {
            var obj = order2;
        }
    }
    public class HomeController : Controller
    {
        public DBContext order { get; set; }
        public TestA testA { get; set; }
        private TestB testB;
        public HomeController(TestB testB)
        {
          
            this.testB = testB;
        }
        public ActionResult Index()
        {
            testB.test();
            var obj2 = this.order.GetHashCode();
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