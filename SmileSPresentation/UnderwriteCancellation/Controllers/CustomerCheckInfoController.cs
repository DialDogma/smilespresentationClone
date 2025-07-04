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
    public class CustomerCheckInfoController : Controller
    {
        #region Context

        private readonly UnderwriteCancellationEntities _context;

        public CustomerCheckInfoController()
        {
            _context = new UnderwriteCancellationEntities();
        }

        #endregion Context

        // GET: CancellationCustomerCheck ตรวจสอบข้อมูลลูกค้ายกเลิก
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,UWCancellation_QC")]
        public ActionResult UnderwriteCancelIndex(string queueId)
        {
            try
            {
                int? QueueId = null;
                if (!string.IsNullOrEmpty(queueId))
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                    ViewBag.QueueId = QueueId;
                    var callStatus = _context.usp_CallStatus_Select(null);
                    ViewBag.CallStatus = callStatus;

                    var _ = _context.usp_QueueFullDetailById_Select(QueueId).ToList();
                    ViewBag.QueueFullDetail = _.FirstOrDefault();

                    //sss url
                    ViewBag.SSS = Properties.Settings.Default.SSSURL;

                    if (_.Count == 0) return RedirectToAction("BadRequest", "Error", new { Msg = "ไม่พบคิวงาน" });

                    return View();
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error", new { Msg = e.Message });
            }
        }

        [Authorization(Roles = "Developer,UWCancellation_QC,UWCancellation_QCManager")]
        public ActionResult UnderwriteCancelView(string queueId)
        {
            try
            {
                int? QueueId = null;
                if (!string.IsNullOrEmpty(queueId))
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                    ViewBag.QueueId = QueueId;
                    var callStatus = _context.usp_CallStatus_Select(null);
                    ViewBag.CallStatus = callStatus;

                    var _ = _context.usp_QueueFullDetailById_Select(QueueId).ToList();
                    ViewBag.QueueFullDetail = _.FirstOrDefault();

                    //sss url
                    ViewBag.SSS = Properties.Settings.Default.SSSURL;

                    if (_.Count == 0) return RedirectToAction("BadRequest", "Error", new { Msg = "ไม่พบคิวงาน" });

                    return View();
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error", new { Msg = e.Message });
            }
        }

        #region API

        [HttpPost]
        public ActionResult UnderwriteCancelInsert(FormCollection form)
        {
            try
            {
                var queueId = form["queueId"];
                var isAudit = form["isAudit"]; //สถานะการตรวจสอบ
                var hdisAudit = form["hd-isAudit"]; //กรณีแก้ไข disable แล้วมันจะไม่ได้ค้ามา
                var cancelCallStatus = form["cancelCallStatus"]; //สถานะการโทร
                var cancelCallStatusRemark = form["cancelCallStatusRemark"]; //หมายเหตุการโทร

                var isCancelCause = form["isCancelCause"]; //สาเหตุการยกเลิก
                var cancelCauseRemark = form["cancelCauseRemark"]; //สาเหตุการยกเลิก ปัญหาอื่นๆ

                var cancelisIssue = form["cancelisIssue"]; //มีประเด็นหรือไม่
                var isIssueRemark = form["isIssueRemark"]; // รายละเอียด

                // กรณีแก้ไข disable แล้วมันจะไม่ได้ค้ามา ดึงค้าจาก hidden
                if (!string.IsNullOrEmpty(hdisAudit))
                {
                    isAudit = hdisAudit;
                }

                int updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                var dtoInsert = new UnderwriteCancelDto_Insert();

                if (!string.IsNullOrEmpty(queueId))
                {
                    if (!string.IsNullOrEmpty(isAudit))
                    {
                        switch (isAudit)
                        {
                            case "False": //รอตรวจสอบ
                                dtoInsert.IsAudit = Convert.ToBoolean(isAudit);
                                dtoInsert.CallStatusId = Convert.ToInt32(cancelCallStatus);
                                dtoInsert.CallRemark = cancelCallStatusRemark;
                                break;

                            case "True": //ตรวจสอบแล้ว
                                dtoInsert.IsAudit = Convert.ToBoolean(isAudit);
                                dtoInsert.CallStatusId = Convert.ToInt32(cancelCallStatus);
                                dtoInsert.IsIssue = Convert.ToBoolean(cancelisIssue);
                                dtoInsert.CancelCauseRemark = cancelCauseRemark;
                                dtoInsert.IssueRemark = isIssueRemark;
                                var dataArray = isCancelCause.Split(',').ToList();
                                foreach (var item in dataArray)
                                {
                                    switch (item.ToString())
                                    {
                                        case "1":
                                            dtoInsert.IsCancelCause1 = true; //ปัญหาด้านการเงิน
                                            break;

                                        case "2":
                                            dtoInsert.IsCancelCause2 = true; //มีประกันหลายบริษัท/หลายกรมธรรม์
                                            break;

                                        case "3":
                                            dtoInsert.IsCancelCause3 = true; //การบริการผู้แทน/พนักงาน
                                            break;

                                        case "4":
                                            dtoInsert.IsCancelCause4 = true; //การเคลมสินไหมล่าช้า
                                            break;

                                        case "5":
                                            dtoInsert.IsCancelCause5 = true; //เบิกเคลมไม่ได้
                                            break;

                                        case "6":
                                            dtoInsert.IsCancelCause6 = true; //ไม่มีการเบิกเคลม
                                            break;

                                        case "7":
                                            dtoInsert.IsCancelCause7 = true; //ผลิตภัณฑ์ไม่ตรงตามความต้องการ
                                            break;

                                        case "8":
                                            dtoInsert.IsCancelCause8 = true; //การคัดกรองและได้รับกรมธรรม์เป็นไปอย่างล่าช้า
                                            break;

                                        case "9":
                                            dtoInsert.IsCancelCause9 = true; //ช่องทางในการชำระเบี้ยประกันไม่ตอบโจทย์
                                            break;

                                        case "10":
                                            dtoInsert.IsCancelCause10 = true; //ความน่าเชื่อถือของบริษัทสยามสไมล์
                                            break;

                                        case "11":
                                            dtoInsert.IsCancelCause11 = true; //ความน่าเชื่อถือของบริษัทรับประกัน
                                            break;

                                        case "12":
                                            dtoInsert.IsCancelCause12 = true; //การเดินทางและที่ตั้งสาขา
                                            break;

                                        case "13":
                                            dtoInsert.IsCancelCause13 = true; //สถานพยาบาล/คู่สัญญา
                                            break;

                                        case "14":
                                            dtoInsert.IsCancelCause14 = true; //ไม่ทราบว่าถูกยกเลิก
                                            break;

                                        case "15":
                                            dtoInsert.IsCancelCause15 = true; //ปัญหาอื่นๆ
                                            break;

                                        case "16":
                                            dtoInsert.IsCancelCause16 = true; //ไม่ให้ข้อมูล
                                            break;

                                        case "17":
                                            dtoInsert.IsCancelCause17 = true; //ประสงค์ยกเลิก
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        return Json(new { IsResult = false, Msg = "สถานะการตรวจสอบไม่ถูกต้อง หรือ เป็นค่าว่าง" });
                    }
                }
                else
                {
                    return Json(new { IsResult = false, Msg = "queueId เป็นค่าว่าง" });
                }

                var result = _context.usp_QueueResult_Insert(Convert.ToInt32(queueId),
                                                                dtoInsert.IsAudit,
                                                                dtoInsert.CallStatusId,
                                                                dtoInsert.CallRemark,
                                                                dtoInsert.IsIssue,
                                                                dtoInsert.IssueRemark,
                                                                dtoInsert.IsCancelCause1,
                                                                dtoInsert.IsCancelCause2,
                                                                dtoInsert.IsCancelCause3,
                                                                dtoInsert.IsCancelCause4,
                                                                dtoInsert.IsCancelCause5,
                                                                dtoInsert.IsCancelCause6,
                                                                dtoInsert.IsCancelCause7,
                                                                dtoInsert.IsCancelCause8,
                                                                dtoInsert.IsCancelCause9,
                                                                dtoInsert.IsCancelCause10,
                                                                dtoInsert.IsCancelCause11,
                                                                dtoInsert.IsCancelCause12,
                                                                dtoInsert.IsCancelCause13,
                                                                dtoInsert.IsCancelCause14,
                                                                dtoInsert.IsCancelCause15,
                                                                dtoInsert.IsCancelCause16,
                                                                dtoInsert.IsCancelCause17,
                                                                dtoInsert.CancelCauseRemark,
                                                                updatedByUserId).FirstOrDefault();
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

        public ActionResult GetQueueDetail(string queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            try
            {
                var result = _context.usp_QueueDetail_Select(Convert.ToInt32(queueId), indexStart, pageSize, sortField, orderType).ToList();
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

        [HttpPost]
        public ActionResult QueueDetailRemarkUpdate(FormCollection form)
        {
            try
            {
                var queueDetailId = form["queueDetailId"];
                var modalRemark = form["modalRemark"];
                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_QueueDetailRemark_Update(Convert.ToInt32(queueDetailId), modalRemark, userId).FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message });
            }
        }

        public ActionResult GetQueueLog(string queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            try
            {
                var result = _context.usp_QueueLog_Select(Convert.ToInt32(queueId), indexStart, pageSize, sortField, orderType).ToList();
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

        #endregion API
    }
}