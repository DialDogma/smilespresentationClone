using System.Web.Mvc;
using SmileSPaymentGateway.Helper;

namespace SmileSPaymentGateway.Controllers
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