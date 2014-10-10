using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bettery.WebRole.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {

            
            ViewBag.Message = "Welcome to BETTERY!";

            return RedirectToAction("LogIn", "Account");
            
        }
        [Authorize]
        public ActionResult About()
        {
            return View();
        }
    }
}
