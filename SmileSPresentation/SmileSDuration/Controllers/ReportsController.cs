using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSDuration.ComunicateService;
using SmileSDuration.Helper;
using SmileSDuration.Models;

namespace SmileSDuration.Controllers
{
    [Authorization]
    public class ReportsController : Controller
    {
        #region Declare

        //**start set context
        private readonly DurationV1Entities _context;

        private readonly CommunicateV1Entities1 _contextCommunicate;

        public ReportsController()
        {
            _context = new DurationV1Entities();
            _contextCommunicate = new CommunicateV1Entities1();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _contextCommunicate.Dispose();
        }

        //**end set context

        #endregion Declare

        #region Action

        [Authorization(Roles = "Developer")]
        public ActionResult Index()
        {
            ViewBag.Amount = _contextCommunicate.usp_TransactionNotGetDeliveryNotify_Count_Select().FirstOrDefault();
            ViewBag.TransactionStatus = _contextCommunicate.usp_TransactionStatus_Select().ToList();
            ViewBag.TransactionDetailStatus = _contextCommunicate.usp_TransactionDetailStatus_Select().ToList();
            ViewBag.SMSType = _contextCommunicate.usp_SMSType_Select().ToList();
            return View();
        }

        [Authorization(Roles = "Developer,SmileDuration_Admin,SmileDuration_Operation")]
        public ActionResult ReportNew()
        {
            ViewBag.ComunicateType = _context.usp_ComunicateType_Select(null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer")]
        [HttpPost]
        public ActionResult ReportSMS(FormCollection form)
        {
            var search = form["input_textsearch"];
            var dateStart = form["date_start"];
            var dateEnd = form["date_end"];
            var smsType = Convert.ToInt32(form["select_type"]);
            var trStatusId = Convert.ToInt32(form["select_trStatus"]);
            var trStatusDetailId = Convert.ToInt32(form["select_trStatusDetail"]);
            var dtStart = GlobalFunction.ConvertDatePickerToDate_BE(dateStart);
            var dtEnd = GlobalFunction.ConvertDatePickerToDate_BE(dateEnd);
            var list = _context.usp_Comunicate_Select(0,
                                                        1000000,
                                                        "",
                                                        "",
                                                        search,
                                                        dtStart,
                                                        dtEnd,
                                                        smsType,
                                                        trStatusId,
                                                        trStatusDetailId).ToList();

            //Export To Excel
            ExcelManager.ExportToExcel(this.HttpContext, list, "DurationSMSList", "DurationSMSReport");

            return RedirectToAction("Index", "Reports");
        }

        #endregion Action

        #region GetData

        [Authorization(Roles = "Developer")]
        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, int smsType, int trStatusId, int trStatusDetailId, string dateStart, string dateEnd)
        {
            var dtStart = GlobalFunction.ConvertDatePickerToDate_BE(dateStart);
            var dtEnd = GlobalFunction.ConvertDatePickerToDate_BE(dateEnd);
            var list = _context.usp_Comunicate_Select(pageStart,
                                                        pageSize,
                                                        sortField,
                                                        orderType,
                                                        search,
                                                        dtStart,
                                                        dtEnd,
                                                        smsType,
                                                        trStatusId,
                                                        trStatusDetailId).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer,SmileDuration_Admin,SmileDuration_Operation")]
        public JsonResult GetDatatable_ReportNew(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, int smsType, string dateStart, string dateEnd)
        {
            //var dtStart = GlobalFunction.ConvertDatePickerToDate_BE(dateStart);
            //var dtEnd = GlobalFunction.ConvertDatePickerToDate_BE(dateEnd);
            var dtStart = Convert.ToDateTime(dateStart);
            var dtEnd = Convert.ToDateTime(dateEnd);

            var list = _context.usp_Comunicate_Select_V3(pageStart,
                                                            pageSize,
                                                            sortField,
                                                            orderType,
                                                            search,
                                                            dtStart,
                                                            dtEnd,
                                                            smsType).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer,SmileDuration_Admin,SmileDuration_Operation")]
        [HttpPost]
        public ActionResult ReportSMS_New(FormCollection form)
        {
            var search = form["input_textsearch"];
            var dateStart = form["date_start"];
            var dateEnd = form["date_end"];
            var smsType = Convert.ToInt32(form["select_type"]);
            var strName = ("From_" + dateStart + "_To_" + dateEnd).Replace("/", "");
            var dtStart = Convert.ToDateTime(dateStart);
            var dtEnd = Convert.ToDateTime(dateEnd);

            var list = _context.usp_Comunicate_Select_V3(0,
                                                         10000000,
                                                         "",
                                                         "",
                                                         search,
                                                         dtStart,
                                                         dtEnd,
                                                         smsType).Select(o => new
                                                         {
                                                             AppId = o.AppID,
                                                             เบอร์โทรศัพท์ = o.MobilePhoneNo,
                                                             ผู้เอาประกัน = o.CustomerName,
                                                             ผู้ชำระเบี้ย = o.PayerName,
                                                             ประเภทข้อความ = o.ComunicateTypeName,
                                                             ข้อความ = o.Message,
                                                             ViewDocumentCount = o.ViewCount,
                                                             FirstView = o.FirstView,
                                                             LatestView = o.LatestView,
                                                             TransactionID = o.TransactionID,
                                                             TransactionStatusID = o.TransactionStatusID,
                                                             TransactionStatusDetail = o.TransactionStatusDetail,
                                                             Success = o.Success,
                                                             Fail = o.Fail,
                                                             Block = o.Block,
                                                             Expired = o.Expired,
                                                             Processing = o.Processing,
                                                             Unknown = o.Unknown,
                                                             Credit = o.Credit,
                                                         }).ToList();

            //Export To Excel
            ExcelManager.ExportToExcel(this.HttpContext, list, "Report", "DurationSMSReport_" + strName);

            return RedirectToAction("ReportNew", "Reports");
        }

        [Authorization(Roles = "Developer")]
        public JsonResult GetResult()
        {
            //Get Result Detail SMS
            using (var smsService = new SmsServiceClient())
            {
                //Call Service
                var result = smsService.GetTransactionNotDeliveryNotify();

                //return Json
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion GetData

        // GET: Reports
    }
}