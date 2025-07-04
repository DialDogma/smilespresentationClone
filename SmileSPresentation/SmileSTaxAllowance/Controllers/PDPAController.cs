using SmileSTaxAllowance.Helper;
using SmileSTaxAllowance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSTaxAllowance.Controllers
{
    public class PDPAController : Controller
    {
        private TaxAllowanceEntities1 _db;

        // GET: InsuranceManagement
        public PDPAController()
        {
            _db = new TaxAllowanceEntities1();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: PDPA
        public ActionResult Index()
        {
            return View();
        }

        [Route("PDPA/confirm/{Ref}")]
        public ActionResult PDPAConfirmData(string Ref)
        {
            //DeCode
            var referenceBase64EncodedBytes = Convert.FromBase64String(Ref);
            var deCode_Reference = System.Text.Encoding.UTF8.GetString(referenceBase64EncodedBytes);

            ViewBag.Reference = deCode_Reference;

            return View();
        }

        [Route("PDPA/Detail/{Ref}")]
        public ActionResult PDPADetail(string Ref)
        {
            var referenceBase64EncodedBytes = Convert.FromBase64String(Ref);
            var deCode_Reference = System.Text.Encoding.UTF8.GetString(referenceBase64EncodedBytes);

            ViewBag.Reference = deCode_Reference;
            return View();
        }

        public JsonResult GetCustomerData(string reference)
        {
            var ip = GlobalFunction.GetIPAddress();
            var rs = _db.usp_CustomerPDPA_Select(reference, ip).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConfirmData(string reference, string verifyIdCard)
        {
            var ip = GlobalFunction.GetIPAddress();

            var rs = _db.usp_CustomerPDPA_Verify(reference, verifyIdCard, ip).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}