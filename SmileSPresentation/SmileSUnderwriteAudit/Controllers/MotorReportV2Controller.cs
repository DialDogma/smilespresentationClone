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
    public class MotorReportV2Controller : Controller
    {
        #region Context

        private readonly UnderwriteMotorAuditEntity _context;

        public MotorReportV2Controller()
        {
            _context = new UnderwriteMotorAuditEntity();
        }

        #endregion Context

        #region API

        public ActionResult MotorAuditInsureReportCheckPeriod(string period, string dateFrom, string dateTo)
        {
            try
            {
                var assignToUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                DateTime? nPeriod = null;
                if (period != "-1" && period != null)
                {
                    nPeriod = Convert.ToDateTime(period);
                }
                var startNewVersionMT = Properties.Settings.Default.StartNewVersionMT;
                DateTime? oldPeriod = Convert.ToDateTime(startNewVersionMT);
                return MotorAuditInsureReportV2(dateFrom, dateTo);
                /* if (nPeriod >= oldPeriod)
                 {
                     return MotorAuditInsureReportV2(dateFrom, dateTo);
                 }
                 else
                 {
                     return MotorAuditInsureReportV1(period);
                 }*/
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public ActionResult MotorAuditInsureReportV1(string period)
        {
            try
            {
                var a = Convert.ToDateTime(period);
                var b = a.ToString("dd/MM/yyyy");
                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

                var result = _context.usp_QueueMotorAuditInsureReport_Select(a).ToList();
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    {
                        var auditMotor = result.Select(o => new
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

        public ActionResult MotorAuditInsureReportV2(string dateFrom, string dateTo)
        {
            try
            {
                /* var a = Convert.ToDateTime(period);
                 var b = a.ToString("dd/MM/yyyy");*/
                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));
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
                var result = _context.usp_QueueMotorAuditInsureReportV2_Select(ndateFrom, ndateTo).ToList();
                if (result.Count == 0)
                    return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    {
                        var auditMotor = result.Select(o => new
                        {
                            วันเริ่มคุ้มครอง = o.StartCoverDate,
                            App_ID = o.ApplicationCode,
                            สถานะแอพ = o.ApplicationStatus,
                            ผู้เอาประกัน = o.InsuredName,
                            เบอร์โทรผู้เอาประกัน = o.InsuredPhone,
                            ชื่อผู้ชำระเบี้ย = o.PayerName,
                            เบอร์โทรผู้ชำระเบี้ย = o.PayerPhone,
                            สถานที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeName,
                            ID_ตำบล = o.SubDistrict_ID,
                            ตำบล = o.SubDistrictDetail == "n/a" ? null : o.SubDistrictDetail,
                            ID_อำเภอ = o.District_ID,
                            อำเภอ = o.DistrictDetail == "n/a" ? null : o.DistrictDetail,
                            ID_จังหวัด = o.Province_ID,
                            จังหวัด = o.ProvinceDetail == "n/a" ? null : o.ProvinceDetail,
                            สาขาผู้ชำระเบี้ย = o.BranchDetail == "n/a" ? null : o.BranchDetail,
                            โซนผู้ชำระเบี้ย = o.ZoneDetail == "n/a" ? null : o.ZoneDetail,
                            ข้อมูลการเฝ้าระวัง = o.IsBeware == true ? "มี" : "ไม่มี",
                            หมายเหตุข้อมูลการเฝ้าระวัง = o.BewareRemark,
                            รหัส_QC = o.QCCode,
                            ชื่อสกุล_QC = o.QCName,
                            รหัสผู้ดูแลพื้นที่ = o.DirectorCode,
                            ชื่อสกุล_ผู้ดูแลพื้นที่ = o.DirectorName,
                            เขตพื้นที่ = o.StudyArea,
                            รหัสผู้แทน = o.SaleEmployeeCode,
                            ชื่อสกุลผู้แทน = o.SaleEmployeeName,
                            ชื่อเล่นผู้แทน = o.SaleEmployeeNickName,
                            สาขาผู้แทน = o.SaleEmployeeBranch,
                            แผนประกัน = o.ProductDetail == "n/a" ? null : o.ProductDetail,
                            เบี้ยประกัน = o.Premium,
                            ทุนประกันรถยนต์ = o.SumInsured,
                            ประเภทการซ่อม = o.ClaimTypeDetail == "n/a" ? null : o.ClaimTypeDetail,
                            ยี่ห้อ = o.VehicleBrandDetail == "n/a" ? null : o.VehicleBrandDetail,
                            รุ่น = o.VehicleModelDetail == "n/a" ? null : o.VehicleModelDetail,
                            ขนาดเครื่องยนต์_L = o.VehicleEngineSize,
                            ปีที่จดทะเบียน = o.VehicleRegistrationYear,
                            เลขทะเบียน = o.VehicleRegistrationNumber,
                            หมายเลขตัวถัง = o.VehicleChassisNumber,
                            ประเภทยานพาหนะ = o.VehicleTypeDetail == "n/a" ? null : o.VehicleTypeDetail,
                            ประเภทการใช้งาน = o.VehicleUseTypeDetail == "n/a" ? null : o.VehicleUseTypeDetail,
                            //หมายเหตุ = o.VehicleBrandModelDetailRemark,
                            สินเชื่อรถยนต์ = o.VehicleIsFinacial,
                            มีกล้องติดรถยนต์ = o.VehicleIsCamera,
                            ลักษณะการใช้งาน = o.DriveDetail == "n/a" ? null : o.DriveDetail,
                            สถานที่การใช้งาน = o.DriveLocation,
                            //VehicleRemark = o.VehicleRemark,
                            สถานะการตรวจสอบ_ฝ่ายQC = o.QueueAuditStatusDetail == "n/a" ? null : o.QueueAuditStatusDetail,
                            สถานะการโทร = o.CallStatusDetail == "n/a" ? null : o.CallStatusDetail,
                            จำนวนไม่รับสาย = o.CallStatus2,
                            จำนวนไม่สะดวกคุย = o.CallStatus3,
                            จำนวนติดต่อไม่ได้ = o.CallStatus4,
                            จำนวนเบอร์โทรผิด = o.CallStatus7,
                            จำนวนลูกค้าประสงค์ยกเลิก = o.CallStatus6,
                            รวมสถานะการโทร = o.CallStatusTotal,
                            หมายเหตุการโทร = o.AuditRemark,
                            สาเหตุไม่ต้องตรวจสอบ = o.NotAuditedCauseDetail == "n/a" ? null : o.NotAuditedCauseDetail,
                            หมายเหตุไม่ต้องตรวจสอบ = o.NotAuditedCauseRemark,
                            ผู้ให้ข้อมูล = o.IsUnderwriteBy,
                            เป็นบุคคลเดียวกับผู้ชำระเบี้ยหรือไม่ = o.IsSamePersonPayer == null ? null : o.IsSamePersonPayer == true ? "ใช่" : "ไม่ใช่",
                            วิธีการขาย = o.SaleMethodTypeDetail == "n/a" ? null : o.SaleMethodTypeDetail,
                            บริการหลังการขาย = o.SaleServiceTypeDetail == "n/a" ? null : o.SaleServiceTypeDetail,
                            หมายเหตุบริการหลังการขาย = o.SaleServiceTypeRemark,
                            ได้รับกรมธรรม์ = o.IsReceivedInsurance == null ? null : o.IsReceivedInsurance == true ? "ได้รับ" : "ไม่ได้รับ",
                            รายละเอียดการได้รับกรมธรรม์ = o.ReceivingInsuranceTypeDetail == "n/a" ? null : o.ReceivingInsuranceTypeDetail,
                            ผลการมอบกรมธรรม์ = o.InsuranceInsuredIsValid == null ? null : o.InsuranceInsuredIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุผลการมอบกรมธรรม์ = o.InsuranceInsuredRemark,
                            ข้อมูลผอ_บล = o.UnderwriteByUserName,
                            คะแนนความพึงพอใจ = o.QueueAuditStatusId != 3 ? null : o.FeedbackRate,
                            ข้อเสนอแนะ = o.FeedbackRemark,
                            ผลด้านบริการ = o.ServiceResultIsIssue == null ? null : o.ServiceResultIsIssue == true ? "มีประเด็น" : "ไม่มีประเด็น",
                            หมายเหตุผลด้านบริการ = o.ServiceResultRemark,
                            ชื่อสกุลผู้เอาประกัน = o.InsuredNameIsValid == null ? null : o.InsuredNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุชื่อสกุลผู้เอาประกัน = o.InsuredNameRemark,
                            ชื่อสกุลผู้ชำระเบี้ย = o.PayerNameIsValid == null ? null : o.PayerNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุชื่อสกุลผู้ชำระเบี้ย = o.PayerNameRemark,
                            วันเดือนปีเกิด = o.BirthDateIsValid == null ? null : o.BirthDateIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุวันเดือนปีเกิด = o.BirthDateRemark,
                            ทะเบียนรถ = o.VehicleRegistrationNumberIsValid == null ? null : o.VehicleRegistrationNumberIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุทะเบียนรถ = o.VehicleRegistrationNumberRemark,
                            ยี่ห้อรถ = o.VehicleBrandModelDetailIsValid == null ? null : o.VehicleBrandModelDetailIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุยี่ห้อรถ = o.VehicleBrandModelDetailRemark,
                            ประเภทรถ = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 ? null : o.VehicleUseTypeDetail == "n/a" ? null : o.VehicleUseTypeDetailIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุประเภทรถ = o.VehicleUseTypeDetail == "n/a" ? null : o.VehicleUseTypeDetailRemark,
                            ลักษณะการใช้รถ = o.ResultVehicleUseTypeDetail == "n/a" ? null : o.ResultVehicleUseTypeDetail,
                            หมายเหตุลักษณะการใช้รถ = o.VehicleUseTypeRemark,
                            สภาพรถ = o.IsDamaged == null ? null : o.IsDamaged == true ? "มีความเสียหาย" : "ไม่มีความเสียหาย",
                            สภาพความเสียหาย = o.DamageTypeList,
                            หมายเหตุสภาพความเสียหาย = o.DamageTypeRemark,
                            รายการความเสียหาย = o.DamageList,
                            หมายเหตุรายการความเสียหาย = o.DamageRemark,
                            ตำแหน่งความเสียหาย = o.QueueAuditStatusId == null || o.QueueAuditStatusId != 3 || o.PositionDamagedDetail == "n/a" ? null : o.PositionDamagedDetail,
                            หมายเหตุตำแหน่งความเสียหาย = o.PositionDamagedRemark,
                            อุปกรณ์ตกแต่งเพิ่มเติม = o.QueueAuditStatusId != 3 ? null : o.IsAccessories == null ? "อื่นๆ" : o.IsAccessories == true ? "มี" : "ไม่มี",
                            หมายเหตุอุปกรณ์ตกแต่งเพิ่มเติม = o.AccessoriesRemark,
                            อาชีพผู้เอาประกัน = o.PayerOccupationIsValid == null ? null : o.PayerOccupationIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุอาชีพผู้เอาประกัน = o.PayerOccupationRemark,
                            หน่วยงานผู้ชำระเบี้ย = o.PayerBuildingNameIsValid == null ? null : o.PayerBuildingNameIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุหน่วยงานผู้ชำระเบี้ย = o.PayerBuildingNameRemark,
                            แผนประกัน_ = o.ProductIsValid == null ? null : o.ProductIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุแผนประกัน = o.ProductRemark,
                            เบี้ยประกัน_ = o.PremiumIsValid == null ? null : o.PremiumIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุเบี้ยประกัน = o.PremiumRemark,
                            ประเภทการชำระเบี้ยประกัน = o.PeriodTypeIsValid == null ? null : o.PeriodTypeIsValid == true ? "ถูกต้อง" : "ไม่ถูกต้อง",
                            หมายเหตุประเภทการชำระเบี้ยประกัน = o.PeriodTypeRemark,
                            ผลการตรวจสอบการทำประกัน = o.AuditMotorStatusDetail == "n/a" ? null : o.AuditMotorStatusDetail,
                            ระบุในใบสมัคร = o.AuditMotorSpecifyStatusDetail == "n/a" ? null : o.AuditMotorSpecifyStatusDetail,
                            หมายเหตุประวัติคัดกรอง = o.AuditMotorRemark,
                            พฤติกรรมการคัดกรอง_MA = o.UnderwritingBehaviorTypeDetail == "n/a" ? null : o.UnderwritingBehaviorTypeDetail,
                            หมายเหตุพฤติกรรมการคัดกรอง_MA = o.UnderwritingBehaviorRemark,
                            หมายเหตุอื่นๆ = o.AuditOtherMotorRemark,
                            วันที่ทำรายการ = o.AuditQCUpdatedByUserCode == null ? null : o.AuditQCUpdatedDate,
                            เวลา = o.AuditQCUpdatedByUserCode == null ? null : o.AuditQCUpdatedTime,
                            รหัสฝ่ายQC = o.AuditQCUpdatedByUserCode,
                            /*รอบความคุ้มครอง = o.CoverDate,*/
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
                            รอบการตรวจสอบ = o.AuditPeriod,
                            สถานะคิวงาน_DC = o.QueueStatus
                        }).ToList();

                        workSheet.Cells.LoadFromCollection(auditMotor, true);

                        //edit name headerCells
                        //workSheet.Cells["AH1"].Value = "หมายเหตุ";
                        //workSheet.Cells["AX1"].Value = "หมายเหตุ";
                        //workSheet.Cells["AC1"].Value = "ขนาดเครื่องยนต์ (L)";
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

        #endregion API
    }
}