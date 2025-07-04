using System.Collections.Generic;
using System.Web.Mvc;

using SmileSCommissionDataImport.Helper;
using SmileSCommissionDataImport.Models;

namespace SmileSCommissionDataImport.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public HomeController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Declare Variables

        [HttpGet]
        public ActionResult Index()
        {
            // check if have latest period
            var periodResult = GlobalFunction.GetLatestPeriod();
            if (!periodResult.IsResult)
            {
                //return error page
                return RedirectToAction("InternalServerError", "Error", new { errorMassge = "ไม่พบงวดค่าตอบแทนล่าสุด กรุณาตั้งงวดค่าตอบแทนรอบใหม่" });
            }

            return View();
        }
    }
}