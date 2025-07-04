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
    public class CancerReportController : Controller
    {
        private readonly UnderwriteCancerAuditEntities _context;

        public CancerReportController()
        {
            _context = new UnderwriteCancerAuditEntities();
        }

        // GET: CancerReport
        public ActionResult CancerReportIndex()
        {
            return View();
        }

        public ActionResult CancerReportCheckAndConsideringUnderwrite()
        {
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 13);
            ViewBag.PeriodList = periodList;
            return View();
        }

        public ActionResult CancerAuditInsureReport(string period)
        {
            try
            {
                var a = Convert.ToDateTime(period);
                var b = a.ToString("dd/MM/yyyy");
                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));
                DateTime? oldStartCoverdate = Convert.ToDateTime(Properties.Settings.Default.StartNewVersionCI);
                //DateTime? oldStartCoverdate = Convert.ToDateTime("2567-09-01");


                if (a >= oldStartCoverdate)
                {
                    var result = _context.usp_QueueCIAuditInsureReportV2_Select(a).ToList();
                    if (result.Count == 0)
                    {
                        return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
                    }
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Detail");
                        {
                            var auditMotor = _context.usp_QueueCIAuditInsureReportV2_Select(a).Select(o => new
                            {
                                วันเริ่มคุ้มครอง = o.StartCoverDate,
                                App_ID = o.ApplicationCode,
                                สถานะแอพ = o.ApplicationStatus,
                                ผู้เอาประกัน = o.InsuredName,
                                เบอร์โทรผู้เอาประกัน = o.InsuredPhone,
                                ชื่อผู้ชำระเบี้ย = o.PayerName,
                                เบอร์โทรผู้ชำระเบี้ย = o.PayerPhone,
                                สถานที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeName,
                                ID_ตำบล = o.PayerSubDistrictId,
                                ตำบล = o.PayerSubDistrict,
                                ID_อำเภอ = o.PayerDistrictId,
                                อำเภอ = o.PayerDistrict,
                                ID_จังหวัด = o.PayerProvinceId,
                                จังหวัด = o.PayerProvince,
                                สาขาผู้ชำระเบี้ย = o.PayerBranchDetail,
                                ภาคผู้ชำระเบี้ย = o.ZoneDetail,
                                ข้อมูลการเฝ้าระวัง = o.IsBeware == true ? "มี" : "ไม่มี",
                                หมายเหตุข้อมูลการเฝ้าระวัง = o.BewareRemark,
                                รหัสผู้รับมอบคิวงาน = o.QCCode,
                                ชื่อสกุล_ผู้รับมอบคิวงาน = o.QCName,
                                รหัส = o.DirectorCode,
                                ชื่อสกุล_ผอ_บล = o.DirectorName,
                                เขตพื้นที่ = o.StudyArea,
                                รหัสผู้แทน = o.SaleEmployeeCode,
                                ชื่อสกุลผู้แทน = o.SaleEmployeeName,
                                ชื่อเล่นผู้แทน = o.SaleEmployeeNickName,
                                สาขาผู้แทน = o.SaleEmployeeBranch,
                                สถานะการตรวจสอบ_ฝ่ายQC = o.QueueAuditStatusDetail == "n/a" ? null : o.QueueAuditStatusDetail,
                                สถานะการโทร = o.CallStatusDetail == "n/a" ? null : o.CallStatusDetail,
                                จำนวนไม่รับสาย = o.CallStatus2,
                                จำนวนไม่สะดวกคุย = o.CallStatus3,
                                จำนวนติดต่อไม่ได้ = o.CallStatus4,
                                จำนวนเบอร์โทรผิด = o.CallStatus7,
                                จำนวนลูกค้าประสงค์ยกเลิก = o.CallStatus8,
                                รวมสถานะการโทร = o.CallStatusTotal,
                                หมายเหตุการโทร = o.CallStatusRemark,
                                สาเหตุไม่ต้องตรวจสอบ = o.NotAuditedCauseDetail == "n/a" ? null : o.NotAuditedCauseDetail,
                                หมายเหตุไม่ต้องตรวจสอบ = o.NotAuditedCauseRemark,
                                วิธีการขาย = o.SaleMethodTypeDetail == "n/a" ? null : o.SaleMethodTypeDetail,
                                บริการหลังการขาย = o.SaleServiceTypeDetail == "n/a" ? null : o.SaleServiceTypeDetail,
                                หมายเหตุบริการหลังการขาย = o.SaleServiceTypeRemark,
                                ได้รับกรมธรรม์ = o.IsReceivedInsurance == null ? null : o.IsReceivedInsurance == true ? "ได้รับ" : "ไม่ได้รับ",
                                รายละเอียดการได้รับกรมธรรม์ = o.ReceivingInsuranceTypeDetail == "n/a" ? null : o.ReceivingInsuranceTypeDetail,
                                ผลการมอบกรมธรรม์ = o.InsuranceInsuredIsValid == null ? null : o.InsuranceInsuredIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุผลการมอบกรมธรรม์ = o.InsuranceInsuredRemark,
                                ข้อมูลผอ_บล = o.UnderwriteByUserName,
                                เพิ่มเพื่อนกับสยามสไมล์ทาง_Line_OA = o.AddLINEIsSuccess == null ? null : o.AddLINEIsSuccess == true ? "เรียบร้อย" : "ไม่เรียบร้อย",
                                คะแนนความพึงพอใจ = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.FeedbackRate == null ? "ไม่ประเมิน" : o.FeedbackRate.ToString(),
                                ข้อเสนอแนะ = o.FeedbackRemark,
                                ผลด้านบริการ = o.ServiceResultIsIssue == null ? null : o.ServiceResultIsIssue == true ? "มีประเด็น" : "ไม่มีประเด็น",
                                หมายเหตุผลด้านบริการ = o.ServiceResultRemark,
                                ชื่อสกุลผู้เอาประกัน = o.InsuredNameIsValid == null ? null : o.InsuredNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุชื่อสกุลผู้เอาประกัน = o.InsuredNameRemark,
                                ชื่อสกุลผู้ชำระเบี้ย = o.PayerNameIsValid == null ? null : o.PayerNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุชื่อสกุลผู้ชำระเบี้ย = o.PayerNameRemark,
                                วันเดือนปีเกิด = o.BirthDateIsValid == null ? null : o.BirthDateIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุวันเดือนปีเกิด = o.BirthDateRemark,
                                น้ำหนักส่วนสูง = o.WeightHeightIsValid == null ? null : o.WeightHeightIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุน้ำหนัก_ส่วนสูง = o.WeightHeightRemark,
                                ประวัติการแพ้_เช่น_แพ้ยา_แพ้อาหาร_แพ้อากาศ_หรือประวัติการแพ้อื่นๆ = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsAllergyHistory == null ? "จำไม่ได้" : o.IsAllergyHistory == true ? "มี" : "ไม่มี",
                                หมายเหตุประวัติการแพ้ = o.AllergyHistoryRemark,
                                ประวัติการตรวจพบความผิดปกติของตับ_ไต_หัวใจ_ปอด_และสมอง_หรือโรคอื่นๆ_ที่ต้องทานยาเป็นประจำ = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsCriticalIllnessHistory == null ? "จำไม่ได้" : o.IsCriticalIllnessHistory == true ? "มี" : "ไม่มี",
                                หมายเหตุ_ประวัติการตรวจพบความผิดปกติของตับ_ไต_หัวใจ_ปอด_และสมอง_หรือโรคอื่นๆ_ที่ต้องทานยาเป็นประจำ = o.CriticalIllnessHistoryRemark,
                                ประวัติอุบัติเหตุ_เช่น_กระดูกแตก_หัก_ร้าว_เข้าเฝือก_ดามเหล็ก = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsAccidentHistory == null ? "จำไม่ได้" : o.IsAccidentHistory == true ? "มี" : "ไม่มี",
                                หมายเหตุประวัติอุบัติเหตุ = o.AccidentHistoryRemark,
                                ประวัติการผ่าตัด_เช่น_ฝี_ซีสต์_ถุงน้ำ_ก้อนเนื้อ_ก้อนไขมัน_ไส้ติ่ง_ใส้เลื่อน_หรือการผ่าตัดอื่นๆ = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsSurgeryHistory == null ? "จำไม่ได้" : o.IsSurgeryHistory == true ? "มี" : "ไม่มี",
                                หมายเหตุประวัติการผ่าตัด = o.SurgeryHistoryRemark,
                                ประวัติการตรวจสุขภาพประจำปี = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsHealthCheckHistory == null ? "จำไม่ได้" : o.IsHealthCheckHistory == true ? "ตรวจ" : "ไม่ตรวจ",
                                หมายเหตุประวัติการตรวจสุขภาพ = o.HealthCheckHistoryRemark,
                                ประวัติโรคเรื้อรัง_เช่น_ความดันโลหิตสูง_ไขมัน_ไตรกลีเซอไรด์_น้ำตาลในเลือด_กรดยูริค_หรือค่าอื่นๆ = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsChronicDiseaseHistory == null ? "จำไม่ได้" : o.IsChronicDiseaseHistory == true ? "ปกติ" : "ไม่ปกติ",
                                หมายเหตุประวัติโรคเรื้อรัง = o.ChronicDiseaseHistoryRemark,
                                ประวัติการรักษาครั้งล่าสุด = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsMedicalLatestHistory == null ? "จำไม่ได้" : o.IsMedicalLatestHistory == true ? "มี" : "ไม่มี",
                                หมายเหตุประวัติการรักษาครั้งล่าสุด = o.MedicalLatestHistoryRemark,
                                ประวัติสูบบุหรี่ = o.IsSmokingHistory == null ? null : o.IsSmokingHistory == true ? "สูบ" : "ไม่สูบ",
                                หมายเหตุประวัติสูบบุหรี่ = o.SmokingHistoryRemark,
                                ประวัติดื่มสุรา = o.IsDrinkingHistory == null ? null : o.IsDrinkingHistory == true ? "ดื่ม" : "ไม่ดื่ม",
                                หมายเหตุประวัติดื่มสุรา = o.DrinkingHistoryRemark,
                                ค่าการมองเห็น = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsVisionValue == null ? "จำไม่ได้" : o.IsVisionValue == true ? "ปกติ" : "ไม่ปกติ",
                                หมายเหตุค่าการมองเห็น = o.VisionValueRemark,
                                ค่าการได้ยิน = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsHearingValue == null ? "จำไม่ได้" : o.IsHearingValue == true ? "ปกติ" : "ไม่ปกติ",
                                หมายเหตุค่าการได้ยิน = o.HearingValueRemark,
                                ประวัติการตั้งครรภ์_เพศหญิง = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsPregnantHistory == null ? "อื่นๆ" : o.IsPregnantHistory == true ? "ตั้งครรภ์" : "ไม่ตั้งครรภ์",
                                ประวัติสุขภาพอื่นๆ_เพิ่มเติม = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.IsOtherHealthHistory == null ? "จำไม่ได้" : o.IsOtherHealthHistory == true ? "มี" : "ไม่มี",
                                หมายเหตุประวัติสุขภาพอื่นๆ_เพิ่มเติม = o.OtherHealthHistoryRemark,
                                อาชีพผู้เอาประกัน = o.PayerOccupationIsValid == null ? null : o.PayerOccupationIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุอาชีพผู้เอาประกัน = o.PayerOccupationRemark,
                                หน่วยงานผู้ชำระเบี้ย = o.PayerBuildingNameIsValid == null ? null : o.PayerBuildingNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุหน่วยงานผู้ชำระเบี้ย = o.PayerBuildingNameRemark,
                                แผนประกัน = o.ProductIsValid == null ? null : o.ProductIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุแผนประกัน = o.ProductRemark,
                                เบี้ยประกัน = o.PremiumIsValid == null ? null : o.PremiumIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุเบี้ยประกัน = o.PremiumRemark,
                                ประเภทการชำระเบี้ยประกัน = o.PeriodTypeIsValid == null ? null : o.PeriodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุประเภทการชำระเบี้ยประกัน = o.PeriodTypeRemark,
                                ช่องทางการชำระเบี้ยประกัน = o.PayMethodTypeIsValid == null ? null : o.PayMethodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                                หมายเหตุช่องทางการชำระเบี้ยประกัน = o.PayMethodTypeRemark,
                                ผลการตรวจสอบการทำประกัน = o.AuditCIStatusDetail == "n/a" ? null : o.AuditCIStatusDetail,
                                ระบุในใบสมัคร = o.AuditCISpecifyStatusDetail == "n/a" ? null : o.AuditCISpecifyStatusDetail,
                                หมายเหตุประวัติคัดกรอง = o.AuditCIRemark,
                                พฤติกรรมการคัดกรอง_MA = o.UnderwritingBehaviorTypeDetail == "n/a" ? null : o.UnderwritingBehaviorTypeDetail,
                                หมายเหตุพฤติกรรมการคัดกรอง_MA = o.UnderwritingBehaviorRemark,
                                หมายเหตุอื่นๆ = o.AuditOtherCIRemark,
                                วันที่ทำรายการ = o.AuditQCUpdatedByUserCode == null ? null : o.AuditQCUpdatedDate,
                                เวลา = o.AuditQCUpdatedByUserCode == null ? null : o.AuditQCUpdatedTime,
                                รหัสฝ่ายQC = o.AuditQCUpdatedByUserCode,
                                ชื่อผู้ตรวจสอบฝ่ายQC = o.AuditQCUpdatedByUserName,
                                สถานะการตรวจสอบฝ่ายรับประกัน = o.AuditInsureStatusDetail == "n/a" ? null : o.AuditInsureStatusDetail,
                                หมายเหตุ_รอพิจารณา = o.AuditInsureStatusRemark,
                                ผลการพิจารณา_ฝ่ายรับประกัน = o.AuditInsureConsiderStatusDetail == "n/a" ? null : o.AuditInsureConsiderStatusDetail,
                                หมายเหตุ_ผลการพิจารณา = o.AuditInsureConsiderRemark,
                                วันที่ทำรายการพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedDate,
                                เวลาพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedTime,
                                รหัสฝ่ายรับประกัน = o.AuditInsureUpdatedByUserCode,
                                ชื่อผู้พิจารณาฝ่ายรับประกัน = o.AuditInsureUpdatedByUserName,
                                สถานะปิดคิวงาน = o.QueueStatusDetail == "n/a" ? null : o.QueueStatusDetail,
                                สถานะการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                                หมายเหตุการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureCloseRemark
                            }).ToList();

                            workSheet.Cells.LoadFromCollection(auditMotor, true);

                            //edit name headerCells


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
                else
                {
                    var result = _context.usp_QueueCIAuditInsureReport_Select(a).ToList();
                    if (result.Count == 0)
                    {
                        return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
                    }
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Detail");
                        {
                            var auditMotor = _context.usp_QueueCIAuditInsureReport_Select(a).Select(o => new
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
                                วันที่ทำรายการ = (o.วันที่ทำรายการ != null ? Convert.ToDateTime(o.วันที่ทำรายการ).AddYears(543).ToString("dd/MM/yyyy") : ""),
                                เวลา = o.เวลา,
                                รหัสฝ่ายQC = o.รหัส_ฝ่ายQC,
                                ชื่อผู้ตรวจสอบฝ่ายQC = o.ชื่อผู้ตรวจสอบ_ฝ่ายQC,
                                สถานะการตรวจสอบฝ่ายรับประกัน = o.สถานะการตรวจสอบ_ฝ่ายรับประกัน == "n/a" ? "" : o.สถานะการตรวจสอบ_ฝ่ายรับประกัน,
                                หมายเหตุ_รอพิจารณา = o.หมายเหตุ_รอพิจารณา,
                                ผลการพิจารณา_ฝ่ายรับประกัน = o.ผลการพิจารณา_ฝ่ายรับประกัน == "n/a" ? "" : o.ผลการพิจารณา_ฝ่ายรับประกัน,
                                หมายเหตุ_ผลการพิจารณา = o.หมายเหตุ_ผลการพิจารณา,
                                วันที่ทำรายการพิจารณา = (o.วันที่ทำรายการพิจารณา != null ? Convert.ToDateTime(o.วันที่ทำรายการพิจารณา).AddYears(543).ToString("dd/MM/yyyy") : ""),
                                เวลาพิจารณา = o.เวลาพิจารณา == "n/a" ? "" : o.เวลาพิจารณา,
                                รหัสฝ่ายรับประกัน = o.รหัส_ฝ่ายรับประกัน == "n/a" ? "" : o.รหัส_ฝ่ายรับประกัน,
                                ชื่อผู้พิจารณาฝ่ายรับประกัน = o.ชื่อผู้พิจารณา_ฝ่ายรับประกัน == "n/a" ? "" : o.ชื่อผู้พิจารณา_ฝ่ายรับประกัน,
                                สถานะปิดคิวงาน = o.สถานะปิดคิวงาน
                            }).ToList();

                            workSheet.Cells.LoadFromCollection(auditMotor, true);

                            //edit name headerCells

                            workSheet.Cells["AG1"].Value = "หมายเหตุ";
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


                //var result = _context.usp_QueueCIAuditInsureReport_Select(a).ToList();


                // data empty

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

        public ActionResult DownloadV2(string reportName)
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"{reportName}{DateTime.Now.ToString("ddMMyy")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}