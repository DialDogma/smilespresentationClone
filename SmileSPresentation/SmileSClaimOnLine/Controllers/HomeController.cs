using System;
using System.Web.Mvc;
using SmileSClaimOnLine.Helper;

namespace SmileSClaimOnLine.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}