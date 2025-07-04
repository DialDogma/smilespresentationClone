using System.Web.Mvc;

namespace SmileSCommunicate.Controllers
{
    [Helper.Authorization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}