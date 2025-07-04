using System.Linq;
using System.Web.Mvc;
using StandardDevelopment.Helper;
using StandardDevelopment.Models;

namespace StandardDevelopment.Controllers
{
    [Authorization]
    public class HomeController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        public ActionResult RolesMenuPreview()
        {
            ViewBag.RoleList = MenuRoles.GetAllRoles().ToList();
            return View();
        }

        public JsonResult GetMenuPreview(string role)
        {
            var result = MenuRoles.GetMenuByRole(role).ToList();
            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}