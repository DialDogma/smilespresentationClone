using System.Web.Mvc;
using SmileSMiscellaneous.Helper;

namespace SmileSMiscellaneous.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.AppId = 1;
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}
