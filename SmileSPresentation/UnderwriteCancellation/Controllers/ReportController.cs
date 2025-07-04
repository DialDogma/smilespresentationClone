using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using UnderwriteCancellation.Helper;
using UnderwriteCancellation.Models;

namespace UnderwriteCancellation.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        private readonly UnderwriteCancellationEntities _context;

        public ReportController()
        {
            _context = new UnderwriteCancellationEntities();
        }

        // GET: Report รายงาน
        public ActionResult Index()
        {
            return View();
        }

        //รายงานคิวงานมีประเด็นดำเนินการแล้ว
        public ActionResult ReportQueueIssueSuccess()
        {
            return View();
        }

        //รายงานการโทรหาลูกค้ายกเลิก
        public ActionResult ReportCancellation()
        {
            return View();
        }

        public ActionResult CancellationReport(string period, string dateFrom, string dateTo)
        {
            //await Task.Yield();
            using (var db = new UnderwriteCancellationEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                /*var p_eriod = form["period"];*/
                DateTime? c_period = null;
                DateTime? c_dateFrom = null;
                DateTime? c_dateTo = null;
                if (period != null && period != "")
                {
                    c_period = GlobalFunction.ConvertDatePickerToDate_BE(period);
                }
                else if (dateFrom != null && dateFrom != "" || dateTo != null && dateTo != "")
                {
                    c_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
                    c_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);
                }

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("ผู้ชำระเบี้ย");
                    {
                        var data_sheet1 = _context.usp_QueueReport1_Select(c_period, c_dateFrom, c_dateTo).Select(o => new
                        {
                            Period = (o.Period != null ? o.Period.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            PayerName = o.PayerName,
                            PayerIdCardNo = o.PayerIdCardNo,
                            เบอร์โทรศัพท์ผู้ชำระเบี้ย = o.PayerPhone,
                            ที่อยู่ที่ทำงานผู้ชำระเบี้ย = o.PayerBuildingName,
                            สาขาผู้ชำระเบี้ย = o.PayerBranchDetail,
                            ภาคผู้ชำระเบี้ย = o.PayerAreaDetail,
                            จำนวนผู้เอาประกัน = o.QueueDetailAllCount,
                            สถานะการโทร = o.CallStatusDetail,
                            หมายเหตุการโทร = o.CallRemark,
                            ติดต่อไม่ได้ = o.CallStatusId3,
                            ไม่สะดวก = o.CallStatusId6,
                            ไม่รับสาย = o.CallStatusId4,
                            เบอร์ผิด = o.CallStatusId5,
                            จำนวนครั้งในการโทรทั้งหมด = o.CallStatusCount,
                            รหัสพนักงานโทร = o.EmployeeCode,
                            ชื่อพนักงานโทร = o.EmployeePersonName,
                            วันที่ทำรายการ = (o.UpdatedDate),
                            เวลาที่ทำรายการ = o.UpdatedTime,
                            สถานะการตรวจสอบ = (o.QueueStatusDetail != null ? (o.QueueStatusDetail == "n/a" ? "" : o.QueueStatusDetail) : ""),
                            ประเด็น = (o.IsIssue != null ? (o.IsIssue.Value == true ? "มี" : "ไม่มี") : ""),
                            รายละเอียด = o.IssueRemark,
                            สาเหตุการยกเลิก = o.CancelCauseFullDetail,
                            เพิ่มเติม = o.CancelCauseRemark
                        }).ToList();

                        workSheet1.Cells.LoadFromCollection(data_sheet1, true);
                        workSheet1.Cells["A1"].Value = "DCR ยกเลิก";
                        workSheet1.Cells["B1"].Value = "ชื่อ-นามสกุลผู้ชำระเบี้ย";
                        workSheet1.Cells["C1"].Value = "เลขบัตรประชาชน/Passport";

                        // Select only the header cells
                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];

                        // Set their text to bold.
                        headerCells1.Style.Font.Bold = true;

                        // Set their background color
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        // Apply the auto-filter to all the columns
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];

                        allCells1.AutoFilter = true;

                        // Auto-fit all the columns
                        allCells1.AutoFitColumns();
                    }

                    var workSheet2 = package.Workbook.Worksheets.Add("ผู้เอาประกัน");
                    {
                        var data_sheet2 = _context.usp_QueueReport2_Select(c_period, c_dateFrom, c_dateTo).Select(o => new
                        {
                            DCR_ยกเลิก = (o.Period != null ? o.Period.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            สถานะ = o.AppStatus,
                            AppID = o.ApplicationCode,
                            ผลิตภัณฑ์ = o.ProductGroup,
                            เบี้ย = o.Premium,
                            วิธีการชำระ = o.PayMethodType,
                            วันที่เริ่มคุ้มครอง = (o.AppStartCoverDate != null ? o.AppStartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            ชื่อ_นามสกุล_ผู้ชำระเบี้ย = o.PayerName,
                            เลขบัตรประชาชน_Passport = o.PayerIdCardNo,
                            เบอร์โทรศัพท์ผู้ชำระเบี้ย = o.PayerPhone,
                            อาชีพ = o.PayerOccupation,
                            ระดับอาชีพ = o.PayerOccupationLevel,
                            ที่อยู่ที่ทำงานผู้ชำระเบี้ย = o.PayerBuildingName,
                            สาขาผู้ชำระเบี้ย = o.PayerBranchDetail,
                            ภาคผู้ชำระเบี้ย = o.PayerAreaDetail,
                            ชื่อ_นามสกุล_ผู้เอาประกัน = o.InsuredName,
                            รหัสผู้แทน = o.SaleEmployeeCode,
                            ชื่อ_นามสกุลผู้แทน = o.SaleEmployeeName,
                            สาขาผู้แทน = o.SaleEmployeeBranch,
                            ผลการโทร = o.CallStatusDetail,
                            หมายเหตุการโทร = o.CallRemark,
                            รหัสพนักงานโทร = o.EmployeeCode,
                            ชื่อพนักงานโทร = o.EmployeePersonName,
                            วันที่ทำรายการ = o.UpdatedDate,
                            เวลาที่ทำรายการ = o.UpdatedTime,
                            สถานะการตรวจสอบ = (o.QueueStatusDetail != null ? (o.QueueStatusDetail == "n/a" ? "" : o.QueueStatusDetail) : ""),
                            ประเด็น = (o.IsIssue != null ? (o.IsIssue.Value == true ? "มี" : "ไม่มี") : ""),
                            รายละเอียด = o.IssueRemark,
                            ระบุหมายเหตุ = o.QueueDetailRemark,
                            สาเหตุการยกเลิก = o.CancelCauseFullDetail,
                            เพิ่มเติม = o.CancelCauseRemark
                        }).ToList();

                        workSheet2.Cells.LoadFromCollection(data_sheet2, true);
                        workSheet2.Cells["A1"].Value = "DCR ยกเลิก";
                        workSheet2.Cells["H1"].Value = "ชื่อ-นามสกุลผู้ชำระเบี้ย";
                        workSheet2.Cells["P1"].Value = "ชื่อ-นามสกุลผู้เอาประกัน";
                        workSheet2.Cells["I1"].Value = "เลขบัตรประชาชน/Passport";
                        workSheet2.Cells["R1"].Value = "ชื่อ-นามสกุลผู้แทน";
                        // Select only the header cells

                        var headerCells2 = workSheet2.Cells[1, 1, 1, 32];
                        // Set their text to bold.
                        headerCells2.Style.Font.Bold = true;
                        // Set their background color
                        headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells2.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                        // Apply the auto-filter to all the columns
                        var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                        allCells2.AutoFilter = true;
                        // Auto-fit all the columns
                        allCells2.AutoFitColumns();
                    }

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileUnderwriteQueueReport"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult QueueIssueSuccessReport(string period, string dateFrom, string dateTo)
        {
            //await Task.Yield();
            using (var db = new UnderwriteCancellationEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                /*var p_eriod = form["period"];*/
                DateTime? c_period = null;
                DateTime? c_dateFrom = null;
                DateTime? c_dateTo = null;
                if (period != null && period != "")
                {
                    c_period = GlobalFunction.ConvertDatePickerToDate_BE(period);
                }
                else if (dateFrom != null && dateFrom != "" || dateTo != null && dateTo != "")
                {
                    c_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
                    c_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);
                }

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("คิวงานมีประเด็น");
                    {
                        var data_sheet1 = _context.usp_QueueIssueReport_Select(c_period, c_dateFrom, c_dateTo).Select(o => new
                        {
                            Period = (o.Period != null ? o.Period.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            เลขApp = o.ApplicationCode,
                            ผลิตภัณฑ์ = o.ProductGroup,
                            เบี้ย = o.Premium,
                            วิธีการชำระ = o.PayMethodType,
                            วันที่เริ่มคุ้มครอง = (o.AppStartCoverDate != null ? o.AppStartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            PayerName = o.PayerName,
                            PayerIdCardNo = o.PayerIdCardNo,
                            เบอร์โทรศัพท์ผู้ชำระเบี้ย = o.PayerPhone,
                            สาขาผู้ชำระเบี้ย = o.PayerBranchDetail,
                            ภาคผู้ชำระเบี้ย = o.PayerAreaDetail,
                            ผู้เอาประกัน = o.InsuredName,
                            รหัสผู้แทน = o.SaleEmployeeCode,
                            ชื่อสกุลผู้แทน = o.SaleEmployeeName,
                            รหัสพนักงานโทร = o.EmployeeCode,
                            ชื่อพนักงานโทร = o.EmployeePersonName,
                            วันที่ทำรายการ = o.UpdatedDate,
                            เวลาที่ทำรายการ = o.UpdatedTime,
                            ประเด็น = (o.IsIssue != null ? (o.IsIssue.Value == true ? "มี" : "ไม่มี") : ""),
                            รายละเอียด = o.IssueRemark,
                            ระบุหมายเหตุ = o.QueueDetailRemark,
                            สาเหตุการยกเลิก = o.CancelCauseFullDetail,
                            เพิ่มเติม = o.CancelCauseRemark,
                            สถานะคิวงานมีประเด็น = (o.IsIssueComplete != null ? (o.IsIssueComplete.Value == true ? "ดำเนินการแล้ว" : "รอดำเนินการ") : "รอดำเนินการ"),
                            บันทึกข้อความมีประเด็น = o.IssueCompleteRemark,
                            ผู้ปิดคิวงานมีประเด็น = o.IssueCompleteByPersonName,
                            วันที่ปิดคิวงานมีประเด็น = (o.IssueCompleteDate != null ? o.IssueCompleteDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),
                        }).ToList();

                        workSheet1.Cells.LoadFromCollection(data_sheet1, true);
                        workSheet1.Cells["A1"].Value = "DCR ยกเลิก";
                        workSheet1.Cells["B1"].Value = "เลข App";

                        workSheet1.Cells["G1"].Value = "ชื่อ-นามสกุลผู้ชำระเบี้ย";
                        workSheet1.Cells["H1"].Value = "เลขบัตรประชาชน/Passport";
                        workSheet1.Cells["N1"].Value = "ชื่อ-นามสกุลผู้แทน";


                        // Select only the header cells
                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];

                        // Set their text to bold.
                        headerCells1.Style.Font.Bold = true;

                        // Set their background color
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        // Apply the auto-filter to all the columns
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];

                        allCells1.AutoFilter = true;

                        // Auto-fit all the columns
                        allCells1.AutoFitColumns();
                    }

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileUnderwriteQueueReport"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Download(string reportName)
        {
            if (Session["DownloadExcel_FileUnderwriteQueueReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileUnderwriteQueueReport"] as byte[];
                string excelName = $"{reportName}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}