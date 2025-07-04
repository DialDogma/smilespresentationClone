using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSPoint.Models;
using SmileSPoint.Helper;
using System.Globalization;

namespace SmileSPoint.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        private SmilePointEntities _context;

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ReportController()
        {
            _context = new SmilePointEntities();
        }

        // GET: Report
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        #region PointAccount Report

        [HttpGet]
        public ActionResult PointAccount()
        {
            //ViewBag Account Type
            ViewBag.AccountTypeId = _context.PointAccountTypes.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult PointAccount(FormCollection form)
        {
            var pointAccountTypeId = Convert.ToInt32(form["AccountTypeId"]);

            List<PointAccount> lst;

            if (pointAccountTypeId == -1)
            {
                //All Types
                lst = _context.PointAccounts.ToList();
            }
            else
            {
                lst = _context.PointAccounts.Where(x => x.PointAccountTypeId == pointAccountTypeId).ToList();
            }

            //Export To Excel
            ExcelManager.ExportToExcel(this.HttpContext, lst, "PointAccountList", "PointAccountReport");

            ViewBag.AccountTypeId = _context.PointAccountTypes.ToList();
            return View();
        }

        #endregion PointAccount Report

        #region Transaction Report

        [HttpGet]
        public ActionResult Transaction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transaction(FormCollection form)
        {
            //Get Form Values
            var dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(form["DateFrom"]);
            var dateTo = GlobalFunction.ConvertDatePickerToDate_BE(form["DateTo"]).Value.AddDays(1);

            var increaseOrDecrease = Convert.ToInt32(form["IncreaseOrDecrease"]);
            //Get Data
            var lst = _context.usp_ReportTransactionDetail(increaseOrDecrease, dateFrom, dateTo).ToList();
            var sheetName = "TransactionReport" + dateFrom.ToString() + "_To_" + dateTo.ToString();
            //Export To Excel
            ExcelManager.ExportToExcel(this.HttpContext, lst, "TransactionReport", sheetName);
            return View();
        }

        #endregion Transaction Report

        #region Dash Board

        [HttpGet]
        public ActionResult DashBoard()
        {
            return View();
        }

        public JsonResult GetDashBoardPointRemainByAccountType()
        {
            //Get Result
            var result = _context.usp_Report_Dashboard_PointRemainByAccountType().ToList();
            //Return Json
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDashBoardRankingPointRemain_Branch()
        {
            //Get Ranking
            var result = _context.PointAccounts.Where(y => y.PointAccountTypeId == 20).OrderByDescending(x => x.PointRemain).Take(5).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDashBoardRankingPointRemain_Employee()
        {
            //Get Ranking
            var result = _context.PointAccounts.Where(y => y.PointAccountTypeId == 10).OrderByDescending(x => x.PointRemain).Take(5).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDashBoardRankingPointRemain_StudyArea()
        {
            //Get Ranking
            var result = _context.PointAccounts.Where(y => y.PointAccountTypeId == 30).OrderByDescending(x => x.PointRemain).Take(5).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDashboardTransactionByDate()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1);
            var result = _context.usp_Report_Dashboard_TransactionByDate(0, firstDayOfMonth, lastDayOfMonth);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Dash Board
    }
}