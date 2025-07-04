using System.Web.Mvc;

namespace SmileSCustomerBase.Controllers
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