using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class CancerReportController : Controller
    {
        private readonly UnderwriteBranchV2CancerEntities _contextCI;
        private readonly UnderwriteBranchV2Entities _contextPH;

        public CancerReportController()
        {
            _contextCI = new UnderwriteBranchV2CancerEntities();
            _contextPH = new UnderwriteBranchV2Entities();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult CancerReportUnderwriteAndPolicies()
        {
            var changePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 5);
            ViewBag.PeriodList = periodList;
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Chairman")]
        public ActionResult CancerReportUnderwriteAndPoliciesCardDirector()
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branch = _contextPH.usp_BranchByChairmanUserId_Select(userId).ToList();
            ViewBag.branch = branch;
            var changePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(changePeriodDay, 3);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("yyyy-MM-dd");
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam,UnderwriteV2_InsuranceDept")]
        public ActionResult CancerReportPromotion()
        {
            return View();
        }


        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult CancerReportQueueManualAssign()
        {
            return View();
        }
        public ActionResult CancerReportUnderwriteAndPoliciesCardBO()
        {
            var changePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 13);
            ViewBag.PeriodList = periodList;
            return View();
        }

        public ActionResult CancerReportQueuePoliciesPending()
        {
            var changePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;
            return View();
        }

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
                var result = _contextCI.usp_pivotQueueCIStatusByAssignToUserId_Select(nStartCoverdate, assignToUserId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportReportQueueCancerDirector(string period)
        {
            try
            {
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                var result = _contextCI.usp_QueueCIDirectorReport_Select(nStartCoverdate, null, null, null, assignToUserId).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานมอบกรมธรรม์_CI_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportReportQueueCancerDirector_V2(string period)
        {
            try
            {
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                var result = _contextCI.usp_QueueCIDirectorReportV2_Select(nStartCoverdate, null, null, null, assignToUserId).ToList();
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานการมอบกรมธรรม์");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportCancerReportUnderwriteAndPoliciesCardDirector(string period, int branchId)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }

                var result = _contextCI.usp_QueueCIChairmanReport_Select(nStartCoverdate, branchId, null, null, null).ToList();

                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานการมอบกรมธรรม์ของประธานสาขา");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportCancerReportUnderwriteAndPoliciesCardBO(string period)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }

                var result = _contextCI.usp_QueueCIBusinessPromoteTeamReport_Select(nStartCoverdate, null, null, null, null).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานการมอบกรมธรรม์_CI_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportCancerReportUnderwriteAndPoliciesCardBO_V2(string period)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }

                var result = _contextCI.usp_QueueCIBusinessPromoteTeamReportV2_Select(nStartCoverdate, null, null, null, null).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานการมอบกรมธรรม์_CI_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void CancerExportReportQueuePoliciesPending()
        {
            try
            {
                var result = _contextCI.usp_QueueCIPoliciesPendingBusinessPromoteTeamReportV3_Select().ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานรอมอบกรมธรรม์_CI_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        //รายงานโยกคิวงาน
        public void ExportCancerReportQueueManualAssign(string period)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }

                var result = _contextCI.usp_QueueCIManualAssignReport_Select(nStartCoverdate, null, null, null, null).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานโยกคิวงาน_CI_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
    }
}