using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class MotorHomeController : Controller
    {
        // GET: MotorHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MotorInsurance()
        {
            using (var dbMotor = new UnderwriteBranchV2MotorModelContainer())
            using (var dbPH = new UnderwriteBranchV2Entities())

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

                var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
                var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
                var periodListDashBoard = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
                var currentDCRDashBoard = Convert.ToDateTime(periodListDashBoard[0].Value);


                var objQueueStatus = dbMotor.usp_pivotQueueMotorStatus_Select(currentDCRDashBoard, null, null, null).FirstOrDefault();
                ViewBag.QueueStatus = (objQueueStatus.QueueStatusId2 + objQueueStatus.QueueStatusId8);
                //เข้าพบคัดกรองและมอบกรมธรรม์
                var lstObj0 = dbMotor.usp_QueueMotorUnderwritePending_Select(null, null, null, null, user.User_ID, null, null, null, 0, 10, null, null).ToList();
                //มอบกรมธรรม์ภายหลังกรณีโทรคัดกรอง
                var lstObj1 = dbMotor.usp_QueueMotorPoliciesPendingV2_Select(null, null, null, null, user.User_ID, null, null, null, null, null, null, 0, 10, null, null).ToList();
                var lstObj2 = dbMotor.usp_QueueMotorApprovePendingCountByChairmanUserId_Select(user.User_ID).ToList();
                //คิวงานคัดกรองเกินกำหนด
                var lstObj3 = dbMotor.usp_QueueMotorApprovePendingByBusinessPromoteTeamUserIdV2_Select(user.User_ID, null, null, null, null, null, null, 0, 10, null, null).ToList();
                //รับทราบคิวงานคัดกรอง
                var lstObj4 = dbMotor.usp_QueueMotorAccept_Select(null, null, null, user.User_ID, 0, 10, null, null).ToList();

                #region motorQueueApprovePendingCount

                /*                var motorQueueApprovePendingCount = 0;
                                if (accessRole.Contains("DEV"))
                                {
                                    var branchList = dbPH.usp_BranchByChairmanUserId_Select(user.User_ID).ToList();

                                    if (branchList == null) throw new NullReferenceException();

                                    foreach (var item in branchList)
                                    {
                                        //พิจารณาผลการคัดกรอง
                                        var r = db.usp_pivotQueueApproveByBranchId_Select(currentDCR, item.Branch_ID, null, null, null).SingleOrDefault()?.QueueApprovePending.Value;
                                        motorQueueApprovePendingCount += r ?? 0;
                                    }
                                }
                                else if (accessRole.Contains("CM"))
                                {
                                    //สาขาที่ประธานดูแล
                                    var branchList = dbPH.usp_BranchByChairmanUserId_Select(user.User_ID).ToList();

                                    if (branchList == null) throw new NullReferenceException();

                                    foreach (var item in branchList)
                                    {
                                        //พิจารณาผลการคัดกรอง
                                        var r = db.usp_pivotQueueApproveByBranchId_Select(currentDCR, item.Branch_ID, null, null, null).SingleOrDefault()?.QueueApprovePending.Value;
                                        motorQueueApprovePendingCount += r ?? 0;
                                    }
                                }
                                else if (accessRole.Contains("BPT"))
                                {
                                    //สาขาที่ทีมส่งเสริมดูแล
                                    var branchList = db.usp_BranchByBusinessPromoteTeamUserId_Select(user.User_ID).ToList();

                                    if (branchList == null) throw new NullReferenceException();

                                    foreach (var item in branchList)
                                    {
                                        //พิจารณาผลการคัดกรอง
                                        var r = db.usp_pivotQueueApproveByBranchId_Select(currentDCR, item.Branch_ID, null, null, null).SingleOrDefault()?.QueueApprovePending.Value;
                                        motorQueueApprovePendingCount += r ?? 0;
                                    }
                                }*/

                #endregion motorQueueApprovePendingCount

                ViewBag.MotorQueueUnderwritePendingCount = (lstObj0.Count > 0 ? lstObj0[0].TotalCount : 0);
                ViewBag.MotorPoliciesQueueCount = (lstObj1.Count > 0 ? lstObj1[0].TotalCount : 0);
                ViewBag.MotorQueueApprovePendingCount = (lstObj2.Count > 0 ? lstObj2[0].TotalCount : 0);
                ViewBag.QueueOverdue = (lstObj3.Count > 0 ? lstObj3[0].TotalCount : 0);
                ViewBag.QueueAccept = (lstObj4.Count > 0 ? lstObj4[0].TotalCount : 0);
            }
            return View();
        }
    }
}