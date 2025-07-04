using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSTaxAllowance.Helper;
using SmileSTaxAllowance.Models;

namespace SmileSTaxAllowance.Controllers
{
    //[Authorization]
    //[Authorization]
    public class TaxCustomerController : Controller
    {
        // GET: TaxCustomer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaxCustomer()
        {
            return View();
        }

        public ActionResult TaxCustomerSearch()
        {
            ViewBag.show = "1";
            return View();
        }

        public ActionResult TaxCustomerConfirm(string zcardId, string birthDate)
        {
            //string zcardId,string birthDate

            //DeCode
            var zcardIdBase64EncodedBytes = Convert.FromBase64String(zcardId);
            var deCode_ZcardId = System.Text.Encoding.UTF8.GetString(zcardIdBase64EncodedBytes);

            var birthDateBase64EncodedBytes = Convert.FromBase64String(birthDate);
            var deCode_birthDate = System.Text.Encoding.UTF8.GetString(birthDateBase64EncodedBytes);

            ViewBag.CustZCardId = deCode_ZcardId;
            ViewBag.CustBirthDate = deCode_birthDate;

            return View();
        }

        public ActionResult TaxCustomerPreview(string custIDCard, string disclosureStatusId, string custFirstname, string custLastname)
        {
            var custIDCardBase64EncodeBytes = Convert.FromBase64String(custIDCard);
            var deCode_CustIdCard = System.Text.Encoding.UTF8.GetString(custIDCardBase64EncodeBytes);

            var disclosureStatusIDBase64EncodeBypes = Convert.FromBase64String(disclosureStatusId);
            var deCode_disclosureStatusId = System.Text.Encoding.UTF8.GetString(disclosureStatusIDBase64EncodeBypes);

            var custFirstnameBase64EncodeBypes = Convert.FromBase64String(custFirstname);
            var deCode_custFirstname = System.Text.Encoding.UTF8.GetString(custFirstnameBase64EncodeBypes);

            var custLastnameBase64EncodeBypes = Convert.FromBase64String(custLastname);
            var deCode_custLastname = System.Text.Encoding.UTF8.GetString(custLastnameBase64EncodeBypes);

            ViewBag.CustIdCard = deCode_CustIdCard;
            ViewBag.disclosureStatusID = deCode_disclosureStatusId;
            ViewBag.custFirstName = deCode_custFirstname;
            ViewBag.custLastName = deCode_custLastname;

            return View();
        }

        #region "Method"

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

        public ActionResult SaveTaxCustomerData(string applicationCode, string custFirstName, string custLastName, string custIDCard, string custBirthDate, string custMobile,
                                                string custEmail, string payerFirstName, string payerLastName, string payerIDCard, string payerBirthDate, string payerMobile,
                                                string payerEmail, string disclosureStatus)
        {
            DateTime? c_custBirthDate;
            DateTime? c_payerBirthDate;

            c_custBirthDate = GlobalFunction.ConvertDatePickerToDate_BE(custBirthDate);
            c_payerBirthDate = GlobalFunction.ConvertDatePickerToDate_BE(payerBirthDate);

            string[] rs = new string[3];

            try
            {
                using (var db = new TaxAllowanceEntities1())
                {
                    usp_CustomerLog_Insert_Result obj_tax = new usp_CustomerLog_Insert_Result();

                    obj_tax = db.usp_CustomerLog_Insert(applicationCode, null, custFirstName, custLastName, custIDCard, c_custBirthDate, custMobile, custEmail, null, payerFirstName,
                                                        payerLastName, payerIDCard, c_payerBirthDate, payerMobile, payerEmail, Convert.ToInt32(disclosureStatus)).Single();

                    rs[0] = obj_tax.IsResult.Value.ToString();
                    rs[1] = obj_tax.Result;
                    rs[2] = obj_tax.Msg;

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTaxCustomerData(string custIDCard, string custFirstName, string custLastName)
        {
            var db = new TaxAllowanceEntities1();

            int year = 2563;

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

        #endregion "Method"
    }
}