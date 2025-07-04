using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using UnderwriteCancellation.Helper;
using UnderwriteCancellation.Models;

namespace UnderwriteCancellation.Controllers
{
    [Authorization]
    public class IssuedQueueController : Controller
    {
        #region Context

        private readonly UnderwriteCancellationEntities _context;

        public IssuedQueueController()
        {
            _context = new UnderwriteCancellationEntities();
        }

        #endregion Context

        // GET: IssuedQueue คิวงานมีประเด็น
        public ActionResult Index()
        {
            return View();
        }

        //ปิดคิวงานมีประเด็น
        [Authorization(Roles = "Developer,UWCancellation_QCManager,UWCancellation_BusinessPromoteTeam")]
        public ActionResult CloseIssuedQueue(string queueId)
        {
            int? QueueId = null;
            if (!string.IsNullOrEmpty(queueId))
            {
                byte[] dataQueue = Convert.FromBase64String(queueId);
                var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                QueueId = Convert.ToInt32(decodedQueueIdString);
                ViewBag.QueueId = QueueId;

                var _ = _context.usp_QueueFullDetailById_Select(QueueId).ToList();
                var QueueFullDetail = _.FirstOrDefault();
                ViewBag.QueueFullDetail = QueueFullDetail;
                var cancelCauseFullDetail = QueueFullDetail.CancelCauseFullDetail.Split(',').ToList();
                var a1 = new List<string>();
                var a2 = new List<string>();
                var a3 = new List<string>();

                for (int i = 0; i < cancelCauseFullDetail.Count; i++)
                {
                    if (a1.Count() <= 5)
                    {
                        a1.Add(cancelCauseFullDetail[i]);
                    }
                    else if (a2.Count() <= 5)
                    {
                        a2.Add(cancelCauseFullDetail[i]);
                    }
                    else if (a3.Count() <= 5)
                    {
                        a3.Add(cancelCauseFullDetail[i]);
                    }
                    else
                    {
                        a3.Add(cancelCauseFullDetail[i]);
                    }
                }
                ViewBag.a1 = a1;
                ViewBag.a2 = a2;
                ViewBag.a3 = a3;

                if (_.Count == 0) return RedirectToAction("BadRequest", "Error", new { Msg = "ไม่พบคิวงาน" });

                //sss url
                ViewBag.SSS = Properties.Settings.Default.SSSURL;
                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        [HttpPost]
        public ActionResult ResultRemarkInsert(FormCollection form)
        {
            try
            {
                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var queueId = form["queueId"];
                if (string.IsNullOrEmpty(queueId))
                {
                    return Json(new { IsResult = false, Msg = "queueId เป็นค่าว่าง" });
                }
                var txtResultRemark = form["txtResultRemark"];
                var result = _context.usp_QueueIssueComplete_Update(Convert.ToInt32(queueId), txtResultRemark, userId).FirstOrDefault();

                //Signal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group("1234").ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message });
            }
        }

        public ActionResult GetQueueDocument(string queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            try
            {
                var result = _context.usp_QueueDocument_Select(Convert.ToInt32(queueId), 44, indexStart, pageSize, sortField, orderType).ToList();
                var dt = new Dictionary<string, object>
                {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
    }
}