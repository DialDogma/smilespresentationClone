using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class ReportsController : Controller
    {
        #region Context

        private readonly UnderwriteAuditEntities _context;

        public ReportsController()
        {
            _context = new UnderwriteAuditEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult ReportCheckUnderwrite()
        {
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 25);
            ViewBag.PeriodList = periodList;
            return View();

        }

        #region API

        //public void ExportQueueReport(string period)
        //{
        //    DateTime? reriod = GlobalFunction.ConvertDatePickerToDate_BE(period);
        //    var data_sheet1 = _context.usp_QueueAuditFullDetailReport_Select(reriod).Select(s => new
        //    {
        //        DCR = s.Period,
        //        App_ID = s.ApplicationCode,
        //        ผู้เอาประกัน = s.CustomerName,
        //        เบอร์โทรผู้เอาประกัน = s.CustomerPhone,
        //        ผู้ชำระเบี้ย = s.PayerName,
        //        เบอร์โทรผู้ชำระเบี้ย = s.PayerPhone,
        //        สถานที่ทำงานผู้ชำระเบี้ย = s.PayerBuildingName,
        //        ที่อยู่ผู้ชำระเบี้ย = s.PayerAddress,
        //        สาขาผู้ชำระเบี้ย = s.PayerBranch,
        //        ภาคผู้ชำระเบี้ย = s.AreaDetail,
        //        รหัส = s.DirectorEmployeeCode,
        //        ชื่อสกุล_ผอ_บล = s.DirectorName,
        //        เขตพื้นที่ = s.StudyAreaName,
        //        รหัสตัวแทน = s.SaleEmployeeCode,
        //        ชื่อสกุลตัวแทน = s.SaleEmployeeName,
        //        สาขาตัวแทน = s.SaleEmployeeBranch,
        //        สถานะการตรวจสอบ_ฝ่ายส่งเสริม = s.AuditStatusDetail,
        //        สถานะการโทร = s.CallStatusDetail,
        //        ผลการตรวจสอบ_ผอ_บล = s.AuditUnderwriteResultStatusDetail,
        //        หมายเหตุ_ผลการตรวจสอบ_ผอ_บล = s.AuditUnderwriteResultRemark,
        //        ผลการตรวจสอบด้านสุขภาพ_ฝ่ายส่งเสริม = s.AuditHealthStatusDetail,
        //        ระบุในใบสมัคร = s.AuditHealthSpecifyStatusDetail,
        //        หมายเหตุ = s.AuditHealthRemark,
        //        วันที่ทำรายการ = s.Audit1UpdatedDate,
        //        เวลา = s.Audit1UpdatedTime,
        //        รหัส_ฝ่ายส่งเสริม = s.BusinessPromoteTeamEmployeeCode,
        //        ชื่อผู้ตรวจสอบ_ฝ่ายส่งเสริม = s.BusinessPromoteTeamName,
        //        สถานะการตรวจสอบ_ฝ่ายรับประกัน = s.AuditInsureStatusDetail,
        //        หมายเหตุ_รอพิจารณา = s.AuditInsureRemark,
        //        ผลการพิจารณา_ฝ่ายรับประกัน = s.AuditInsureConsiderStatusDetail,
        //        หมายเหตุ_ผลการพิจารณา = s.AuditInsureConsiderRemark,
        //        วันที่ทำรายการพิจารณา = s.Audit2UpdatedDate,
        //        เวลาพิจารณา = s.Audit2UpdatedTime,
        //        รหัส_ฝ่ายรับประกัน = s.InsureEmployeeCode,
        //        ชื่อผู้พิจารณา_ฝ่ายรับประกัน = s.InsureName
        //    }).ToList();
        //    var dt1 = GlobalFunction.ToDataTable(data_sheet1);
        //    ExcelManager.ExportToExcel(this.HttpContext, "รายงานตรวจสอบการคัดกรอง", dt1, "รายงานตรวจสอบการคัดกรอง");
        //}

        public ActionResult underwriteExportQueueReport(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var p_eriod = form["period"];
                DateTime? reriod = GlobalFunction.ConvertDatePickerToDate_BE(p_eriod);
                var data_sheet1 = _context.usp_QueueAuditFullDetailReport_Select(reriod).Select(s => new
                {
                    DCR = s.Period,
                    App_ID = s.ApplicationCode,
                    ผู้เอาประกัน = s.CustomerName,
                    เบอร์โทรผู้เอาประกัน = s.CustomerPhone,
                    ผู้ชำระเบี้ย = s.PayerName,
                    เบอร์โทรผู้ชำระเบี้ย = s.PayerPhone,
                    สถานที่ทำงานผู้ชำระเบี้ย = s.PayerBuildingName,
                    ID_ตำบล = s.SubDistrictId,
                    ตำบล = s.SubDistrictName,
                    ID_อำเภอ = s.DistrictId,
                    อำเภอ = s.DistrictName,
                    จังหวัด = s.ProvinceName,
                    สาขาผู้ชำระเบี้ย = s.PayerBranch,
                    ภาคผู้ชำระเบี้ย = s.AreaDetail,
                    รหัสผู้รับมอบคิวงาน = s.AssignToEmployeeCode,
                    ชื่อสกุล_ผู้รับมอบคิวงาน = s.AssignToEmployeeName,
                    รหัส = s.DirectorEmployeeCode,
                    ชื่อสกุล_ผอ_บล = s.DirectorName,
                    เขตพื้นที่ = s.StudyAreaName,
                    รหัสผู้แทน = s.SaleEmployeeCode,
                    ชื่อสกุลผู้แทน = s.SaleEmployeeName,
                    สาขาผู้แทน = s.SaleEmployeeBranch,
                    สถานะการตรวจสอบ_ของแผนกQC = s.AuditStatusDetail,
                    สถานะการโทร = s.CallStatusDetail,
                    จำนวนไม่รับสาย = s.CallStatusId2,
                    จำนวนไม่สะดวกคุย = s.CallStatusId3,
                    จำนวนติดต่อไม่ได้ = s.CallStatusId4,
                    จำนวนยกเลิก = s.CallStatusId6,
                    จำนวนเบอร์โทรผิด = s.CallStatusId7,
                    รวมสถานะการโทร = s.CallStatusTotal,
                    หมายเหตุการโทร = s.AuditRemark,
                    ผลการตรวจสอบการคัดกรอง = s.AuditUnderwriteStatusDetail == "n/a" ? " " : s.AuditUnderwriteStatusDetail,
                    ผลการตรวจสอบการมอบบัตร = s.AuditCobrandGivenStatusDetail == "n/a" ? " " : s.AuditCobrandGivenStatusDetail,
                    หมายเหตุ_ผลการตรวจสอบ_ผอ_บล = s.AuditUnderwriteResultRemark,
                    ผลการตรวจสอบด้านสุขภาพ_ของแผนกQC = s.AuditHealthStatusDetail == "n/a" ? " " : s.AuditHealthStatusDetail,
                    ระบุในใบสมัคร = s.AuditHealthSpecifyStatusDetail == "n/a" ? " " : s.AuditHealthSpecifyStatusDetail,
                    หมายเหตุ = s.AuditHealthRemark,
                    วันที่ทำรายการ = s.Audit1UpdatedDate,
                    เวลา = s.Audit1UpdatedTime,
                    รหัส_แผนกQC = s.BusinessPromoteTeamEmployeeCode,
                    ชื่อผู้ตรวจสอบ_แผนกQC = s.BusinessPromoteTeamName,
                    สถานะการตรวจสอบ_ฝ่ายรับประกัน = s.AuditInsureStatusDetail,
                    หมายเหตุ_รอพิจารณา = s.AuditInsureRemark,
                    ผลการพิจารณา_ฝ่ายรับประกัน = s.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = s.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = s.Audit2UpdatedDate == "n/a" ? " " : s.Audit2UpdatedDate,
                    เวลาพิจารณา = s.Audit2UpdatedTime == "n/a" ? " " : s.Audit2UpdatedTime,
                    รหัส_ฝ่ายรับประกัน = s.InsureEmployeeCode == "n/a" ? " " : s.InsureEmployeeCode,
                    ชื่อผู้พิจารณา_ฝ่ายรับประกัน = s.InsureName == "n/a" ? " " : s.InsureName,
                    สถานะปิดคิวงาน = s.QueueStatusDetail
                }).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;
                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileUnderwriteQueueReport"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult underwriteDownloadQueueReport()
        {
            if (Session["DownloadExcel_FileUnderwriteQueueReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileUnderwriteQueueReport"] as byte[];
                string excelName = $"รายงานคัดกรอง-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion API
    }
}