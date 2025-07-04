using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmileSPACommunity.Controllers
{
    public class PAPersonnelReportController : Controller
    {
        private PACommunityEntities _db;

        // GET: PAPersonnelReport
        public PAPersonnelReportController()
        {
            _db = new PACommunityEntities();
        }

        public ActionResult ApplicationReport() 
        {
            ViewBag.Status = _db.usp_PersonnelApplicationStatus_Select(null, 0, 99, null, null, null).ToList();
            ViewBag.Branch = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
            ViewBag.Years = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList().OrderByDescending(x => x.Code);
            return View();
        }

        public ActionResult RequestPolicyReport()
        {
            ViewBag.Organize = _db.usp_Organize_Select(null, null, 2, null, 0, 999999, null, null, null).Where(s =>  s.OrganizeId == 389190).ToList();
            return View();
        }

        public ActionResult ResultSendSMSReport()
        {
            ViewBag.Branch = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();

            return View();
        }

        public ActionResult ResultSendSMSForExport()
        {

            return View();
        }


        #region = "Report"
        public async Task<ActionResult> ExportApplicationReport(int? year = null, string personnelAppStatus = null, int? branchId = null)
        {
            await Task.Yield();
            if (branchId == -1)
            {
                branchId = null;
            }
            var result = _db.usp_ReportPersonnelApplication_Select(year, personnelAppStatus, branchId, 0, 999999, null, null, null)
             .Select((x) => new
             {
                 สาขา = x.BranchDetail,
                 จังหวัด = x.ProvinceDetail,
                 เขตพื้นที่การศึกษา = x.StudyArea,
                 รหัสสถานศึกษา = x.School_id,
                 AppID = x.PersonnelApplicationCode,
                 สถานศึกษา = x.PersonnelApplicationName,
                 ประเภทสัญญา = x.Detail,
                 จำนวนบุคลากร = x.CountCustomer,
                 วันคุ้มครอง = x.EffectiveDate != null ? x.EffectiveDate.Value.Date.ToString("dd/MM/yyyy") : null,
                 วันที่สิ้นสุด = x.EndCoverDate != null ? x.EndCoverDate.Value.Date.ToString("dd/MM/yyyy") : null,
                 แผน_PA_ยิ้มแฉ่ง = x.ProductDetail,
                 เสียชีวิตอุบัติเหตุ = x.CoverageAccident,
                 ฆาตกรรม_MC = x.CoverageMC,
                 เบี้ยต่อราย = x.PremiumPerPolicy,
                 เบี้ยรวม = x.PremiumTotal,
                 สถานะ = x.PersonnelApplicationStatusName,
             }).ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                workSheet1.Cells.LoadFromCollection(result, true);

                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                headerCells1.Style.Font.Bold = true;
                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                allCells1.AutoFilter = true;
                allCells1.AutoFitColumns();


                package.Save();

                stream.Position = 0;
                Session["DownloadExcelApplicationReport"] = package.GetAsByteArray();

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportApplicationReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["DownloadExcelApplicationReport"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["DownloadExcelApplicationReport"] as byte[];
                string excelName = $"รายงานApplication_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult SelectCancelApplicationRoundReport(
                                  int? draw = null, int? pageSize = null, int? pageStart = null,
                                  string sortField = null, string orderType = null, string search = null,
                                  int? insuranceId = null)
        {
            var result = _db.usp_PersonnelApplicationRoundForGenReport_Select(insuranceId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public ActionResult GetGroupReport(string CreatedDateReportFrom = null, string CreatedDateReportTo = null,
                                          int? draw = null, int? pageSize = null, int? pageStart = null,
                                          string sortField = null, string orderType = null, string search = null)
        {
            int employeeID;
            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            DateTime? datefrom;
            DateTime? dateto;

            if (CreatedDateReportFrom != null)
            {
                datefrom = GlobalFunction.ConvertDatePickerToDate_BE(CreatedDateReportFrom);
                dateto = GlobalFunction.ConvertDatePickerToDate_BE(CreatedDateReportTo);
            }
            else
            {
                datefrom = null;
                dateto = null;
            }
            
            var result = _db.usp_PersonnelReportGroup_Select( datefrom, dateto, pageStart, pageSize, sortField, orderType, search).ToList();
            
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenCancelReport(int? InsuranceId = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_PersonnelReportHeader_Insert(InsuranceId, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public void ExporCancelReport(int groupid, string reportname)
        {
            var data_sheet1 = _db.usp_PersonnelReportHeader_Select(groupid,false,0,99999999,null,null,"").Select((s, Index) => new
            {
                จังหวัด = s.Province,
                รหัสรายการ = s.PersonnelReportHeaderCode,
                ลำดับที่ = Index + 1,
                รหัสโรงเรียน = s.SchoolId,
                AppID = s.PersonnelApplicationCode,
                ครั้งที่Endorse = s.EndorseNo,
                บริษัท = s.Insurance,
                ระดับ = s.LevelSchool,
                ประเภทสัญญา = s.ContactType,
                ชื่อโรงเรียน = s.SchoolName,
                จำนวนผู้เอาประกัน = s.CustomerCount,
                วันคุ้มครอง = s.StartCoverDate,
                วันที่สิ้นสุด = s.EndCoverDate,
                วันที่มีผล = s.EffectiveDate,
                เสียชีวิตอุบัติเหตุ =s.AccidentDeathSum,
                ฆาตกรรม_MC = s.MurderSum,
                แผนประกัน = s.Product,
                ต่อราย = s.Premium,
                เบี้ยรวม = s.PremiumSum,
                ที่อยู่ = s.AddressDetail,
                หมายเหตุการแก้ไข = s.Remark
            }).ToList();
            var dt1 = GlobalFunction.ToDataTable(data_sheet1);
            //Sheet2
            var data_sheet2 = _db.usp_PersonnelReportHeader_Select(groupid, true, 0, 99999999, null, null, "").Select((s, Index) => new
            {
                จังหวัด = s.Province,
                รหัสรายการ = s.PersonnelReportHeaderCode,
                ลำดับที่ = Index + 1,
                รหัสโรงเรียน = s.SchoolId,
                AppID = s.PersonnelApplicationCode,
                ครั้งที่Endorse = s.EndorseNo,
                บริษัท = s.Insurance,
                ระดับ = s.LevelSchool,
                ประเภทสัญญา = s.ContactType,
                ชื่อโรงเรียน = s.SchoolName,
                จำนวนผู้เอาประกัน = s.CustomerCount,
                วันคุ้มครอง = s.StartCoverDate,
                วันที่สิ้นสุด = s.EndCoverDate,
                วันที่มีผล = s.EffectiveDate,
                เสียชีวิตอุบัติเหตุ = s.AccidentDeathSum,
                ฆาตกรรม_MC = s.MurderSum,
                แผนประกัน = s.Product,
                ต่อราย = s.Premium,
                เบี้ยรวม = s.PremiumSum,
                ที่อยู่ = s.AddressDetail,
                หมายเหตุการแก้ไข = s.Remark
            }).ToList();
            var dt2 = GlobalFunction.ToDataTable(data_sheet2);
            //Sheet 3
            var data_sheet3 = _db.usp_PersonnelReportDetail_Select(groupid, 0, 99999999, null, null, "").Select((s, Index) => new
            {
                ลำดับ = Index + 1,
                รหัสรายการ = s.PersonnelReportHeaderCode,
                AppID = s.PersonnelApplicationCode,
                รหัสสถานศึกษา = s.SchoolId,
                ชื่อสถานศึกษา = s.SchoolName,
                จังหวัด = s.Province,
                รหัสผู้เอาประกัน = s.PersonnelCustomerDetailCode,
                ประเภทลูกค้า = s.CustomerType,
                คำนำหน้าชื่อ = s.Title,
                ชื่อ = s.FirstName,
                นามสกุล = s.LastName,
                เลขที่ประจำตัวประชาชน = s.CardId,
                เลขที่Passport = s.Passport,
                วันที่เริ่มต้นคุ้มครอง = s.StartCoverDate,
                วันที่มีผลคุ้มครอง = s.EffectiveDate, 
                วันที่สิ้นสุดคุ้มครอง = s.EndCoverDate,
                ครั้งที่ = s.No
            }).ToList();
            var dt3 = GlobalFunction.ToDataTable(data_sheet3);
            ExcelManager.ExportToExcel(this.HttpContext, reportname, dt1,"ยืนยันกรมธรรม์",dt2,"สลักหลัง-เพิ่ม",dt3, "รายชื่อ");

            //return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PersonnelGroupUpdate(int? GroupId = null, int? GroupStatusId = null)
        {
            var lstArr = new string[3];          
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_PersonnelReportGroup_Update(GroupId, GroupStatusId, userID).Single();

            if (rs.IsResult == true)
            {
                // Send SMS
                var result = _db.usp_PersonnelCustomerDetailToSendSMS_Insert(GroupId, userID).Single();
                lstArr[0] = rs.IsResult.ToString();
                lstArr[1] = rs.Result.ToString();
                lstArr[2] = rs.Msg.ToString();
            }     
            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportResultSendSMSReport(string DateFrom = null, string DateTo = null,
                                      int? SmsStatus = null, int? branchId = null)
        {
            DateTime? datefrom;
            DateTime? dateto;

            await Task.Yield();
            if (branchId == -1)
            {
                branchId = null;
            }


            if (DateFrom != null)
            {
                datefrom = GlobalFunction.ConvertDatePickerToDate_BE(DateFrom);
                dateto = GlobalFunction.ConvertDatePickerToDate_BE(DateTo);
            }
            else
            {
                datefrom = null;
                dateto = null;
            }

                var result = _db.usp_PersonnelResultSendSMS_Select(datefrom, dateto, SmsStatus, branchId, 0, 999999, null, null, null)
                .Select((x) => new
                {
                    ปีการศึกษา = x.PAYear,
                    วันที่ส่งSMS = x.SendDate != null ? x.SendDate.Value.Date.ToString("dd/MM/yyyy") : null,
                    AppID = x.PersonnelApplicationCode,
                    สถานศึกษา = x.PersonnelApplicationName,
                    รหัสผู้เอาประกัน = x.PersonnelCustomerDetailCode,
                    ชื่อ_นามสกุล = x.CustomerName,
                    เบอร์โทร = x.PhoneNo,
                    ข้อความ = x.Message,
                    สถานะการส่งSMS = x.SMSStatusName,
                }).ToList();

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("SendSMS");
                    workSheet1.Cells.LoadFromCollection(result, true);

                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                    headerCells1.Style.Font.Bold = true;
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    allCells1.AutoFitColumns();


                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcelSendSMSReport"] = package.GetAsByteArray();

                    return Json("", JsonRequestBehavior.AllowGet);
                }
        }
        
        public ActionResult ExportResultSendSMSReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["DownloadExcelSendSMSReport"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["DownloadExcelSendSMSReport"] as byte[];
                string excelName = $"รายงานSendSMS_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult GetResultSendSMSMonitor(string DateFrom = null, string DateTo = null,int? draw = null,
                                                    int? pageSize = null, int? pageStart = null,string sortField = null
                                                    ,string orderType = null, string search = null,
                                                    int? SmsStatus = null ,int? branchId = null )
        {
           
            DateTime? datefrom;
            DateTime? dateto;

            if (DateFrom != null)
            {
                datefrom = GlobalFunction.ConvertDatePickerToDate_BE(DateFrom);
                dateto = GlobalFunction.ConvertDatePickerToDate_BE(DateTo);
            }
            else
            {
                datefrom = null;
                dateto = null;
            }

            var result = _db.usp_PersonnelResultSendSMS_Select(datefrom, dateto, SmsStatus, branchId,  pageStart, pageSize, sortField,orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PersonnelSentToSMI(int? GroupId = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_PersonnelReportGroupToSMI_Update(GroupId, userID).Single();

            if (rs.IsResult == true)
            {
                var result = _db.usp_PersonnelCustomerDetailToSendSMS_Insert(GroupId, userID).Single();
                lstArr[0] = result.IsResult.ToString();
                lstArr[1] = result.Result.ToString();
                lstArr[2] = result.Msg.ToString();
            }
            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}