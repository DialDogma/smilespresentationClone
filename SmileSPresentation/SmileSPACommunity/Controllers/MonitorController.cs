using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;

namespace SmileSPACommunity.Controllers
{
    [Authorization]
    public class MonitorController : Controller
    {
        private PACommunityEntities _db;

        public MonitorController()
        {
            _db = new PACommunityEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: Monitor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EndorseApplicationContactMonitor()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "PACommunity_MO" }; //MO
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PACommunity_PAUnderwrite" }; //ฝ่าย PA Underwrite

            ViewBag.Role = rolelist;

            ViewBag.RoleList = role;

            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 999, null, null, null).ToList();
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 999, null, null, null).ToList();
            }
            ViewBag.ApproveStatus = _db.usp_ApproveStatus_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public JsonResult GetRequestEndorseContactAndAccountDetail(string approveStatusId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, string branchID = null)
        {
            int? StatusId = null;
            int? c_branchID = null;
            if (approveStatusId == "-1" || approveStatusId == null)
            {
                StatusId = null;
            }
            else
            {
                StatusId = Convert.ToInt32(approveStatusId);
            }

            if (branchID != "-1" && branchID != null)
            {
                c_branchID = Convert.ToInt32(branchID);
            }

            var result = _db.usp_RequestEndorseContactAndAccount_Select(c_branchID, StatusId, null, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
    }
}