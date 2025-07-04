using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class MotorReportController : Controller
    {
        #region Context

        private readonly UnderwriteMotorAuditEntity _context;

        public MotorReportController()
        {
            _context = new UnderwriteMotorAuditEntity();
        }

        #endregion Context

        #region View

        // GET: MotorReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MotorReportCheckUnderwrite()
        {
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            return View();
        }

        public ActionResult MotorReportCheckAndNotApproveUnderwrite(string report)
        {
            var changePeriodDay = Properties.Settings.Default.MotorReportChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 3);
            ViewBag.PeriodList = periodList[1].Display;

            //not show รายงานติดเงื่อนไข/ไม่ผ่านอนุมัติจากฝ่ายรับประกัน
            ViewBag.Report = "InsureClose";
            if (report == "abc")
            {
                ViewBag.Report = "NotInsureClose";
            }


            ViewBag.changePeriodDay = changePeriodDay;

            return View();
        }
        public ActionResult MotorReportAuditInsureClose(string report)
        {
            var changePeriodDay = Properties.Settings.Default.MotorReportChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 1).FirstOrDefault();
            ViewBag.PeriodList = periodList.Display;
            ViewBag.changePeriodDay = changePeriodDay;

            return View();
        }

        public ActionResult MotorReportNotApproveByChairmanUnderwrite()
        {
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 13);
            ViewBag.PeriodList = periodList;

            return View();
        }

        public ActionResult MotorReportUnderwriteAndInsurance()
        {
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 13);
            ViewBag.PeriodList = periodList;

            return View();
        }

        public ActionResult MotorReportCheckAndConsideringUnderwrite()
        {
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 13);
            ViewBag.PeriodList = periodList;

            return View();
        }

        #endregion View

        #region API

        public ActionResult MotorAuditInsureReport(string period)
        {
            try
            {
                var a = Convert.ToDateTime(period);
                var b = a.ToString("dd/MM/yyyy");
                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

                var result = _context.usp_QueueMotorAuditInsureReport_Select(a).ToList();

                // data empty
                if (result.Count == 0)
                {
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
                }
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    {
                        var auditMotor = _context.usp_QueueMotorAuditInsureReport_Select(a).Select(o => new
                        {
                            วันเริ่มคุ้มครอง = (o.วันเริ่มคุ้มครอง != null ? o.วันเริ่มคุ้มครอง.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            App_ID = o.App_ID,
                            ผู้เอาประกัน = o.ชื่อผู้เอาประกัน,
                            เบอร์โทรผู้เอาประกัน = o.เบอร์โทรผู้เอาประกัน,
                            ชื่อผู้ชำระเบี้ย = o.ชื่อผู้ชำระเบี้ย,
                            เบอร์โทรผู้ชำระเบี้ย = o.เบอร์โทรผู้ชำระเบี้ย,
                            สถานที่ทำงานผู้ชำระเบี้ย = o.สถานที่ทำงานผู้ชำระเบี้ย,
                            ID_ตำบล = o.ID_ตำบล,
                            ตำบล = o.ตำบล,
                            ID_อำเภอ = o.ID_อำเภอ,
                            อำเภอ = o.อำเภอ,
                            จังหวัด = o.จังหวัด,
                            สาขาผู้ชำระเบี้ย = o.สาขาผู้ชำระเบี้ย,
                            ภาคผู้ชำระเบี้ย = o.ภาคผู้ชำระเบี้ย,
                            รหัสผู้รับมอบคิวงาน = o.รหัสผู้รับมอบคิวงาน,
                            ชื่อสกุล_ผู้รับมอบคิวงาน = o.ชื่อสกุล_ผู้รับมอบคิวงาน,
                            รหัส = o.รหัส,
                            ชื่อสกุล_ผอ_บล = o.ชื่อสกุล_ผอ_บล,
                            /*รหัสประธานสาขา = o.รหัสประธานสาขา,
                            ชื่อสกุล_ประธานสาขา = o.ชื่อสกุล_ประธานสาขา,*/
                            เขตพื้นที่ = o.เขตพื้นที่,
                            รหัสผู้แทน = o.รหัสตัวแทน,
                            ชื่อสกุลผู้แทน = o.ชื่อสกุลตัวแทน,
                            สาขาผู้แทน = o.สาขาตัวแทน,
                            แผนประกัน = o.แผนประกัน,
                            เบี้ยประกัน = o.เบี้ยประกัน,
                            ทุนประกันรถยนต์ = o.ทุนประกันรถยนต์,
                            ประเภทการซ่อม = o.ประเภทการซ่อม,
                            ยี่ห้อ = o.ยี่ห้อ,
                            รุ่น = o.รุ่น,
                            ปีที่จดทะเบียน = (o.ปีที่จดทะเบียน != null ? Convert.ToInt32(o.ปีที่จดทะเบียน) + 543 : ' '),
                            เลขทะเบียน = o.เลขทะเบียน,
                            หมายเลขตัวถัง = o.หมายเลขตัวถัง,
                            ประเภทยานพาหนะ = o.ประเภทยานพาหนะ,
                            ประเภทการใช้งาน = o.ประเภทการใช้งาน,
                            ขนาดเครื่องยนต์__L_ = o.ขนาดเครื่องยนต์__L_,
                            สินเชื่อรถยนต์ = o.สินเชื่อรถยนต์,
                            มีกล้องติดรถยนต์ = o.มีกล้องติดรถยนต์,
                            ลักษณะการใช้งาน = o.ลักษณะการใช้งาน,
                            สถานที่การใช้งาน = o.สถานที่การใช้งาน,
                            VehicleRemark = o.หมายเหตุ,
                            สถานะการตรวจสอบ_ฝ่ายQC = o.สถานะการตรวจสอบ_ฝ่ายQC,
                            สถานะการโทร = o.สถานะการโทร,
                            จำนวนไม่รับสาย = o.จำนวนไม่รับสาย,
                            จำนวนไม่สะดวกคุย = o.จำนวนไม่สะดวกคุย,
                            จำนวนติดต่อไม่ได้ = o.จำนวนติดต่อไม่ได้,
                            จำนวนยกเลิก = o.จำนวนยกเลิก,
                            จำนวนเบอร์โทรผิด = o.จำนวนเบอร์โทรผิด,
                            รวมสถานะการโทร = o.รวมสถานะการโทร,
                            หมายเหตุการโทร = o.หมายเหตุการโทร,
                            ผลการตรวจสอบการคัดกรอง = o.ผลการตรวจสอบการคัดกรอง == "n/a" ? "" : o.ผลการตรวจสอบการคัดกรอง,
                            ผลการตรวจสอบการมอบกรมธรรม์ = o.ผลการตรวจสอบการมอบกรมธรรม์ == "n/a" ? "" : o.ผลการตรวจสอบการมอบกรมธรรม์,
                            ผลการตรวจสอบการชำระเบี้ย = o.ผลการตรวจสอบการชำระเบี้ย == "n/a" ? "" : o.ผลการตรวจสอบการชำระเบี้ย,
                            หมายเหตุ_มีประเด็น = o.หมายเหตุมีประเด็น,
                            ผลการตรวจสอบการทำประกัน = o.ผลการตรวจสอบการทำประกัน == "n/a" ? "" : o.ผลการตรวจสอบการทำประกัน,
                            ระบุในใบสมัคร = o.ระบุในใบสมัคร == "n/a" ? "" : o.ระบุในใบสมัคร,
                            AuditMotorRemark = o.หมายเหตุ2,

                            วันที่ทำรายการ = (o.วันที่ทำรายการ != null ?
                        (DateTime.TryParseExact(o.วันที่ทำรายการ, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date2) ?
                        date2.ToString("dd/MM/yyyy") : "-") : "-"),
                            เวลา = o.เวลา,
                            รหัสฝ่ายQC = o.รหัส_ฝ่ายQC,
                            ชื่อผู้ตรวจสอบฝ่ายQC = o.ชื่อผู้ตรวจสอบ_ฝ่ายQC,
                            สถานะการตรวจสอบฝ่ายรับประกัน = o.สถานะการตรวจสอบ_ฝ่ายรับประกัน,
                            หมายเหตุ_รอพิจารณา = o.หมายเหตุ_รอพิจารณา,
                            ผลการพิจารณา_ฝ่ายรับประกัน = o.ผลการพิจารณา_ฝ่ายรับประกัน == "n/a" ? "" : o.ผลการพิจารณา_ฝ่ายรับประกัน,
                            หมายเหตุ_ผลการพิจารณา = o.หมายเหตุ_ผลการพิจารณา,
                            วันที่ทำรายการพิจารณา = (o.วันที่ทำรายการพิจารณา != null ?
                        (DateTime.TryParseExact(o.วันที่ทำรายการพิจารณา, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ?
                        date.ToString("dd/MM/yyyy") : "-") : "-"),
                            เวลาพิจารณา = o.เวลาพิจารณา == "n/a" ? "" : o.เวลาพิจารณา,
                            รหัสฝ่ายรับประกัน = o.รหัส_ฝ่ายรับประกัน == "n/a" ? "" : o.รหัส_ฝ่ายรับประกัน,
                            ชื่อผู้พิจารณาฝ่ายรับประกัน = o.ชื่อผู้พิจารณา_ฝ่ายรับประกัน == "n/a" ? "" : o.ชื่อผู้พิจารณา_ฝ่ายรับประกัน,
                            สถานะปิดคิวงาน = o.สถานะปิดคิวงาน
                        }).ToList();

                        workSheet.Cells.LoadFromCollection(auditMotor, true);

                        //edit name headerCells
                        workSheet.Cells["AH1"].Value = "หมายเหตุ";
                        workSheet.Cells["AX1"].Value = "หมายเหตุ";
                        workSheet.Cells["AC1"].Value = "ขนาดเครื่องยนต์ (L)";
                    }
                    var rowCount = workSheet.Dimension.Rows;
                    var colCount = workSheet.Dimension.Columns;

                    // ลำดับที่
                    /*for (int i = 2; i - 2 < result.Count; i++)
                    {
                        workSheet.Cells[i, 1].Value = i - 1;
                    }*/

                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightSeaGreen);

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
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult MotorAuditInsureCloseReport(string dateFrom, string dateTo)
        {
            try
            {
                DateTime? ndateFrom = null;
                DateTime? ndateTo = null;
                if (!string.IsNullOrEmpty(dateFrom) && dateFrom != "-1")
                {
                    ndateFrom = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(dateTo) && dateTo != "-1")
                {
                    ndateTo = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                var result = _context.usp_QueueMotorAuditInsureCloseReport_Select(ndateFrom, ndateTo).ToList();

                // data empty
                if (result.Count == 0)
                {
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
                }
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    {
                        var auditMotor = _context.usp_QueueMotorAuditInsureCloseReport_Select(ndateFrom, ndateTo).Select(o => new
                        {
                            วันเริ่มคุ้มครอง = (o.วันเริ่มคุ้มครอง != null ? o.วันเริ่มคุ้มครอง.Value.ToString("dd/MM/yyyy") : ""),
                            App_ID = o.AppID,
                            ผู้เอาประกัน = o.ผู้เอาประกัน,
                            ชื่อผู้ชำระเบี้ย = o.ผู้ชำระเบี้ย,
                            สถานที่ทำงานผู้ชำระเบี้ย = o.สถานที่ทำงานผู้ชำระเบี้ย,
                            ID_ตำบล = o.ID_ตำบล,
                            ตำบล = o.ตำบล,
                            ID_อำเภอ = o.ID_อำเภอ,
                            อำเภอ = o.อำเภอ,
                            จังหวัด = o.จังหวัด,
                            สาขาผู้ชำระเบี้ย = o.สาขาผู้ชำระเบี้ย,
                            ภาคผู้ชำระเบี้ย = o.ภาคผู้ชำระเบี้ย,
                            รหัสผู้แทน = o.รหัสตัวแทน,
                            ชื่อสกุลผู้แทน = o.ชื่อสกุลตัวแทน,
                            สาขาผู้แทน = o.สาขาตัวแทน,
                            แผนประกัน = o.แผนประกัน,
                            เบี้ยประกัน = o.เบี้ยประกัน,
                            ทุนประกันรถยนต์ = o.ทุนประกันรถยนต์,
                            ประเภทการซ่อม = o.ประเภทการซ่อม,
                            ยี่ห้อ = o.ยี่ห้อ,
                            รุ่น = o.รุ่น,
                            ปีที่จดทะเบียน = (o.ปีที่จดทะเบียน != null ? Convert.ToInt32(o.ปีที่จดทะเบียน) + 543 : ' '),
                            เลขทะเบียน = o.เลขทะเบียน,
                            หมายเลขตัวถัง = o.หมายเลขตัวถัง,
                            ประเภทยานพาหนะ = o.ประเภทยานพาหนะ,
                            ประเภทการใช้งาน = o.ประเภทการใช้งาน,
                            ขนาดเครื่องยนต์__L_ = o.ขนาดเครื่องยนต์__L_,
                            สินเชื่อรถยนต์ = o.สินเชื่อรถยนต์,
                            มีกล้องติดรถยนต์ = o.มีกล้องติดรถยนต์,
                            ลักษณะการใช้งาน = o.ลักษณะการใช้งาน,
                            สถานที่การใช้งาน = o.สถานที่การใช้งาน,
                            VehicleRemark = o.หมายเหตุ,
                            ผลการพิจารณา_ฝ่ายรับประกัน = o.ผลการพิจารณาฝ่ายรับประกัน,
                            หมายเหตุ_ผลการพิจารณา = o.หมายเหตุผลการพิจารณา,
                            ผู้พิจารณาฝ่ายรับประกัน = o.ผู้พิจารณาฝ่ายรับประกัน,
                            วันที่พิจารณา = (o.วันที่พิจารณา != null ? o.วันที่พิจารณา.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""),
                            สถานะดำเนินการติดเงื่อนไข_ไม่ผ่านอนุมัติจากฝ่ายรับประกัน = o.สถานะดำเนินการติดเงื่อนไข_ไม่ผ่านอนุมัติจากฝ่ายรับประกัน,
                            ผู้ปิดคิวงาน = (o.ผู้ปิดคิวงาน != null ? o.ผู้ปิดคิวงาน : ""),
                            วันที่ปิดคิวงาน = (o.วันที่ปิดคิวงาน != null ? o.วันที่ปิดคิวงาน.Value.ToString("dd/MM/yyyy HH:mm:ss") : "")
                        }).ToList();

                        workSheet.Cells.LoadFromCollection(auditMotor, true);

                        //edit name headerCells
                        workSheet.Cells["AA1"].Value = "หมายเหตุ";
                        //workSheet.Cells["BA1"].Value = "หมายเหตุ";
                        //workSheet.Cells["AF1"].Value = "ขนาดเครื่องยนต์ (L)";
                    }
                    var rowCount = workSheet.Dimension.Rows;
                    var colCount = workSheet.Dimension.Columns;

                    // ลำดับที่
                    /* for (int i = 2; i - 2 < result.Count; i++)
                     {
                         workSheet.Cells[i, 1].Value = i - 1;
                     }*/

                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);

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
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Download(string reportName)
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"{reportName}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult ExportMotorMotorAuditChairmanCloseReport(string period)
        {
            try
            {
                var a = Convert.ToDateTime(period);
                var b = a.ToString("dd/MM/yyyy");
                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

                var result = _context.usp_QueueMotorAuditChairmanCloseReport_Select(a).ToList();

                // data empty
                if (result.Count == 0)
                {
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
                }
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    {
                        var auditMotor = _context.usp_QueueMotorAuditChairmanCloseReport_Select(a).Select(o => new
                        {
                            วันเริ่มคุ้มครอง = (o.วันเริ่มคุ้มครอง != null ? o.วันเริ่มคุ้มครอง.Value.ToString("dd/MM/yyyy") : ""),
                            App_ID = o.App_ID,
                            ชื่อผู้เอาประกัน = o.ชื่อผู้เอาประกัน,
                            ชื่อผู้ชำระเบี้ย = o.ชื่อผู้ชำระเบี้ย,
                            ที่ทำงานผู้ชำระเบี้ย = o.ที่ทำงานผู้ชำระเบี้ย,
                            สาขา = o.สาขา,
                            ภาค = o.ภาค,
                            รหัสตัวแทน = o.รหัสตัวแทน,
                            ชื่อตัวแทน = o.ชื่อตัวแทน,
                            สาขาตัวแทน = o.สาขาตัวแทน,
                            รหัสเจ้าของรถ = o.รหัสเจ้าของรถ,
                            แผนประกัน = o.แผนประกัน,
                            เบี้ยประกัน = o.เบี้ยประกัน,
                            ทุนประกันรถยนต์ = o.ทุนประกันรถยนต์,
                            ประเภทการซ่อม = o.ประเภทการซ่อม,
                            ยี่ห้อ = o.ยี่ห้อ,
                            รุ่น = o.รุ่น,
                            ปีที่จดทะเบียน = (o.ปีที่จดทะเบียน != null ? Convert.ToInt32(o.ปีที่จดทะเบียน) + 543 : ' '),
                            เลขทะเบียน = o.เลขทะเบียน,
                            หมายเลขตัวถัง = o.หมายเลขตัวถัง,
                            ประเภทยานพาหนะ = o.ประเภทยานพาหนะ,
                            ประเภทการใช้งาน = o.ประเภทการใช้งาน,
                            ขนาดเครื่องยนต์__L_ = o.ขนาดเครื่องยนต์__L_,
                            สินเชื่อรถยนต์ = o.สินเชื่อรถยนต์,
                            มีกล้องติดรถยนต์ = o.มีกล้องติดรถยนต์,
                            ลักษณะการใช้งาน = o.ลักษณะการใช้งาน,
                            สถานที่การใช้งาน = o.สถานที่การใช้งาน,
                            หมายเหตุ = o.หมายเหตุ,
                            ผลการพิจารณาของประธานสาขา = o.ผลการพิจารณาของประธานสาขา,
                            หมายเหตุการอนุมัติ = o.หมายเหตุการอนุมัติ,
                            ประธานไม่ให้ผ่านสภาพรถยนต์ไม่ตรงตามใบสมัคร = o.ประธานไม่ให้ผ่านสภาพรถยนต์ไม่ตรงตามใบสมัคร,
                            ประธานไม่ให้ผ่านประเภทการใช้งานผิดประเภท = o.ประธานไม่ให้ผ่านประเภทการใช้งานผิดประเภท,
                            ประธานไม่ให้ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้ = o.ประธานไม่ให้ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้,
                            ประธานไม่ให้ผ่านติดต่อไม่ได้_ไม่รับสาย_ไม่สะดวกคุย = o.ประธานไม่ให้ผ่านติดต่อไม่ได้_ไม่รับสาย_ไม่สะดวกคุย,
                            ประธานไม่ให้ผ่านอื่นๆ = o.ประธานไม่ให้ผ่านอื่นๆ,
                            รหัสผู้อนุมัติ = o.รหัสผู้อนุมัติ,
                            ผู้อนุมัติ = o.ผู้อนุมัติ,
                            วันที่อนุมัติของประธานสาขา = (o.วันที่อนุมัติของประธานสาขา != null ? o.วันที่อนุมัติของประธานสาขา.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""),
                            สถานะดำเนินการไม่ผ่านคัดกรองจากประธานสาขา = o.สถานะดำเนินการไม่ผ่านคัดกรองจากประธานสาขา,
                            ผู้ปิดคิวงาน = (o.ผู้ปิดคิวงาน != null ? o.ผู้ปิดคิวงาน : ""),
                            วันที่ปิดคิวงาน = (o.วันที่ปิดคิวงาน != null ? o.วันที่ปิดคิวงาน.Value.ToString("dd/MM/yyyy HH:mm:ss") : "")
                        }).ToList();

                        workSheet.Cells.LoadFromCollection(auditMotor, true);

                        //edit name headerCells
                        //workSheet.Cells["AK1"].Value = "หมายเหตุ";
                        //workSheet.Cells["BA1"].Value = "หมายเหตุ";
                        //workSheet.Cells["AF1"].Value = "ขนาดเครื่องยนต์ (L)";
                    }
                    var rowCount = workSheet.Dimension.Rows;
                    var colCount = workSheet.Dimension.Columns;

                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.Orchid);

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
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ExportMotorReportUnderwriteAndInsurance(string period)
        {
            try
            {
                var a = Convert.ToDateTime(period);
                var b = a.ToString("dd/MM/yyyy");
                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

                var result = _context.usp_QueueMotorAuditPoliciesReport_Select(a).ToList();

                // data empty
                if (result.Count == 0)
                {
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
                }
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    {
                        var auditMotor = _context.usp_QueueMotorAuditPoliciesReport_Select(a).Select(o => new
                        {
                            สาขา = o.สาขา,
                            ID_ตำบล = o.ID_ตำบล,
                            ตำบล = o.ตำบล,
                            อำเภอ = o.อำเภอ,
                            จังหวัด = o.จังหวัด,
                            รหัส_ผอ_บล_ = o.รหัส_ผอ_บล_,
                            ชื่อ_ผอ_บล_ = o.ชื่อ_ผอ_บล_,
                            งวดความคุ้มครอง = (o.งวดความคุ้มครอง != null ? o.งวดความคุ้มครอง.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            ApplicationCode = o.ApplicationCode,
                            ชื่อผู้เอาประกัน = o.ชื่อผู้เอาประกัน,
                            เบอร์โทรศัพท์ผู้เอาประกัน = o.เบอร์โทรศัพท์ผู้เอาประกัน,
                            ชื่อผู้ชำระเบี้ย = o.ชื่อผู้ชำระเบี้ย,
                            ที่ทำงานผู้ชำระเบี้ย = o.ที่ทำงานผู้ชำระเบี้ย,
                            เบอร์โทรศัพท์ผู้ชำระเบี้ย = o.เบอร์โทรศัพท์ผู้ชำระเบี้ย,
                            รหัสตัวแทน = o.รหัสตัวแทน,
                            ชื่อตัวแทน = o.ชื่อตัวแทน,
                            สาขาตัวแทน = o.สาขาตัวแทน,
                            รหัสเจ้าของรถ = o.รหัสเจ้าของรถ,
                            แผนประกัน = o.แผนประกัน,
                            เบี้ยประกัน = o.เบี้ยประกัน,
                            ทุนประกันรถยนต์ = o.ทุนประกันรถยนต์,
                            ประเภทการซ่อม = o.ประเภทการซ่อม,
                            ยี่ห้อ = o.ยี่ห้อ,
                            รุ่น = o.รุ่น,
                            ปีที่จดทะเบียน = (o.ปีที่จดทะเบียน != null ? Convert.ToInt32(o.ปีที่จดทะเบียน) + 543 : ' '),
                            เลขทะเบียน = o.เลขทะเบียน,
                            หมายเลขตัวถัง = o.หมายเลขตัวถัง,
                            ประเภทยานพาหนะ = o.ประเภทยานพาหนะ,
                            ประเภทการใช้งาน = o.ประเภทการใช้งาน,
                            ขนาดเครื่องยนต์__L_ = o.ขนาดเครื่องยนต์__L_,
                            สินเชื่อรถยนต์ = o.สินเชื่อรถยนต์,
                            มีกล้องติดรถยนต์ = o.มีกล้องติดรถยนต์,
                            ลักษณะการใช้งาน = o.ลักษณะการใช้งาน,
                            สถานที่การใช้งาน = o.สถานที่การใช้งาน,
                            หมายเหตุ = o.หมายเหตุ,
                            วันที่สร้างคิวงาน = (o.วันที่สร้างคิวงาน != null ? o.วันที่สร้างคิวงาน.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                            วันหมดอายุคิวงาน = (o.วันหมดอายุคิวงาน != null ? o.วันหมดอายุคิวงาน.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                            วันที่ส่งคิวงาน = (o.วันที่ส่งคิวงาน != null ? o.วันที่ส่งคิวงาน.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                            วันที่รับคิวงาน = (o.วันที่รับคิวงาน != null ? o.วันที่รับคิวงาน.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                            สถานะคัดกรองสาขา = o.สถานะคัดกรองสาขา,
                            สถานะการโทร = o.สถานะการโทร,
                            ช่องทางการคัดกรอง = o.ช่องทางการคัดกรอง,
                            สาเหตุการโทรคัดกรอง = o.สาเหตุการโทรคัดกรอง,
                            คัดกรองลูกค้า = o.คัดกรองลูกค้า,
                            คัดกรองผู้ชำระเบี้ย = o.คัดกรองผู้ชำระเบี้ย,
                            การชำระเบี้ย = o.การชำระเบี้ย,
                            ถูกต้องตามข้อมูลการสมัครทุกประการ = o.ถูกต้องตามข้อมูลการสมัครทุกประการ,
                            ถูกต้องติดเงื่อนไข_เฉพาะประกันชั้น_1_ = o.ถูกต้องติดเงื่อนไข__เฉพาะประกันชั้น_1_,
                            ระบุในใบสมัครหรือไม่ = o.ระบุในใบสมัครหรือไม่,
                            ข้อมูลติดเงื่อนไข = o.ข้อมูลติดเงื่อนไข,
                            ไม่ถูกต้อง_สภาพรถยนต์ไม่ตรงตามใบสมัคร = o.ไม่ถูกต้อง_สภาพรถยนต์ไม่ตรงตามใบสมัคร,
                            ไม่ถูกต้อง_ประเภทการใช้งานผิดประเภท = o.ไม่ถูกต้อง_ประเภทการใช้งานผิดประเภท,
                            ไม่ถูกต้อง_ไม่สามารถชำระเบี้ยตามเงื่อนไขได้ = o.ไม่ถูกต้อง_ไม่สามารถชำระเบี้ยตามเงื่อนไขได้,
                            ไม่ถูกต้อง_ติดต่อไม่ได้_ไม่รับสาย_ไม่สะดวกคุย = o.ไม่ถูกต้อง_ติดต่อไม่ได้_ไม่รับสาย_ไม่สะดวกคุย,
                            ไม่ถูกต้อง_อื่นๆ = o.ไม่ถูกต้อง_อื่นๆ,
                            หมายเหตุไม่ถูกต้องอื่นๆ = o.หมายเหตุไม่ถูกต้องอื่นๆ,
                            หมายเหตุผลการคัดกรองสาขา = o.หมายเหตุผลการคัดกรองสาขา,
                            รหัสผู้บันทึกคัดกรอง = o.รหัสผู้บันทึกคัดกรอง,
                            ชื่อผู้บันทึกคัดกรอง = o.ชื่อผู้บันทึกคัดกรอง,
                            วันที่บันทึกผลคัดกรองสาขา = (o.วันที่บันทึกผลคัดกรองสาขา != null ? o.วันที่บันทึกผลคัดกรองสาขา.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                            จำนวนวันที่เกินกำหนดบันทึกผลคัดกรองสาขา = o.จำนวนวันที่เกินกำหนดบันทึกผลคัดกรองสาขา,
                            ผลการพิจารณา = o.ผลการพิจารณา,
                            สถานะคิวงาน = o.สถานะคิวงาน,
                            CurrentApplicationStatus = o.CurrentApplicationStatus,
                            วันที่มอบกรมธรรม์ = (o.วันที่มอบกรมธรรม์ != null ? o.วันที่มอบกรมธรรม์.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                            ประเภทการมอบกรมธรรม์ = o.ประเภทการมอบกรมธรรม์,
                            รหัสผู้มอบกรมธรรม์ = o.รหัสผู้มอบกรมธรรม์,
                            ผู้มอบกรมธรรม์ = o.ผู้มอบกรมธรรม์,
                            ตำแหน่งผู้มอบกรมธรรม์ = o.ตำแหน่งผู้มอบกรมธรรม์,
                            จำนวนวันที่ปิดงาน = o.จำนวนวันที่ปิดงาน,
                            ชำระเงินล่วงหน้า = o.ชำระเงินล่วงหน้า
                        }).ToList();

                        workSheet.Cells.LoadFromCollection(auditMotor, true);

                        //edit name headerCells
                        workSheet.Cells["AZ1"].Value = "ไม่ถูกต้อง_ติดต่อไม่ได้/ไม่รับสาย/ไม่สะดวกคุย";
                        workSheet.Cells["AT1"].Value = "ถูกต้องติดเงื่อนไข (เฉพาะประกันชั้น 1)";
                        workSheet.Cells["AB1"].Value = "ขนาดเครื่องยนต์ (L)";
                    }
                    var rowCount = workSheet.Dimension.Rows;
                    var colCount = workSheet.Dimension.Columns;

                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightSalmon);

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
            catch (Exception)
            {
                throw;
            }
        }

        #endregion API
    }
}