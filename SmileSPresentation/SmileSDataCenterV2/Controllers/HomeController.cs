using System.Web.Mvc;
using SmileSDataCenterV2.Helper;

namespace SmileSDataCenterV2.Controllers
{
    [Authorization(Roles = "Developer")]
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