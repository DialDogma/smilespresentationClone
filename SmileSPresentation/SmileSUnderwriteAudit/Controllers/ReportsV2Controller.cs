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
    public class ReportsV2Controller : Controller
    {
        #region Context

        private readonly UnderwriteAuditEntities _context;

        public ReportsV2Controller()
        {
            _context = new UnderwriteAuditEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult ReportCheckUnderwrite()
        {
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var periodList = GlobalFunction.PHGetPeriodList(changePeriodDay, 25);
            ViewBag.PeriodList = periodList;
            return View();

        }

        #region API

        public ActionResult underwriteExportQueueReportCheckPeriod(FormCollection form)
        {
            try
            {
                var period = form["period"];
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                DateTime? nPeriod = null;
                if (period != "-1" && period != null)
                {
                    nPeriod = Convert.ToDateTime(period);
                }
                var startNewVersionPH = Properties.Settings.Default.StartNewVersionPH;
                DateTime? oldPeriod = Convert.ToDateTime(startNewVersionPH);

                if (nPeriod >= oldPeriod)
                {
                    return underwriteExportQueueReportV2(form);
                }
                else
                {
                    return underwriteExportQueueReportV1(form);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public ActionResult underwriteExportQueueReportV1(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var p_eriod = form["period"];
                DateTime? reriod = GlobalFunction.ConvertDatePickerToDate_BE(p_eriod);

                var result = _context.usp_QueueAuditFullDetailReport_Select(reriod).ToList();
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var data_sheet1 = result.Select(s => new
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

        public ActionResult underwriteExportQueueReportV2(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var p_eriod = form["period"];
                DateTime? reriod = GlobalFunction.ConvertDatePickerToDate_BE(p_eriod);

                var result = _context.usp_QueueAuditFullDetailReportV2_Select(reriod).ToList();
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var data_sheet1 = result.Select(s => new
                {
                    DCR = s.Period,
                    App_ID = s.ApplicationCode,
                    สถานะแอพ = s.AppStatus,
                    ผู้เอาประกัน = s.InsuredName,
                    เบอร์โทรผู้เอาประกัน = s.InsuredPhone,
                    ผู้ชำระเบี้ย = s.PayerName,
                    เบอร์โทรผู้ชำระเบี้ย = s.PayerPhone,
                    สถานที่ทำงานผู้ชำระเบี้ย = s.PayerBuildingName,
                    ID_ตำบล = s.PayerSubDistrictId,
                    ตำบล = s.SubDistrictDetail,
                    ID_อำเภอ = s.PayerDistrictId,
                    อำเภอ = s.DistrictDetail,
                    ID_จังหวัด = s.Province_ID,
                    จังหวัด = s.ProvinceDetail,
                    สาขาผู้ชำระเบี้ย = s.BranchDetail,
                    โซนผู้ชำระเบี้ย = s.ZoneDetail,
                    ข้อมูลการเฝ้าระวัง = s.IsBeware == true ? "มี" : "ไม่มี",
                    หมายเหตุข้อมูลการเฝ้าระวัง = s.BewareRemark,
                    รหัส_QC = s.QCCode,
                    ชื่อสกุล_QC = s.QCName,
                    รหัส = s.DirectorCode,
                    ชื่อสกุล_ผอ_บล = s.DirectorName,
                    เขตพื้นที่ = s.PayerStudyArea,
                    รหัสผู้แทน = s.SaleEmployeeCode,
                    ชื่อสกุลผู้แทน = s.SaleEmployeeName,
                    ชื่อเล่นผู้แทน = s.SaleEmployeeNickName,
                    สาขาผู้แทน = s.SaleEmployeeBranch,
                    สถานะการตรวจสอบ_ของแผนกQC = s.AuditStatusDetail == "n/a" ? null : s.AuditStatusDetail,
                    สถานะการโทร = s.CallStatusDetail == "n/a" ? null : s.CallStatusDetail,
                    จำนวนไม่รับสาย = s.CallStatusId2,
                    จำนวนไม่สะดวกคุย = s.CallStatusId3,
                    จำนวนติดต่อไม่ได้ = s.CallStatusId4,
                    จำนวนเบอร์โทรผิด = s.CallStatusId7,
                    จำนวนลูกค้าประสงค์ยกเลิก = s.CallStatusId8,
                    รวมสถานะการโทร = s.CallStatusTotal,
                    หมายเหตุการโทร = s.CallStatusRemark,
                    สาเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseDetail == "n/a" ? null : s.NotAuditedCauseDetail,
                    หมายเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseRemark,
                    วิธีการขาย = s.SaleMethodTypeDetail == "n/a" ? null : s.SaleMethodTypeDetail,
                    หมายเหตุวิธีการขาย = s.SaleMethodTypeRemark,
                    บริการหลังการขาย = s.SaleServiceTypeDetail == "n/a" ? null : s.SaleServiceTypeDetail,
                    หมายเหตุบริการหลังการขาย = s.SaleServiceTypeRemark,
                    ผลการมอบบัตร = s.CobrandInsuredIsValid == null ? null : s.CobrandInsuredIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุผลการมอบบัตร = s.CobrandInsuredRemark,
                    ประเภทบัตร = s.InsuranceCardTypeDetail == "n/a" ? null : s.InsuranceCardTypeDetail,
                    หมายเหตุประเภทบัตร = s.InsuranceCardTypeRemark,
                    ข้อมูล_ผอ_บล = s.UnderwriteByUserName,
                    เพิ่มเพื่อนกับสยามสไมล์ทาง_Line_OA = s.QueueAuditStatusId != 3 ? null : s.AddLINEIsSuccess == null ? "ไม่เรียบร้อย" : "เรียบร้อย",
                    รูปแบบบัตรประกัน = s.QueueAuditStatusId != 3 ? null : s.InsuranceCardType,
                    คู่มือ = s.QueueAuditStatusId != 3 ? null : s.IsReceivedManual == null ? "ไม่ได้รับ" : "ได้รับ",
                    รายละเอียดการรับคู่มือ = s.ReceivingManualTypeDetail == "n/a" ? null : s.ReceivingManualTypeDetail,
                    คะแนนความพึงพอใจ = s.QueueAuditStatusId != 3 ? null : s.FeedbackRate == null ? "ไม่แประเมิน" : s.FeedbackRate.ToString(),
                    ข้อเสนอแนะ = s.FeedbackRemark,
                    ผลด้านบริการ = s.QueueAuditStatusId != 3 ? null : s.ServiceResultIsIssue != null ? null : s.ServiceResultIsIssue == true ? "มีประเด็น" : "ไม่มีประเด็น",
                    หมายเหตุผลด้านบริการ = s.ServiceResultRemark,
                    ชื่อสกุลผู้เอาประกัน = s.InsuredNameIsValid == null ? null : s.InsuredNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้เอาประกัน = s.InsuredNameRemark,
                    ชื่อสกุลผู้ชำระเบี้ย = s.PayerNameIsValid == null ? null : s.PayerNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้ชำระเบี้ย = s.PayerNameRemark,
                    วันเดือนปีเกิด = s.BirthDateIsValid == null ? null : s.BirthDateIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุวันเดือนปีเกิด = s.BirthDateRemark,
                    น้ำหนักส่วนสูง = s.WeightHeightIsValid == null ? null : s.WeightHeightIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุน้ำหนัก_ส่วนสูง = s.WeightHeightRemark,
                    ประวัติการแพ้_เช่น_แพ้ยา_แพ้อาหาร_แพ้อากาศ_หรือประวัติการแพ้อื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsAllergyHistory == null ? "จำไม่ได้" : s.IsAllergyHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการแพ้ = s.AllergyHistoryRemark,
                    ประวัติการรักษาทั่วไป_เช่น_หอบหืด_ภูมิแพ้_ทอลซิล_ไทรอยด์_ไซนัส_การเคย_พ่นยาขยายหลอดลม_โรคเกี่ยวกับกระเพาะอาหาร_โรคตับ_โรคไต_โรคหัวใจ_โรคประจำตัวอื่นๆ_หรือต้องทานยาเป็นประจำ = s.QueueAuditStatusId != 3 ? null : s.IsMedicalHistory == null ? "จำไม่ได้" : s.IsMedicalHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาทั่วไป = s.MedicalHistoryRemark,
                    ประวัติอุบัติเหตุ_เช่น_กระดูกแตก_หัก_ร้าว_เข้าเฝือก_ดามเหล็ก = s.QueueAuditStatusId != 3 ? null : s.IsAccidentHistory == null ? "จำไม่ได้" : s.IsAccidentHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติอุบัติเหตุ = s.AccidentHistoryRemark,
                    ประวัติการผ่าตัด_เช่น_ฝี_ซีสต์_ถุงน้ำ_ก้อนเนื้อ_ก้อนไขมัน_ไส้ติ่ง_ใส้เลื่อน_หรือการผ่าตัดอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsSurgeryHistory == null ? "จำไม่ได้" : s.IsSurgeryHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการผ่าตัด = s.SurgeryHistoryRemark,
                    ประวัติการตรวจสุขภาพประจำปี = s.QueueAuditStatusId != 3 ? null : s.IsHealthCheckHistory == null ? "จำไม่ได้" : s.IsHealthCheckHistory == true ? "ตรวจ" : "ไม่ตรวจ",
                    หมายเหตุประวัติการตรวจสุขภาพ = s.HealthCheckHistoryRemark,
                    ประวัติโรคเรื้อรัง_เช่น_ความดันโลหิตสูง_ไขมัน_ไตรกลีเซอไรด์_น้ำตาลในเลือด_กรดยูริค_หรือค่าอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsChronicDiseaseHistory == null ? "จำไม่ได้" : s.IsChronicDiseaseHistory == true ? "ปกติ" : "ไม่ปกติ",
                    หมายเหตุประวัติโรคเรื้อรัง = s.ChronicDiseaseHistoryRemark,
                    ประวัติการรักษาครั้งล่าสุด = s.QueueAuditStatusId != 3 ? null : s.IsMedicalLatestHistory == null ? "จำไม่ได้" : s.IsMedicalLatestHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาครั้งล่าสุด = s.MedicalLatestHistoryRemark,
                    ประวัติสูบบุหรี่ = s.IsSmokingHistory == null ? null : s.IsSmokingHistory == true ? "สูบ" : "ไม่สูบ",
                    หมายเหตุประวัติสูบบุหรี่ = s.SmokingHistoryRemark,
                    ประวัติดื่มสุรา = s.IsDrinkingHistory == null ? null : s.IsDrinkingHistory == true ? "ดื่ม" : "ไม่ดื่ม",
                    หมายเหตุประวัติดื่มสุรา = s.DrinkingHistoryRemark,
                    ประวัติการตั้งครรภ์_เพศหญิง = s.QueueAuditStatusId != 3 ? null : s.IsPregnantHistory == null ? "อื่นๆ" : s.IsPregnantHistory == true ? "ตั้งครรภ์" : "ไม่ตั้งครรภ์",
                    ประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.QueueAuditStatusId != 3 ? null : s.IsOtherHealthHistory == null ? "จำไม่ได้" : s.IsOtherHealthHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.OtherHealthHistoryRemark,
                    อาชีพผู้เอาประกัน = s.PayerOccupationIsValid == null ? null : s.PayerOccupationIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุอาชีพผู้เอาประกัน = s.PayerOccupationRemark,
                    หน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameIsValid == null ? null : s.PayerBuildingNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุหน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameRemark,
                    แผนประกัน = s.ProductIsValid == null ? null : s.ProductIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุแผนประกัน = s.ProductRemark,
                    เบี้ยประกัน = s.PremiumIsValid == null ? null : s.PremiumIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุเบี้ยประกัน = s.PremiumRemark,
                    ประเภทการชำระเบี้ยประกัน = s.PeriodTypeIsValid == null ? null : s.PeriodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุประเภทการชำระเบี้ยประกัน = s.PeriodTypeRemark,
                    ช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeIsValid == null ? null : s.PayMethodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeRemark,
                    ผลการตรวจสอบด้านสุขภาพ_ของแผนกQC = s.AuditHealthStatusDetail == "n/a" ? null : s.AuditHealthStatusDetail,
                    ระบุในใบสมัคร = s.AuditHealthSpecifyStatusDetail == "n/a" ? null : s.AuditHealthSpecifyStatusDetail,
                    หมายเหตุประวัติคัดกรอง = s.AuditHealthRemark,
                    พฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorTypeDetail == "n/a" ? null : s.UnderwritingBehaviorTypeDetail,
                    หมายเหตุพฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorRemark,
                    หมายเหตุอื่นๆ = s.AuditOtherHealthRemark,
                    วันที่ทำรายการ = s.AuditQCUpdatedDate,
                    เวลา = s.AuditQCUpdatedTime,
                    รหัส_แผนกQC = s.AuditQCUpdatedByUserCode,
                    ชื่อผู้ตรวจสอบ_แผนกQC = s.AuditQCUpdatedByUserName,
                    สถานะการตรวจสอบ_ฝ่ายรับประกัน = s.AuditInsureStatusDetail == "n/a" ? null : s.AuditInsureStatusDetail,
                    หมายเหตุ_รอพิจารณา = s.AuditInsureStatusRemark,
                    ผลการพิจารณา_ฝ่ายรับประกัน = s.AuditInsureConsiderStatusDetail == "n/a" ? null : s.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = s.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = s.AuditInsureUpdatedDate,
                    เวลาพิจารณา = s.AuditInsureUpdatedTime,
                    รหัส_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserCode,
                    ชื่อผู้พิจารณา_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserName,
                    สถานะปิดคิวงาน = s.QueueStatusDetail == "n/a" ? null : s.QueueStatusDetail,
                    สถานะการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    หมายเหตุการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureCloseRemark,
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
        public ActionResult underwriteExportQueueReportAuditInsure(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var p_eriod = form["period"];
                DateTime? reriod = GlobalFunction.ConvertDatePickerToDate_BE(p_eriod);
                var result = _context.usp_QueueAuditFullDetailReportByConsider_Select(reriod, null).ToList();//
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var data_sheet1 = result.Select(s => new
                {
                    DCR = s.Period,
                    App_ID = s.ApplicationCode,
                    สถานะแอพ = s.AppStatus,
                    ผู้เอาประกัน = s.InsuredName,
                    เบอร์โทรผู้เอาประกัน = s.InsuredPhone,
                    ผู้ชำระเบี้ย = s.PayerName,
                    เบอร์โทรผู้ชำระเบี้ย = s.PayerPhone,
                    สถานที่ทำงานผู้ชำระเบี้ย = s.PayerBuildingName,
                    ID_ตำบล = s.PayerSubDistrictId,
                    ตำบล = s.SubDistrictDetail,
                    ID_อำเภอ = s.PayerDistrictId,
                    อำเภอ = s.DistrictDetail,
                    ID_จังหวัด = s.Province_ID,
                    จังหวัด = s.ProvinceDetail,
                    สาขาผู้ชำระเบี้ย = s.BranchDetail,
                    โซนผู้ชำระเบี้ย = s.ZoneDetail,
                    ข้อมูลการเฝ้าระวัง = s.IsBeware == true ? "มี" : "ไม่มี",
                    หมายเหตุข้อมูลการเฝ้าระวัง = s.BewareRemark,
                    รหัส_QC = s.QCCode,
                    ชื่อสกุล_QC = s.QCName,
                    รหัส = s.DirectorCode,
                    ชื่อสกุล_ผอ_บล = s.DirectorName,
                    เขตพื้นที่ = s.PayerStudyArea,
                    รหัสผู้แทน = s.SaleEmployeeCode,
                    ชื่อสกุลผู้แทน = s.SaleEmployeeName,
                    ชื่อเล่นผู้แทน = s.SaleEmployeeNickName,
                    สาขาผู้แทน = s.SaleEmployeeBranch,
                    สถานะการตรวจสอบ_ของแผนกQC = s.AuditStatusDetail == "n/a" ? null : s.AuditStatusDetail,
                    สถานะการโทร = s.CallStatusDetail == "n/a" ? null : s.CallStatusDetail,
                    จำนวนไม่รับสาย = s.CallStatusId2,
                    จำนวนไม่สะดวกคุย = s.CallStatusId3,
                    จำนวนติดต่อไม่ได้ = s.CallStatusId4,
                    จำนวนเบอร์โทรผิด = s.CallStatusId7,
                    จำนวนลูกค้าประสงค์ยกเลิก = s.CallStatusId8,
                    รวมสถานะการโทร = s.CallStatusTotal,
                    หมายเหตุการโทร = s.CallStatusRemark,
                    สาเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseDetail == "n/a" ? null : s.NotAuditedCauseDetail,
                    หมายเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseRemark,
                    วิธีการขาย = s.SaleMethodTypeDetail == "n/a" ? null : s.SaleMethodTypeDetail,
                    หมายเหตุวิธีการขาย = s.SaleMethodTypeRemark,
                    บริการหลังการขาย = s.SaleServiceTypeDetail == "n/a" ? null : s.SaleServiceTypeDetail,
                    หมายเหตุบริการหลังการขาย = s.SaleServiceTypeRemark,
                    ผลการมอบบัตร = s.CobrandInsuredIsValid == null ? null : s.CobrandInsuredIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุผลการมอบบัตร = s.CobrandInsuredRemark,
                    ประเภทบัตร = s.InsuranceCardTypeDetail == "n/a" ? null : s.InsuranceCardTypeDetail,
                    หมายเหตุประเภทบัตร = s.InsuranceCardTypeRemark,
                    ข้อมูล_ผอ_บล = s.UnderwriteByUserName,
                    เพิ่มเพื่อนกับสยามสไมล์ทาง_Line_OA = s.QueueAuditStatusId != 3 ? null : s.AddLINEIsSuccess == null ? "ไม่เรียบร้อย" : "เรียบร้อย",
                    รูปแบบบัตรประกัน = s.QueueAuditStatusId != 3 ? null : s.InsuranceCardType,
                    คู่มือ = s.QueueAuditStatusId != 3 ? null : s.IsReceivedManual == null ? "ไม่ได้รับ" : "ได้รับ",
                    รายละเอียดการรับคู่มือ = s.ReceivingManualTypeDetail == "n/a" ? null : s.ReceivingManualTypeDetail,
                    คะแนนความพึงพอใจ = s.QueueAuditStatusId != 3 ? null : s.FeedbackRate == null ? "ไม่แประเมิน" : s.FeedbackRate.ToString(),
                    ข้อเสนอแนะ = s.FeedbackRemark,
                    ผลด้านบริการ = s.QueueAuditStatusId != 3 ? null : s.ServiceResultIsIssue != null ? null : s.ServiceResultIsIssue == true ? "มีประเด็น" : "ไม่มีประเด็น",
                    หมายเหตุผลด้านบริการ = s.ServiceResultRemark,
                    ชื่อสกุลผู้เอาประกัน = s.InsuredNameIsValid == null ? null : s.InsuredNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้เอาประกัน = s.InsuredNameRemark,
                    ชื่อสกุลผู้ชำระเบี้ย = s.PayerNameIsValid == null ? null : s.PayerNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้ชำระเบี้ย = s.PayerNameRemark,
                    วันเดือนปีเกิด = s.BirthDateIsValid == null ? null : s.BirthDateIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุวันเดือนปีเกิด = s.BirthDateRemark,
                    น้ำหนักส่วนสูง = s.WeightHeightIsValid == null ? null : s.WeightHeightIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุน้ำหนัก_ส่วนสูง = s.WeightHeightRemark,
                    ประวัติการแพ้_เช่น_แพ้ยา_แพ้อาหาร_แพ้อากาศ_หรือประวัติการแพ้อื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsAllergyHistory == null ? "จำไม่ได้" : s.IsAllergyHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการแพ้ = s.AllergyHistoryRemark,
                    ประวัติการรักษาทั่วไป_เช่น_หอบหืด_ภูมิแพ้_ทอลซิล_ไทรอยด์_ไซนัส_การเคย_พ่นยาขยายหลอดลม_โรคเกี่ยวกับกระเพาะอาหาร_โรคตับ_โรคไต_โรคหัวใจ_โรคประจำตัวอื่นๆ_หรือต้องทานยาเป็นประจำ = s.QueueAuditStatusId != 3 ? null : s.IsMedicalHistory == null ? "จำไม่ได้" : s.IsMedicalHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาทั่วไป = s.MedicalHistoryRemark,
                    ประวัติอุบัติเหตุ_เช่น_กระดูกแตก_หัก_ร้าว_เข้าเฝือก_ดามเหล็ก = s.QueueAuditStatusId != 3 ? null : s.IsAccidentHistory == null ? "จำไม่ได้" : s.IsAccidentHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติอุบัติเหตุ = s.AccidentHistoryRemark,
                    ประวัติการผ่าตัด_เช่น_ฝี_ซีสต์_ถุงน้ำ_ก้อนเนื้อ_ก้อนไขมัน_ไส้ติ่ง_ใส้เลื่อน_หรือการผ่าตัดอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsSurgeryHistory == null ? "จำไม่ได้" : s.IsSurgeryHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการผ่าตัด = s.SurgeryHistoryRemark,
                    ประวัติการตรวจสุขภาพประจำปี = s.QueueAuditStatusId != 3 ? null : s.IsHealthCheckHistory == null ? "จำไม่ได้" : s.IsHealthCheckHistory == true ? "ตรวจ" : "ไม่ตรวจ",
                    หมายเหตุประวัติการตรวจสุขภาพ = s.HealthCheckHistoryRemark,
                    ประวัติโรคเรื้อรัง_เช่น_ความดันโลหิตสูง_ไขมัน_ไตรกลีเซอไรด์_น้ำตาลในเลือด_กรดยูริค_หรือค่าอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsChronicDiseaseHistory == null ? "จำไม่ได้" : s.IsChronicDiseaseHistory == true ? "ปกติ" : "ไม่ปกติ",
                    หมายเหตุประวัติโรคเรื้อรัง = s.ChronicDiseaseHistoryRemark,
                    ประวัติการรักษาครั้งล่าสุด = s.QueueAuditStatusId != 3 ? null : s.IsMedicalLatestHistory == null ? "จำไม่ได้" : s.IsMedicalLatestHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาครั้งล่าสุด = s.MedicalLatestHistoryRemark,
                    ประวัติสูบบุหรี่ = s.IsSmokingHistory == null ? null : s.IsSmokingHistory == true ? "สูบ" : "ไม่สูบ",
                    หมายเหตุประวัติสูบบุหรี่ = s.SmokingHistoryRemark,
                    ประวัติดื่มสุรา = s.IsDrinkingHistory == null ? null : s.IsDrinkingHistory == true ? "ดื่ม" : "ไม่ดื่ม",
                    หมายเหตุประวัติดื่มสุรา = s.DrinkingHistoryRemark,
                    ประวัติการตั้งครรภ์_เพศหญิง = s.QueueAuditStatusId != 3 ? null : s.IsPregnantHistory == null ? "อื่นๆ" : s.IsPregnantHistory == true ? "ตั้งครรภ์" : "ไม่ตั้งครรภ์",
                    ประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.QueueAuditStatusId != 3 ? null : s.IsOtherHealthHistory == null ? "จำไม่ได้" : s.IsOtherHealthHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.OtherHealthHistoryRemark,
                    อาชีพผู้เอาประกัน = s.PayerOccupationIsValid == null ? null : s.PayerOccupationIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุอาชีพผู้เอาประกัน = s.PayerOccupationRemark,
                    หน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameIsValid == null ? null : s.PayerBuildingNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุหน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameRemark,
                    แผนประกัน = s.ProductIsValid == null ? null : s.ProductIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุแผนประกัน = s.ProductRemark,
                    เบี้ยประกัน = s.PremiumIsValid == null ? null : s.PremiumIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุเบี้ยประกัน = s.PremiumRemark,
                    ประเภทการชำระเบี้ยประกัน = s.PeriodTypeIsValid == null ? null : s.PeriodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุประเภทการชำระเบี้ยประกัน = s.PeriodTypeRemark,
                    ช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeIsValid == null ? null : s.PayMethodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeRemark,
                    ผลการตรวจสอบด้านสุขภาพ_ของแผนกQC = s.AuditHealthStatusDetail == "n/a" ? null : s.AuditHealthStatusDetail,
                    ระบุในใบสมัคร = s.AuditHealthSpecifyStatusDetail == "n/a" ? null : s.AuditHealthSpecifyStatusDetail,
                    หมายเหตุประวัติคัดกรอง = s.AuditHealthRemark,
                    พฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorTypeDetail == "n/a" ? null : s.UnderwritingBehaviorTypeDetail,
                    หมายเหตุพฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorRemark,
                    หมายเหตุอื่นๆ = s.AuditOtherHealthRemark,
                    วันที่ทำรายการ = s.AuditQCUpdatedDate,
                    เวลา = s.AuditQCUpdatedTime,
                    รหัส_แผนกQC = s.AuditQCUpdatedByUserCode,
                    ชื่อผู้ตรวจสอบ_แผนกQC = s.AuditQCUpdatedByUserName,
                    สถานะการตรวจสอบ_ฝ่ายรับประกัน = s.AuditInsureStatusDetail == "n/a" ? null : s.AuditInsureStatusDetail,
                    หมายเหตุ_รอพิจารณา = s.AuditInsureStatusRemark,
                    ผลการติดตาม = s.AuditInsureNotConsideredVerifiedTypeDetail,
                    ผลการพิจารณา_ฝ่ายรับประกัน = s.AuditInsureConsiderStatusDetail == "n/a" ? null : s.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = s.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = s.AuditInsureUpdatedDate,
                    เวลาพิจารณา = s.AuditInsureUpdatedTime,
                    รหัส_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserCode,
                    ชื่อผู้พิจารณา_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserName,
                    สถานะปิดคิวงาน = s.QueueStatusDetail == "n/a" ? null : s.QueueStatusDetail,
                    สถานะการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    วันที่ปิดคิวงาน = s.AuditInsureCloseUpdatedDate,
                    เวลาที่ปิดคิวงาน = s.AuditInsureCloseUpdatedTime,
                    หมายเหตุการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureCloseRemark,
                    หมายเหตุติดตามการโทร = s.AuditInsureNotConsideredVerifiedTypeId != 2 ? null : s.AuditInsureCloseRemark,
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

        public ActionResult underwriteExportQueueReportAuditInsureConsiderStatus(string period, string auditInsureConsiderStatusId)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                DateTime? c_period = null;
                if (period != null && period != "")
                {
                    c_period = GlobalFunction.ConvertDatePickerToDate_BE(period);
                }
                var result = _context.usp_QueueAuditFullDetailReportByConsider_Select(c_period, auditInsureConsiderStatusId).ToList();
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var data_sheet1 = result.Select(s => new
                {
                    DCR = s.Period,
                    App_ID = s.ApplicationCode,
                    สถานะแอพ = s.AppStatus,
                    ผู้เอาประกัน = s.InsuredName,
                    เบอร์โทรผู้เอาประกัน = s.InsuredPhone,
                    ผู้ชำระเบี้ย = s.PayerName,
                    เบอร์โทรผู้ชำระเบี้ย = s.PayerPhone,
                    สถานที่ทำงานผู้ชำระเบี้ย = s.PayerBuildingName,
                    ID_ตำบล = s.PayerSubDistrictId,
                    ตำบล = s.SubDistrictDetail,
                    ID_อำเภอ = s.PayerDistrictId,
                    อำเภอ = s.DistrictDetail,
                    ID_จังหวัด = s.Province_ID,
                    จังหวัด = s.ProvinceDetail,
                    สาขาผู้ชำระเบี้ย = s.BranchDetail,
                    โซนผู้ชำระเบี้ย = s.ZoneDetail,
                    ข้อมูลการเฝ้าระวัง = s.IsBeware == true ? "มี" : "ไม่มี",
                    หมายเหตุข้อมูลการเฝ้าระวัง = s.BewareRemark,
                    รหัส_QC = s.QCCode,
                    ชื่อสกุล_QC = s.QCName,
                    รหัส = s.DirectorCode,
                    ชื่อสกุล_ผอ_บล = s.DirectorName,
                    เขตพื้นที่ = s.PayerStudyArea,
                    รหัสผู้แทน = s.SaleEmployeeCode,
                    ชื่อสกุลผู้แทน = s.SaleEmployeeName,
                    ชื่อเล่นผู้แทน = s.SaleEmployeeNickName,
                    สาขาผู้แทน = s.SaleEmployeeBranch,
                    สถานะการตรวจสอบ_ของแผนกQC = s.AuditStatusDetail == "n/a" ? null : s.AuditStatusDetail,
                    สถานะการโทร = s.CallStatusDetail == "n/a" ? null : s.CallStatusDetail,
                    จำนวนไม่รับสาย = s.CallStatusId2,
                    จำนวนไม่สะดวกคุย = s.CallStatusId3,
                    จำนวนติดต่อไม่ได้ = s.CallStatusId4,
                    จำนวนเบอร์โทรผิด = s.CallStatusId7,
                    จำนวนลูกค้าประสงค์ยกเลิก = s.CallStatusId8,
                    รวมสถานะการโทร = s.CallStatusTotal,
                    หมายเหตุการโทร = s.CallStatusRemark,
                    สาเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseDetail == "n/a" ? null : s.NotAuditedCauseDetail,
                    หมายเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseRemark,
                    วิธีการขาย = s.SaleMethodTypeDetail == "n/a" ? null : s.SaleMethodTypeDetail,
                    หมายเหตุวิธีการขาย = s.SaleMethodTypeRemark,
                    บริการหลังการขาย = s.SaleServiceTypeDetail == "n/a" ? null : s.SaleServiceTypeDetail,
                    หมายเหตุบริการหลังการขาย = s.SaleServiceTypeRemark,
                    ผลการมอบบัตร = s.CobrandInsuredIsValid == null ? null : s.CobrandInsuredIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุผลการมอบบัตร = s.CobrandInsuredRemark,
                    ประเภทบัตร = s.InsuranceCardTypeDetail == "n/a" ? null : s.InsuranceCardTypeDetail,
                    หมายเหตุประเภทบัตร = s.InsuranceCardTypeRemark,
                    ข้อมูล_ผอ_บล = s.UnderwriteByUserName,
                    เพิ่มเพื่อนกับสยามสไมล์ทาง_Line_OA = s.QueueAuditStatusId != 3 ? null : s.AddLINEIsSuccess == null ? "ไม่เรียบร้อย" : "เรียบร้อย",
                    รูปแบบบัตรประกัน = s.QueueAuditStatusId != 3 ? null : s.InsuranceCardType,
                    คู่มือ = s.QueueAuditStatusId != 3 ? null : s.IsReceivedManual == null ? "ไม่ได้รับ" : "ได้รับ",
                    รายละเอียดการรับคู่มือ = s.ReceivingManualTypeDetail == "n/a" ? null : s.ReceivingManualTypeDetail,
                    คะแนนความพึงพอใจ = s.QueueAuditStatusId != 3 ? null : s.FeedbackRate == null ? "ไม่แประเมิน" : s.FeedbackRate.ToString(),
                    ข้อเสนอแนะ = s.FeedbackRemark,
                    ผลด้านบริการ = s.QueueAuditStatusId != 3 ? null : s.ServiceResultIsIssue != null ? null : s.ServiceResultIsIssue == true ? "มีประเด็น" : "ไม่มีประเด็น",
                    หมายเหตุผลด้านบริการ = s.ServiceResultRemark,
                    ชื่อสกุลผู้เอาประกัน = s.InsuredNameIsValid == null ? null : s.InsuredNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้เอาประกัน = s.InsuredNameRemark,
                    ชื่อสกุลผู้ชำระเบี้ย = s.PayerNameIsValid == null ? null : s.PayerNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้ชำระเบี้ย = s.PayerNameRemark,
                    วันเดือนปีเกิด = s.BirthDateIsValid == null ? null : s.BirthDateIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุวันเดือนปีเกิด = s.BirthDateRemark,
                    น้ำหนักส่วนสูง = s.WeightHeightIsValid == null ? null : s.WeightHeightIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุน้ำหนัก_ส่วนสูง = s.WeightHeightRemark,
                    ประวัติการแพ้_เช่น_แพ้ยา_แพ้อาหาร_แพ้อากาศ_หรือประวัติการแพ้อื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsAllergyHistory == null ? "จำไม่ได้" : s.IsAllergyHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการแพ้ = s.AllergyHistoryRemark,
                    ประวัติการรักษาทั่วไป_เช่น_หอบหืด_ภูมิแพ้_ทอลซิล_ไทรอยด์_ไซนัส_การเคย_พ่นยาขยายหลอดลม_โรคเกี่ยวกับกระเพาะอาหาร_โรคตับ_โรคไต_โรคหัวใจ_โรคประจำตัวอื่นๆ_หรือต้องทานยาเป็นประจำ = s.QueueAuditStatusId != 3 ? null : s.IsMedicalHistory == null ? "จำไม่ได้" : s.IsMedicalHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาทั่วไป = s.MedicalHistoryRemark,
                    ประวัติอุบัติเหตุ_เช่น_กระดูกแตก_หัก_ร้าว_เข้าเฝือก_ดามเหล็ก = s.QueueAuditStatusId != 3 ? null : s.IsAccidentHistory == null ? "จำไม่ได้" : s.IsAccidentHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติอุบัติเหตุ = s.AccidentHistoryRemark,
                    ประวัติการผ่าตัด_เช่น_ฝี_ซีสต์_ถุงน้ำ_ก้อนเนื้อ_ก้อนไขมัน_ไส้ติ่ง_ใส้เลื่อน_หรือการผ่าตัดอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsSurgeryHistory == null ? "จำไม่ได้" : s.IsSurgeryHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการผ่าตัด = s.SurgeryHistoryRemark,
                    ประวัติการตรวจสุขภาพประจำปี = s.QueueAuditStatusId != 3 ? null : s.IsHealthCheckHistory == null ? "จำไม่ได้" : s.IsHealthCheckHistory == true ? "ตรวจ" : "ไม่ตรวจ",
                    หมายเหตุประวัติการตรวจสุขภาพ = s.HealthCheckHistoryRemark,
                    ประวัติโรคเรื้อรัง_เช่น_ความดันโลหิตสูง_ไขมัน_ไตรกลีเซอไรด์_น้ำตาลในเลือด_กรดยูริค_หรือค่าอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsChronicDiseaseHistory == null ? "จำไม่ได้" : s.IsChronicDiseaseHistory == true ? "ปกติ" : "ไม่ปกติ",
                    หมายเหตุประวัติโรคเรื้อรัง = s.ChronicDiseaseHistoryRemark,
                    ประวัติการรักษาครั้งล่าสุด = s.QueueAuditStatusId != 3 ? null : s.IsMedicalLatestHistory == null ? "จำไม่ได้" : s.IsMedicalLatestHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาครั้งล่าสุด = s.MedicalLatestHistoryRemark,
                    ประวัติสูบบุหรี่ = s.IsSmokingHistory == null ? null : s.IsSmokingHistory == true ? "สูบ" : "ไม่สูบ",
                    หมายเหตุประวัติสูบบุหรี่ = s.SmokingHistoryRemark,
                    ประวัติดื่มสุรา = s.IsDrinkingHistory == null ? null : s.IsDrinkingHistory == true ? "ดื่ม" : "ไม่ดื่ม",
                    หมายเหตุประวัติดื่มสุรา = s.DrinkingHistoryRemark,
                    ประวัติการตั้งครรภ์_เพศหญิง = s.QueueAuditStatusId != 3 ? null : s.IsPregnantHistory == null ? "อื่นๆ" : s.IsPregnantHistory == true ? "ตั้งครรภ์" : "ไม่ตั้งครรภ์",
                    ประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.QueueAuditStatusId != 3 ? null : s.IsOtherHealthHistory == null ? "จำไม่ได้" : s.IsOtherHealthHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.OtherHealthHistoryRemark,
                    อาชีพผู้เอาประกัน = s.PayerOccupationIsValid == null ? null : s.PayerOccupationIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุอาชีพผู้เอาประกัน = s.PayerOccupationRemark,
                    หน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameIsValid == null ? null : s.PayerBuildingNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุหน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameRemark,
                    แผนประกัน = s.ProductIsValid == null ? null : s.ProductIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุแผนประกัน = s.ProductRemark,
                    เบี้ยประกัน = s.PremiumIsValid == null ? null : s.PremiumIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุเบี้ยประกัน = s.PremiumRemark,
                    ประเภทการชำระเบี้ยประกัน = s.PeriodTypeIsValid == null ? null : s.PeriodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุประเภทการชำระเบี้ยประกัน = s.PeriodTypeRemark,
                    ช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeIsValid == null ? null : s.PayMethodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeRemark,
                    ผลการตรวจสอบด้านสุขภาพ_ของแผนกQC = s.AuditHealthStatusDetail == "n/a" ? null : s.AuditHealthStatusDetail,
                    ระบุในใบสมัคร = s.AuditHealthSpecifyStatusDetail == "n/a" ? null : s.AuditHealthSpecifyStatusDetail,
                    หมายเหตุประวัติคัดกรอง = s.AuditHealthRemark,
                    พฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorTypeDetail == "n/a" ? null : s.UnderwritingBehaviorTypeDetail,
                    หมายเหตุพฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorRemark,
                    หมายเหตุอื่นๆ = s.AuditOtherHealthRemark,
                    วันที่ทำรายการ = s.AuditQCUpdatedDate,
                    เวลา = s.AuditQCUpdatedTime,
                    รหัส_แผนกQC = s.AuditQCUpdatedByUserCode,
                    ชื่อผู้ตรวจสอบ_แผนกQC = s.AuditQCUpdatedByUserName,
                    สถานะการตรวจสอบ_ฝ่ายรับประกัน = s.AuditInsureStatusDetail == "n/a" ? null : s.AuditInsureStatusDetail,
                    หมายเหตุ_รอพิจารณา = s.AuditInsureStatusRemark,
                    ผลการติดตาม = s.AuditInsureNotConsideredVerifiedTypeDetail,
                    ผลการพิจารณา_ฝ่ายรับประกัน = s.AuditInsureConsiderStatusDetail == "n/a" ? null : s.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = s.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = s.AuditInsureUpdatedDate,
                    เวลาพิจารณา = s.AuditInsureUpdatedTime,
                    รหัส_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserCode,
                    ชื่อผู้พิจารณา_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserName,
                    สถานะปิดคิวงาน = s.QueueStatusDetail == "n/a" ? null : s.QueueStatusDetail,
                    สถานะการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    วันที่ปิดคิวงาน = s.AuditInsureCloseUpdatedDate,
                    เวลาที่ปิดคิวงาน = s.AuditInsureCloseUpdatedTime,
                    หมายเหตุการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureCloseRemark,
                    หมายเหตุติดตามการโทร = s.AuditInsureNotConsideredVerifiedTypeId != 2 ? null : s.AuditInsureCloseRemark,
                }).ToList();


                var data_sheet2 = result.Select(s => new
                {
                    DCR = s.Period,
                    App_ID = s.ApplicationCode,
                    สถานะแอพ = s.AppStatus,
                    ผู้เอาประกัน = s.InsuredName,
                    เบอร์โทรผู้เอาประกัน = s.InsuredPhone,
                    ผู้ชำระเบี้ย = s.PayerName,
                    เบอร์โทรผู้ชำระเบี้ย = s.PayerPhone,
                    สถานที่ทำงานผู้ชำระเบี้ย = s.PayerBuildingName,
                    ID_ตำบล = s.PayerSubDistrictId,
                    ตำบล = s.SubDistrictDetail,
                    ID_อำเภอ = s.PayerDistrictId,
                    อำเภอ = s.DistrictDetail,
                    ID_จังหวัด = s.Province_ID,
                    จังหวัด = s.ProvinceDetail,
                    สาขาผู้ชำระเบี้ย = s.BranchDetail,
                    โซนผู้ชำระเบี้ย = s.ZoneDetail,
                    ข้อมูลการเฝ้าระวัง = s.IsBeware == true ? "มี" : "ไม่มี",
                    หมายเหตุข้อมูลการเฝ้าระวัง = s.BewareRemark,
                    รหัส_QC = s.QCCode,
                    ชื่อสกุล_QC = s.QCName,
                    รหัส = s.DirectorCode,
                    ชื่อสกุล_ผอ_บล = s.DirectorName,
                    เขตพื้นที่ = s.PayerStudyArea,
                    รหัสผู้แทน = s.SaleEmployeeCode,
                    ชื่อสกุลผู้แทน = s.SaleEmployeeName,
                    ชื่อเล่นผู้แทน = s.SaleEmployeeNickName,
                    สาขาผู้แทน = s.SaleEmployeeBranch,
                    สถานะการตรวจสอบ_ของแผนกQC = s.AuditStatusDetail == "n/a" ? null : s.AuditStatusDetail,
                    สถานะการโทร = s.CallStatusDetail == "n/a" ? null : s.CallStatusDetail,
                    จำนวนไม่รับสาย = s.CallStatusId2,
                    จำนวนไม่สะดวกคุย = s.CallStatusId3,
                    จำนวนติดต่อไม่ได้ = s.CallStatusId4,
                    จำนวนเบอร์โทรผิด = s.CallStatusId7,
                    จำนวนลูกค้าประสงค์ยกเลิก = s.CallStatusId8,
                    รวมสถานะการโทร = s.CallStatusTotal,
                    หมายเหตุการโทร = s.CallStatusRemark,
                    สาเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseDetail == "n/a" ? null : s.NotAuditedCauseDetail,
                    หมายเหตุไม่ต้องตรวจสอบ = s.NotAuditedCauseRemark,
                    วิธีการขาย = s.SaleMethodTypeDetail == "n/a" ? null : s.SaleMethodTypeDetail,
                    หมายเหตุวิธีการขาย = s.SaleMethodTypeRemark,
                    บริการหลังการขาย = s.SaleServiceTypeDetail == "n/a" ? null : s.SaleServiceTypeDetail,
                    หมายเหตุบริการหลังการขาย = s.SaleServiceTypeRemark,
                    ผลการมอบบัตร = s.CobrandInsuredIsValid == null ? null : s.CobrandInsuredIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุผลการมอบบัตร = s.CobrandInsuredRemark,
                    ประเภทบัตร = s.InsuranceCardTypeDetail == "n/a" ? null : s.InsuranceCardTypeDetail,
                    หมายเหตุประเภทบัตร = s.InsuranceCardTypeRemark,
                    ข้อมูล_ผอ_บล = s.UnderwriteByUserName,
                    เพิ่มเพื่อนกับสยามสไมล์ทาง_Line_OA = s.QueueAuditStatusId != 3 ? null : s.AddLINEIsSuccess == null ? "ไม่เรียบร้อย" : "เรียบร้อย",
                    รูปแบบบัตรประกัน = s.QueueAuditStatusId != 3 ? null : s.InsuranceCardType,
                    คู่มือ = s.QueueAuditStatusId != 3 ? null : s.IsReceivedManual == null ? "ไม่ได้รับ" : "ได้รับ",
                    รายละเอียดการรับคู่มือ = s.ReceivingManualTypeDetail == "n/a" ? null : s.ReceivingManualTypeDetail,
                    คะแนนความพึงพอใจ = s.QueueAuditStatusId != 3 ? null : s.FeedbackRate == null ? "ไม่แประเมิน" : s.FeedbackRate.ToString(),
                    ข้อเสนอแนะ = s.FeedbackRemark,
                    ผลด้านบริการ = s.QueueAuditStatusId != 3 ? null : s.ServiceResultIsIssue != null ? null : s.ServiceResultIsIssue == true ? "มีประเด็น" : "ไม่มีประเด็น",
                    หมายเหตุผลด้านบริการ = s.ServiceResultRemark,
                    ชื่อสกุลผู้เอาประกัน = s.InsuredNameIsValid == null ? null : s.InsuredNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้เอาประกัน = s.InsuredNameRemark,
                    ชื่อสกุลผู้ชำระเบี้ย = s.PayerNameIsValid == null ? null : s.PayerNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุชื่อสกุลผู้ชำระเบี้ย = s.PayerNameRemark,
                    วันเดือนปีเกิด = s.BirthDateIsValid == null ? null : s.BirthDateIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุวันเดือนปีเกิด = s.BirthDateRemark,
                    น้ำหนักส่วนสูง = s.WeightHeightIsValid == null ? null : s.WeightHeightIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุน้ำหนัก_ส่วนสูง = s.WeightHeightRemark,
                    ประวัติการแพ้_เช่น_แพ้ยา_แพ้อาหาร_แพ้อากาศ_หรือประวัติการแพ้อื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsAllergyHistory == null ? "จำไม่ได้" : s.IsAllergyHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการแพ้ = s.AllergyHistoryRemark,
                    ประวัติการรักษาทั่วไป_เช่น_หอบหืด_ภูมิแพ้_ทอลซิล_ไทรอยด์_ไซนัส_การเคย_พ่นยาขยายหลอดลม_โรคเกี่ยวกับกระเพาะอาหาร_โรคตับ_โรคไต_โรคหัวใจ_โรคประจำตัวอื่นๆ_หรือต้องทานยาเป็นประจำ = s.QueueAuditStatusId != 3 ? null : s.IsMedicalHistory == null ? "จำไม่ได้" : s.IsMedicalHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาทั่วไป = s.MedicalHistoryRemark,
                    ประวัติอุบัติเหตุ_เช่น_กระดูกแตก_หัก_ร้าว_เข้าเฝือก_ดามเหล็ก = s.QueueAuditStatusId != 3 ? null : s.IsAccidentHistory == null ? "จำไม่ได้" : s.IsAccidentHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติอุบัติเหตุ = s.AccidentHistoryRemark,
                    ประวัติการผ่าตัด_เช่น_ฝี_ซีสต์_ถุงน้ำ_ก้อนเนื้อ_ก้อนไขมัน_ไส้ติ่ง_ใส้เลื่อน_หรือการผ่าตัดอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsSurgeryHistory == null ? "จำไม่ได้" : s.IsSurgeryHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการผ่าตัด = s.SurgeryHistoryRemark,
                    ประวัติการตรวจสุขภาพประจำปี = s.QueueAuditStatusId != 3 ? null : s.IsHealthCheckHistory == null ? "จำไม่ได้" : s.IsHealthCheckHistory == true ? "ตรวจ" : "ไม่ตรวจ",
                    หมายเหตุประวัติการตรวจสุขภาพ = s.HealthCheckHistoryRemark,
                    ประวัติโรคเรื้อรัง_เช่น_ความดันโลหิตสูง_ไขมัน_ไตรกลีเซอไรด์_น้ำตาลในเลือด_กรดยูริค_หรือค่าอื่นๆ = s.QueueAuditStatusId != 3 ? null : s.IsChronicDiseaseHistory == null ? "จำไม่ได้" : s.IsChronicDiseaseHistory == true ? "ปกติ" : "ไม่ปกติ",
                    หมายเหตุประวัติโรคเรื้อรัง = s.ChronicDiseaseHistoryRemark,
                    ประวัติการรักษาครั้งล่าสุด = s.QueueAuditStatusId != 3 ? null : s.IsMedicalLatestHistory == null ? "จำไม่ได้" : s.IsMedicalLatestHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติการรักษาครั้งล่าสุด = s.MedicalLatestHistoryRemark,
                    ประวัติสูบบุหรี่ = s.IsSmokingHistory == null ? null : s.IsSmokingHistory == true ? "สูบ" : "ไม่สูบ",
                    หมายเหตุประวัติสูบบุหรี่ = s.SmokingHistoryRemark,
                    ประวัติดื่มสุรา = s.IsDrinkingHistory == null ? null : s.IsDrinkingHistory == true ? "ดื่ม" : "ไม่ดื่ม",
                    หมายเหตุประวัติดื่มสุรา = s.DrinkingHistoryRemark,
                    ประวัติการตั้งครรภ์_เพศหญิง = s.QueueAuditStatusId != 3 ? null : s.IsPregnantHistory == null ? "อื่นๆ" : s.IsPregnantHistory == true ? "ตั้งครรภ์" : "ไม่ตั้งครรภ์",
                    ประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.QueueAuditStatusId != 3 ? null : s.IsOtherHealthHistory == null ? "จำไม่ได้" : s.IsOtherHealthHistory == true ? "มี" : "ไม่มี",
                    หมายเหตุประวัติสุขภาพอื่นๆ_เพิ่มเติม = s.OtherHealthHistoryRemark,
                    อาชีพผู้เอาประกัน = s.PayerOccupationIsValid == null ? null : s.PayerOccupationIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุอาชีพผู้เอาประกัน = s.PayerOccupationRemark,
                    หน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameIsValid == null ? null : s.PayerBuildingNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุหน่วยงานผู้ชำระเบี้ย = s.PayerBuildingNameRemark,
                    แผนประกัน = s.ProductIsValid == null ? null : s.ProductIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุแผนประกัน = s.ProductRemark,
                    เบี้ยประกัน = s.PremiumIsValid == null ? null : s.PremiumIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุเบี้ยประกัน = s.PremiumRemark,
                    ประเภทการชำระเบี้ยประกัน = s.PeriodTypeIsValid == null ? null : s.PeriodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุประเภทการชำระเบี้ยประกัน = s.PeriodTypeRemark,
                    ช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeIsValid == null ? null : s.PayMethodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                    หมายเหตุช่องทางการชำระเบี้ยประกัน = s.PayMethodTypeRemark,
                    ผลการตรวจสอบด้านสุขภาพ_ของแผนกQC = s.AuditHealthStatusDetail == "n/a" ? null : s.AuditHealthStatusDetail,
                    ระบุในใบสมัคร = s.AuditHealthSpecifyStatusDetail == "n/a" ? null : s.AuditHealthSpecifyStatusDetail,
                    หมายเหตุประวัติคัดกรอง = s.AuditHealthRemark,
                    พฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorTypeDetail == "n/a" ? null : s.UnderwritingBehaviorTypeDetail,
                    หมายเหตุพฤติกรรมการคัดกรอง_MA = s.UnderwritingBehaviorRemark,
                    หมายเหตุอื่นๆ = s.AuditOtherHealthRemark,
                    วันที่ทำรายการ = s.AuditQCUpdatedDate,
                    เวลา = s.AuditQCUpdatedTime,
                    รหัส_แผนกQC = s.AuditQCUpdatedByUserCode,
                    ชื่อผู้ตรวจสอบ_แผนกQC = s.AuditQCUpdatedByUserName,
                    สถานะการตรวจสอบ_ฝ่ายรับประกัน = s.AuditInsureStatusDetail == "n/a" ? null : s.AuditInsureStatusDetail,
                    หมายเหตุ_รอพิจารณา = s.AuditInsureStatusRemark,
                    ผลการพิจารณา_ฝ่ายรับประกัน = s.AuditInsureConsiderStatusDetail == "n/a" ? null : s.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = s.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = s.AuditInsureUpdatedDate,
                    เวลาพิจารณา = s.AuditInsureUpdatedTime,
                    รหัส_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserCode,
                    ชื่อผู้พิจารณา_ฝ่ายรับประกัน = s.AuditInsureUpdatedByUserName,
                    สถานะปิดคิวงาน = s.QueueStatusDetail == "n/a" ? null : s.QueueStatusDetail,
                    วันที่ปิดคิวงาน = s.AuditInsureCloseUpdatedDate,
                    เวลาที่ปิดคิวงาน = s.AuditInsureCloseUpdatedTime,
                    สถานะการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    หมายเหตุการติดตามลูกค้า = s.QueueAuditStatusId != 3 ? null : s.AuditInsureCloseRemark,

                }).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {

                    if (auditInsureConsiderStatusId == "4")
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
                    else
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("Sheet1");

                        workSheet1.Cells.LoadFromCollection(data_sheet2, true);

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
        }

        public ActionResult underwriteDownloadQueueReport(string reportName)
        {
            if (Session["DownloadExcel_FileUnderwriteQueueReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileUnderwriteQueueReport"] as byte[];
                string excelName = $"{reportName}{DateTime.Now.ToString("ddMMyy")}.xlsx";

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