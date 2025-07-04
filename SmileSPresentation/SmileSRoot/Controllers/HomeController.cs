using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SmileSRoot.Models;

namespace SmileSRoot.Controllers
{
    public class HomeController:Controller
    {
        public ActionResult Index()
        {
            using(var db = new SSSRootDBContext())
            {
                var result = db.usp_RootMenu_Select(2).ToList();
                ViewBag.RootMenuWebApp = result;
            }

            return View();
        }

        public ActionResult Index2()
        {
            using(var db = new SSSRootDBContext())
            {
                var result = db.usp_RootMenu_Select(3).ToList();
                ViewBag.RootMenuWebApp = result;
            }
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}