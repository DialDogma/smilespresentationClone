using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UnderwriteCancellation.Helper;
using UnderwriteCancellation.Models;

namespace UnderwriteCancellation.Controllers
{
    [Authorization]
    public class TeamManageAssignController : Controller
    {
        #region Context

        private readonly UnderwriteCancellationEntities _context;

        public TeamManageAssignController()
        {
            _context = new UnderwriteCancellationEntities();
        }

        #endregion Context

        // GET: TeamManageAssign ระบบจัดการทีม
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TeamManageAssignCancellation()
        {
            ViewBag.PersonName = _context.usp_QCEmployee_Select("").ToList();
            return View();
        }

        #region API

        [HttpPost]
        public ActionResult GetTeamManageAssignCancellation(string searchText, int? draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            try
            {
                /* var user = GlobalFunction.GetLoginDetail(HttpContext);
                 var roleListRaw = new SSOServiceClient().GetRoleByUserName(user.UserName);
                 var roleList = roleListRaw.Split(',');*/
                //var coverDate = Convert.ToDateTime(startCoverDate);

                var result = _context.usp_QCUser_Select(searchText, indexStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                      {
                            {"draw", draw },
                            {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                            {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                            {"data", result.ToList()}
                        };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public JsonResult UpdateChangeEmployee(int? qCUserId, bool isActive)
        {
            int updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QCUser_Update(qCUserId, isActive, updatedByUserId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertChangeEmployee(int? userId)
        {
            int createdByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QCUser_Insert(userId, createdByUserId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQCUserById(int? qCUserId)
        {
            var QCUserById = _context.usp_QCUserById_Select(qCUserId).Single();
            ViewBag.IsActive = QCUserById.IsActive;

            return Json(QCUserById, JsonRequestBehavior.AllowGet);
        }

        #endregion API
    }
}