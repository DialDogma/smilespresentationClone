using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class MotorReportController : Controller
    {
        private readonly UnderwriteBranchV2MotorModelContainer _contextMotor;
        private readonly UnderwriteBranchV2Entities _contextPH;

        public MotorReportController()
        {
            _contextMotor = new UnderwriteBranchV2MotorModelContainer();
            _contextPH = new UnderwriteBranchV2Entities();
        }

        #region View

        // GET: MotorReport
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult MotorReportUnderwriteAndPolicies()
        {
            var changePeriodDay = Properties.Settings.Default.MotorReportChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 3);
            ViewBag.PeriodList = periodList[1].Display;
            ViewBag.changePeriodDay = changePeriodDay;
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Chairman")]
        public ActionResult MotorReportUnderwriteAndPoliciesCardDirector()
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branch = _contextPH.usp_BranchByChairmanUserId_Select(userId).ToList();
            ViewBag.branch = branch;

            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            return View();
        }

        /*[Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam,UnderwriteV2_InsuranceDept")]*/
        public ActionResult MotorReportPromotion()
        {
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult MotorReportQueueManualAssign()
        {
            return View();
        }

        public ActionResult MotorReportUnderwriteAndPoliciesCardBO()
        {
            var changePeriodDay = Properties.Settings.Default.MotorReportChangePeriodDay;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 3);
            ViewBag.PeriodList = periodList[1].Display;
            ViewBag.changePeriodDay = changePeriodDay;
            return View();
        }

        public ActionResult MotorReportQueuePoliciesPending()
        {
            var changePeriodDay = Properties.Settings.Default.MotorReportChangePeriodDay;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 3);
            ViewBag.PeriodList = periodList[1].Display;
            ViewBag.changePeriodDay = changePeriodDay;
            return View();
        }

        #endregion View

        #region API

        [HttpGet]
        public ActionResult GetReportForChart(string period)
        {
            try
            {
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                var result = _contextMotor.usp_pivotQueueMotorStatusByAssignToUserId_Select(nStartCoverdate, assignToUserId).FirstOrDefault();
                if (result.AssignToUserId != null)
                {
                    result = _contextMotor.usp_pivotQueueMotorStatusByAssignToUserId_Select(nStartCoverdate, assignToUserId).FirstOrDefault();
                }
                else
                {
                    result = null;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        //UI19
        public ActionResult GetBranchByChairManUserId(int UserId)
        {
            var result = _contextPH.usp_BranchByChairmanUserId_Select(UserId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //รายงานการมอบกรมธรรม์ กล่องฟ้า(สาขา)
        public void ExportReportQueueMotorDirector(string dateFrom, string dateTo)
        {
            try
            {
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                DateTime? ndateFrom = null;
                DateTime? ndateTo = null;
                if (!string.IsNullOrEmpty(dateFrom) && dateFrom != "-1")
                {
                    ndateFrom = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(dateTo) && dateTo != "-1")
                {
                    ndateTo = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                var result = _contextMotor.usp_QueueMotorDirectorReportV2_Select(ndateFrom, ndateTo, null, null, null, assignToUserId).ToList();

                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานมอบกรมธรรม์");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        //รายงานการมอบกรมธรรม์ กล่องม่วง(ทั่วประเทศ)
        public void ExportMotorReportUnderwriteAndPoliciesCardBO(string dateFrom, string dateTo)
        {
            try
            {
                DateTime? ndateFrom = null;
                DateTime? ndateTo = null;
                if (!string.IsNullOrEmpty(dateFrom) && dateFrom != "-1")
                {
                    ndateFrom = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(dateTo) && dateTo != "-1")
                {
                    ndateTo = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                var result = _contextMotor.usp_QueueMotorBusinessPromoteTeamReportV2_Select(ndateFrom, ndateTo, null, null, null, null).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานมอบกรมธรรม์_MP_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportMotorReportUnderwriteAndPoliciesCardDirector(string period, int branchId)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }

                var result = _contextMotor.usp_QueueMotorChairmanReport_Select(nStartCoverdate, branchId, null, null, null).ToList();

                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานการมอบกรมธรรม์ของประธานสาขา");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void MotorExportReportQueuePoliciesPending(string dateFrom, string dateTo)
        {
            try
            {
                DateTime? ndateFrom = null;
                DateTime? ndateTo = null;
                if (!string.IsNullOrEmpty(dateFrom) && dateFrom != "-1")
                {
                    ndateFrom = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(dateTo) && dateTo != "-1")
                {
                    ndateTo = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                var result = _contextMotor.usp_QueueMotorPoliciesPendingBusinessPromoteTeamReportV3_Select(ndateFrom, ndateTo).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานรอมอบกรมธรรม์_MP_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        //รายงานโยกคิวงาน
        public void MotorExportReportQueueManualAssign(string period)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextMotor.usp_QueueMotorManualAssignReport_Select(nStartCoverdate, null, null, null, null).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานโยกคิวงาน_MP_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion API
    }
}