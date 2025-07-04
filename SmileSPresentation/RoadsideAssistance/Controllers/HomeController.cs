using RoadsideAssistance.Helper;
using System.Web.Mvc;

namespace RoadsideAssistance.Controllers
{
    [Authorization(Roles = "Developer,RoadsideAssistance_Callcenter")]
    public class HomeController : Controller
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