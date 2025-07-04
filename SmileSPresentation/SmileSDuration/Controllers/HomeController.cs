using System.Web.Mvc;

namespace SmileSDuration.Controllers
{
    [Helper.Authorization(Roles = "Developer")]
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