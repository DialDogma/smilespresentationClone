using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileUnderwriteBranch.Helper;
using SmileUnderwriteBranch.Models;

namespace SmileUnderwriteBranch.Controllers
{
    [Authorization]
    public class TaskController : Controller
    {
        public ActionResult TaskMain(string Id, string AreaName)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();
            var rolesDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolesDirector = new[] { "SmileUnderwriteBranch_Director" }; //ผอ พื้นที่

            if (lstUserRole.Intersect(rolesDirector).Count() > 0 || lstUserRole.Intersect(rolesDev).Count() > 0)
            {
                ViewBag.AreaId = Id;
                ViewBag.AreaName = AreaName;
                ViewBag.UserId = user.User_ID;
                return View();
            }

            return RedirectToAction("UnAuthorize", "Auth");
        }

        public ActionResult TaskList(string areaId)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();
            var rolesDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolesDirector = new[] { "SmileUnderwriteBranch_Director" }; //ผอ พื้นที่

            if (lstUserRole.Intersect(rolesDirector).Count() > 0 || lstUserRole.Intersect(rolesDev).Count() > 0)
            {
                ViewBag.UserId = user.User_ID;
                ViewBag.AreaId = areaId;
                return View();
            }

            return RedirectToAction("UnAuthorize", "Auth");
        }

        [HttpGet]
        public ActionResult Get_StatusByStudyArea(int studyAreaId)
        {
            //GET LOGIN DETAIL
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            using (var db = new UnderwriteBranchEntities())
            {
                //DECLARE New Object
                var newObject = new StatusDetailNew();
                //GET CONFIG FROM STOREPROCEDURE
                var config = db.usp_PHQueueConfig_Select().First();
                //GET LIST STATUS
                var lstStatus = db.usp_PHQueueStatusCountByStudyAreaIdV4_Select(studyAreaId, userId, Convert.ToInt32(config.DurationDays), null).First();

                //SET VALUE TO NEWLIST
                newObject.PHQueueAll = lstStatus.PHQueueAll.Value;
                newObject.PHQueueOnProcess = lstStatus.PHQueueOnProcess.Value;
                newObject.PHQueueResultPass = lstStatus.PHQueueResultPass.Value;
                newObject.PHQueueResultNotPass = lstStatus.PHQueueResultNotPass.Value;

                //RETURN JSON NEWLIST
                return Json(newObject, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDatatablesTask(int draw, int schoolAreaId, string period, int? pHQueueStatusId, string search, bool? isResultPass, int pageStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //get userId
                var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                //GET CONFIG FROM STOREPROCEDURE
                var config = db.usp_PHQueueConfig_Select().First();
                DateTime? convert_period;
                //try Convert period to datetime
                try
                {
                    convert_period = DateTime.Parse(period);
                }
                catch (Exception)
                {
                    convert_period = null;
                }
                //check period != null || period == null
                var lst = convert_period != null ? db.usp_PHQueue_Select(userId, schoolAreaId, null, convert_period, pHQueueStatusId, search, isResultPass, pageStart, pageSize, sortField, orderType).ToList() : db.usp_PHQueue_Select(userId, schoolAreaId, config.DurationDays, null, pHQueueStatusId, search, isResultPass, pageStart, pageSize, sortField, orderType).ToList();

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