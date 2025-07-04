using System.Web.Mvc;
using SmileSDataExport.Helper;

namespace SmileSDataExport.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            ViewBag.UserDetail = user;
            return View();
        }

        [HttpGet]
        [Authorization(Roles = "Developer,DataExport_Financial")]
        public ActionResult ReportClaimPH()
        {
            return View();
        }

        [HttpGet]
        [Authorization(Roles = "Developer,DataExport_Financial")]
        public ActionResult ReportClaimPA()
        {
            return View();
        }
    }
}