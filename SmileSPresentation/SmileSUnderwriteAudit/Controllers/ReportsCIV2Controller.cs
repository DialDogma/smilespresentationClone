﻿using OfficeOpenXml;
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
    public class ReportsCIV2Controller : Controller
    {
        #region Context

        private readonly UnderwriteCancerAuditEntities _context;

        public ReportsCIV2Controller()
        {
            _context = new UnderwriteCancerAuditEntities();
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

                var result = _context.usp_QueueCIAuditInsureReportV2_Select(reriod).ToList();
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var data_sheet1 = result.Select(o => new
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
                    ผลการติดตาม = o.AuditInsureNotConsideredVerifiedTypeDetail,
                    ผลการพิจารณา_ฝ่ายรับประกัน = o.AuditInsureConsiderStatusDetail == "n/a" ? null : o.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = o.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedDate,
                    เวลาพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedTime,
                    รหัสฝ่ายรับประกัน = o.AuditInsureUpdatedByUserCode,
                    ชื่อผู้พิจารณาฝ่ายรับประกัน = o.AuditInsureUpdatedByUserName,
                    สถานะปิดคิวงาน = o.QueueStatusDetail == "n/a" ? null : o.QueueStatusDetail,
                    สถานะการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    หมายเหตุการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureCloseRemark,
                    หมายเหตุติดตามการโทร = o.AuditInsureNotConsideredVerifiedTypeId != 2 ? null : o.AuditInsureCloseRemark,
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

                // var result = _context.usp_QueueCIAuditInsureReportV2_Select(reriod).ToList();
                var result = _context.usp_QueueCIAuditFullDetailReportByConsider_Select(reriod, null).ToList();

                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var data_sheet1 = result.Select(o => new
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
                    ผลการติดตาม = o.AuditInsureNotConsideredVerifiedTypeDetail,
                    ผลการพิจารณา_ฝ่ายรับประกัน = o.AuditInsureConsiderStatusDetail == "n/a" ? null : o.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = o.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedDate,
                    เวลาพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedTime,
                    รหัสฝ่ายรับประกัน = o.AuditInsureUpdatedByUserCode,
                    ชื่อผู้พิจารณาฝ่ายรับประกัน = o.AuditInsureUpdatedByUserName,
                    สถานะปิดคิวงาน = o.QueueStatusDetail == "n/a" ? null : o.QueueStatusDetail,
                    วันที่ปิดคิวงาน = o.AuditInsureCloseUpdatedDate,
                    เวลาที่ปิดคิวงาน = o.AuditInsureCloseUpdatedTime,
                    สถานะการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    หมายเหตุการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureCloseRemark,
                    หมายเหตุติดตามการโทร = o.AuditInsureNotConsideredVerifiedTypeId != 2 ? null : o.AuditInsureCloseRemark,
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
                var result = _context.usp_QueueCIAuditFullDetailReportByConsider_Select(c_period, auditInsureConsiderStatusId).ToList();
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var data_sheet1 = result.Select(o => new
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
                    วันที่ปิดคิวงาน = o.AuditInsureCloseUpdatedDate,
                    เวลาที่ปิดคิวงาน = o.AuditInsureCloseUpdatedTime,
                    สถานะการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    หมายเหตุการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureCloseRemark
                }).ToList();

                var data_sheet2 = result.Select(o => new
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
                    ผลการติดตาม = o.AuditInsureNotConsideredVerifiedTypeDetail,
                    ผลการพิจารณา_ฝ่ายรับประกัน = o.AuditInsureConsiderStatusDetail == "n/a" ? null : o.AuditInsureConsiderStatusDetail,
                    หมายเหตุ_ผลการพิจารณา = o.AuditInsureConsiderRemark,
                    วันที่ทำรายการพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedDate,
                    เวลาพิจารณา = o.AuditInsureUpdatedByUserCode == null ? null : o.AuditInsureUpdatedTime,
                    รหัสฝ่ายรับประกัน = o.AuditInsureUpdatedByUserCode,
                    ชื่อผู้พิจารณาฝ่ายรับประกัน = o.AuditInsureUpdatedByUserName,
                    สถานะปิดคิวงาน = o.QueueStatusDetail == "n/a" ? null : o.QueueStatusDetail,
                    วันที่ปิดคิวงาน = o.AuditInsureCloseUpdatedDate,
                    เวลาที่ปิดคิวงาน = o.AuditInsureCloseUpdatedTime,
                    สถานะการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureClose == true ? "ดำเนินการแล้ว" : "รอดำเนินการ",
                    หมายเหตุการติดตามลูกค้า = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.AuditInsureCloseRemark,
                    หมายเหตุติดตามการโทร = o.AuditInsureNotConsideredVerifiedTypeId != 2 ? null : o.AuditInsureCloseRemark,
                }).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    if (auditInsureConsiderStatusId == "4")
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
                    else
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