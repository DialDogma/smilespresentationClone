using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    // start 23/1/2568
    [Authorization]

    public class PHInsureController : Controller
    {
        #region Context

        private readonly UnderwriteAuditEntities _context;
        private readonly UnderwriteBranchV2Entities _context2;

        public PHInsureController()
        {
            _context = new UnderwriteAuditEntities();
            _context2 = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _context2.Dispose();
        }

        #endregion Context

        #region View
        // GET: PHInsure
        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure")]
        [Route("ph/insure/queue/task")]
        public ActionResult InsureQueueConsideringUnderwriteInsureResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueHealthAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 2, null, null).ToList();
            ViewBag.AuditInsureCount = AuditInsureCount.OrderBy(p => p.Period).ToList();
            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();
            ViewBag.Today = periodList[0].Value.ToString("MM");
            return View();
        }


        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        [Route("ph/insure/queue/conditional")]
        public ActionResult InsureQueueConditional()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueHealthAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "3", false).ToList();
            ViewBag.AuditInsureCount = AuditInsureCount.OrderBy(p => p.Period).ToList();
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();
            ViewBag.Today = periodList[0].Value.ToString("MM");
            return View();
        }


        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure")]
        [Route("ph/insure/queue/notconsidered")]
        public ActionResult InsureQueueNotConsidered()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();

            // กำหนด Culture ภาษาไทย
            CultureInfo thaiCulture = new CultureInfo("th-TH");
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueHealthAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "4", false).ToList();
            ViewBag.AuditInsureCount = AuditInsureCount.OrderBy(p => p.Period).ToList();
            ViewBag.Today = periodList[0].Value.ToString("MM");

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        [Route("ph/insure/report")]
        public ActionResult InsureReport()
        {
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 25);
            ViewBag.PeriodList = periodList.ToList();
            return View();
        }
        #endregion

        #region API
        public JsonResult GetQueueHealthAuditInsureNotConsidered_Wait(int? auditHealthStatusId,
                                             int? auditInsureStatusId,
                                             string applicationCode,
                                             string insuredName,
                                             string payerName,
                                             string c_period,
                                             int? draw,
                                             int? pageSize,
                                             int? indexStart,
                                             string sortField,
                                             string orderType,
                                             string auditInsureConsiderStatusIdList, bool? auditInsureClose)
        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            var result = _context.usp_QueueHealthAuditInsure_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                 auditHealthStatusId,
                 auditInsureStatusId,
                 auditInsureConsiderStatusIdList == "-1" ? "4" : auditInsureConsiderStatusIdList,
                 auditInsureClose,
                indexStart,
                pageSize,
                sortField,
                orderType
                ).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQueueHealthAuditInsureNotConsidered_Complete(int? auditHealthStatusId,
                                              int? auditInsureStatusId,
                                              string applicationCode,
                                              string insuredName,
                                              string payerName,
                                              string c_period,
                                              int? draw,
                                              int? pageSize,
                                              int? indexStart,
                                              string sortField,
                                              string orderType,
                                              string auditInsureConsiderStatusIdList, bool? auditInsureClose)
        {
            var period = Convert.ToDateTime(c_period);

            var result = _context.usp_QueueHealthAuditInsure_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditHealthStatusId,
                auditInsureStatusId,
                auditInsureConsiderStatusIdList == "-1" ? "4" : auditInsureConsiderStatusIdList,
                auditInsureClose,
                indexStart,
                pageSize,
                sortField == "" ? "AuditInsureConsiderStatusDetail" : sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetQueueHealthAuditInsureConditional_Wait(int? auditHealthStatusId,
                                         int? auditInsureStatusId,
                                         string applicationCode,
                                         string insuredName,
                                         string payerName,
                                         string c_period,
                                         int? draw,
                                         int? pageSize,
                                         int? indexStart,
                                         string sortField,
                                         string orderType,
                                         string auditInsureConsiderStatusIdList,
                                         bool? auditInsureClose)
        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            var result = _context.usp_QueueHealthAuditInsure_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                 auditHealthStatusId,
                auditInsureStatusId,
                auditInsureConsiderStatusIdList,
                 auditInsureClose,
                indexStart,
                pageSize,
                sortField,
                orderType
                ).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQueueHealthAuditInsureConditional_Complete(int? auditHealthStatusId,
                                              int? auditInsureStatusId,
                                              string applicationCode,
                                              string insuredName,
                                              string payerName,
                                              string c_period,
                                              int? draw,
                                              int? pageSize,
                                              int? indexStart,
                                              string sortField,
                                              string orderType,
                                              string auditInsureConsiderStatusIdList, bool? auditInsureClose)
        {
            var period = Convert.ToDateTime(c_period);

            var result = _context.usp_QueueHealthAuditInsure_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditHealthStatusId,
                auditInsureStatusId,
                auditInsureConsiderStatusIdList,
                auditInsureClose,
                indexStart,
                pageSize,
                sortField == "" ? "AuditInsureConsiderStatusDetail" : sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetMonitorQueueUnderwrite(int? auditHealthStatusId,
                                              int? auditInsureStatusId,
                                              string applicationCode,
                                              string insuredName,
                                              string payerName,
                                              string c_period,
                                              int? draw,
                                              int? pageSize,
                                              int? indexStart,
                                              string sortField,
                                              string orderType,
                                              string auditInsureConsiderStatus)
        {

            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            string auditInsureConsiderStatusIdList = null;
            if (auditInsureConsiderStatus == "-1")
            {
                auditInsureConsiderStatusIdList = "2,3,4";
            }
            else
            {
                auditInsureConsiderStatusIdList = auditInsureConsiderStatus;
            }
            // auditInsureStatusId รอพิจารณา = 2, พิจารณาแล้ว = 3
            var result = _context.usp_QueueHealthAuditInsure_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditHealthStatusId,
                auditInsureStatusId,
                (auditInsureStatusId == 2 ? null : auditInsureConsiderStatusIdList),
                null,
                indexStart,
                pageSize,
                sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetQueueInsuerDCRCount(int? auditInsureStatusId, string auditInsureConsiderStatusIdList)
        {

            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueHealthAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, auditInsureStatusId, auditInsureConsiderStatusIdList, false).ToList();

            return Json(AuditInsureCount, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}