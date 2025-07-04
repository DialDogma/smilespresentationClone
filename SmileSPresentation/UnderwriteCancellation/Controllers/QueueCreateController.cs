using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UnderwriteCancellation.Helper;
using UnderwriteCancellation.Models;

namespace UnderwriteCancellation.Controllers
{
    [Authorization]
    public class QueueCreateController : Controller
    {
        private readonly UnderwriteCancellationEntities _context;

        public QueueCreateController()
        {
            _context = new UnderwriteCancellationEntities();
        }

        // GET: QueueCreate สร้างคิวงาน
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueueCreateCancellation()
        {
            return View();
        }

        public JsonResult GetCreateQueueHistory(string period = null, int? draw = null, int? pageSize = null, int? indexStart = null,
                                                  string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }

            var result = _context.usp_QueueCreate_Select(indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"data", result.ToList()}
             };
            //var dt = new Dictionary<string, object>();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCreateQueueDetail(string period = null, int? draw = null, int? pageSize = null, int? indexStart = null,
                                                  string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }
            var result = _context.usp_pvQueueQCUser_Select(c_startCoverdate, indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"data", result.ToList()}
             };
            //var dt = new Dictionary<string, object>();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCount(string period)
        {
            // int assignToUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }

            var count = _context.usp_pvQueueStatusCount_Select(c_startCoverdate, null).Single();

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCreateQueue(string dcrDate)
        {
            try
            {
                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                DateTime? dcr = null;
                if (dcrDate != null && dcrDate != "")
                {
                    dcr = GlobalFunction.ConvertDatePickerToDate_BE(dcrDate);
                }
                var result = _context.usp_QueueCreate_Insert(dcr, userID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}