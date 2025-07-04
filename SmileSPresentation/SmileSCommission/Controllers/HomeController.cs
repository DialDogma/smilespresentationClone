using System.Web.Mvc;
using SmileSCommission.Helper;

namespace SmileSCommission.Controllers
{
    [Authorization]
    public class HomeController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}