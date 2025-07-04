using System.Web.Mvc;

namespace RoadsideAssistance.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult InternalServerError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BadReques(string ErrMsg)
        {
            ViewBag.Msg = ErrMsg;
            return View();
        }
    }
}
