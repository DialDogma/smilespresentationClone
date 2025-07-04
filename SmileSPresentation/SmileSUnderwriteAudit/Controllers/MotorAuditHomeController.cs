using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class MotorAuditHomeController : Controller
    {
        private readonly UnderwriteMotorAuditEntity _context;

        public MotorAuditHomeController()
        {
            _context = new UnderwriteMotorAuditEntity();
        }

        // GET: MotorAuditHome
        public ActionResult Index()
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
            //คิวงานพิจารณาจากการ QC
            var lstObj0 = _context.usp_QueueMotorAuditInsureV2_Select(null, null, null, null, null, 2, null, false, 0, 10, null, null).ToList();
            //คิวงานไม่ผ่านคัดกรองจากประธานสาขา
            var lstObj1 = _context.usp_QueueMotorAuditChairmanClose_Select(null, null, null, null, null, 4, false, 0, 10, null, null).ToList();

            ViewBag.MotorQueueconsider = (lstObj0.Count > 0 ? lstObj0[0].TotalCount : 0);
            ViewBag.MotorQueueChairman = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);

            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 1);
            var Today1 = periodList.FirstOrDefault();
            ViewBag.Today = Today1.Display;
            var period = Convert.ToDateTime(Today1.Display);

            var queCount = _context.usp_QueueMotorAuditPendingCount_Select(period, null).Single();
            ViewBag.queCountAll = queCount.Total;

            return View();
        }

        public ActionResult IndexQC()
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
            var lstObj0 = _context.usp_QueueMotorAuditInsureClose_Select(null, null, null, null, null, null, 3, "3,4", false, 0, 10, null, null).ToList();

            //คิวงานพิจารณาจากการ QC
            //var lstObj1 = _context.usp_QueueMotorAuditInsure_Select(null, null, null, null, null, 2, null, 0, 10, null, null).ToList();
            var lstObj1 = _context.usp_QueueMotorAuditInsureV2_Select(null, null, null, null, null, 2, null, false, 0, 10, null, null).ToList();
            ViewBag.MotorQueueInsure = (lstObj0.Count > 0 ? lstObj0[0].TotalCount : 0);
            ViewBag.MotorQueueconsider = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);
            return View();
        }

        public ActionResult IndexChairman()
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
            //คิวงานไม่ผ่านคัดกรองจากประธานสาขา
            var lstObj1 = _context.usp_QueueMotorAuditChairmanClose_Select(null, null, null, null, null, 4, false, 0, 10, null, null).ToList();
            ViewBag.MotorQueueChairman = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);
            return View();
        }

        public ActionResult IndexReport()
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
            return View();
        }
    }
}