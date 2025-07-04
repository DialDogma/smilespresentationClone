using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new UnderwriteBranchV2Entities())
            {
                //User
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                ViewBag.UserDetail = user;

                //DCR
                var periodList = GlobalFunction.GetPeriodList();
                ViewBag.PeriodList = periodList;
                var currentDCR = Convert.ToDateTime(periodList[0].Value);

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;
                var roleListPH = roleListRaw.Split(',').ToList();
                ViewBag.AccessRolePH = roleListPH;

                //สถานะคิวงานคัดกรอง
                var objQueueStatus = db.usp_pivotQueueStatus_Select(currentDCR, null, null, null).FirstOrDefault();
                //เข้าพบคัดกรองและมอบบัตร
                var lstObj0 = db.usp_QueueUnderwritePending_Select(null, null, null, null, user.User_ID, 0, 10, null, null, null, null, null).ToList();
                ////มอบบัตรภายหลังกรณีโทรคัดกรอง
                var lstObj1 = db.usp_QueueCoBrandPendingV2_Select(null, null, null, null, user.User_ID, 0, 10, null, null, null, null, null, null, null, null).ToList();
                ////รับทราบคิวงานคัดกรอง
                var lstObj2 = db.usp_QueueAccept_Select(null, null, null, user.User_ID, 0, 10, null, null).ToList();
                ////คิวงานคัดกรองเกินกำหนด

                /*  var lstObj3 = db.usp_CountIndexPagePH_Select(user.User_ID, currentDCR).FirstOrDefault();
                  ViewBag.QueueUnderwritePendingCountV2 = lstObj3.QueueUnderwritePendingTotalCount;                
                  ViewBag.QueueCoBrandPendingCountV2 = lstObj3.QueueCoBrandTotalCount;
                   ViewBag.QueueAcceptV2 = lstObj3.QueueAcceptTotalCount;
                   ViewBag.QueueStatusV2 = (lstObj3.QueueStatusId2 + lstObj3.QueueStatusId8);
                 ViewBag.QueueStatusV2 = (objQueueStatus.QueueStatusId2 + objQueueStatus.QueueStatusId8);*/


                ViewBag.QueueStatus = (objQueueStatus.QueueStatusId2 + objQueueStatus.QueueStatusId8);
                ViewBag.QueueUnderwritePendingCount = (lstObj0.Count > 0 ? lstObj0[0].TotalCount : 0);
                ViewBag.QueueCoBrandPendingCount = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);
                ViewBag.QueueAccept = (lstObj2.Count > 0 ? lstObj2[0].TotalCount : 0);


            }
            return View();
        }

        [HttpGet]
        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}