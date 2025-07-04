using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSCriticalIllness.Helper;
using SmileSCriticalIllness.Models;

namespace SmileSCriticalIllness.Controllers
{
    [Authorization]
    public class ConfirmPolicyController : Controller
    {
        #region Context

        private readonly CriticalIllnessEntities _context;

        public ConfirmPolicyController()
        {
            _context = new CriticalIllnessEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        // GET: ConfirmPolicy
        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult ConfirmPolicy()
        {
            var dcrDate = _context.usp_GetNextDcrForCreatedNewApp_Select().FirstOrDefault();
            ViewBag.dcrDate = Convert.ToDateTime(dcrDate).AddYears(-543).ToString("MM/dd/yyyy");
            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult CancelFromInsurance()
        {
            var dcrDate = _context.usp_GetNextDcrForCreatedNewApp_Select().FirstOrDefault();
            ViewBag.dcrDate = Convert.ToDateTime(dcrDate).AddYears(-543).ToString("MM/dd/yyyy");
            return View();
        }

        #endregion View

        #region api

        [HttpPost]
        public JsonResult SendPolicyToTNI(string startCoverDate)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var startCoverDateConvert = DateTime.ParseExact(startCoverDate, "dd-MM-yyyy", new CultureInfo("en-Us"));

            //call application list status = 3 and startcoverdate
            var applicationList = _context.usp_ApplicationDetail_Select(null, startCoverDateConvert, null, null, "8", null, null,
                null, null, null, 99999, null, null, null).ToList();

            //list IsCallSuccess == true
            var validateAppResultList = new List<usp_IssuePolicy_validate_Result>();
            //list IsCallSuccess == false
            var notPassValidateAppResultList = new List<usp_IssuePolicy_validate_Result>();

            try
            {
                //loop validate appcode and underwrite status "ตรวจเอกสารผ่าน"
                foreach (var itm in applicationList)
                {
                    var validateResult = _context.usp_IssuePolicy_validate(itm.ApplicationId).FirstOrDefault();

                    if (validateResult.IsCallSuccess == true)
                    {
                        validateAppResultList.Add(validateResult);
                    }
                    else
                    {
                        notPassValidateAppResultList.Add(validateResult);
                    }
                }

                //call detail application that pass
                var applicationDetailResultList = new List<usp_RequestPolicy_Select_Result>();
                foreach (var itm in validateAppResultList)
                {
                    var applicationDetailResult = _context.usp_RequestPolicy_Select(itm.ApplicationId, null, null, null, 1, null, null, null);
                    applicationDetailResultList.AddRange(applicationDetailResult);
                }
                //validate with helper (web service) application that pass validate
                var requestPolicyList = new List<TNIWebServiceResult>();

                foreach (var itm in applicationDetailResultList)
                {
                    var appCode = GenCode();

                    var requestPolicy = new cTNIWebService().IssuePolicy(
                        itm.ApplicationId,
                        appCode,
                        Convert.ToDateTime(itm.app_date),
                        itm.cust_prefix,
                        itm.Cust_name,
                        itm.Cust_surname,
                        itm.Cust_IDCard,
                        itm.Cust_sex,
                        Convert.ToDateTime(itm.cust_birthdate),
                        itm.Cust_occupation,
                        itm.Cust_email,
                        itm.addr_poladdr,
                        itm.addr_polbld,
                        itm.addr_polmoo,
                        itm.addr_polsoi,
                        itm.addr_polroad,
                        itm.addr_district,
                        itm.addr_amphur,
                        itm.addr_province,
                        itm.addr_postcode,
                        itm.addr_tel,
                        itm.send_prefix,
                        itm.send_name,
                        itm.send_surname,
                        itm.send_addr1,
                        itm.send_addr2,
                        itm.send_district,
                        itm.send_amphur,
                        itm.send_province,
                        itm.send_postcode,
                        itm.send_tel,
                        Convert.ToDouble(itm.prem_total),
                        Convert.ToDateTime(itm.paid_date),
                        itm.payer_fullname,
                        Convert.ToDateTime(itm.receipt_date),
                        itm.payer_IDCard,
                        itm.product_code,
                        itm.package_code,
                        Convert.ToDateTime(itm.effect_date),
                        Convert.ToDateTime(itm.expire_date),
                        Convert.ToSingle(itm.sumins),
                        Convert.ToSingle(itm.net),
                        Convert.ToSingle(itm.stamp),
                        Convert.ToSingle(itm.vat),
                        Convert.ToSingle(itm.total),
                        itm.inscus_bene01_prefix,
                        itm.inscus_bene01_name,
                        itm.inscus_bene01_surname,
                        itm.inscus_bene01_relation,
                        Convert.ToString(itm.inscus_bene01_rate),
                        itm.inscus_bene02_prefix,
                        itm.inscus_bene02_name,
                        itm.inscus_bene02_surname,
                        itm.inscus_bene02_relation,

                        Convert.ToString(itm.inscus_bene02_rate)
                        );

                    requestPolicyList.Add(requestPolicy);
                }

                //declare log result
                var logAddResultList = new List<usp_WebServiceLog_Insert_Result>();

                //add not pass result to log
                if (notPassValidateAppResultList.Count > 0)
                {
                    foreach (var itm in notPassValidateAppResultList)
                    {
                        var logAddResult = _context.usp_WebServiceLog_Insert(itm.ApplicationId, itm.IsCallSuccess, itm.ErrorText,
                            itm.InvokeDateTime, itm.Message_Code, itm.Message_Desc, itm.PolicyNo, itm.AppNo, itm.FullResultReturn, null, userId);
                        logAddResultList.AddRange(logAddResult);
                    }
                }

                //add pass result to log
                if (requestPolicyList.Count > 0)
                {
                    foreach (var itm in requestPolicyList)
                    {
                        var logAddResult = _context.usp_WebServiceLog_Insert(itm.ApplicationId, itm.IsCallSuccess, itm.ErrorText,
                            itm.InvokeDateTime, itm.Message_Code, itm.Message_Desc, itm.PolicyNo, itm.AppNo, itm.FullResultReturn, itm.JsonOut, userId);
                        logAddResultList.AddRange(logAddResult);
                    }
                }

                return Json(new { IsResult = true, Msg = "นำส่งApplicationสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = "เกิดข้อผิดพลาด:" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public void ExportExcel(string startCoverDate)
        {
            var base64EncodedBytes = Convert.FromBase64String(startCoverDate);

            startCoverDate = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            var startCoverDateConvert = DateTime.ParseExact(startCoverDate, "dd-MM-yyyy", new CultureInfo("en-Us"));

            var result = _context.usp_RequestPolicy_Select(null, startCoverDateConvert, "8", null, 99999, null, null, null).ToList();

            GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานส่งความคุ้มครอง");
        }

        [HttpGet]
        public void ExportExcelRenew(string period)
        {
            var base64EncodedBytes = Convert.FromBase64String(period);

            period = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            var periodConvert = DateTime.ParseExact(period, "dd-MM-yyyy", new CultureInfo("en-Us"));

            var result = _context.usp_ApplicationReNew_Select(periodConvert, 0, 99999, null, null, null).ToList();

            GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานส่งความคุ้มครอง - ต่ออายุ");
        }

        [HttpPost]
        public JsonResult CancelBDCR(int applicationId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_CancelBeforeDcr_Update(applicationId, userId).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion api

        #region func

        public string GenCode()
        {
            var returnValue = new ObjectParameter("Result", typeof(string));
            _context.usp_GenerateCode("WSTNI", 8, returnValue);
            return (string)returnValue.Value;
        }

        #endregion func
    }
}