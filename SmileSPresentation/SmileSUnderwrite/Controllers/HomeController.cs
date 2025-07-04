using System.Web.Mvc;
using SmileSUnderwrite.Helper;

namespace SmileSUnderwrite.Controllers
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