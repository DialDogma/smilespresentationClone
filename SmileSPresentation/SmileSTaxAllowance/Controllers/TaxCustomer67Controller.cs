using SmileSTaxAllowance.Helper;
using SmileSTaxAllowance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSTaxAllowance.Controllers
{
    public class TaxCustomer67Controller : Controller
    {
        private TaxAllowanceEntities1 _db;
        public TaxCustomer67Controller()
        {
            _db = new TaxAllowanceEntities1();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        public ActionResult TaxCustomerSearch()
        {
            return View();
        }
        public ActionResult TaxCustomerPreview(string custIDCard = null, string disclosureStatusId = null, string custFirstname = null, string custLastname = null, string custGuid = null)
        {
            if (custGuid == null)
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
            }
            else
            {
                var custDatabyGuid = getCustomerDetailByGuid(custGuid);

                ViewBag.CustIdCard = custDatabyGuid.CustIdCard;
                ViewBag.custFirstName = custDatabyGuid.custFirstName;
                ViewBag.custLastName = custDatabyGuid.custLastName;
            }

            return View();

        }

        public ActionResult GetCustomerDetailByZcardId(string zcardId, string birthDate)
        {
            DateTime? dBirthDate;

            string ZcardCustomerID;


            ZcardCustomerID = zcardId.Replace("-", "");
            dBirthDate = GlobalFunction.ConvertDatePickerToDate_BE(birthDate);

            int c;

            c = _db.usp_GetCustomerDetailLatest(ZcardCustomerID, dBirthDate).Count();

            if (c > 0)
            {
                try
                {
                    usp_GetCustomerDetailLatest_Result lst = new usp_GetCustomerDetailLatest_Result();

                    lst = _db.usp_GetCustomerDetailLatest(ZcardCustomerID, dBirthDate).Single();

                    int year = 2568;

                    var tax = _db.usp_TaxAllowance_Select(lst.CustIDCard, lst.CustFirstName, lst.CustLastName, year, 0, 99999, null, null, null).Count();

                    if (tax > 0)
                    {
                        return Json(lst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTaxCustomerData(string custIDCard, string custFirstName, string custLastName)
        {

            int year = 2568;

            var c = _db.usp_TaxAllowance_Select(custIDCard, custFirstName, custLastName, year, 0, 99999, null, null, null).Count();

            if (c > 0)
            {
                List<usp_TaxAllowance_Select_Result> lst = new List<usp_TaxAllowance_Select_Result>();

                lst = _db.usp_TaxAllowance_Select(custIDCard, custFirstName, custLastName, year, 0, 99999, null, null, null).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private class dataCustomer
        {
            public string CustIdCard { get; set; }
            public string custFirstName { get; set; }
            public string custLastName { get; set; }
        }

        private dataCustomer getCustomerDetailByGuid(string custGuid)
        {

            try
            {
                usp_GetCustomerDetailLatest_Result lst = new usp_GetCustomerDetailLatest_Result();
                var dateFormat = "10/02/2509";
                var dBirthDate = GlobalFunction.ConvertDatePickerToDate_BE(dateFormat);
                lst = _db.usp_GetCustomerDetailLatest("3320900819121", dBirthDate).Single();

                int year = 2568;

                var tax = _db.usp_TaxAllowance_Select(lst.CustIDCard, lst.CustFirstName, lst.CustLastName, year, 0, 99999, null, null, null).Count();



                if (tax > 0)
                {
                    var custFirstnameBase64EncodeBypes = System.Text.Encoding.UTF8.GetBytes(lst.CustFirstName);
                    var deCode_custFirstname = System.Text.Encoding.UTF8.GetString(custFirstnameBase64EncodeBypes);

                    var custLastnameBase64EncodeBypes = System.Text.Encoding.UTF8.GetBytes(lst.CustLastName);
                    var deCode_custLastname = System.Text.Encoding.UTF8.GetString(custLastnameBase64EncodeBypes);
                    var list = new dataCustomer
                    {
                        CustIdCard = lst.CustIDCard,
                        custFirstName = deCode_custFirstname,
                        custLastName = deCode_custLastname
                    };
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}