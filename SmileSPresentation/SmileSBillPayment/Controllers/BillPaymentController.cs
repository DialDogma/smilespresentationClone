using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using RestSharp.Authenticators;
using SmileSBillPayment.Helper;
using SmileSBillPayment.Models;
using SmileSBillPayment.SSOService;

namespace SmileSBillPayment.Controllers
{
    [Authorization]
    public class BillPaymentController : Controller
    {
        #region View

        // GET: Report Bill Payment
        //[Authorization(Roles = "Billpayment_Callcenter,Billpayment_Premium,Billpayment_Refund,Developer")]
        public ActionResult ReportBillPayment()
        {
            using (var db = new SSSCashReceiveEntities())
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = new SSOServiceClient().GetRoleByUserName(user.UserName);
                var arrRole = roleListRaw.Split(',');
                if (arrRole.Contains("Developer") || arrRole.Contains("Billpayment_Premium") || arrRole.Contains("Billpayment_Callcenter"))
                {
                    ViewBag.BranchList = db.usp_Branch_Select(null, 0, 99999, null, null, null).ToList();
                }
                else
                {
                    ViewBag.BranchList = db.usp_Branch_Select(user.Branch_ID, 0, 99999, null, null, null).ToList();
                }

                ViewBag.URL_View_Doc = Properties.Settings.Default.URL_View_Document;

                ViewBag.BPStatus = db.usp_BillPaymentStatusForBillFollow_Select(null, null, null, null, null, null).ToList();
                return View();
            }
        }

        // GET: Generate Bill Payment
        [Authorization(Roles = "Billpayment_Callcenter,Billpayment_Premium,Developer")]
        public ActionResult GenBillPayment()
        {
            ViewBag.URL_View_Doc = Properties.Settings.Default.URL_View_Document;
            return View();
        }

        public ActionResult RefundPayment()
        {
            //get refund type
            using (var db = new SSSCashReceiveEntities())
            {
                var refundType = db.usp_RefundType_Select(null, null, null, 99, null, null, null).ToList();
                ViewBag.refundType = refundType;
            }

            return View();
        }

        public ActionResult BillPaymentRenew()
        {
            return View();
        }

        #endregion View

        #region Datatable

        /// <summary>
        /// Get Datatable Report Bill Payment
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBillPayment(string period, string billPaymentStatusIdList, int? payerBranchId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                if (period == "" || period == null)
                {
                    throw new ArgumentNullException();
                }

                try
                {
                    var BPStatusIdList = billPaymentStatusIdList == "-1" ? null : billPaymentStatusIdList;
                    var branchId = payerBranchId == -1 ? null : payerBranchId;
                    var nPeriod = Convert.ToDateTime(period);

                    var lst = db.usp_BillPaymentForBillFollow_Select(null,
                                                            nPeriod,
                                                            branchId,
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            BPStatusIdList,
                                                            null,
                                                            indexStart,
                                                            pageSize,
                                                            sortField,
                                                            orderType,
                                                            search).ToList();

                    var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        /// <summary>
        /// for History Gen Bill
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBillPaymentHistory(string mobileNumber, string payerName, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                try
                {
                    var lst = db.usp_BillPaymentHistory_Select(mobileNumber.Trim(),
                                                        payerName.Trim(),
                                                        indexStart,
                                                        pageSize,
                                                        sortField,
                                                        orderType,
                                                        search).ToList();

                    var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        /// <summary>
        /// get datatable refund
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRefund_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            //call function get dt
            try
            {
                using (var db = new SSSCashReceiveEntities())
                {
                    var result = db.usp_ReserveAccountShort_Select(null, null, null, indexStart, pageSize, sortField, orderType, search).ToList();
                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };

                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// get debt for regen
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDebtForRegenBillPayment(string period, int billId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                DateTime debtPeriod = Convert.ToDateTime(period);
                try
                {
                    var lst = db.usp_DebtForRegenBillPayment_Select_V2(debtPeriod,
                                                                       billId,
                                                                       indexStart,
                                                                       pageSize,
                                                                       null,
                                                                       null,
                                                                       search).ToList();

                    var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        /// <summary>
        /// app renew ต่ออายุรายปี
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="draw"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApplicationDetailForGenerateBillPeriodTypeYear(string appId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                try
                {
                    var lst = db.usp_ApplicationDetailForGenerateBillPeriodTypeYear_Select(appId, indexStart, pageSize, sortField, orderType, search).ToList();

                    var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        #endregion Datatable

        #region Method

        [HttpGet]
        public async Task<ActionResult> ExportReportBillPayment(string period, int? payerBranchId, string billPaymentStatusIdList, string search)
        {
            await Task.Yield();
            using (var db = new SSSCashReceiveEntities())
            {
                if (period == "" || period == null)
                {
                    throw new ArgumentNullException();
                }

                try
                {
                    var BPStatusIdList = billPaymentStatusIdList == "-1" ? null : billPaymentStatusIdList;
                    var branchId = payerBranchId == -1 ? null : payerBranchId;
                    var nPeriod = Convert.ToDateTime(period);

                    var lst = db.usp_ReportBillPaymentForBillFollow_Select(null,
                                                        nPeriod,
                                                        branchId,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        BPStatusIdList,
                                                        null,
                                                        0,
                                                        999999999,
                                                        null,
                                                        null,
                                                        search.Trim()).ToList();

                    var stream = new MemoryStream();

                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("ReportBillPayment");
                        workSheet1.Cells.LoadFromCollection(lst, true);

                        // Select only the header cells
                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                        // Set their text to bold.
                        headerCells1.Style.Font.Bold = true;
                        // Set their background color
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                        // Apply the auto-filter to all the columns
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                        allCells1.AutoFilter = true;
                        // Auto-fit all the columns
                        allCells1.AutoFitColumns();

                        package.Save();

                        stream.Position = 0;

                        //get new GUID
                        var dataSessionKey = Guid.NewGuid().ToString();
                        //tempData GUID
                        TempData["keyData"] = dataSessionKey;
                        //Session Data
                        Session[dataSessionKey] = package.GetAsByteArray();

                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadExportReportBillPayment()
        {
            var dataKey = TempData["keyData"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-BillPayment-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Insert Regenerate BillPayment And SentSMS
        /// </summary>
        /// <param name="lstDebtId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegenBillPaymentAnSentSMS(string[] strArray)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                var UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
                var strListDebId = "";

                if (strArray != null)
                {
                    strListDebId = string.Join(",", strArray);
                }

                var result = db.usp_RegenBillPaymentAnSentSMS_Insert(strListDebId, UserDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BillPaymentYearAndSentSMS(string appId, string mobileNumber, string period)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);

                DateTime? nPeriod = null;

                try
                {
                    //Convert String To DateTime
                    nPeriod = Convert.ToDateTime(period);
                }
                catch (Exception e)
                {
                    return Json(new { IsResult = false, Msg = String.Format("{0} : {1}", e.Message, e.InnerException) }, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    //Insert Data
                    var resultBillInsert = db.usp_BillPaymentYearAndSentSMS_Insert(appId, mobileNumber.Trim(), nPeriod, user.User_ID).ToList().SingleOrDefault();

                    //IsSuccess == true
                    if (resultBillInsert.IsResult.Value)
                    {
                        var smsInstandId = Convert.ToInt32(resultBillInsert.Result);

                        var resultBillSelect = db.usp_BillPaymentSMSInstant_Select(smsInstandId, 0, 9999, null, null, null).ToList().SingleOrDefault();

                        //TODO : รอขึ้นตัวจริง
                        ////Call Service Send SMS
                        //var client = new RestClient("http://operation.siamsmile.co.th:9215/api/sms/SendSMSV2");

                        ////assign value to object params
                        //// var param = new { SMSTypeId = 1, Message = "Mewnich BNK48", PhoneNo = "0922742125", CreatedById = 3522 };

                        //var param = new { SMSTypeId = resultBillSelect.SMSTypeId, Message = resultBillSelect.SMSMessage, PhoneNo = resultBillSelect.SMSMobileNumber, CreatedById = user.User_ID };

                        ////Add Json Body to Request
                        //var request = new RestRequest().AddJsonBody(param);

                        ////Add Header Token
                        //request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");

                        ////Post Request
                        //var response = client.Post(request);

                        //if (response.IsSuccessful)
                        //{
                        //    var smsResult = JsonConvert.DeserializeObject<SMSResult>(response.Content);
                        //    //update success
                        //    db.usp_BillPaymentSMSInstant_Update(smsInstandId, smsResult.Transaction, response.Content);
                        //    return Json(new { IsResult = response.IsSuccessful, SMSStatus = smsResult.Status, UrlBill = resultBillSelect.UrlBillPayment, Msg = String.Format("ResponseStatus {0};SMS Status {1};Detail {2}", response.ResponseStatus, smsResult.Status, smsResult.Detail) }, JsonRequestBehavior.AllowGet);
                        //}
                        //else
                        //{
                        //    //update failed
                        //    db.usp_BillPaymentSMSInstant_Update(smsInstandId, "-1", response.Content);
                        //    return Json(new { IsResult = response.IsSuccessful, Msg = String.Format("ResponseStatus {0};StatusCode {1};Description {2}", response.ResponseStatus, response.StatusCode, response.StatusDescription) }, JsonRequestBehavior.AllowGet);
                        //}

                        //ตัวทดสอบ ให้ส่งผ่านตลอด
                        if (true)
                        {
                            //update success
                            db.usp_BillPaymentSMSInstant_Update(smsInstandId, null, String.Format("TEST BY {0}", user.User_ID));
                            return Json(new { IsResult = true, SMSStatus = "000", UrlBill = resultBillSelect.UrlBillPayment, Msg = String.Format("ResponseStatus {0};SMS Status {1};Detail {2}", "ทดสอบ OK", "000", "SMS OK") }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //Failed return detail
                        return Json(resultBillInsert, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { IsResult = false, Msg = String.Format("{0} : {1}", e.Message, e.InnerException) }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion Method
    }
}