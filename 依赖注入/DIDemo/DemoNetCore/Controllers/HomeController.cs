using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoNetCore.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCore.Controllers
{

    public interface IUser
    {
        string GetName();
    }
    public class User : IUser
    {
        public string GetName()
        {
            return "农码一生";
        }
    }

    public interface IUserService
    {
        string GetName();
    }

    public class UserService : IUserService
    {
        private IUser _user;
        public UserService(IUser user)
        {
            _user = user;
        }

        public string GetName()
        {
            return _user.GetName();
        }
    }

    public class HomeController : Controller
    {
       

        private readonly IUser _user; 
        public HomeController(IUser user)
        {
            _user = user;
            var test = _user.GetName();
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
