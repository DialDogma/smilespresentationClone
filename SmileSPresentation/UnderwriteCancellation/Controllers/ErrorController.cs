using System.Web.Mvc;

namespace UnderwriteCancellation.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult InternalServerError(string Msg = null)
        {
            ViewBag.MsgInternalServerError = Msg;
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BadRequest(string Msg = null)
        {
            ViewBag.MsgBadRequest = Msg;

            return View();
        }
    }
}