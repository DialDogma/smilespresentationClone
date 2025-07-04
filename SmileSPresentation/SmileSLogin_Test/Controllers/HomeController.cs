using System.Web.Mvc;
using SmileSLogin_Test.Helper;
using SmileSLogin_Test.Properties;

namespace SmileSLogin_Test.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

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