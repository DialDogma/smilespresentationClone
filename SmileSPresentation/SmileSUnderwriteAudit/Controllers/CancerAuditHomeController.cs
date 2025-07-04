using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System.Linq;
using System;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class CancerAuditHomeController : Controller
    {
        private readonly UnderwriteMotorAuditEntity _context2;
        private readonly UnderwriteCancerAuditEntities _context;

        public CancerAuditHomeController()
        {
            _context = new UnderwriteCancerAuditEntities();
            _context2 = new UnderwriteMotorAuditEntity();
        }

        // GET: CancerAuditHome
        public ActionResult CancerAuditHomeIndex()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserDetail = user;

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
            ViewBag.AccessRole = roleList.ToList();
            //คิวงานติดเงื่อนไข/ไม่ผ่านอนุมัติจากฝ่ายรับประกัน
            var lstObj0 = _context.usp_QueueCIAuditInsureV2_Select(null, null, null, null, null, 3, "3,4", false, 0, 10, null, null).ToList();

            //คิวงานพิจารณาจากการ QC
            var lstObj1 = _context.usp_QueueCIAuditInsureV2_Select(null, null, null, null, null, 2, null, false, 0, 10, null, null).ToList();
            ViewBag.CIQueueconsider = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);
            ViewBag.CIQueueInsure = (lstObj0.Count > 0 ? lstObj0[0].TotalCount : 0);

            var periodlist = GlobalFunction.GetPeriodList();
            var period = Convert.ToDateTime(periodlist[0].Display);
            var queCount = _context.usp_QueueCIAuditPendingCount_Select(period, null).Single();
            ViewBag.queCountAll = queCount.Total;

            var changePeriodDay = Properties.Settings.Default.CancerChangPeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.CIGetPeriodList(changePeriodDay, 4);

            var lstObj2 = _context.usp_QueueCIAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 2, null, false).ToList();
            var lstObj3 = _context.usp_QueueCIAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "3", false).ToList();
            var lstObj4 = _context.usp_QueueCIAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "4", false).ToList();
            var count4DCR = lstObj2.Sum(x => x.TotalCount);
            var count4DCR2 = lstObj3.Sum(x => x.TotalCount);
            var count4DCR3 = lstObj4.Sum(x => x.TotalCount);
            ViewBag.CIQueueconsider = count4DCR;  // คิวงานรอพิจารณา
            ViewBag.InsureQueueConditional = count4DCR2;  //คิวงานติดเงื่อนไข
            ViewBag.InsureQueueNotConsidered = count4DCR3; // ไม่ผ่านอนุมัติจากฝ่ายรับประกัน



            return View();
        }

        public ActionResult CancerIndexQC()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserDetail = user;

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
            ViewBag.AccessRole = roleList.ToList();

            ViewBag.IsDev = false;
            if (roleList.Contains("Developer"))
            {
                ViewBag.IsDev = true;
            }
            //คิวงานติดเงื่อนไข/ไม่ผ่านอนุมัติจากฝ่ายรับประกัน
            var lstObj0 = _context.usp_QueueCIAuditInsureV2_Select(null, null, null, null, null, 3, "3,4", false, 0, 10, null, null).ToList();

            //คิวงานพิจารณาจากการ QC
            var lstObj1 = _context.usp_QueueCIAuditInsureV2_Select(null, null, null, null, null, 2, null, false, 0, 10, null, null).ToList();
            ViewBag.CIQueueInsure = (lstObj0.Count > 0 ? lstObj0[0].TotalCount : 0);
            ViewBag.CIQueueconsider = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);
            return View();

        }
    }
}