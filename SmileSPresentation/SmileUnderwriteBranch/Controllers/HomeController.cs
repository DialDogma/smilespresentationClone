using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmileUnderwriteBranch.Models;
using SmileUnderwriteBranch.Helper;

namespace SmileUnderwriteBranch.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        /// <summary>
        /// หน้าแรก
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //GET LOGIN DETAIL
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();
            var rolesDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolesCM = new[] { "SmileUnderwriteBranch_CM" }; //ประธานสาขา
            var rolesDirector = new[] { "SmileUnderwriteBranch_Director" }; //ผอ พื้นที่
            var roleCMAndDirector = new[] { "SmileUnderwriteBranch_CM", "SmileUnderwriteBranch_Director" }; //ประธานสาขา และ ผอ เป็นคนเดียวกัน

            //ฝ่ายพัฒนาระบบ , ผอ และ ประธาน เป็นคนเดียวกัน return view Index ให้เลือกว่าจะเข้าใช้งานส่วน ผอ หรือ ประธาน
            if ((lstUserRole.Intersect(roleCMAndDirector).Count() == 2 || lstUserRole.Intersect(rolesDev).Count() > 0)) return View();

            //ผอ Redirect Index Director ผอ สาขา
            if (lstUserRole.Intersect(rolesDirector).Count() > 0) return RedirectToAction("IndexDirector");

            //ประธานสาขา Redirect Index IndexCM ประธาน สาขา
            if (lstUserRole.Intersect(rolesCM).Count() > 0) return RedirectToAction("IndexCM");

            //อื่นๆ ไม่อนุญาต Redirect to unauthorize
            return RedirectToAction("UnAuthorize", "Auth");
        }

        /// <summary>
        /// หน้าแรกสำหรับประธานสาขา
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCM()
        {
            //GET LOGIN DETAIL
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();
            var rolesDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolesCM = new[] { "SmileUnderwriteBranch_CM" }; //ประธานสาขา

            //ประธาน return view IndexCM
            if (lstUserRole.Intersect(rolesCM).Count() > 0 || lstUserRole.Intersect(rolesDev).Count() > 0)
            {
                var periodList = LocalFunction.GetPeriodList();
                var currentMont = DateTime.Now.Month;

                ViewBag.CurrentMonth = currentMont;
                ViewBag.PeriodList = periodList;
                //IsCM =true ไม่ใช่ประธานสาขา
                ViewBag.IsCM = true;
                //return View IndexCM ประธานสาขา
                return View();
            }
            else
            {
                return RedirectToAction("UnAuthorize", "Auth");
            }
        }

        /// <summary>
        /// หน้าแรกสำหรับ ผอ สาขา
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexDirector()
        {
            //GET LOGIN DETAIL
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();
            var rolesDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolesDirector = new[] { "SmileUnderwriteBranch_Director" }; //ผอ พื้นที่

            //ผอ return view Index ผอ สาขา
            if (lstUserRole.Intersect(rolesDirector).Count() > 0 || lstUserRole.Intersect(rolesDev).Count() > 0)
            {
                var periodList = LocalFunction.GetPeriodList();
                ViewBag.PeriodList = periodList;
                //IsCM =false ไม่ใช่ประธานสาขา
                ViewBag.IsCM = false;
                //return View Index ผอ สาขา
                return View();
            }
            else
            {
                return RedirectToAction("UnAuthorize", "Auth");
            }
        }

        /// <summary>
        /// INSERT LOG
        /// </summary>
        [HttpPost]
        public void Analytic_Insert()
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //GET LOGIN DETAIL
                var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                //GET CURRENT URL
                var url = GlobalFunction.GetCurrentURL();
                //GET IP ADDRESS
                var ip = GlobalFunction.GetIPAddress();
                //INSERT TO DATABASE
                db.usp_PHQueueAnalytic_Insert(userId, url, ip);
            }
        }

        [HttpGet]
        public ActionResult Get_StudyAreaUser()
        {
            //GET LOGIN DETAIL
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            using (var db = new UnderwriteBranchEntities())
            {
                //DECLARE NEWLIST
                var newList = new List<StudyAreaDetailCM>();
                //GET CONFIG FROM STOREPROCEDURE
                var config = db.usp_PHQueueConfig_Select().First();
                //GET LIST STUDYAREAUSERCONFIG
                var lst = db.usp_StudyAreaUserConfigByUserId_Select(userId).ToList();
                //LOOP TO ADD NEW LIST
                for (var i = 0; i < lst.Count(); i++)
                {
                    //GET LIST STATUS
                    var lstStatus = db.usp_PHQueueStatusCountByStudyAreaIdV4_Select(lst[i].StudyAreaId, lst[i].UserId, Convert.ToInt32(config.DurationDays), null).First();
                    //SET VALUE TO NEWLIST
                    newList.Add(new StudyAreaDetailCM
                    {
                        StudyAreaId = lst[i].StudyAreaId.Value,
                        StudyAreaName = lst[i].StudyArea,
                        PHQueueAll = lstStatus.PHQueueAll.Value,
                        PHQueueOnProcess = lstStatus.PHQueueOnProcess.Value,
                        PHQueueResultPass = lstStatus.PHQueueResultPass.Value,
                        PHQueueResultNotPass = lstStatus.PHQueueResultNotPass.Value
                    });
                }
                //RETURN JSON NEWLIST
                return Json(newList, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Get_StudyAreaUserCM(string period)
        {
            //GET LOGIN DETAIL
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            using (var db = new UnderwriteBranchEntities())
            {
                //DECLARE NEWLIST
                var newList = new List<StudyAreaDetailCM>();
                int? durationDays = null;
                DateTime? dPeriod = null;
                if (period == "-1")
                {
                    durationDays = db.usp_PHQueueConfig_Select().FirstOrDefault().DurationDays;
                }
                else
                {
                    var d = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(period));
                    dPeriod = d.AddYears(-543);
                }

                //GET LIST STUDYAREAUSERCONFIG
                var lst = db.usp_StudyAreaUserConfigByCMUserId_Select(userId).ToList();

                //LOOP TO ADD NEW LIST
                for (var i = 0; i < lst.Count(); i++)
                {
                    //GET LIST STATUS
                    var lstStatus = db.usp_PHQueueStatusCountByStudyAreaIdV4_Select(lst[i].StudyAreaId, lst[i].UserId, durationDays, dPeriod).First();
                    //SET VALUE TO NEWLIST
                    newList.Add(new StudyAreaDetailCM
                    {
                        StudyAreaId = lst[i].StudyAreaId.Value,
                        StudyAreaName = lst[i].StudyArea,
                        UserId = lst[i].UserId.Value,
                        DirectorFullName = lst[i].DirectorFullName,
                        PHQueueAll = lstStatus.PHQueueAll.Value,
                        PHQueueOnProcess = lstStatus.PHQueueOnProcess.Value,
                        PHQueueComplete = lstStatus.PHQueueComplete.Value,
                        PHQueueResultPass = lstStatus.PHQueueResultPass.Value,
                        PHQueueResultNotPass = lstStatus.PHQueueResultNotPass.Value,
                        PHQueueCMConsidered = lstStatus.PHQueueCMConsidered.Value,
                        PHQueueCMOnProcess = lstStatus.PHQueueCMOnProcess.Value,
                        PHQueueCMApprove = lstStatus.PHQueueCMApprove.Value,
                        PHQueueCMNotApprove = lstStatus.PHQueueCMNotApprove.Value
                    });
                }
                //RETURN JSON NEWLIST
                return Json(newList, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetDatatables_Ranking_Call_DESC
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="dcr"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetDatatablesRankingCall_DESC(int draw, string dcr, int pageStart, int pageSize, string sortField, string orderType, string search)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_Ranking_Call_DESC_Select(DateTime.Parse(dcr), search, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="dcr"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetDatatablesRankingCall_ASC(int draw, string dcr, int pageStart, int pageSize, string sortField, string orderType, string search)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_Ranking_Call_ASC_Select(DateTime.Parse(dcr), search, pageStart, pageSize, sortField, orderType).ToList();
                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }
    }
}