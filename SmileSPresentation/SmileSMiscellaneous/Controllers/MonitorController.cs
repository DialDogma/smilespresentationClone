using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMiscellaneous.Models;
using SmileSMiscellaneous.Helper;

namespace SmileSMiscellaneous.Controllers
{
    [Authorization]
    public class MonitorController : Controller
    {
        private MiscellaneousDBContext _db;

        public MonitorController()
        {
            _db = new MiscellaneousDBContext();
        }

        // GET: Monitor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgentMonitor()
        {
            ViewBag.Type = _db.usp_ProductType_Select(0, 999999, null, null, null).ToList();
            return View();
        }

        public ActionResult ApplicationSearch()
        {
            return View();
        }

        public ActionResult UnderwriteMonitor()
        {
            ViewBag.Type = _db.usp_ProductType_Select(0, 999999, null, null, null).ToList();
            ViewBag.AppStaus = _db.usp_ApplicationStatus_Select(null, 0, 999999, null, null, null).ToList();
            ViewBag.PaymentStatue = _db.usp_PaymentStatus_Select(null, 0, 999999, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            return View();
        }

        public JsonResult GetAgentMonitor(int? typeid = null, int? statusid = null, int? payid = null, bool IsFilterUser = false, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            int? userid = null;
            if (IsFilterUser) { userid = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID; }
            var result = _db.usp_ApplicationMonitor_Select(null, payid == -1 ? null : payid, statusid == -1 ? null : statusid,
                typeid == -1 ? null : typeid, userid, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRequestCancelApplication(int applicationId)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_Application_Delete(applicationId, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public void ExporAgentMonitor(int? typeid = null, string search = null, bool IsFilterUser = false)
        {
            int? userid = null;
            if (IsFilterUser) { userid = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID; }
            var data_sheet1 = _db.usp_ApplicationMonitor_Select(null, null, null, typeid == -1 ? null : typeid, userid, 0, 9999999, null, null, search).Select(s => new
            {
                เลขที่Application = s.ApplicationCode.Trim(),
                ผู้เอาประกันภัย = s.InsuredName.Trim(),
                รหัสตัวแทน = s.AgentCode.Trim(),
                ประเภท = s.ProductType.Trim(),
                แผน = s.ProductDetail.Trim(),
                จำนวนคน = s.CountCustomer,
                เบี้ยรวม = s.SumPremiumAmount,
                วันที่เริ่มคุ้มครอง = s.StartCoverDate,
                วันที่สิ้นสุด = s.EndCoverDate,
                สถานะ = s.ApplicationStatus.Trim(),
                การชำระเบี้ย = s.PaymentStatus.Trim(),
                CreateDate = s.CreatedDate,
                UpdateDate = s.UpdatedDate,
            }).ToList();
            var dt1 = GlobalFunction.ToDataTable(data_sheet1);
            ExcelManager.ExportToExcel(this.HttpContext, "รายงาน Agent Monitor", dt1, "รายการ");
        }

        public void ExportApproveMonitor(int? typeid = null, string search = null, int? statusId = null, int? statusPaymentId = null)
        {
            int? userid = null;

            var data_sheet1 = _db.usp_ApplicationMonitor_Select(null, statusPaymentId == -1 ? null : statusPaymentId, statusId == -1 ? null : statusId
                , typeid == -1 ? null : typeid, userid, 0, 9999999, null, null, search).Select(s => new
                {
                    เลขที่Application = s.ApplicationCode.Trim(),
                    ผู้เอาประกันภัย = s.InsuredName.Trim(),
                    รหัสตัวแทน = s.AgentCode.Trim(),
                    ประเภท = s.ProductType.Trim(),
                    แผน = s.ProductDetail.Trim(),
                    จำนวนคน = s.CountCustomer,
                    เบี้ยรวม = s.SumPremiumAmount,
                    วันที่เริ่มคุ้มครอง = s.StartCoverDate,
                    วันที่สิ้นสุด = s.EndCoverDate,
                    สถานะ = s.ApplicationStatus.Trim(),
                    การชำระเบี้ย = s.PaymentStatus.Trim(),
                    CreateDate = s.CreatedDate,
                    UpdateDate = s.UpdatedDate,
                }).ToList();
            var dt1 = GlobalFunction.ToDataTable(data_sheet1);
            ExcelManager.ExportToExcel(this.HttpContext, "รายงาน Approve Monitor", dt1, "รายการ");
        }

        //public JsonResult GetDocumentRequestCancelDetail(int referenceId, int documentTypeId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        //{
        //    var result = _db.usp_DocumentRequestCancel_Select(referenceId, documentTypeId, pageStart, pageSize, sortField, orderType, search).ToList();

        //    var dt = new Dictionary<string, object>
        //    {
        //        {"draw", draw },
        //        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"data", result.ToList()}
        //    };

        //    return Json(dt, JsonRequestBehavior.AllowGet);
        //}

        //    public ActionResult SaveRequestCancelApplication(int applicationId)
        //    {
        //        var lstArr = new string[3];
        //        var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

        //        var rs = _db.usp_RequestCancelApplication_Insert(applicationId, userID).Single();

        //        lstArr[0] = rs.IsResult.Value.ToString();
        //        lstArr[1] = rs.Result;
        //        lstArr[2] = rs.Msg;

        //        return Json(lstArr, JsonRequestBehavior.AllowGet);
        //    }
    }
}