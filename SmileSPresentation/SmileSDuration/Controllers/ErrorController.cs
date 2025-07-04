using System.Web.Mvc;

namespace SmileSDuration.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult FileFormatError(string header, string msg)
        {
            ViewBag.MSG = msg;
            ViewBag.Header = header;
            return View();
        }
    }
}