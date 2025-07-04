using System.Web.Mvc;

namespace SmileSPettyCash.Controllers
{
    public class ErrorController:Controller
    {
        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}
