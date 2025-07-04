using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SmileSPACommunity.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        private PACommunityEntities _db;

        public ReportController()
        {
            _db = new PACommunityEntities();
        }

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrintCardMonitor()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "PACommunity_MO" }; //MO
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PACommunity_PAUnderwrite" }; //ฝ่าย PA Underwrite

            //ถ้าเป็น สิทธิ์ของ Dev และ PA ให้แสดงสาขาทั้งหมด
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Role = rolelist;
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            //ถ้าเป็น สิทธิ์ของ MO ให้แสดงแค่สาขาตัวเอง
            else if (rolelist.Intersect(roleMO).Count() > 0)
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Role = rolelist;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            else
            {
                return RedirectToAction("UnAuthorize", "Auth");
            }
        }

        public ActionResult PrintCardDetail(string applicationId = null)
        {
            int? d_applicationid;

            //deCode
            var appIDBase64EncodedBytes = Convert.FromBase64String(applicationId);
            var deCode_appID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);

            d_applicationid = Convert.ToInt32(deCode_appID);

            ViewBag.ApplicationId = d_applicationid;

            return View();
        }

        public ActionResult PolicyReport()
        {
            ViewBag.Organize = _db.usp_Organize_Select(null, null, 2, null, 0, 60, null, null, null).Where(s => s.OrganizeId == 389190).ToList();
            return View();
        }

        public ActionResult CancelReport()
        {
            ViewBag.Organize = _db.usp_Organize_Select(null, null, 2, null, 0, 60, null, null, null).Where(s => s.OrganizeId == 389190).ToList();
            return View();
        }

        public ActionResult CompensationReport()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult GetGroupReport(string CreatedDateReportFrom = null, string CreatedDateReportTo = null,
                                          int? draw = null, int? pageSize = null, int? pageStart = null,
                                          string sortField = null, string orderType = null, string search = null,
                                          string ReportGroupCode = null, string period = null, int? reportTypeId = null)
        {
            int employeeID;
            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            DateTime? datefrom;
            DateTime? dateto;
            DateTime? d_period;

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

            if (period != null)
            {
                d_period = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }
            else
            {
                d_period = null;
            }
            var result = _db.usp_ReportGroup_Select(ReportGroupCode, d_period, datefrom, dateto, reportTypeId, pageStart, pageSize, sortField, orderType, search).ToList();

            //var xxx = result.FirstOrDefault()?.ReportGroupId;
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectApplicationRoundReport(
                                  int? draw = null, int? pageSize = null, int? pageStart = null,
                                  string sortField = null, string orderType = null, string search = null
                                , int? ApplicationRoundId = null, string period = null, int? insuranceId = null)
        {
            DateTime? d_period = GlobalFunction.ConvertDatePickerToDate_BE(period);

            var result = _db.usp_ReportApplicationRound_Select(ApplicationRoundId, d_period, insuranceId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //public void ExportPolicyReport(int id)
        //{
        //    var data_sheet1 = _db.usp_ReportGroup_SelectById(id).ToList();
        //    var data_sheet2 = _db.usp_ReportDetail_Select(id).ToList();
        //    var dt1 = GlobalFunction.ToDataTable(data_sheet1);
        //    var dt2 = GlobalFunction.ToDataTable(data_sheet2);
        //    ExcelManager.ExportToExcel(this.HttpContext, "รายงานยืนยันออกกรมธรรม", dt1, "ยืนยันออกกรมธรรม", dt2, "รายชื่อ");

        //    // return Json(true, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public void ExportPolicyReport(int id, string code)
        {
            //await Task.Yield();
            using (var db = new PACommunityEntities())
            {
                var lst = db.usp_ReportGroup_SelectById(id).OrderByDescending(s => s.CreatedDate)
                    .Select((s, i) => new
                    {
                        ลำดับ = i + 1,
                        จังหวัด = s.Province,
                        รหัสรายการ = s.ReportHeaderCode,
                        วันคุ้มครอง = s.StartCoverDate,
                        วันสิ้นสุด = s.EndCoverDate,
                        App_id = s.ApplicationCode,
                        รหัสหน่วยงาน = s.OrganizeCode,
                        บริษัท = s.Insurance,
                        ชื่อกรมธรรม์ = s.ApplicationName,
                        จำนวนผู้เอาประกัน = s.CustomerBuy,
                        บุคลากรฟรี = s.CustomerFree,
                        เบี้ยต่อราย = s.PremiumPerUnit,
                        เบี้ยรวม = s.PremiumAmount,
                        ที่อยู่ = s.AddressDetail
                    }).ToList();
                var lst2 = db.usp_ReportDetail_Select(id)
                   .Select((s, i) => new
                   {
                       ลำดับ = i + 1,
                       จังหวัด = s.Province,
                       รหัสรายการ = s.ReportHeaderCode,
                       App_id = s.ApplicationCode,
                       รหัสหน่วยงาน = s.OrganizeCode,
                       บริษัท = s.Insurance,
                       ชื่อกรมธรรม์ = s.ApplicationName,
                       รหัสผู้เอาประกัน = s.CustomerDetailCode,
                       คำนำหน้า = s.Title,
                       ชื่อ = s.FirstName,
                       สกุล = s.LastName,
                       รหัสประจำตัวประชาชน = s.IdCardNumber,
                       วันเดือนปีเกิด = s.BirthDate,
                       อายุ = s.AgeAtRegisterYear,
                       อาชีพ = s.Occupation,
                       สิทธิ์ = s.CustomerDetailType,
                       วันคุ้มครอง = s.StartCoverDate,
                       วันสิ้นสุด = s.EndCoverDate
                   }).ToList();

                var dt1 = GlobalFunction.ToDataTable(lst);
                var dt2 = GlobalFunction.ToDataTable(lst2);
                ExcelManager.ExportToExcel(this.HttpContext, code, dt1, "ยืนยันออกกรมธรรม์", dt2, "รายชื่อ");
            }
        }

        public ActionResult DownloadExportPolicyGroupReport()
        {
            if (Session["DownloadExcel_FilePolicyReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FilePolicyReport"] as byte[];
                string excelName = $"{Session["ReportGroupCode"]}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public JsonResult GenPolicyReport(int? InsuranceId = null, string period = null)
        {
            var lstArr = new string[3];

            DateTime? d_period = GlobalFunction.ConvertDatePickerToDate_BE(period);

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_GenerateReport_Insert(InsuranceId, d_period, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectCancelApplicationRoundReport(
                                  int? draw = null, int? pageSize = null, int? pageStart = null,
                                  string sortField = null, string orderType = null, string search = null,
                                  int? insuranceId = null)
        {
            var result = _db.usp_ReportCancelApplicationRound_Select(null, insuranceId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public void ExporCompensationReport(string period)
        {
            DateTime? reriod = GlobalFunction.ConvertDatePickerToDate_BE(period);
            var data_sheet1 = _db.usp_ReportCompensationApplicationRound_Select(reriod).Select(s => new
            {
                สาขา = s.Branch,
                เขตพื้นที่การศึกษา = s.StudyArea,
                จังหวัด = s.Province,
                อำเภอ = s.District,
                ตำบล = s.SubDistrict,
                App_id = s.ApplicationCode,
                รหัสหน่วยงาน = s.OrganizeCode,
                วันคุ้มครอง = s.ApplicationStartCoverDate,
                ชื่อกรมธรรม์ = s.ApplicationName,
                ชำระเบี้ย = s.CountCustomerBuy,
                ฟรี = s.CountCustomerFree,
                แผน = s.Product,
                เบี้ย = s.Premium,
                เบี้ยหน้าตาราง = s.SumPremiumAmount,
                ประเภทการขาย = s.ContactSaleType,
                รหัสพนักงาน_ผู้ประสานงานโครงการ = s.AgentEmployeeCode,
                ชื่อ_ผู้ประสานงานโครงการ = s.AgentFirstName,
                สกุล_ผู้ประสานงานโครงการ = s.AgentLastName,
                ทีม_ผู้ประสานงานโครงการ = s.AgentTeam,
                ชื่อ_สกุล_ผู้ประสานงานชุมชน = s.ContactFullName,
                หมายเลขบัตรประจำตัวประชาชน_ผู้ประสานงานชุมชน = s.ContactIdCardNumber,
                เบอร์โทร_ผู้ประสานงานชุมชน = s.ContactMobileNumber,
                ธนาคาร_ผู้ประสานงานชุมชน = s.Bank,
                ชื่อบัญชี_ผู้ประสานงานชุมชน = s.AccountName,
                เลขบัญชี_ผู้ประสานงานชุมชน = s.AccountNo
            }).ToList();
            var dt1 = GlobalFunction.ToDataTable(data_sheet1);
            ExcelManager.ExportToExcel(this.HttpContext, "รายงานคิดค่าตอบแทน", dt1, "รายงานคิดค่าตอบแทน");

            // return Json(true, JsonRequestBehavior.AllowGet);
        }

        public void ExporCancelReport(int groupid, string reportname)
        {
            var data_sheet1 = _db.usp_ReportGroup_SelectById(groupid).Select((s, Index) => new
            {
                ลำดับ = Index + 1,
                จังหวัด = s.Province,
                รหัสรายการ = s.ReportHeaderCode,
                วันคุ้มครอง = s.StartCoverDate,
                App_id = s.ApplicationCode,
                รหัสหน่วยงาน = s.OrganizeCode,
                บริษัท = s.Insurance,
                ชื่อกรมธรรม์ = s.ApplicationName,
                จำนวนผู้เอาประกัน = s.CustomerBuy,
                บุคลากรฟรี = s.CustomerFree,
                รวมผู้เอาประกัน = s.CustomerBuy + s.CustomerFree,
                เบี้ยต่อราย = s.PremiumPerUnit,
                เบี้ยรวม = s.PremiumAmount,
                ที่อยู่ = s.AddressDetail,
                หมายเหตุการยกเลิก = s.ApplicationCancelCauseRemark
            }).ToList();
            var dt1 = GlobalFunction.ToDataTable(data_sheet1);
            ExcelManager.ExportToExcel(this.HttpContext, reportname, dt1, "รายงานยกเลิกกรมธรรม์");

            //return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenCancelReport(int? InsuranceId = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_GenerateReportCancel_Insert(InsuranceId, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        #region "Method Ticket"

        public JsonResult GetApplicationTicketHerder(int? draw = null, int? pageSize = null, int? pageStart = null,
                                  string sortField = null, string orderType = null, string search = null, int? branchId = null)
        {
            var result = _db.usp_TicketHeader_Select(null, null, null, branchId == -1 ? null : branchId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationTicketHerderById(int applicationId)
        {
            var rs = _db.usp_TicketHeader_Select(applicationId, null, null, null, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDetailById(int? draw = null, int? pageSize = null, int? pageStart = null,
                                 string sortField = null, string orderType = null, string search = null, int? applicationId = null)
        {
            var result = _db.usp_CustomerDetail_Select(null, applicationId, null, null, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDetailAll(int? applicationId)
        {
            var rs = _db.usp_CustomerDetail_Select(null, applicationId, null, null, null, 0, 99999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetCustomerDetail(string[] customerDetaillist, int applicationId)
        {
            try
            {
                string temCode = "";

                //Gen Code TicketHeader
                var rs_TicketHeader = _db.usp_TmpTicketHeader_Insert(applicationId).Single();

                if (rs_TicketHeader.IsResult == true)
                {
                    temCode = rs_TicketHeader.Result;
                    //Insert TicketDetail By CustomerDetailId
                    foreach (var item in customerDetaillist)
                    {
                        var rs = _db.usp_TmpTicketDetail_Insert(temCode, Convert.ToInt32(item)).Single();

                        if (rs.IsResult == false)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                //Return CodeTicketHeader
                return Json(temCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion "Method Ticket"
    }
}