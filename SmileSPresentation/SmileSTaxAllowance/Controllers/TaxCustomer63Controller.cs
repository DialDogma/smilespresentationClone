using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSTaxAllowance.Helper;
using SmileSTaxAllowance.Models;

namespace SmileSTaxAllowance.Controllers
{
    //[Route("[controller]")]
    public class TaxCustomer63Controller : Controller
    {
        // GET: TaxCustomer63
        public ActionResult Index()
        {
            return View();
        }

        //[Route("TaxCustomer")]
        public ActionResult TaxCustomerSearch()
        {
            return View();
        }

        public ActionResult TaxCustomerPreview(string custIDCard, string disclosureStatusId, string custFirstname, string custLastname)
        {
            var custIDCardBase64EncodeBytes = Convert.FromBase64String(custIDCard);
            var deCode_CustIdCard = System.Text.Encoding.UTF8.GetString(custIDCardBase64EncodeBytes);

            var custFirstnameBase64EncodeBypes = Convert.FromBase64String(custFirstname);
            var deCode_custFirstname = System.Text.Encoding.UTF8.GetString(custFirstnameBase64EncodeBypes);

            var custLastnameBase64EncodeBypes = Convert.FromBase64String(custLastname);
            var deCode_custLastname = System.Text.Encoding.UTF8.GetString(custLastnameBase64EncodeBypes);

            ViewBag.CustIdCard = deCode_CustIdCard;
            ViewBag.custFirstName = deCode_custFirstname;
            ViewBag.custLastName = deCode_custLastname;

            return View();
        }

        public ActionResult GetCustomerDetailByZcardId(string zcardId, string birthDate)
        {
            DateTime? dBirthDate;

            string ZcardCustomerID;

            var db = new TaxAllowanceEntities1();

            ZcardCustomerID = zcardId.Replace("-", "");
            dBirthDate = GlobalFunction.ConvertDatePickerToDate_BE(birthDate);

            int c;

            c = db.usp_GetCustomerDetailLatest(ZcardCustomerID, dBirthDate).Count();

            if (c > 0)
            {
                try
                {
                    usp_GetCustomerDetailLatest_Result lst = new usp_GetCustomerDetailLatest_Result();

                    lst = db.usp_GetCustomerDetailLatest(ZcardCustomerID, dBirthDate).Single();

                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //ViewBag.show = "0";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTaxCustomerData(string custIDCard, string custFirstName, string custLastName)
        {
            var db = new TaxAllowanceEntities1();

            int year = 2564;

            var c = db.usp_TaxAllowance_Select(custIDCard, custFirstName, custLastName, year, 0, 99999, null, null, null).Count();

            if (c > 0)
            {
                List<usp_TaxAllowance_Select_Result> lst = new List<usp_TaxAllowance_Select_Result>();

                lst = db.usp_TaxAllowance_Select(custIDCard, custFirstName, custLastName, year, 0, 99999, null, null, null).ToList();

                //db.Dispose();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}