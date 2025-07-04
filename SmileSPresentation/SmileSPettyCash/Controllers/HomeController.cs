using System.Web.Mvc;
using SmileSPettyCash.Helper;

namespace SmileSPettyCash.Controllers
{
    [Authorization]
    public class HomeController:Controller
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