using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileUnderwriteBranch.Models;
using SmileUnderwriteBranch.Helper;

namespace SmileUnderwriteBranch.Controllers
{
    [Authorization]
    public class TaskCMController : Controller
    {
        // GET: TaskCM
        public ActionResult TaskWaitToConsider(string userId, string areaId, string period)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();

            var base64EncodedBytes_userId = Convert.FromBase64String(userId);
            var base64EncodedBytes_areaId = Convert.FromBase64String(areaId);
            var base64EncodedBytes_period = Convert.FromBase64String(period);

            userId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes_userId);
            areaId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes_areaId);
            period = System.Text.Encoding.UTF8.GetString(base64EncodedBytes_period);

            using (var db = new UnderwriteBranchEntities())
            {
                var rolesDev = new[] { "Developer" };
                var rolesCM = new[] { "SmileUnderwriteBranch_CM" };

                if (lstUserRole.Intersect(rolesDev).Count() > 0)
                {
                    //get ข้อมูลพื้นที่ของ ผอ
                    var lstDirector = db.usp_StudyAreaUserConfigByUserId_Select(Convert.ToInt32(userId)).ToList();

                    foreach (var item in lstDirector)
                    {
                        //เช็คว่าเป็น ผอ พื้นทึ่
                        if (item.UserId == Convert.ToInt32(userId) && item.StudyAreaId == Convert.ToInt32(areaId))
                        {
                            ViewBag.UserID = item.UserId;
                            ViewBag.AreaID = item.StudyAreaId;
                            ViewBag.Period = period;
                            ViewBag.Name = item.StudyArea;
                            ViewBag.CMDueDate = Properties.Settings.Default.CMDueDate;
                            ViewBag.CurrentUser = user.User_ID;
                            return View();
                        }
                    }
                }
                else if (lstUserRole.Intersect(rolesCM).Count() > 0)
                {
                    //get ข้อมูลพื้นที่ของประธาน
                    var lstCM = db.usp_StudyAreaUserConfigByCMUserId_Select(user.User_ID).ToList();

                    foreach (var item in lstCM)
                    {
                        //เช็คว่าเป็น ผอ อยู่ในพื้นที่ดูของประธาน
                        if (item.UserId == Convert.ToInt32(userId) && item.StudyAreaId == Convert.ToInt32(areaId))
                        {
                            ViewBag.UserID = userId;
                            ViewBag.AreaID = areaId;
                            ViewBag.Period = period;
                            ViewBag.Name = item.StudyArea;
                            ViewBag.CurrentUser = user.User_ID;
                            return View();
                        }
                    }
                }
            }

            return RedirectToAction("UnAuthorize", "Auth");
        }

        [HttpPost]
        public ActionResult GetDatatablesTaskCM(int draw,
                                          int userId,
                                          int schoolAreaId,
                                          string period,
                                          string search,
                                          int pageStart,
                                          int pageSize,
                                          string sortField,
                                          string orderType,
                                          bool isConsider)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //GET LOGIN USER ID
                var cmUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                //GET CONFIG FROM STOREPROCEDURE
                var config = db.usp_PHQueueConfig_Select().First();

                int? durationDays = null;
                int? pHQueueStatusId = null;
                DateTime? dPeriod = null;

                if (period == "-1")
                {
                    durationDays = config.DurationDays;
                }
                else
                {
                    //Add Milliseconds
                    var d = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(period));
                    dPeriod = d.AddYears(-543);
                }

                var lst = db.usp_PHCMQueue_Select(cmUserId,
                                                     userId,
                                                     schoolAreaId,
                                                     durationDays,
                                                     dPeriod,
                                                     pHQueueStatusId,
                                                     search,
                                                     pageStart,
                                                     pageSize,
                                                     sortField,
                                                     orderType,
                                                     isConsider).ToList();

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