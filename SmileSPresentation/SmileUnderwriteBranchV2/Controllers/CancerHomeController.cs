
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class CancerHomeController : Controller
    {
        private readonly UnderwriteBranchV2CancerEntities _contextCI;
        private readonly UnderwriteBranchV2Entities _contextPH;

        public CancerHomeController()
        {
            _contextCI = new UnderwriteBranchV2CancerEntities();
            _contextPH = new UnderwriteBranchV2Entities();
        }

        // GET: CancerHome
        public ActionResult IndexCancer()
        {
            //User
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserDetail = user;

            //DCR
            var periodList = GlobalFunction.GetPeriodList();
            ViewBag.PeriodList = periodList;
            var currentDCR = Convert.ToDateTime(periodList[0].Value);

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();//.GetRoleByUserName(user.UserName);
            var roleList = roleListRaw.Split(',').ToList();
            //var accessRole = GlobalFunction.GetAccessRole(roleList);
            ViewBag.AccessRole = roleList;

            //find roles dev
            var findDev = roleList.Find(x => x.Contains("Developer"));
            ViewBag.Dev = false;
            if (findDev != null)
            {
                ViewBag.Dev = true;
            }

            //เข้าพบคัดกรองและมอบกรมธรรม์
            var lstObj0 = _contextCI.usp_QueueCIUnderwritePending_Select(null, null, null, null, user.User_ID, null, null, null, 0, 10, null, null).ToList();
            //มอบกรมธรรม์ภายหลังกรณีโทรคัดกรอง
            var lstObj1 = _contextCI.usp_QueueCIPoliciesPendingV2_Select(null, null, null, null, user.User_ID, null, null, null, null, null, null, 0, 10, null, null).ToList();
            var lstObj2 = _contextCI.usp_QueueCIApprovePendingCountByChairmanUserId_Select(user.User_ID).ToList();

            ViewBag.CIQueueUnderwritePendingCount = (lstObj0.Count > 0 ? lstObj0[0].TotalCount : 0);
            ViewBag.CIPoliciesQueueCount = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);
            ViewBag.CIQueueApprovePendingCount = (lstObj2.Count > 0 ? lstObj2[0].TotalCount : 0);

            //รับทราบคิวงานคัดกรอง
            var lstObj3 = _contextCI.usp_QueueCIAccept_Select(null, null, null, user.User_ID, 0, 10, null, null).ToList();
            ViewBag.QueueAccept = (lstObj3.Count > 0 ? lstObj3[0].TotalCount : 0);

            //คิวงานคัดกรองเกินกำหนด
            var lstObj4 = _contextCI.usp_QueueCIApprovePendingByBusinessPromoteTeamUserIdV2_Select(user.User_ID, null, null, null, null, null, null, 0, 10, null, null).ToList();
            ViewBag.QueueOverdue = (lstObj4.Count > 0 ? lstObj4[0].TotalCount : 0);

            var objQueueStatus = _contextCI.usp_pivotQueueCIStatus_Select(currentDCR, null, null, null).FirstOrDefault();
            ViewBag.QueueStatus = (objQueueStatus.QueueStatusId2 + objQueueStatus.QueueStatusId8);
            return View();
        }
    }
}