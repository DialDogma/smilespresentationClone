using System.Web.Mvc;
using SmileClaimV2.Helper;

namespace SmileClaimV2.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        public ActionResult Index(string txt)
        {
            ViewBag.txt = txt;
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}