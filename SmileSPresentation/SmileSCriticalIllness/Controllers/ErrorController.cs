using System.Web.Mvc;

namespace SmileSCriticalIllness.Controllers
{
    public class ErrorController:Controller
    {
        [HttpGet]
        public ActionResult InternalServerError(string errorText)
        {
            ViewBag.errorText = errorText;
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}