using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult InternalServerError(string Msg)
        {
            if (!string.IsNullOrEmpty(Msg))
            {
                ViewBag.MsgInternalServerError = Msg;
            }
            else
            {
                ViewBag.MsgInternalServerError = null;
            }
            return View();
        }

        [HttpGet]
        public ActionResult NotFound(string Msg)
        {
            ViewBag.MsgBadRequest = Msg;
            return View();
        }

        [HttpGet]
        public ActionResult BadRequest(string Msg)
        {
            ViewBag.MsgBadRequest = Msg;

            return View();
        }
    }
}