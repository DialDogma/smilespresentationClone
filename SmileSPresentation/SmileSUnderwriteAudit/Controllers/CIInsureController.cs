using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    public class CIInsureController : Controller
    {

        #region Context

        private readonly UnderwriteCancerAuditEntities _context;
        private readonly UnderwriteCancerBranchV2Entities _contextCancer;

        public CIInsureController()
        {
            _context = new UnderwriteCancerAuditEntities();
            _contextCancer = new UnderwriteCancerBranchV2Entities();
        }

        #endregion Context
        // GET: CIInsure
        // GET: PHInsure
        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_ConditionInsure")]
        [Route("ci/insure/queue/task")]
        public ActionResult InsureQueueCIConsideringUnderwriteInsureResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();
            var changePeriodDay = Properties.Settings.Default.CancerChangPeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.CIGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueCIAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 2, null, null).ToList();
            ViewBag.AuditInsureCount = AuditInsureCount.OrderBy(p => p.Period).ToList();

            ViewBag.PeriodList = periodList.OrderBy(p => p.Value).ToList();
            ViewBag.Today = periodList[0].Value.ToString("MM");
            return View();
        }




        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_ConditionInsure")]
        [Route("ci/insure/queue/conditional")]
        public ActionResult InsureQueueCIConditional()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();




            var changePeriodDay = Properties.Settings.Default.CancerChangPeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.CIGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueCIAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "3", false).ToList();
            ViewBag.AuditInsureCount = AuditInsureCount.OrderBy(p => p.Period).ToList();
            ViewBag.PeriodList = periodList.OrderBy(p => p.Value).ToList();

            ViewBag.Today = periodList[0].Value.ToString("MM");
            return View();
        }


        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_ConditionInsure")]
        [Route("ci/insure/queue/notconsidered")]
        public ActionResult InsureQueueCINotConsidered()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();

            var changePeriodDay = Properties.Settings.Default.CancerChangPeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.CIGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueCIAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "4", false).ToList();
            ViewBag.AuditInsureCount = AuditInsureCount.OrderBy(p => p.Period).ToList();
            // กำหนด Culture ภาษาไทย

            ViewBag.PeriodList = periodList.OrderBy(p => p.Value);


            ViewBag.Today = periodList[0].Value.ToString("MM");

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_Insure,UnderwriteAudit_ConditionInsure")]
        [Route("ci/insure/report")]
        public ActionResult InsureReportCI()
        {
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 25);
            ViewBag.PeriodList = periodList.ToList();
            return View();
        }


        #region API
        public JsonResult GetQueueCIAuditInsureNotConsidered_Wait(int? auditHealthStatusId,
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
            var result = _context.usp_QueueCIAuditInsureV2_Select(
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

        public JsonResult GetQueueCIAuditInsureNotConsidered_Complete(int? auditHealthStatusId,
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

            var result = _context.usp_QueueCIAuditInsureV2_Select(
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


        public JsonResult GetQueueCIAuditInsureConditional_Wait(int? auditHealthStatusId,
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
            var result = _context.usp_QueueCIAuditInsureV2_Select(
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

        public JsonResult GetQueueCIAuditInsureConditional_Complete(int? auditHealthStatusId,
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

            var result = _context.usp_QueueCIAuditInsureV2_Select(
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
            var result = _context.usp_QueueCIAuditInsureV2_Select(
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
            var changePeriodDay = Properties.Settings.Default.CancerChangPeriodDay;
            var periodList = GlobalFunction.CIGetPeriodList(changePeriodDay, 4);
            var AuditInsureCount = _context.usp_QueueCIAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, auditInsureStatusId, auditInsureConsiderStatusIdList, false).ToList();

            return Json(AuditInsureCount, JsonRequestBehavior.AllowGet);
        }

        #endregion





    }
}