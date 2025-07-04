using System.Web.Mvc;

namespace SmileSCommissionDataImport.Controllers
{
    public class ErrorController:Controller
    {
        [HttpGet]
        public ActionResult InternalServerError(string errorMassage)
        {
            ViewBag.errorMassege = errorMassage;
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}