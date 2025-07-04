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
    public class MonitorController : Controller
    {
        #region Context

        private readonly CriticalIllnessEntities _context;

        public MonitorController()
        {
            _context = new CriticalIllnessEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        //Main Monitor
        public ActionResult ApplicationMonitor()
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;
            var userName = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var roleList = new SSOService.SSOServiceClient().GetRoleByUserName(userName);
            ViewBag.roleList = roleList;
            ViewBag.userId = userId;
            var dcrDate = _context.usp_GetNextDcrForCreatedNewApp_Select().FirstOrDefault();
            ViewBag.dcrDate = Convert.ToDateTime(dcrDate).AddYears(-543).ToString("MM/dd/yyyy");

            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult ApplicationUnderwriteMonitor()
        {
            var branchList = _context.usp_Branch_Select(null, null, 99, null, null, null).ToList();
            ViewBag.branchList = branchList;
            var dcrDate = _context.usp_GetNextDcrForCreatedNewApp_Select().FirstOrDefault();
            ViewBag.dcrDate = Convert.ToDateTime(dcrDate).AddYears(-543).ToString("MM/dd/yyyy");

            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult Underwrite(string appCode)
        {
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

            var udwStatus_Return = _context.usp_UdwUnApproveCause_Select(null, 2, null, null, null, null, null).ToList();
            var udwStatus_NotPass = _context.usp_UdwUnApproveCause_Select(null, 3, null, null, null, null, null).ToList();
            ViewBag.udwStatus_Return = udwStatus_Return;
            ViewBag.udwStatus_NotPass = udwStatus_NotPass;

            var appDetail = _context.usp_ApplicationDetail_Select(null, null, null, null, null, null, null, appCode, null, null,
                99, null, null, null).FirstOrDefault();
            if (appDetail == null)
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = "ไม่พบข้อมูล application นี้" });
            }
            ViewBag.Application = appDetail;
            ViewBag.StartCoverDate = appDetail.StartCoverDate;
            ViewBag.appId_Int = appDetail.ApplicationId;

            //customer detail
            ViewBag.CustId = appDetail.CustId;

            //payer detail
            ViewBag.payerId = appDetail.PayerId;

            //check age
            var checkAge = _context.usp_CheckProductXAgeAtRegister_Validate(appDetail.ApplicationId).FirstOrDefault();
            ViewBag.chkAge = checkAge;

            return View();
        }

        [Authorization(Roles = "CriticalIllness_Callcenter,CriticalIllness_Underwrite,Developer")]
        public ActionResult FindApplication()
        {
            return View();
        }

        #endregion View

        #region api

        [HttpPost]
        public JsonResult GetMonitorApplication(int? branchId, string startCoverDate, string cancelDate,
            string endCoverDate, string appStatusIdList, string appUdwStatusIdList, int? userId, string appCode,
            int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            //var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            if (branchId == 0)
            {
                branchId = null;
            }
            if (appStatusIdList == "")
            {
                appStatusIdList = null;
            }
            if (appUdwStatusIdList == "")
            {
                appUdwStatusIdList = null;
            }
            //check if null
            DateTime? startCoverDateConvert = null;
            if (startCoverDate != "")
            {
                startCoverDateConvert = DateTime.ParseExact(startCoverDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
            }

            DateTime? cancelDateConvert = null;
            if (cancelDate != "")
            {
                cancelDateConvert = DateTime.ParseExact(cancelDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
            }

            DateTime? endCoverDateConvert = null;
            if (endCoverDate != "")
            {
                endCoverDateConvert = DateTime.ParseExact(endCoverDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
            }

            var result = _context.usp_ApplicationDetail_Select(branchId, startCoverDateConvert, cancelDateConvert,
                endCoverDateConvert, appStatusIdList, appUdwStatusIdList, null, appCode, userId, indexStart,
                pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetApplicationDocDetail(int applicationId)
        {
            var result = _context.usp_TransactionUdwLatestByAppId_Select(applicationId).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorDoc_DT(int applicationId, string documentTypeIdList = null, int? draw = null,
            int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            if (documentTypeIdList == "")
            {
                documentTypeIdList = null;
            }

            var result = _context.usp_Document_Select(applicationId, documentTypeIdList, indexStart, pageSize, sortField,
                orderType, searchDetail).ToList();

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
        public JsonResult ReSendDoc(int applicationId)
        {
            var updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_NewApp_ReScanDoc_Update(applicationId, updatedByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HeirSelect_dt(int appId, int? draw = null,
            int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _context.usp_Heir_Select(appId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

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
        public JsonResult MemoSelect_dt(int? appId, int? memoTypeId, int? draw = null,
            int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _context.usp_ApplicationMemo_Select(appId, memoTypeId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

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
        public JsonResult TransactionSelect_dt(int? appId, int? transactiontypeId, int? draw = null,
            int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _context.usp_ApplicationTransaction_Select(appId, transactiontypeId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();
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
        public JsonResult DebtSelect_dt(int? appId, int? draw = null,
            int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _context.usp_DebtReceive_Select(appId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();
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
        public JsonResult UDWNewAppApprove(int applicationId, int applicationUnderwriteStatusId, int? udwUnApproveCauseId,
            string udwRemark)
        {
            var updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _context.usp_NewApp_UdwApprove_Update(applicationId, applicationUnderwriteStatusId,
                udwUnApproveCauseId, udwRemark, updatedByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion api
    }
}