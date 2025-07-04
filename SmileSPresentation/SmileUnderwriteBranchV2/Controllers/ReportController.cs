using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        #region Context

        private readonly UnderwriteBranchV2Entities _context;

        public ReportController()
        {
            _context = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region view

        /// <summary>
        /// รายงานคัดกรองและมอบบัตรประกัน
        /// </summary>
        /// <returns></returns>
        /*[Authorization(Roles = "Developer,UnderwriteV2_Director")]*/

        public ActionResult ReportUnderwriteAndInsuranceCard()
        {
            var periodList = GlobalFunction.GetPeriodList();
            ViewBag.PeriodList = periodList;
            return View();
        }

        /// <summary>
        /// รายงานคัดกรองและมอบบัตรประกันของประธานสาขา
        /// </summary>
        /// <returns></returns>
        /*[Authorization(Roles = "Developer,UnderwriteV2_Chairman")]*/

        public ActionResult ReportUnderwriteAndInsuranceCardDirector()
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branch = _context.usp_BranchByChairmanUserId_Select(userId).ToList();
            ViewBag.branch = branch;

            var periodList = GlobalFunction.GetPeriodList();
            ViewBag.PeriodList = periodList;

            return View();
        }

        /// <summary>
        /// รายงานการคัดกรองและมอบบัตรของฝ่ายส่งเสริม
        /// </summary>
        /// <returns></returns>
        /*[Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam,UnderwriteV2_Manager")]*/

        public ActionResult ReportUnderwriteAndInsuranceCardBO()
        {
            //เพิ่มใบงาน 411
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var show = 13;
            var periodList = GlobalFunction.GetPeriodList(changePeriodDay, show);
            ViewBag.PeriodList = periodList;

            return View();
        }

        /// <summary>
        /// รายงานรอมอบบัตร
        /// </summary>
        /// <returns></returns>
        /*[Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam,UnderwriteV2_Manager")]*/

        public ActionResult ReportQueueCoBrandPending()
        {
            return View();
        }

        /// <summary>
        /// รายงาน
        /// </summary>
        /// <returns></returns>
        //[Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam,UnderwriteV2_Manager")]
        public ActionResult ReportPromotion()
        {
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult ReportQueueManualAssign()
        {
            return View();
        }

        #endregion view

        #region api

        [HttpGet]
        public ActionResult GetReportForChart(string period)
        {
            try
            {
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                var result = _context.usp_pivotQueueStatusByAssignToUserId_Select(nStartCoverdate, assignToUserId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public ActionResult GetBranchByChairManUserId(int userId)
        {
            var result = _context.usp_BranchByChairmanUserId_Select(userId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void ExportReportUnderwriteAndInsuranceCardDirector(string period, int branchId)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }

                var result = _context.usp_QueueChairmanReport_Select(nStartCoverdate, branchId, null, null, null).ToList();

                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานการคัดกรองและมอบบัตรของประธานสาขา");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportReportQueueDirector(string period)
        {
            try
            {
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                DateTime? oldStartCoverdate = Convert.ToDateTime("2567-06-01");

                if (nStartCoverdate >= oldStartCoverdate)
                {
                    var result = _context.usp_QueueDirectorReportV2_Select(nStartCoverdate, null, null, null, assignToUserId).ToList();
                    GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานการมอบบัตร");
                }
                else
                {
                    var result = _context.usp_QueueDirectorReport_Select(nStartCoverdate, null, null, null, assignToUserId).ToList();
                    GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานการมอบบัตร");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        /*public ActionResult ExportReportQueueDirector(string period)
        {
            //await Task.Yield();
            using (var db = new UnderwriteBranchV2Entities())
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("รายงานคัดกรองและมอบบัตรประกัน");
                    {
                        var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                        var data_sheet1 = _context.usp_QueueDirectorReport_Select(nStartCoverdate, null, null, null, assignToUserId).Select(o => new
                        {
                            สาขา = o.สาขา,
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
                            แผนประกัน = o.แผนประกัน,
                            เบี้ยประกัน = o.เบี้ยประกัน,
                            วันที่สร้างคิวงาน = (o.วันที่สร้างคิวงาน != null ? o.วันที่สร้างคิวงาน.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),
                            วันหมดอายุคิวงาน = (o.วันหมดอายุคิวงาน != null ? o.วันหมดอายุคิวงาน.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),
                            วันที่ส่งคิวงาน = (o.วันที่ส่งคิวงาน != null ? o.วันที่ส่งคิวงาน.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),
                            วันที่รับคิวงาน = (o.วันที่รับคิวงาน != null ? o.วันที่รับคิวงาน.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),
                            สถานะคัดกรองสาขา = o.สถานะคัดกรองสาขา,
                            สถานะการโทร = o.สถานะการโทร,
                            ช่องทางการคัดกรอง = o.ช่องทางการคัดกรอง,
                            สาเหตุการโทรคัดกรอง = o.สาเหตุการโทรคัดกรอง,
                            คัดกรองลูกค้า = o.คัดกรองลูกค้า,
                            คัดกรองผู้ชำระเบี้ย = o.คัดกรองผู้ชำระเบี้ย,
                            การชำระเบี้ย = o.การชำระเบี้ย,
                            ผ่านตามมาตรฐานทุกประการ = o.ผ่านตามมาตรฐานทุกประการ,
                            ผ่านมีประวัติสุขภาพติดข้อยกเว้น = o.ผ่านมีประวัติสุขภาพติดข้อยกเว้น,
                            ระบุในใบสมัครหรือไม่ = o.ระบุในใบสมัครหรือไม่,
                            หมายเหตุผ่านติดข้อยกเว้น = o.หมายเหตุผ่านติดข้อยกเว้น,
                            ไม่ผ่านด้านสุขภาพ = o.ไม่ผ่านด้านสุขภาพ,
                            ไม่ผ่านด้านอาชีพ = o.ไม่ผ่านด้านอาชีพ,
                            ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้ = o.ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้,
                            ไม่ผ่านติดต่อไม่ได้_ไม่รับสาย_ไม่สะดวกคุย = o.ไม่ผ่านติดต่อไม่ได้_ไม่รับสาย_ไม่สะดวกคุย,
                            ไม่ผ่านอื่นๆ = o.ไม่ผ่านอื่นๆ,
                            หมายเหตุไม่ผ่านอื่นๆ = o.หมายเหตุไม่ผ่านอื่นๆ,
                            หมายเหตุผลการคัดกรองสาขา = o.หมายเหตุผลการคัดกรองสาขา,
                            รหัสผู้บันทึกคัดกรอง = o.รหัสผู้บันทึกคัดกรอง,
                            ชื่อผู้บันทึกคัดกรอง = o.ชื่อผู้บันทึกคัดกรอง,
                            วันที่บันทึกผลคัดกรองสาขา = (o.วันที่บันทึกผลคัดกรองสาขา != null ? o.วันที่บันทึกผลคัดกรองสาขา.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),
                            จำนวนวันที่เกินกำหนดบันทึกผลคัดกรองสาขา = o.จำนวนวันที่เกินกำหนดบันทึกผลคัดกรองสาขา,
                            ผลการพิจารณา = o.ผลการพิจารณา,
                            สถานะคิวงาน = o.สถานะคิวงาน,
                            CurrentApplicationStatus = o.CurrentApplicationStatus,
                            เลข_CoBrand = o.เลข_CoBrand,
                            วันที่มอบบัตร = (o.วันที่มอบบัตร != null ? o.วันที่มอบบัตร.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                            ประเภทการมอบบัตร = o.ประเภทการมอบบัตร,
                            เลขพัสดุ = o.เลขพัสดุ,
                            รหัสผู้มอบบัตร = o.รหัสผู้มอบบัตร,
                            ผู้มอบบัตร = o.ผู้มอบบัตร,
                            ตำแหน่งผู้มอบบัตร = o.ตำแหน่งผู้มอบบัตร,
                            จำนวนวันที่ปิดงาน = o.จำนวนวันที่ปิดงาน,
                            ชำระเงินล่วงหน้า = o.ชำระเงินล่วงหน้า
                        }).ToList();

                        workSheet1.Cells.LoadFromCollection(data_sheet1, true);
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
*/
        public void ExportReportQueueCoBrandPending()
        {
            try
            {
                var result = _context.usp_QueueCoBrandPendingBusinessPromoteTeamReportV4_Select().ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานรอมอบบัตร_PH_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

      


        public void ExportReportUnderwriteAndInsuranceCardBO(string period)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                DateTime? oldStartCoverdate = Convert.ToDateTime("2567-06-01");

                if (nStartCoverdate >= oldStartCoverdate)
                {
                    var result = _context.usp_QueueBusinessPromoteTeamReportV2_Select(nStartCoverdate, null, null, null, null).ToList();
                    var downloadDate = DateTime.Now.ToString("ddMMyy");
                    GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานการมอบบัตร_PH_{downloadDate}");
                }
                else
                {
                    var result = _context.usp_QueueBusinessPromoteTeamReport_Select(nStartCoverdate, null, null, null, null).ToList();
                    var downloadDate = DateTime.Now.ToString("ddMMyy");
                    GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานการมอบบัตร_PH_{downloadDate}");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        //รายงานโยกคิวงาน
        public void ExportReportQueueManualAssign(string period)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (period != "-1" && period != null)
                {
                    nStartCoverdate = Convert.ToDateTime(period);
                }
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueManualAssignReport_Select(nStartCoverdate, null, null, null, null).ToList();
                var downloadDate = DateTime.Now.ToString("ddMMyy");
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", $"รายงานโยกคิวงาน_PH_{downloadDate}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
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
        #endregion api
    }
}