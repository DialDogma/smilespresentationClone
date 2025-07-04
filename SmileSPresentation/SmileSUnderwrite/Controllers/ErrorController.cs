using System.Web.Mvc;

namespace SmileSUnderwrite.Controllers
{
    public class ErrorController:Controller
    {
        public ActionResult InternalServerError(string exception)
        {
            ViewBag.Exception = exception;
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}