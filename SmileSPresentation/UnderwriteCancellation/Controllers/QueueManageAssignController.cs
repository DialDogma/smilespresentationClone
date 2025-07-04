using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UnderwriteCancellation.Helper;
using UnderwriteCancellation.Models;

namespace UnderwriteCancellation.Controllers
{
    [Authorization]
    public class QueueManageAssignController : Controller
    {
        private readonly UnderwriteCancellationEntities _context;

        public QueueManageAssignController()
        {
            _context = new UnderwriteCancellationEntities();
        }

        // GET: QueueManageAssign จัดการคิวงาน
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueueManageAssign(string emp)
        {
            if (!string.IsNullOrEmpty(emp))
            {
                ViewBag.emp = emp;
            }
            else
            {
                ViewBag.emp = "";
            }
            var CallStatus = _context.usp_CallStatus_Select(null).ToList();
            ViewBag.CallStatus = CallStatus;

            var QCUser = _context.usp_QCUser_Select(null, 0, 99999, null, null).ToList();
            ViewBag.QCUser = QCUser;

            return View();
        }

        //รอปิดคิวงาน
        public ActionResult WaitCloseQueue(string callStatusId, string dcr)
        {
            ViewBag.DCR = dcr == null ? "" : dcr;

            if (dcr != null)
            {
                var date = dcr.Split('/');
                ViewBag.date = date[0];
                ViewBag.month = date[1];
                ViewBag.year = date[2];
            }

            var conVcallStatusId = Convert.ToInt32(callStatusId);
            ViewBag.CallStatusId = conVcallStatusId;
            var CallStatus = _context.usp_CallStatus_Select(null).ToList();
            ViewBag.CallStatus = CallStatus;
            return View();
        }

        //ปิดคิวงานสำเร็จ
        public ActionResult CloseQueueSuccess(string queueStatusId, string dcr)
        {
            ViewBag.DCR = dcr == null ? "" : dcr;

            if (dcr != null)
            {
                var date = dcr.Split('/');
                ViewBag.date = date[0];
                ViewBag.month = date[1];
                ViewBag.year = date[2];
            }

            var conVQueueStatusId = Convert.ToInt32(queueStatusId);
            ViewBag.QueueStatusId = conVQueueStatusId;
            var QueueStatus = _context.usp_QueueStatus_Select(null).ToList().OrderBy(_ => _.QueueStatusId);
            ViewBag.QueueStatus = QueueStatus;
            return View();
        }

        //คิวงานมีประเด็นรอดำเนินการ
        public ActionResult Queuepending(string dcr)
        {
            ViewBag.DCR = dcr == null ? "" : dcr;
            if (dcr != null)
            {
                var date = dcr.Split('/');
                ViewBag.date = date[0];
                ViewBag.month = date[1];
                ViewBag.year = date[2];
            }

            var Branch = _context.usp_Branch_Select(null).ToList();
            ViewBag.Branch = Branch;
            return View();
        }

        //คิวงานมีประเด็นดำเนินการแล้ว
        public ActionResult QueueIssueSuccess(string dcr)
        {
            ViewBag.DCR = dcr == null ? "" : dcr;
            if (dcr != null)
            {
                var date = dcr.Split('/');
                ViewBag.date = date[0];
                ViewBag.month = date[1];
                ViewBag.year = date[2];
            }
            var Branch = _context.usp_Branch_Select(null).ToList();
            ViewBag.Branch = Branch;
            return View();
        }

        #region API

        #region รอปิดคิวงาน

        public JsonResult GetWaitCloseQueue(string period = null, int? callStatusId = null, string payerName = null, string payerIdCard = null, string payerPhone = null, int? draw = null, int? pageSize = null, int? indexStart = null,
                                                  string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }
            int? assignToUserId = null;
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var roleListRaw = OAuth2Helper.GetRoles().Split(',').ToList();
            if (!roleListRaw.Contains("Developer") && !roleListRaw.Contains("UWCancellation_QCManager"))
            {
                assignToUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            }
            var result = _context.usp_QueuePending_Select(
                c_startCoverdate,
                assignToUserId,
                (callStatusId == -1 ? null : callStatusId),
                (payerName == string.Empty ? null : payerName),
                (payerIdCard == string.Empty ? null : payerIdCard),
                (payerPhone == string.Empty ? null : payerPhone),
                indexStart,
                pageSize,
                sortField,
                orderType).ToList();

            var countToCloseQueue = 0;
            if (result.Count != 0)
            {
                var result2 = _context.usp_QueuePending_Select(
                   c_startCoverdate,
                   assignToUserId,
                   (callStatusId == -1 ? null : callStatusId),
                   (payerName == string.Empty ? null : payerName),
                   (payerIdCard == string.Empty ? null : payerIdCard),
                   (payerPhone == string.Empty ? null : payerPhone),
                   null,
                   999999999,
                   sortField,
                   orderType).ToList();

                var TotalCount = result2.Count() != 0 ? result2.FirstOrDefault()?.TotalCount : result2.Count();
                for (var i = 0; i < TotalCount; i++)
                {
                    if (result2[i].CallStatusId != 2)
                    {
                        countToCloseQueue++;
                    }
                }
            }

            var dt = new Dictionary<string, object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"data", result.ToList()},
                 {"countToCloseQueue", countToCloseQueue}
             };
            //var dt = new Dictionary<string, object>();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion รอปิดคิวงาน

        #region ปิดคิวงานสำเร็จ

        public JsonResult GeDataTableCloseQueueSuccess(string period = null, int? queueStatusId = null, string payerName = null, string payerIdCard = null, string payerPhone = null, int? draw = null, int? pageSize = null, int? indexStart = null,
                                                  string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }
            int? assignToUserId = null;
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var roleListRaw = OAuth2Helper.GetRoles().Split(',').ToList();
            if (!roleListRaw.Contains("Developer") && !roleListRaw.Contains("UWCancellation_QCManager"))
            {
                assignToUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            }
            var result = _context.usp_QueueComplete_Select(c_startCoverdate, assignToUserId, queueStatusId, (payerName == string.Empty ? null : payerName), (payerIdCard == string.Empty ? null : payerIdCard), (payerPhone == string.Empty ? null : payerPhone), indexStart, pageSize, sortField, orderType).ToList();
            var dt = new Dictionary<string, object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"data", result.ToList()}
             };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateCloseQueueAll(int? callStatusId, string payerPhone, string payerIdCard, string payerName, string period)
        {
            int updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            int assignToUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            DateTime? c_period = null;

            if (period != null && period != "")
            {
                c_period = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }

            var rs = _context.usp_QueueCloseAll_Update(c_period, assignToUserId, (callStatusId == -1 ? null : callStatusId), (payerName == string.Empty ? null : payerName), (payerIdCard == string.Empty ? null : payerIdCard), (payerPhone == string.Empty ? null : payerPhone), updatedByUserId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateCloseQueue(int queueId)
        {
            int updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _context.usp_QueueClose_Update(queueId, updatedByUserId).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueueCancelUpdate(int queueId)
        {
            int updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _context.usp_QueueCancel_Update(queueId, updatedByUserId).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #endregion ปิดคิวงานสำเร็จ

        #region จัดการคิวงาน

        public JsonResult getDataQueueManager(int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, int? callStatusId = null, string payerName = null, string payerIdCard = null, int? assignToUserId = null)
        {
            var result = _context.usp_QueueAssign_Select(
                (callStatusId == -1 ? null : callStatusId),
                assignToUserId,
                (payerName == string.Empty ? null : payerName),
                (payerIdCard == string.Empty ? null : payerIdCard),
                indexStart,
                pageSize,
                sortField,
                orderType).ToList();

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

        public JsonResult updateAssignUserIdInQueue(string[] queueArray, int assignToUserId)
        {
            var queueId = "";

            if (queueArray != null)
            {
                queueId = string.Join(",", queueArray);
            }
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueAssign_Update(queueId, assignToUserId, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion จัดการคิวงาน

        #region คิวงานมีประเด็นรอดำเนินการ

        public JsonResult GetQueueIssuePending(string period = null, int? branchId = null, string payerName = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }
            int assignToUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueIssuePending_Select(c_startCoverdate, null, (branchId == -1 ? null : branchId), (payerName == string.Empty ? null : payerName), indexStart, pageSize, sortField, orderType).ToList();
            var dt = new Dictionary<string, object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"data", result.ToList()}
             };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion คิวงานมีประเด็นรอดำเนินการ

        #region คิวงานมีประเด็นดำเนินการแล้ว

        public JsonResult GetQueueIssueComplete(string period = null, int? branchId = null, string payerName = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }
            int assignToUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueIssueComplete_Select(c_startCoverdate, null, (branchId == -1 ? null : branchId), (payerName == string.Empty ? null : payerName), indexStart, pageSize, sortField, orderType).ToList();
            var dt = new Dictionary<string, object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"data", result.ToList()}
             };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion คิวงานมีประเด็นดำเนินการแล้ว

        public ActionResult GetQCuser(string QCuser)
        {
            var QCUser = _context.usp_QCUser_Select(QCuser, 0, 99999, null, null).ToList();
            return Json(QCUser, JsonRequestBehavior.AllowGet);
        }

        #endregion API
    }
}

/*GetCountToCloseQueue*/