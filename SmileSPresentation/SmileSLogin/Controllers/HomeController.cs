using SmileSLogin.Helper;
using SmileSLogin.Services;
using System.Web.Mvc;

namespace SmileSLogin.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.EmpFullName = GlobalFunction.GetLoginDetail(this.HttpContext).FullName;
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}