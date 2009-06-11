using System;
using System.Web.Mvc;
using Commerce.Services;
using System.Web.Security;
using System.Management.Instrumentation;

namespace Commerce.MVC.Web.Controllers
{
    public class HomeController : Controller {
        
        public ActionResult Index() {
            ViewData["Title"] = "Home Page";
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            return View();
        }

        public ActionResult About() {
            ViewData["Title"] = "About Page";

            return View();
        }

    }
}