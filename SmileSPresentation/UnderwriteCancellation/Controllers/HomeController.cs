using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using UnderwriteCancellation.Helper;
using UnderwriteCancellation.Models;

namespace UnderwriteCancellation.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        #region Context

        private readonly UnderwriteCancellationEntities _context;

        public HomeController()
        {
            _context = new UnderwriteCancellationEntities();
        }

        #endregion Context

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        //สำหรับผู้จัดการฝ่ายQC
        [HttpGet]
        public ActionResult ManagerQC()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            ViewBag.AccessRole = roleList.ToList();
            ViewBag.Msg = string.Empty;
            var result = _context.usp_QueueCreate_Select(0, 9999999, null, null).ToList();
            //เช็คว่าเคยสร้างคิวงานหรือยัง
            if (result.Count != 0)
            {
                var a = result.Where(_ => _.CreatedDate == result.Max(_ => _.CreatedDate)).FirstOrDefault();
                ViewBag.Period = a.Period.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                ViewBag.Msg = "ยังไม่เคยสร้างคิวงาน";
            }
            var r = _context.usp_CallStatus_Select(null);
            ViewBag.CallStatus = r;

            var queueStatus = _context.usp_QueueStatus_Select(null);
            ViewBag.QueueStatus = queueStatus;
            return View();
        }

        [HttpGet]
        public ActionResult QC()
        {
            var r = _context.usp_CallStatus_Select(null);
            ViewBag.CallStatus = r;
            return View();
        }

        #region API

        public ActionResult GetManagerQC(string period)
        {
            try
            {
                var cultureInfo = new CultureInfo("en-US");
                DateTime dateTime13;
                bool isSuccess4 = DateTime.TryParseExact(period, "dd/MM/yyyy", cultureInfo, DateTimeStyles.None, out dateTime13);

                if (!isSuccess4)
                {
                    return RedirectToAction("InternalServerError", "Error", new { Msg = "Formath DateTime NotFound" + "(" + period + ")" });
                }

                //Get Role
                int? assignToUserId = null;
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles().Split(',').ToList();
                if (roleListRaw.Contains("UWCancellation_QC"))
                {
                    assignToUserId = user.User_ID;
                };

                var pvQueueStatusCount = _context.usp_pvQueueStatusCount_Select(period: dateTime13, assignToUserId: assignToUserId);
                var pvProductGroupQueueCount = _context.usp_pvProductGroupQueueCount_Select(period: dateTime13, assignToUserId: assignToUserId);
                var data = new Dictionary<string, object>
                        {
                            {"pvQueueStatusCount", pvQueueStatusCount.FirstOrDefault() },
                            {"pvProductGroupQueueCount", pvProductGroupQueueCount.FirstOrDefault() },
                        };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message });
            }
        }

        public ActionResult GetTeamManageQueue(string period, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            try
            {
                var cultureInfo = new CultureInfo("en-US");
                //DateTime dateTime = DateTime.ParseExact(period, "D", cultureInfo);

                DateTime dateTime13;
                if (period != string.Empty)
                {
                    bool isSuccess4 = DateTime.TryParseExact(period, "dd/MM/yyyy", cultureInfo, DateTimeStyles.None, out dateTime13);

                    if (!isSuccess4)
                    {
                        return RedirectToAction("InternalServerError", "Error", new { Msg = "Formath DateTime NotFound" + "(" + period + ")" });
                    }
                    var result = _context.usp_pvQueueQCUser_Select(dateTime13, indexStart, pageSize, sortField, orderType).ToList();
                    var dt = new Dictionary<string, object>
                    {
                         {"draw", draw },
                         {"recordsTotal", result.Count() != 0 ? result : result.Count()},
                         {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                         {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsResult = false, Msg = "period is null" });
                }
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message });
            }
        }

        #endregion API
    }
}