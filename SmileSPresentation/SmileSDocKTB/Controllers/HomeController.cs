using System.Web.Mvc;
using SmileSDocKTB.Helper;
using System.Collections.Generic;
using SmileSDocKTB.Models;
using System.Linq;
using System;

namespace SmileSDocKTB.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        #region Context

        private DocumentKTBContext _context;

        public HomeController()

        {
            _context = new DocumentKTBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        public ActionResult Index(FormCollection form)
        {
            var createdById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.Test = "ลอง ลอง";
            var txtAccCode = form["AccCode"];
            if (txtAccCode != null) { txtAccCode = txtAccCode.Replace("-", ""); };
            var txtAccName = form["AccName"];
            ViewBag.AccCode = txtAccCode;
            ViewBag.AccName = txtAccName;
            var obj = _context.usp_BoxNo_Select().ToList();
            ViewBag.BoxList = obj;

            ViewBag.SSSDocPath = SmileSDocKTB.Properties.Settings.Default.SSSDocPath;

            ViewBag.CurrentUser = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var objNo = _context.usp_DocumentKTB_Insert(createdById).Single();

            ViewBag.RunNo = objNo.DocumentCode;
            ViewBag.AccountKTBId = objNo.AccountKTBId;
            ViewBag.BankId = "3";

            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult CheckDocument(string DocumentNo)
        {
            var objNo = _context.usp_CountDocument(DocumentNo).Single();
            var result = Convert.ToString(objNo.dCOUNT);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CHECK_TEST(string AccCode)
        {
            string result_Duplicate = "";
            string result_AccountName = "";
            string result_Exist = "";

            //Call sp
            if (AccCode != null) { AccCode = AccCode.Replace("-", ""); };
            int count = 0;

            count = _context.usp_AccountKTBDocumnet_Select(AccCode).Count();
            // เช็คว่าเลขบัญชีนี้เคยบันทึกหรือยัง
            //if (count != 0)
            //{
            //    result_Duplicate = "Double";
            //}
            //// ถ้าซ้ำ
            //else
            //{
            //    result_Duplicate = "";
            //}

            //try
            //{
            //    // เช็คว่าอยู่ใน list ktb หรือเปล่า
            //    var obj = _context.usp_AccountKTB_Select(AccCode).Single();
            //    if (obj.AccountName == null || obj.AccountName == "") { result_Exist = ""; } else { result_Exist = obj.AccountNo; }
            //}
            //catch
            //{
            //    result_Exist = "";
            //}

            // ดึงชื่อบัญชี
            try
            {
                var obj2 = _context.usp_MasterAccountNo_Select(AccCode).Single();
                if (obj2.AccountName == null || obj2.AccountName == "") { result_AccountName = ""; } else { result_AccountName = obj2.AccountName; }
            }
            catch
            {
                result_AccountName = "";
            }

            var result = new string[3];

            result[0] = result_Duplicate;
            result[1] = result_AccountName;
            result[2] = result_Exist;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SAVE(string RunNo, string AccCode, string AccName, string AccBox
                                , string DocumentResultStatusId
                                , string InCorrect_Signature
                                , string InCorrect_AccountNo
                                , string InCorrect_ContactBranch
                                , string InCorrect_AccountClosed
                                , string InCorrect_AccountUnAvailable
                                , string InCorrect_AccountUnmatch
                                , string InCorrect_Other
                                , string InCorrect_OtherRemark
                                , string AccountKTBId
                                , string BankId, string Correct)

        {
            // Call sp
            var createdById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var obj = _context.usp_DocumentKTB_Update(Convert.ToInt32(AccountKTBId), Convert.ToInt32(BankId), AccCode, AccName, Convert.ToInt32(AccBox),
                                                  Convert.ToBoolean(Correct), Convert.ToBoolean(InCorrect_Signature),
                                                  Convert.ToBoolean(InCorrect_AccountNo), Convert.ToBoolean(InCorrect_ContactBranch),
                                                  Convert.ToBoolean(InCorrect_AccountClosed), Convert.ToBoolean(InCorrect_AccountUnAvailable),
                                                  Convert.ToBoolean(InCorrect_AccountUnmatch), Convert.ToBoolean(InCorrect_Other), InCorrect_OtherRemark
                                                  , Convert.ToInt32(DocumentResultStatusId), createdById).FirstOrDefault();
            var result = new string[4];

            result[0] = Convert.ToString(obj.IsResult);

            var objNo = _context.usp_DocumentKTB_Insert(createdById).Single();

            result[1] = objNo.DocumentCode;
            result[2] = Convert.ToString(objNo.AccountKTBId);
            result[3] = "3";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            return View();
        }

        public JsonResult GetDatatableReport(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, string dfrom = null, string dto = null)
        {
            int userID;

            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dfrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dto);

            //userID = Convert.ToInt32(Session["User_ID"]);

            //var result = _context.usp_Queue_Select(null, null, null, 1, userID, null, null, null, null
            //    , pageStart, pageSize, sortField, orderType, search, appID, null, null, custName, payerName).ToList();

            var result = _context.usp_AccountKTBDocumentMonitor_Select_v2(dateFrom, dateTo, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportExcel(FormCollection form)
        {
            //int QueueTypeId = Convert.ToInt32(queueType);

            var df = form["dtpDateFrom"];
            var dt = form["dtpDateTo"];

            DateTime? dfrom = GlobalFunction.ConvertDatePickerToDate_BE(df);
            DateTime? dto = GlobalFunction.ConvertDatePickerToDate_BE(dt);

            var result = GlobalFunction.ToDataTable(_context.usp_ReportAccountKTBDocument_Select_v2(null, dfrom, dto).ToList());

            ExcelManager.ExportToExcel(this.HttpContext, "Report_DocumentKTB", result, "Report_DocumentKTB");

            return View();
        }

        public ActionResult Monitor(FormCollection form)

        {
            var obj = _context.usp_ProcessKtb_Select().Single();
            ViewBag.GetProcess = obj;

            //obj.NotfoundDoc จำนวนยังไม่เจอ
            //obj.AllDoc จำนวนทั้งหมด
            //obj.Process กี่%
            return View();
        }

        public ActionResult Monitor_All()
        {
            return View();
        }

        public ActionResult RefreshDate(FormCollection form)

        {
            var obj = _context.usp_ProcessKtb_Select().Single();

            var result = new string[5];

            result[0] = String.Format("{0:0,0}", obj.foundDoc);
            result[1] = String.Format("{0:0,0}", obj.NotfoundDoc);
            result[2] = String.Format("{0:0,0}", obj.AllDoc);
            result[3] = Convert.ToString(obj.Process) + "%";
            result[4] = String.Format("{0:0,0}", obj.foundDoc) + "/" + String.Format("{0:0,0}", obj.AllDoc);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshDoughnutChart(FormCollection form)

        {
            var result = new string[2];

            var obj = _context.usp_ReportAccountKTBGraphSummary_Select(null, null, null, null, null, null, null, true).Single();

            result[0] = Convert.ToString(obj.Correct);
            result[1] = Convert.ToString(obj.InCorrect);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshBarChart(FormCollection form)

        {
            var result = new string[9];
            var obj = _context.usp_ReportAccountKTBGraphSummary_Select(null, null, null, null, null, null, null, true).Single();

            //result[0] = "100";
            //result[1] = "200";
            //result[2] = "300";
            //result[3] = "400";
            //result[4] = "500";
            //result[5] = "600";
            //result[6] = "700";
            //result[7] = "800";
            //result[8] = "900";

            result[0] = Convert.ToString(obj.Correct);
            result[1] = Convert.ToString(obj.InCorrect_Signature);
            result[2] = Convert.ToString(obj.InCorrect_AccountNo);
            result[3] = Convert.ToString(obj.InCorrect_ContactBranch);
            result[4] = Convert.ToString(obj.InCorrect_AccountClosed);
            result[5] = Convert.ToString(obj.InCorrect_AccountUnAvailable);
            result[6] = Convert.ToString(obj.InCorrect_AccountUnmatch);
            result[7] = Convert.ToString(obj.InCorrect_Other);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshPieBarChart()
        {
            var result = new string[3];

            var obj = _context.usp_ReportAccountKTBGraphSummary_Select(null, null, null, null, null, null, null, false).Single();

            result[0] = Convert.ToString(obj.Correct);
            result[1] = Convert.ToString(obj.InCorrect);
            result[2] = String.Format("{0:0,0}", obj.ALL);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshHBarChar()
        {
            var result = new string[9];
            var obj = _context.usp_ReportAccountKTBGraphSummary_Select(null, null, null, null, null, null, null, false).Single();

            result[0] = Convert.ToString(obj.Correct);
            result[1] = Convert.ToString(obj.InCorrect_Signature);
            result[2] = Convert.ToString(obj.InCorrect_AccountNo);
            result[3] = Convert.ToString(obj.InCorrect_ContactBranch);
            result[4] = Convert.ToString(obj.InCorrect_AccountClosed);
            result[5] = Convert.ToString(obj.InCorrect_AccountUnAvailable);
            result[6] = Convert.ToString(obj.InCorrect_AccountUnmatch);
            result[7] = Convert.ToString(obj.InCorrect_Other);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccountKTBGraphBoxCountQuantity()
        {
            var result = _context.usp_ReportAccountKTBGraphBoxCountQuantity_Select().ToList();
            //var obj = _context.usp_ReportAccountKTBGraphSummary_Select(null, null, null, null, null, null, null, false).Single();

            //result[0] = "100";
            //result[1] = "200";
            //result[2] = "300";
            //result[3] = "400";
            //result[4] = "500";
            //result[5] = "600";
            //result[6] = "700";
            //result[7] = "800";
            //result[8] = "900";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccountKTBGraphDaily(string dfrom, string dto)
        {
            DateTime? dateFrom;
            DateTime? dateTo;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dfrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dto);

            var result = _context.usp_ReportAccountKTBGraphDailySummary_Select(dateFrom, dateTo).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}