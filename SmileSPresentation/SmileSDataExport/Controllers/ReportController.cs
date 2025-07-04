using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SmileSDataExport.Models;
using SmileSDataExport.Helper;
using System.Data.SqlClient;

namespace SmileSDataExport.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        #region Context

        private readonly DataExportEntities _context;

        public ReportController()

        {
            _context = new DataExportEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context
        
        [Authorization(Roles = "Developer,DataExport_Financial")]
        public ActionResult ReportPremiumDCR()
        {
            return View();
        }

        public ActionResult GenerateReportPremiumDCR(string period)
        {
            var datePeriod = Convert.ToDateTime(period);
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            _context.Database.CommandTimeout = 5000;
            var genReport = _context.usp_AppXPremium_SSS_Insert(datePeriod, userId).FirstOrDefault();

            return Json(genReport.IsResult, JsonRequestBehavior.AllowGet);
        }

        #region ExportDCR
        public ActionResult ExportPremiumDCR(string period)
        {
            var datePeriod = Convert.ToDateTime(period);
            using (var db = new DataExportEntities())
            {
                _context.Database.CommandTimeout = 5000;
                var dataSheet1 = _context.usp_PremiumDetail_SSS_Select(datePeriod).Select(x => new
                {
                    App_Id = x.App_id,
                    Period = x.Period,
                    Product = x.Product,
                    Premium_Debt = x.PremiumDebt,
                    Premium_Receive = x.PremiumReceive,
                    PaymentType = x.PaymentType,
                    Insured_Company = x.Insured_Company,
                    Bank_Transaction_Datetime = x.Bank_Transaction_Datetime,
                    IsReconver = x.IsReconver,
                    Change_Product_Flag = x.Change_Product_Flag,
                    SumPremium_LTD = x.SumPremium_LTD,
                    SumPremium_12M = x.SumPremium_12M

                }).ToList();

                var dataSheet2 = _context.usp_AppDetail_SSS_Select(datePeriod).Select(x => new
                {
                    App_Id = x.App_id,
                    Product_Group = x.ProductGrouup,
                    Product = x.Product,
                    Premium = x.Premium,
                    StartCoverDate = x.StartCoverDate,
                    CustName = x.CustName,
                    CustZCard_Id = x.CustZCardId,
                    CustGender = x.CustGender,
                    CustDate_of_Birth = x.CustDate_of_Birth,
                    CustOccupation = x.CustOccupation,
                    CustCategories_of_Job = x.CustCategories_of_Job,
                    CustOrganization_Names = x.CustOrganization_Names,
                    PayerName = x.PayerName,
                    PayerCard_Id = x.PayerCardId,
                    PayerOccupation = x.PayerOccupation,
                    PayerCategories_of_Job = x.PayerCategories_of_Job,
                    PayerOrganization_Names = x.PayerOrganization_Names,
                    PayerProvince = x.PayerProvince,
                    PayerDistrict = x.PayerDistrict,
                    PayerBranch = x.PayerBranch,
                    PayerStudyArea = x.PayerStudyArea,
                    Sale_id1 = x.Sale_id1,
                    AppStatus = x.AppStatus,
                    UpdateDate = x.UpdateDate,
                    CancelDate = x.CancelDate,
                    CancelCause = x.CancelCause,
                    CancelRemark = x.CancelRemark,
                    MemoUDW = x.MemoUDW
                }).ToList();

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("DCR");
                    workSheet1.Cells.LoadFromCollection(dataSheet1, true);

                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                    headerCells1.Style.Font.Bold = true;
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    allCells1.AutoFitColumns();

                    var workSheet2 = package.Workbook.Worksheets.Add("Application");
                    workSheet2.Cells.LoadFromCollection(dataSheet2, true);
                    var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                    headerCells2.Style.Font.Bold = true;
                    headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells2.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    var allCells = workSheet2.Cells[workSheet2.Dimension.Address];
                    allCells.AutoFilter = true;
                    allCells.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcelDCR"] = package.GetAsByteArray();
                   
                    //get new GUID
                    var dataSessionKey = Guid.NewGuid().ToString();
                    //TempData GUID
                    TempData["keyReport"] = dataSessionKey;
                    //TempData Data
                    TempData[dataSessionKey] = period;

                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }
    
    public ActionResult ExportPremiumDCRDownload()
    {
         var dataKey = TempData["keyReport"].ToString();
        if (Session["DownloadExcelDCR"] != null)
        {
            var period = TempData[dataKey] as string;

            byte[] data = Session["DownloadExcelDCR"] as byte[];
            string excelName = $"ReportPremium_{period}.xlsx";

            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
        else
        {
            return new EmptyResult();
        }
    }
       #endregion ExportDCR

        #region ExportData-PH

        [Authorization(Roles = "Developer,DataExport_Financial")]
        public async Task<ActionResult> ExportReport(string exportType, string startDate, string endDate)
        {
            await Task.Yield();

            var nStartDate = Convert.ToDateTime(startDate);
            var nEndDate = Convert.ToDateTime(endDate);

            //var lst = db.usp_Transaction_select(nStartDate, nEndDate, smsType, search, 0, 999999, null, null).ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Detail");

                //check type
                switch (exportType)
                {
                    case "PH":
                        var lstPH = _context.usp_ReportClaimDetailSSS_Select(nStartDate, nEndDate, null).ToList();
                        workSheet.Cells.LoadFromCollection(lstPH, true);
                        break;

                    case "PA":
                        var lstPA = _context.usp_ReportClaimDetailSSSPA_Select(nStartDate, nEndDate).ToList();
                        workSheet.Cells.LoadFromCollection(lstPA, true);
                        break;

                    default:
                        return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }

                // Select only the header cells
                var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];
                // Set their text to bold.
                headerCells.Style.Font.Bold = true;
                // Set their background color
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                // Apply the auto-filter to all the columns
                var allCells = workSheet.Cells[workSheet.Dimension.Address];
                allCells.AutoFilter = true;
                // Auto-fit all the columns
                allCells.AutoFitColumns();

                package.Save();

                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,DataExport_Financial")]
        public ActionResult Download()
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion ExportData-PH
    }
}