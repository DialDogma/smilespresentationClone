using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        private readonly UnderwriteAuditEntities _context;

        public HomeController()
        {
            _context = new UnderwriteAuditEntities();
        }
        [HttpGet]
        public ActionResult Index()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserDetail = user;

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
            ViewBag.AccessRole = roleList.ToList();
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 4);

            var lstObj1 = _context.usp_QueueHealthAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 2, null, false).ToList();
            var lstObj2 = _context.usp_QueueHealthAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "3", false).ToList();
            var lstObj3 = _context.usp_QueueHealthAuditInsureDCRCount_Select(null, null, null, periodList[0].Value, null, 3, "4", false).ToList();
            var count4DCR = lstObj1.Sum(x => x.TotalCount);
            var count4DCR2 = lstObj2.Sum(x => x.TotalCount);
            var count4DCR3 = lstObj3.Sum(x => x.TotalCount);
            ViewBag.HealthAQueueconsider = count4DCR;  // คิวงานรอพิจารณา
            ViewBag.InsureQueueConditional = count4DCR2;  //คิวงานติดเงื่อนไข
            ViewBag.InsureQueueNotConsidered = count4DCR3; // ไม่ผ่านอนุมัติจากฝ่ายรับประกัน
            var periodlist = GlobalFunction.GetPeriodList();
            var period = Convert.ToDateTime(periodlist[1].Display); //  PH โทรย้อยหลัง 1 เดือน
            var queCount = _context.usp_QueueAuditPendingCount_Select(period, null).Single();
            ViewBag.queCountAll = queCount.Total;
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_Insure")]
        [Route("ph/insure/index")]
        public ActionResult indexInsure()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserDetail = user;

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
            ViewBag.AccessRole = roleList.ToList();


            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_Insure")]
        [Route("ph/insure/indexreport")]
        public ActionResult indexInsureReport()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserDetail = user;

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
            ViewBag.AccessRole = roleList.ToList();

            return View();
        }
    }
}