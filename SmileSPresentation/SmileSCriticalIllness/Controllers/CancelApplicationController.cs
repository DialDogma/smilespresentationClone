using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSCriticalIllness.Helper;
using SmileSCriticalIllness.Models;

namespace SmileSCriticalIllness.Controllers
{
    [Authorization]
    public class CancelApplicationController : Controller
    {
        #region Context

        private readonly CriticalIllnessEntities _context;

        public CancelApplicationController()
        {
            _context = new CriticalIllnessEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        // GET: CancelApplication
        public ActionResult RequestCancelApplication()
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;
            ViewBag.userId = userId;

            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult UnderwriteCancelApplication()
        {
            return View();
        }

        public ActionResult RequestCancelApplicationDetail(string appCode)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userId = userId;

            DateTime? cancelDate = _context.usp_GetAllowCancelDateFromForRequestCancel_Select().FirstOrDefault();
            if (cancelDate != null)
            {
                ViewBag.reqCancelDate = cancelDate.HasValue ? cancelDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            }

            //get application detail and check application status to disable form

            if (appCode == null && appCode == "")
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = "ไม่พบรายการ application นี้" });
            }
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(appCode);

                appCode = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                ViewBag.AppId = appCode;
            }
            catch (Exception)
            {
                ViewBag.AppId = null;
            }
            //call app detail
            var appDetail = _context.usp_ApplicationDetail_Select(null, null, null, null, null, null, null, appCode, null, null,
                99, null, null, null).FirstOrDefault();

            if (appDetail == null)
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = "ไม่พบรายการ application นี้" });
            }
            ViewBag.Application = appDetail;
            ViewBag.StartCoverDate_string = Convert.ToDateTime(appDetail.StartCoverDate).AddYears(-543).ToString("MM/dd/yyyy");
            ViewBag.StartCoverDate = appDetail.StartCoverDate;
            ViewBag.appId_Int = appDetail.ApplicationId;

            //customer detail
            ViewBag.CustId = appDetail.CustId;

            //payer detail
            ViewBag.payerId = appDetail.PayerId;

            var insertRequestResult = _context.usp_RequestCancel_Insert(appDetail.ApplicationId, userId).FirstOrDefault();

            int? requestCancelId = null;

            if (insertRequestResult.IsResult == true)
            {
                requestCancelId = Convert.ToInt32(insertRequestResult.Result);
                ViewBag.requestCancelId = requestCancelId;
            }
            else
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = insertRequestResult.Msg });
            }
            //cancel cause list
            var cancelCauseList = _context.usp_CancelCause_Select(null, null, 99, null, null, null).ToList();
            ViewBag.cancelCauseList = cancelCauseList;

            //application status list
            var appStatusList = _context.usp_ApplicationStatus_Select(null, null, 99, null, null, null).ToList();
            ViewBag.appStatusList = appStatusList;

            var dcrDate = _context.usp_GetNextDcrForCreatedNewApp_Select().FirstOrDefault();
            if (dcrDate < ViewBag.StartCoverDate)
            {
                ViewBag.dcrDate = ViewBag.StartCoverDate_string;
            }
            else
            {
                ViewBag.dcrDate = Convert.ToDateTime(dcrDate).AddYears(-543).ToString("MM/dd/yyyy");
            }

            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult UnderwriteCancelApplicationDetail(string appCode, string requestCancelId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userId = userId;
            int? requestCancelId_int = null;
            //get application detail and check application status to disable form

            if (appCode == null && appCode == "")
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = "ไม่พบรายการ application นี้" });
            }
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(appCode);
                var base64EncodedBytes_requestCancelId = Convert.FromBase64String(requestCancelId);

                appCode = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                requestCancelId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes_requestCancelId);
                requestCancelId_int = Convert.ToInt32(requestCancelId);

                ViewBag.AppId = appCode;
                ViewBag.requestCancelId = requestCancelId_int;
            }
            catch (Exception)
            {
                ViewBag.AppId = null;
            }
            //call app detail
            var appDetail = _context.usp_ApplicationDetail_Select(null, null, null, null, null, null, null, appCode, null, null,
                99, null, null, null).FirstOrDefault();

            if (appDetail == null)
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = "ไม่พบรายการ application นี้" });
            }
            ViewBag.Application = appDetail;
            ViewBag.StartCoverDate = appDetail.StartCoverDate;
            ViewBag.appId_Int = appDetail.ApplicationId;

            //customer detail
            ViewBag.CustId = appDetail.CustId;

            //payer detail
            ViewBag.payerId = appDetail.PayerId;

            //get cancel cause detail
            var cancelDetail = _context.usp_RequestCancel_Select(requestCancelId_int, null, null, null, 99, null, null, null).FirstOrDefault();
            ViewBag.cancelDetail = cancelDetail;
            ViewBag.cancelDate = Convert.ToDateTime(cancelDetail.CancelDate).AddYears(-543).ToString("MM/dd/yyyy");

            //cancel cause list
            var cancelCauseList = _context.usp_CancelCause_Select(null, null, 99, null, null, null).ToList();
            ViewBag.cancelCauseList = cancelCauseList;

            //application status list
            var appStatusList = _context.usp_ApplicationStatus_Select(null, null, 99, null, null, null).ToList();
            ViewBag.appStatusList = appStatusList;

            //get approve cause
            var approveCauseList = _context.usp_ApproveCause_Select(null, null, 99, null, null, null).ToList();
            ViewBag.approveCauseList = approveCauseList;

            return View();
        }

        #endregion View

        #region api

        [HttpPost]
        public JsonResult SendRequestCancel(int requestCancelId, string cancelDate, int cancelCauseId, string cancelRemark)
        {
            DateTime cancelDateConvert = DateTime.ParseExact(cancelDate, "dd-MM-yyyy", new CultureInfo("en-Us"));

            var updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _context.usp_RequestCancel_Update(requestCancelId, cancelDateConvert, cancelCauseId,
                cancelRemark, updatedByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RequestCancel_dt(int? requestCancelId, string cancelDate, string approveStatusIdList,
            int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            DateTime? cancelDateConvert = null;
            if (cancelDate != "")
            {
                cancelDateConvert = DateTime.ParseExact(cancelDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
            }

            var result = _context.usp_RequestCancel_Select(requestCancelId, cancelDateConvert, approveStatusIdList,
                indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveCancelRequest(int requestCancelId, int approveStatusId, int? approveCauseId,
            string approveRemark)
        {
            var updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _context.usp_RequestCancel_Approve_Update(requestCancelId, approveStatusId, approveCauseId,
                approveRemark, updatedByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RequestCancelDetail(int requestCancelId)
        {
            var result = _context.usp_RequestCancel_Select(requestCancelId, null, null,
                null, 99, null, null, null).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelBeforeDCR(int appId)
        {
            var updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _context.usp_CancelBeforeDcr_Update(appId, updatedByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion api
    }
}