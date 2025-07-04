using SmileSUnderwrite.Helper;
using SmileSUnderwrite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using SmileSUnderwrite.DatacenterOrganizeService;
using SmileSUnderwrite.SmileSPAService;

namespace SmileSUnderwrite.Controllers
{
    [Authorization]
    public class QueueCallController : Controller
    {
        #region dbContext

        private UnderwriteDBContext _context;
        private SSSPAEntities _db;

        public QueueCallController()

        {
            _context = new UnderwriteDBContext();
            _db = new SSSPAEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // index: QueueCall
        public ActionResult QueueCallIndex()
        {
            ViewBag.QueueTypes = _context.usp_QueueType_Select().ToList();
            ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();

            return View();
        }

        public ActionResult Detail(int queueId)
        {
            //insert doc
            _context.usp_DocumentByQueue_Insert(queueId, "26");
            //insert underwrite by queue id
            var underwrite = _context.usp_Underwrite_Insert(queueId, null).FirstOrDefault();
            ViewBag.QueueId = queueId;
            //get call detail id
            var underwriteDetail = _context.usp_Underwrite_Select(underwrite).FirstOrDefault();
            ViewBag.underwriteStatus = underwriteDetail.UnderwriteStatus;
            ViewBag.underwriteStatusId = underwriteDetail.UnderwriteStatusId;
            ViewBag.UnderwiteId = underwrite;
            //call other insurance
            using (var datacenter = new OrganizeServiceClient())
            {
                ViewBag.OtherInsurance = datacenter.GetOtherInsurance(null);
            }
            //get question id
            var queueDetail = _context.usp_Queue_Select(queueId, null, null, null, null, null, null, null, null, null, "", "", "", null).FirstOrDefault();
            ViewBag.QuestionId = queueDetail.QuestionId;
            ViewBag.QueueCreated = queueDetail.CreatedDate;
            ViewBag.QueueTypeDetail = queueDetail.QueueTypeDetail;
            var AppDetail = _context.usp_ApplicationDetail_Select(queueDetail.ReferenceCode).FirstOrDefault();
            ViewBag.AppDetail = AppDetail;
            var db = new ApplicationServiceClient();
            var appResult = db.GetApplicationDetailForPAUnderwrite(queueDetail.ReferenceCode);
            ViewBag.SchoolId = appResult.School_id;
            //call ref id
            ViewBag.AppId = appResult.App_id;
            //check if underwriteDetail is not null
            if (underwriteDetail != null)
            {
                //check if question id
                if (underwriteDetail.QuestionId == 2)
                {
                    var resultQ2 = _context.usp_UdwQ2_Select(underwrite).FirstOrDefault();

                    if (resultQ2 != null)
                    {
                        ViewBag.ResultQ2 = resultQ2;

                        if (resultQ2.A9 != null)
                        {
                            ViewBag.DatePayment = resultQ2.A9.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
                else if (underwriteDetail.QuestionId == 3)
                {
                    var resultQ3 = _context.usp_UdwQ3_Select(underwrite).FirstOrDefault();
                    if (resultQ3 != null)
                    {
                        ViewBag.ResultQ3 = resultQ3;
                    }
                }
                else if (underwriteDetail.QuestionId == 4)
                {
                    var resultQ4 = _context.usp_UdwQ4_Select(underwrite).FirstOrDefault();
                    if (resultQ4 != null)
                    {
                        ViewBag.ResultQ4 = resultQ4;
                        if (resultQ4.A31 != null)
                        {
                            ViewBag.DatePayment2 = resultQ4.A31.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
            }
            //check if have call
            var callSelect = _context.usp_Call_Select(underwrite, true).FirstOrDefault();
            if (callSelect != null)
            {
                ViewBag.CallDetail = callSelect;
            }

            //get document
            var docLink = _context.usp_Document_Select(null, queueId, null, "26", null, null, null, null, null).FirstOrDefault();
            ViewBag.ScanUrl = docLink.UrlLinkScan;
            ViewBag.OpenUrl = docLink.UrlLinkOpen;
            ViewBag.DocCount = docLink.FileCount;

            return View();
        }

        public ActionResult Log(int ReferrenceCode)
        {
            ViewBag.ReferrenceCode = ReferrenceCode;
            ViewBag.AppId = ReferrenceCode.ToString();
            return View();
        }

        public ActionResult Report()
        {
            ViewBag.QueueTypes = _context.usp_QueueType_Select().ToList();
            ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();

            return View();
        }

        public ActionResult ReportToExcel(FormCollection form)
        {
            int queueType = Convert.ToInt32(form["ddlQueueType"]);
            string dFrom = form["dtpDateFrom"];
            string dTo = form["dtpDateTo"];
            string yearData = form["ddlYear"];

            if (queueType == 3)
            {
                yearData = null;
            }

            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int userId;

            userId = Convert.ToInt32(Session["User_ID"]);

            int? queueTypeId = null;

            if (queueType != -1)
            {
                queueTypeId = queueType;
            }

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);

            //result = _context.usp_Queue_Select(null, queueType, statusCall, null, dateFrom, dateTo, pageStart, pageSize, sortField, orderType, search).ToList();

            //เพิ่ม สาขา,เบี้ย,แผนประกัน
            var result = _context.usp_QueueUnderwrite_Select(null, queueTypeId, null, null, dateFrom, dateTo, 0, 10000000, null, null, null, yearData).Select(
                x => new
                {
                    AppID = x.ReferenceCode,
                    ชื่อโรงเรียน = x.Detail1,
                    ที่อยู่ = x.Detail2,
                    สาขา = x.BranchDetail,
                    แผนประกัน = x.ProductDetail,
                    เบี้ย = x.Premium,
                    ผลการโทรคัดกรอง = x.UnderwriteStatusDetail,
                    ประเภทคิว = x.QueueTypeDetail,
                    ผู้รับผิดชอบ = x.AssignToName,
                    ผลการดำเนินการ = x.QueueStatus,
                    หมายเหตุ = x.UnderwriteRemark
                }).ToList();

            ExcelManager.ExportToExcel(this.HttpContext, result, "Report_QueueUnderwrite", "Report_QueueUnderwrite");

            return View();
        }

        //GetDatatableQueueCall
        public JsonResult GetDatatableQueueCall(int? draw = null, int? pageSize = null, int? pageStart = null,
            string sortField = null, string orderType = null, string search = null,
            int? queueType = null, string dFrom = null, string dTo = null, int? statusCall = null, string yearData = null)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int userId;

            userId = Convert.ToInt32(Session["User_ID"]);

            //userId = 10;

            int? queueTypeId = null;

            if (queueType != -1)
            {
                queueTypeId = queueType;
            }

            string strYearData = null;
            if (!string.IsNullOrEmpty(yearData)) strYearData = yearData;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);

            var result = new List<usp_QueueUnderwrite_Select_Result>();

            //result = _context.usp_Queue_Select(null, queueType, statusCall, null, dateFrom, dateTo, pageStart, pageSize, sortField, orderType, search).ToList();

            result = _context.usp_QueueUnderwrite_Select(null, queueTypeId, statusCall, userId, dateFrom, dateTo, pageStart, pageSize, sortField, orderType, search, strYearData).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableReport(int? draw = null, int? pageSize = null, int? pageStart = null,
           string sortField = null, string orderType = null, string search = null,
           int? queueType = null, string dFrom = null, string dTo = null, int? statusCall = null, string yearData = null)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            int? queueTypeId = null;

            if (queueType != -1)
            {
                queueTypeId = queueType;
            }

            string strYearData = null;
            if (!string.IsNullOrEmpty(yearData)) strYearData = yearData;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);

            var result = _context.usp_QueueUnderwrite_Select(null, queueTypeId, null, null, dateFrom, dateTo, pageStart, pageSize, sortField,
                orderType, search, strYearData).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //GetDatatableHisUnderWrite
        public JsonResult GetDatatableHisUnderWrite(int? draw = null, int? pageSize = null, int? pageStart = null,
            string sortField = null, string orderType = null, string search = null, int? ReferenceCode = null)
        {
            var result = new List<usp_UnderwriteLog_Select_Result>();

            result = _context.usp_UnderwriteLog_Select(ReferenceCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Detail(FormCollection form)
        {
            var underwiteId = Convert.ToInt32(form["hd_underwriteId"]);
            var hiddenRemark = form["hiddenRemark"];
            var remark = form["txtRemarkCheck"];
            if (remark.Trim() == "")
            {
                remark = form["hiddenRemark"];
            }
            else
            {
                var strA = hiddenRemark;
                var strB = "|";
                var strC = remark;
                remark = String.Concat(strA, strB, strC);
            }
            var user = Convert.ToInt32(Session["User_ID"]);
            var queueId = Convert.ToInt32(form["hd_queueId"]);
            var result = _context.usp_UnderwriteRemark_Update(underwiteId, remark, user).FirstOrDefault();

            //check form
            bool isCheatValue = (form["chk_IsCheat"] == "on");
            var cheatRemark = form["txtRemarkCheck"];
            //TODO:insert issued sp

            return RedirectToAction("Detail", new { queueId = queueId });
        }

        [HttpGet]
        public ActionResult RefreshDocCount(int queueId)
        {
            try
            {
                var docLink = _context.usp_Document_Select(null, queueId, null, "26", null, null, null, null, null).FirstOrDefault();

                return Json(docLink.FileCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}