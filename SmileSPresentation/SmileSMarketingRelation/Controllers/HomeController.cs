using System.Web.Mvc;
using SmileSMarketingRelation.Helper;

namespace SmileSMarketingRelation.Controllers
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