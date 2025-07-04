using System.Web.Mvc;

namespace SmileSBillPayment.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult InternalServerError()
        {
            ViewBag.errorText = TempData["errormsg"].ToString();
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}