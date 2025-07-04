using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMiscellaneous.Helper;
using SmileSMiscellaneous.Models;

namespace SmileSMiscellaneous.Controllers
{
    public class ReportsController : Controller
    {
        private MiscellaneousDBContext _db;

        public ReportsController()
        {
            _db = new MiscellaneousDBContext();
        }

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendNormalCoverage()
        {
            ViewBag.ProductType = _db.usp_ProductTypeNotSmilePA_Select(0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult SendSmilePACoverage()
        {
            return View();
        }

        public ActionResult GetReportHeaderInsurance(string dateFrom, string dateTo, int? draw = null, int? pageSize = null
                                                    , int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            DateTime? d_dateFrom;
            DateTime? d_dateTo;

            d_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
            d_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);

            var result = _db.usp_ReportHeader_Select(d_dateFrom, d_dateTo, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetApplicationSendInsured(int? applicationId = null, int? isSendInsured = null, int? insuranceCompanyId = null, int? productTypeId = null
                                                      , int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            bool IsSend = false;

            if (isSendInsured == 1)
            {
                IsSend = true;
            }

            var result = _db.usp_ApplicationMonitorSendInsured_Select(applicationId, IsSend, insuranceCompanyId == -1 ? null : insuranceCompanyId, productTypeId == -1 ? null : productTypeId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateReportHeader()
        {
            int createdUserId;
            createdUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_ReportGenerate_Insert(createdUserId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStatusSmilePACoverage(int reportHeaderId, bool isSendInsured, string remark = null)
        {
            int createdUserId;
            createdUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_ReportSendInsurance_Update(reportHeaderId, isSendInsured, remark, createdUserId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStatusNormalCoverage(int appId, bool isSend, string remark = null)
        {
            int createdUserId;
            createdUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_Application_SendInsured_Update(appId, isSend, remark, createdUserId).Single();

            if (rs.IsResult == true)
            {
                InsertTransaction(appId, 84);
            }

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public usp_ApplicationTransaction_Insert_Result InsertTransaction(int applicationId, int transactionTypeId)
        {
            string jsonOutput;
            int createUserId;

            createUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            jsonOutput = new JsonLog().GetJsonOutputByApplicationId(applicationId);

            var result = _db.usp_ApplicationTransaction_Insert(applicationId, transactionTypeId, createUserId, jsonOutput).Single();

            return result;
        }

        public void ExportReportHeaderById(int reportHeaderId)
        {
            var data = _db.usp_ReportDetail_SelectById(reportHeaderId).ToList();

            var dt = GlobalFunction.ToDataTable(data);
            ExcelManager.ExportToExcel(this.HttpContext, "รายงานความคุ้มครองSmilePA", dt, "Detail");
        }
    }
}