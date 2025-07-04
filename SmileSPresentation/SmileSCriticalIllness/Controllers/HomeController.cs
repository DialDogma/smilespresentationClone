using System.Web.Mvc;
using SmileSCriticalIllness.Helper;

namespace SmileSCriticalIllness.Controllers
{
    [Authorization]
    public class HomeController:Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}