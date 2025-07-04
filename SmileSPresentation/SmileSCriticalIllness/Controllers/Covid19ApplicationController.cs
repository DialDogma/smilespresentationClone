using OfficeOpenXml;
using OfficeOpenXml.Style;
using Rotativa;
using Rotativa.Options;
using SmileSCriticalIllness.AddressService;
using SmileSCriticalIllness.Helper;
using SmileSCriticalIllness.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Size = Rotativa.Options.Size;

namespace SmileSCriticalIllness.Controllers
{
    public class Covid19ApplicationController : Controller
    {
        #region Context

        private readonly CriticalIllnessEntities _context;

        public Covid19ApplicationController()
        {
            _context = new CriticalIllnessEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        // GET: Covit19Application
        [Authorization]
        public ActionResult Index()
        {
            return View();
        }

        [Authorization]
        public ActionResult Add()
        {
            using (var client = new AddressServiceClient())
            {
                var provinceList = client.GetProvince(null).ToList();
                ViewBag.provinceList = provinceList;
            }

            ViewBag.sex = _context.usp_Sex_Select(null, null, null, null, null, null).ToList();

            ViewBag.userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            //call plan master
            var planType = _context.usp_ProductCovid_Select(null, null, null, null, null, null, null).ToList();
            ViewBag.planType = planType;

            //select Occupation
            var occupation = _context.usp_Occupation_Select(null, null, null, null, null, null).ToList();
            ViewBag.occupation = occupation;

            var startCoverYear = DateTime.Now.Year;
            var startCoverMonth = DateTime.Now.Month;
            var startCoverDay = DateTime.Now.Day + 1;
            var startCoverYearTH = startCoverYear + 543;
            ViewBag.startCoverDate = startCoverDay + "/" + startCoverMonth + "/" + startCoverYearTH;

            var endCoverYear = DateTime.Now.Year + 1;
            var endCoverMonth = DateTime.Now.Month;
            var endCoverDay = DateTime.Now.Day;
            var endCoverYearTH = endCoverYear + 543;
            ViewBag.endCoverDate = endCoverDay + "/" + endCoverMonth + "/" + endCoverYearTH;

            //var time = DateTime.Now.ToString("HH:mm:ss");
            //ViewBag.time = time;
            //var startCoverDate = appDetail.StartCoverDate?.ToString("dd/MM/yyyy");
            //ViewBag.startCoverDate = startCoverDate;

            //var bDate = appDetail.BirthDate?.ToString("dd/MM/yyyy");
            //ViewBag.bDate = bDate;

            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult EditTable()
        {
            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult Edit(string appId)
        {
            var appIdCheck = appId;
            if (appId == null)
            {
                ViewBag.appId = 0;
                ViewBag.AppCode = "";
                return View();
            }
            var memberIdBase64EncodedBytes = Convert.FromBase64String(Convert.ToString(appId));
            var appID = System.Text.Encoding.UTF8.GetString(memberIdBase64EncodedBytes);

            var appicationId = Convert.ToInt32(appID);

            using (var client = new AddressServiceClient())
            {
                var provinceList = client.GetProvince(null).ToList();
                ViewBag.provinceList = provinceList;
            }

            ViewBag.sex = _context.usp_Sex_Select(null, null, null, null, null, null).ToList();

            ViewBag.userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            //call plan master
            var planType = _context.usp_ProductCovid_Select(null, null, null, null, null, null, null).ToList();
            ViewBag.planType = planType;

            //select Occupation
            var occupation = _context.usp_Occupation_Select(null, null, null, null, null, null).ToList();
            ViewBag.occupation = occupation;

            //if appId not null is edit
            if (appIdCheck != null)
            {
                ViewBag.appId = appicationId;
                var appDetail = _context.usp_CovidApplicationByAppId_Select(appicationId).FirstOrDefault();
                if (appDetail != null)
                {
                    ViewBag.appDetail = appDetail;

                    var startCoverYear = appDetail.StartCoverDate.Value.Year;
                    var startCoverMonth = appDetail.StartCoverDate.Value.Month;
                    var startCoverDay = appDetail.StartCoverDate.Value.Day;
                    var startCoverYearTH = startCoverYear + 543;

                    ViewBag.startCoverDate = startCoverDay + "/" + startCoverMonth + "/" + startCoverYearTH;
                    //var startCoverDate = appDetail.StartCoverDate?.ToString("dd/MM/yyyy");

                    var endCoverYear = appDetail.EndCoverDate.Value.Year;
                    var endCoverMonth = appDetail.EndCoverDate.Value.Month;
                    var endCoverDay = appDetail.EndCoverDate.Value.Day;
                    var endCoverYearTH = endCoverYear + 543;
                    ViewBag.endCoverDate = endCoverDay + "/" + endCoverMonth + "/" + endCoverYearTH;

                    //var endCoverDate = appDetail.EndCoverDate?.ToString("dd/MM/yyyy");
                    //ViewBag.endCoverDate = endCoverDate;

                    var bDate = appDetail.BirthDate?.ToString("dd/MM/yyyy");
                    ViewBag.bDate = bDate;

                    ViewBag.AppCode = appDetail.CovidApplicationCode;
                    ViewBag.IdentityCard = appDetail.IdentityCardNo;
                    ViewBag.OccupationId = appDetail.OccupationId;
                    ViewBag.ProductId = appDetail.ProductId;
                    ViewBag.SexId = appDetail.SexId;
                    ViewBag.FirstName = appDetail.FirstName;
                    ViewBag.LastName = appDetail.LastName;
                    ViewBag.MobilePhoneNumber = appDetail.MobilePhoneNumber;
                    ViewBag.Email = appDetail.Email;
                    ViewBag.LineId = appDetail.LineId;
                    ViewBag.Address = appDetail.Address;
                    ViewBag.SubDistrictId = appDetail.SubDistrictId;
                    ViewBag.sDate = appDetail.StartCoverDate;

                    //check value is null
                    if (appDetail.IsSendCover != null)
                    {
                        ViewBag.IsSendCover = appDetail.IsSendCover.Value.ToString();
                    }
                    else
                    {
                        ViewBag.IsSendCover = "False";
                    }

                    using (var client = new AddressServiceClient())
                    {
                        var district = client.GetAddressDataBySubDistrictSelectResult(Convert.ToInt32(appDetail.SubDistrictId));
                        ViewBag.districtId = district.District_ID;
                        ViewBag.provinceId = client.GetDistrictSelect(Convert.ToInt32(district.District_ID)).Province_ID;
                    }
                }
            }

            return View();
        }

        [Authorization]
        public ActionResult See(string appId)
        {
            var appIdCheck = appId;
            var memberIdBase64EncodedBytes = Convert.FromBase64String(Convert.ToString(appId));
            var appID = System.Text.Encoding.UTF8.GetString(memberIdBase64EncodedBytes);

            var appicationId = Convert.ToInt32(appID);

            if (appIdCheck != null)
            {
                ViewBag.appId = appicationId;
                var appDetail = _context.usp_CovidApplicationByAppId_Select(appicationId).FirstOrDefault();
                if (appDetail != null)
                {
                    ViewBag.appDetail = appDetail;

                    var startCoverYear = appDetail.StartCoverDate.Value.Year;
                    var startCoverMonth = appDetail.StartCoverDate.Value.Month.ToString("d2");
                    var startCoverDay = appDetail.StartCoverDate.Value.Day.ToString("d2");
                    var startCoverYearTH = startCoverYear + 543;

                    ViewBag.startCoverDate = startCoverDay + "/" + startCoverMonth + "/" + startCoverYearTH;

                    var endCoverYear = appDetail.EndCoverDate.Value.Year;
                    var endCoverMonth = appDetail.EndCoverDate.Value.Month.ToString("d2");
                    var endCoverDay = appDetail.EndCoverDate.Value.Day.ToString("d2");
                    var endCoverYearTH = endCoverYear + 543;
                    ViewBag.endCoverDate = endCoverDay + "/" + endCoverMonth + "/" + endCoverYearTH;

                    //var startCoverDate = appDetail.StartCoverDate?.ToString("dd/MM/yyyy HH:mm:ss");
                    //ViewBag.startCoverDate = startCoverDate;

                    var bDate = appDetail.BirthDate?.ToString("dd/MM/yyyy");
                    ViewBag.bDate = bDate;

                    ViewBag.AppCode = appDetail.CovidApplicationCode;
                    ViewBag.AppStatus = appDetail.CovidApplicationStatus;
                    ViewBag.IdentityCard = appDetail.IdentityCardNo;
                    ViewBag.Occupation = appDetail.OccupationDetail;
                    ViewBag.ProductId = appDetail.ProductId;
                    ViewBag.Plan = appDetail.ProductDetail;
                    ViewBag.PremiumPerYear = appDetail.PremiumPerYear;
                    ViewBag.MaxCover = appDetail.MaxCover;
                    ViewBag.Sex = appDetail.SexDetail;
                    ViewBag.FirstName = appDetail.FirstName;
                    ViewBag.LastName = appDetail.LastName;
                    ViewBag.MobilePhoneNumber = appDetail.MobilePhoneNumber;
                    ViewBag.Email = appDetail.Email;
                    ViewBag.LineId = appDetail.LineId;
                    ViewBag.Address = appDetail.FullAddress;
                    ViewBag.SaleEmployeeCode = appDetail.SaleEmployeeCode;
                    ViewBag.SaleEmployeeName = appDetail.SaleEmployeeName;
                    ViewBag.SaleBranchName = appDetail.SaleBranchName;
                    ViewBag.PolicyNo = appDetail.PolicyNo;




                    //ViewBag.SubDistrictId = appDetail.SubDistrictId;
                }
            }

            return View();
        }

        [Authorization]
        public ActionResult ReportForUnderwrite()
        {
            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult ReportBroker()
        {
            return View();
        }

        public ActionResult ExportReport(string startDate)
        {
            var a = Convert.ToDateTime(startDate);
            var b = a.ToString("dd/MM/yyyy");
            var sDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

            var user = GlobalFunction.GetLoginDetail(HttpContext);

            var result = _context.usp_CovidApplicationSendCoverReport_Select(sDate, user.User_ID).ToList();

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
                    var covid = _context.usp_CovidApplicationSendCoverReport_Select(sDate, user.User_ID).Select(o => new
                    {
                        เลขApplication = o.CovidApplicationCode,
                        วันเริ่มคุ้มครอง = o.StartCoverDate,
                        วันสิ้นสุดคุ้มครอง = o.EndCoverDate,
                        คำนำหน้า = o.Title,
                        ชื่อ = o.FirstName,
                        นามสกุล = o.LastName,
                        เพศ = o.SexDetail,
                        ที่อยู่ผู้เอาประกันภัย = o.Address,
                        InsuredAddress2 = o.Address2,
                        ตำบลแขวง = o.SubDistrictDetail,
                        อำเภอเขต = o.DistrictDetail,
                        จังหวัด = o.ProvinceDetail,
                        รหัสไปรษณีย์ = o.ZipCode,
                        อีเมลของผู้เอาประกัน = o.Email,
                        เบอร์โทรศัพท์มือถือ = o.MobilePhoneNumber,
                        หมายเลขบัตรประจำตัวประชาชน = o.IdentityCardNo,
                        วันเดือนปีเกิดผู้เอาประกัน = o.BirthDate,
                        อายุผู้เอาประกัน = o.AgeAtRegister_Year,
                        เลขที่ตัวแทน = o.SaleId,
                        แผน = o.ProductDetail,
                        อาชีพ = o.OccupationDetail,
                        //ผู้รับผลประโยชน์ = o.Heir,
                        ผู้รับผลประโยชน์ = "ทายาทโดยธรรม",
                        //ความสัมพันธ์กับผู้เอาประกัน = o.InsuredRelation,
                        ความสัมพันธ์กับผู้เอาประกัน = "ทายาทโดยธรรม",
                        ช่องทางการจัดส่ง = "",
                        ข้อมูล1 = o.Data1,
                        ข้อมูล2 = o.Data2,
                        ข้อมูล3 = o.Data3,
                        //ช่องทางการขาย = o.SaleChannel,
                        ช่องทางการขาย = "Broker"
                    }).ToList();

                    workSheet.Cells.LoadFromCollection(covid, true);

                    //edit name headerCells
                    workSheet.Cells["A1"].Value = "เลข Application";
                    workSheet.Cells["J1"].Value = "ตำบล/แขวง";
                    workSheet.Cells["K1"].Value = "อำเภอ/เขต";
                    workSheet.Cells["R1"].Value = "อายุผู้เอาประกัน (ปี)";
                }
                var rowCount = workSheet.Dimension.Rows;
                var colCount = workSheet.Dimension.Columns;

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

        public ActionResult ExportReportCompensation(string startDate)
        {
            var a = Convert.ToDateTime(startDate);
            var b = a.ToString("dd/MM/yyyy");
            var sDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

            var user = GlobalFunction.GetLoginDetail(HttpContext);

            var result = _context.usp_CovidApplicationCompensateReport_Select(sDate).ToList();

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
                    var covid = _context.usp_CovidApplicationCompensateReport_Select(sDate).Select(o => new
                    {
                        ลำดับที่ = "",
                        วันที่บันทึกApplication = o.CreatedDate,
                        วันที่คุ้มครอง = o.StartCoverDate,
                        เลขApplication = o.CovidApplicationCode,
                        ข้อมูลผู้เอาประกัน = o.CustomerName,
                        ตำบล = o.SubDistrictDetail,
                        อำเภอ = o.DistrictDetail,
                        จังหวัด = o.ProvinceDetail,
                        พื้นที่ของผู้เอา = o.StudyArea,
                        ผลิตภัณฑ์ = o.ProductGroup,
                        แผน = o.ProductDetail,
                        เบี้ยสุทธิ = o.PremiumPerYear,
                        ค่าคอม = o.Commission,
                        Code = o.SaleEmployeeCode,
                        ชื่อสกุล = o.SaleEmployeeName,
                        ทีม = o.SaleTeam,
                        สาขา = o.SaleBranchName
                    }).ToList();

                    workSheet.Cells.LoadFromCollection(covid, true);

                    //edit name headerCells
                    workSheet.Cells["O1"].Value = "ชื่อ-สกุล";
                    workSheet.Cells["B1"].Value = "วันที่บันทึก Application";
                    workSheet.Cells["D1"].Value = "เลข Application";
                    workSheet.Cells["I1"].Value = "พื้นที่ ของผู้เอา";
                }
                var rowCount = workSheet.Dimension.Rows;
                var colCount = workSheet.Dimension.Columns;

                // ลำดับที่
                for (int i = 2; i - 2 < result.Count; i++)
                {
                    workSheet.Cells[i, 1].Value = i - 1;
                }

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

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult ReportCompensation()
        {
            return View();
        }

        [Authorization(Roles = "CriticalIllness_Underwrite,Developer")]
        public ActionResult ReportBeneficiary()
        {
            return View();
        }

        public ActionResult Doc(string id)
        {
            if (id == null)
            {
                ViewBag.appId = 0;
                return View();
            }
            var memberIdBase64EncodedBytes = Convert.FromBase64String(id);
            var appID = Encoding.UTF8.GetString(memberIdBase64EncodedBytes);
            ViewBag.id = id;
            var appDetail = _context.usp_CovidApplicationByAppId_Select(Convert.ToInt32(appID)).FirstOrDefault();
            ViewBag.appId = appID;
            if (appDetail != null)
            {
                ViewBag.appDetail = appDetail;

                var startCoverDate = appDetail.StartCoverDate?.ToString("dd/MM/yyyy");
                ViewData["startCoverDate"] = startCoverDate;

                var endCoverDate = appDetail.EndCoverDate?.ToString("dd/MM/yyyy");
                ViewData["endCoverDate"] = endCoverDate;

                var xxx = Convert.ToDateTime(appDetail.BirthDate).AddYears(543); ;
                ViewData["HBD"] = xxx.ToString("dd/MM/yyyy");
                var year = appDetail.AgeAtRegister_Year; //DateTime.Now.Year - bDate.Year;
                ViewBag.bDate = year;

                ViewBag.AppCode = appDetail.CovidApplicationCode;
                ViewBag.AppStatus = appDetail.CovidApplicationStatus;
                ViewBag.IdentityCard = appDetail.IdentityCardNo;
                ViewBag.ProductId = appDetail.ProductId;
                ViewBag.Plan = appDetail.ProductDetail;
                ViewData["Sex"] = appDetail.SexDetail;
                ViewData["FirstName"] = appDetail.FirstName;
                ViewData["LastName"] = appDetail.LastName;
                ViewBag.MobilePhoneNumber = appDetail.MobilePhoneNumber;
                ViewBag.Email = appDetail.Email;
                ViewBag.LineId = appDetail.LineId;
                ViewData["Address"] = appDetail.FullAddress;
                ViewData["premium"] = appDetail.PremiumPerYear;
                ViewData["MaxCover"] = appDetail.MaxCover;
                ViewData["Occupation"] = appDetail.OccupationDetail;
                ViewData["sum"] = appDetail.PremiumPerYear.Value + 20;

                if (appDetail.ProductId == 175)
                {
                    ViewBag.Coverage = Properties.Settings.Default.Covid_Coverage_Plan1;
                    ViewBag.Coverage2 = Properties.Settings.Default.Covid_Coverage_Plan1_2;
                }
                else if (appDetail.ProductId == 176)
                {
                    ViewBag.Coverage = Properties.Settings.Default.Covid_Coverage_Plan2;
                    ViewBag.Coverage2 = Properties.Settings.Default.Covid_Coverage_Plan2_2;
                }
            }

            return View();
        }

        public ActionResult CovidPDF(string appId)
        {
            if (appId == null)
            {
                ViewBag.appId = 0;
                return View();
            }
            var memberIdBase64EncodedBytes = Convert.FromBase64String(appId);
            var appID = Encoding.UTF8.GetString(memberIdBase64EncodedBytes);

            var appDetail = _context.usp_CovidApplicationByAppId_Select(Convert.ToInt32(appID)).FirstOrDefault();
            ViewBag.appId = appID;
            if (appDetail != null)
            {
                ViewBag.appDetail = appDetail;

                var startCoverDate = appDetail.StartCoverDate?.ToString("dd/MM/yyyy");
                ViewData["startCoverDate"] = startCoverDate;

                var endCoverDate = appDetail.EndCoverDate?.ToString("dd/MM/yyyy");
                ViewData["endCoverDate"] = endCoverDate;

                var xxx = Convert.ToDateTime(appDetail.BirthDate).AddYears(543); ;
                ViewData["HBD"] = xxx.ToString("dd/MM/yyyy");
                var year = appDetail.AgeAtRegister_Year; //DateTime.Now.Year - bDate.Year;
                ViewBag.bDate = year;

                ViewData["Sex"] = appDetail.SexDetail;
                ViewData["FirstName"] = appDetail.FirstName;
                ViewData["LastName"] = appDetail.LastName;
                ViewData["Address"] = appDetail.FullAddress;
                ViewData["premium"] = appDetail.PremiumPerYear;
                ViewData["MaxCover"] = appDetail.MaxCover;
                ViewData["Occupation"] = appDetail.OccupationDetail;
                ViewData["sum"] = appDetail.PremiumPerYear.Value + 20;

                if (appDetail.ProductId == 175)
                {
                    ViewBag.Coverage = Properties.Settings.Default.Covid_Coverage_Plan1;
                    ViewBag.Coverage2 = Properties.Settings.Default.Covid_Coverage_Plan1_2;
                }
                else if (appDetail.ProductId == 176)
                {
                    ViewBag.Coverage = Properties.Settings.Default.Covid_Coverage_Plan2;
                    ViewBag.Coverage2 = Properties.Settings.Default.Covid_Coverage_Plan2_2;
                }
            }

            var r = new ViewAsPdf("CovidPDF");
            r.PageSize = Size.A4;
            r.PageOrientation = Orientation.Portrait;
            r.IsGrayScale = false;
            r.FileName = "CovidPDF.pdf";

            r.CustomSwitches =
            "--footer-left \"ผู้จัดการโครงการ : บริษัท สยามสไมล์โบรกเกอร์ (ประเทศไทย) จำกัด " + "\"" +
            " --footer-line --footer-font-size \"9\" --footer-spacing -5 --footer-font-name \"Kanit\"";

            return r;
            //return View();
        }

        [Authorization]
        public ActionResult SearchApplication()
        {
            ViewBag.DocReact = Properties.Settings.Default.DocReact;
            return View();
        }

        #endregion View

        #region datatable

        //where saleld
        public ActionResult GetApplicationMonitor(int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).UserName;
            //if (searchDetail != null)
            //{
            //    user = null;
            //}

            var result = _context.usp_CovidApplication_Select(user, indexStart, pageSize, sortField, orderType, searchDetail, null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllApplicationMonitor(int? draw = null, int? indexStart = null, int? pageSize = null,
        string sortField = null, string orderType = null, string searchDetail = null)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).UserName;
            if (searchDetail != null)
            {
                user = null;
            }

            var result = _context.usp_CovidApplication_Select(null, indexStart, pageSize, sortField, orderType, searchDetail, null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUdwReport()
        {
            //var result = _context

            //var dt = new Dictionary<string,object>
            //{
            //    {"draw", draw },
            //    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
            //    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
            //    {"data", result.ToList()}
            //};

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        #endregion datatable

        #region api

        [HttpPost]
        public ActionResult AddNewApplication(int productId, string startCoverDate, string firstName, string lastName, string identityCardNo, int sexId,
            string birthDate, string mobilePhoneNumber, string email, string lineId, string address, string subDistrictId, int occupationId)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                int res;
                int end;

                //check session expired
                if (user.UserName is null && user.User_ID == 0)
                {
                    return Json(new { IsResult = false, Msg = "Your session is expired", SessionTimeOut = true, JsonRequestBehavior.AllowGet });
                }

                //check firstName
                end = firstName.Length;
                res = 0;
                res = firstName.IndexOf("฿", 0, end);
                if (res != -1)
                {
                    return Json(new { IsResult = false, Msg = $"ตรวจสอบชื่อผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                }

                Regex regexFirstName = new Regex(@"[ก-ฮA-Zะ-ไเ-ไ็่้๊๋์ํa-z.]+$");
                Match matchFirstName = regexFirstName.Match(firstName.Trim());
                if (!matchFirstName.Success)
                {
                    return Json(new { IsResult = false, Msg = $"ตรวจสอบชื่อผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                }

                //check lastName
                end = lastName.Length;
                res = 0;
                res = lastName.IndexOf("฿", 0, end);
                if (res != -1)
                {
                    return Json(new { IsResult = false, Msg = $"ตรวจสอบนามสกุลผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                }

                //check lastName เช่น ณ อยุธยา
                Regex regexLastName = new Regex(@"^ณ [ก-๙A-Za-z]+$");
                Match matchLastName = regexLastName.Match(lastName.Trim());
                if (!matchLastName.Success)
                {
                    //check lastName เช่น เทพหัสดิน ณ อยุธยา
                    Regex regexLastName3 = new Regex(@"[ก-๙A-Za-z] ณ [ก-๙A-Za-z]+$");
                    Match matchLastName3 = regexLastName3.Match(lastName.Trim());
                    if (!matchLastName3.Success)
                    {
                        Regex regexLastName2 = new Regex(@"[ก-ฮA-Zะ-ไเ-ไ็่้๊๋์ํa-z.]+$");
                        Match matchLastName2 = regexLastName2.Match(lastName.Trim());
                        //lName = lastName;
                        if (!matchLastName2.Success)
                        {
                            return Json(new { IsResult = false, Msg = $"ตรวจสอบนามสกุลผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                        }
                    }
                }
                //end check lastName

                //var startCoverDateConvert = DateTime.ParseExact(startCoverDate, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-Us"));

                var birthDateConvert = DateTime.ParseExact(birthDate, "dd-MM-yyyy", new CultureInfo("en-Us"));

                DateTime today = DateTime.Today;
                double age = today.Year - birthDateConvert.Year;

                //check age min 1 Year
                if (today.Month < birthDateConvert.Month ||
                   ((today.Month == birthDateConvert.Month) && (today.Day < birthDateConvert.Day)))
                {
                    if (age == 1)
                    {
                        age--;
                        if (age < 1)
                        {
                            return Json(new { IsResult = false, Msg = "ผู้ขอเอาประกันภัยต้องมีอายุตั้งแต่ 1 ปีขึ้นไป", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                        }
                    }
                }

                //check age max 100 Year
                if (today.Month > birthDateConvert.Month ||
                   ((today.Month == birthDateConvert.Month) && (today.Day > birthDateConvert.Day)))
                {
                    if (age == 99)
                    {
                        age++;
                        if (age > 99)
                        {
                            return Json(new { IsResult = false, Msg = "ผู้ขอเอาประกันภัยต้องมีอายุไม่เกิน 99 ปี", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                        }
                    }
                }

                //replace mask phon
                var phone = mobilePhoneNumber.Trim().Replace("-", "");

                //get current datetime
                var startCoverYear = DateTime.Now.Year;
                var startCoverDay = DateTime.Now.AddDays(1);
                var aaa = startCoverDay.ToString("dd/MM/yyyy");
                var xxx = aaa + " " + "00" + ':' + "01" + ':' + "00";
                var startCoverDateConvert = Convert.ToDateTime(xxx);
                //--end-- get current datetime --end--//

                var result = _context.usp_CovidApplication_Insert(productId, startCoverDateConvert, firstName.Trim(), lastName.Trim(), identityCardNo, sexId, birthDateConvert,
                    phone, email, lineId, address, subDistrictId, user.UserName, user.User_ID, occupationId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message, SessionTimeOut = false, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpGet]
        public ActionResult GetDocReact(string id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.appId = 0;
                    return View();
                }
                var memberIdBase64EncodedBytes = Convert.FromBase64String(id);
                var appID = Encoding.UTF8.GetString(memberIdBase64EncodedBytes);
                ViewBag.id = id;
                var appDetail = _context.usp_CovidApplicationByAppId_Select(Convert.ToInt32(appID)).FirstOrDefault();
                var startCoverDate = appDetail.StartCoverDate?.ToString("dd/MM/yyyy");
                ViewData["startCoverDate"] = startCoverDate;

                var endCoverDate = appDetail.EndCoverDate?.ToString("dd/MM/yyyy");
                ViewData["endCoverDate"] = endCoverDate;

                var xxx = Convert.ToDateTime(appDetail.BirthDate);
                ViewData["HBD"] = xxx.ToString("dd/MM/yyyy");
                var year = appDetail.AgeAtRegister_Year; //DateTime.Now.Year - bDate.Year;
                ViewBag.bDate = year;
                var obj = new Doc
                {
                    FastName = appDetail.FirstName,
                    LastName = appDetail.LastName,
                    Sex = appDetail.SexDetail,
                    Address = appDetail.FullAddress,
                    Age = Convert.ToInt32(appDetail.AgeAtRegister_Year),
                    Premium = Convert.ToInt32(appDetail.PremiumNet),
                    MaxCover = Convert.ToInt32(appDetail.MaxCover),
                    Phone = appDetail.MobilePhoneNumber.Trim().Replace("-", ""),
                    Occupation = appDetail.OccupationDetail,
                    AppId = appID,
                    HBD = xxx.ToString("dd/MM/yyyy"),
                    StartCoverDate = startCoverDate,
                    EndCoverDate = endCoverDate,
                    Sum = Convert.ToInt32(appDetail.PremiumPerYear),
                    Duty = Convert.ToInt32(appDetail.Duty),
                    Vat = Convert.ToInt32(appDetail.Vat)
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult UpdateDataCenter(string appId)
        {
            try
            {
                if (appId != "")
                {
                    var user = GlobalFunction.GetLoginDetail(HttpContext);
                    var rs = _context.usp_CustomerApplication_CV_Person_InsertOrUpdate(Convert.ToInt32(appId), "C", user.User_ID);
                    return Json(new { IsResult = true, Msg = "", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { IsResult = false, Msg = "appID not found", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult UpdateLogView(int id, string remark)
        {
            try
            {
                var rs = _context.usp_CovidApplicationView_Insert(id, remark).FirstOrDefault();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpGet]
        public ActionResult CheckEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                return Json(new { IsResult = false, Msg = $"E-Mail ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsResult = true, Msg = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CheckMaxCover(string id)
        {
            if (id == "")
            {
                return Json(new { IsResult = false, Msg = $"id not found" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var rs = _context.usp_CovidApplicationCoverage_Validate(id).FirstOrDefault();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult UpdateApplication(int appId, int productId, DateTime startCoverDate, string firstName, string lastName, string identityCardNo, int sexId,
            string birthDate, string mobilePhoneNumber, string email, string lineId, string address, string subDistrictId, int occupationId)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            int res;
            int end;

            //check session expired
            if (user.User_ID == 0)
            {
                return Json(new { IsResult = false, Msg = "Your session is expired", SessionTimeOut = true, JsonRequestBehavior.AllowGet });
            }

            //check firstName
            end = firstName.Length;
            res = 0;
            res = firstName.IndexOf("฿", 0, end);
            if (res != -1)
            {
                return Json(new { IsResult = false, Msg = $"ตรวจสอบชื่อผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
            }

            Regex regexFirstName = new Regex(@"[ก-ฮA-Zะ-ไเ-ไ็่้๊๋์ํa-z.]+$");
            Match matchFirstName = regexFirstName.Match(firstName.Trim());
            if (!matchFirstName.Success)
            {
                return Json(new { IsResult = false, Msg = $"ตรวจสอบชื่อผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
            }
            //end check firstName

            //check lastName
            end = lastName.Length;
            res = 0;
            res = lastName.IndexOf("฿", 0, end);
            if (res != -1)
            {
                return Json(new { IsResult = false, Msg = $"ตรวจสอบนามสกุลผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
            }

            //check lastName เช่น ณ อยุธยา
            Regex regexLastName = new Regex(@"^ณ [ก-๙A-Za-z]+$");
            Match matchLastName = regexLastName.Match(lastName.Trim());
            if (!matchLastName.Success)
            {
                //check lastName เช่น เทพหัสดิน ณ อยุธยา
                Regex regexLastName3 = new Regex(@"[ก-๙A-Za-z] ณ [ก-๙A-Za-z]+$");
                Match matchLastName3 = regexLastName3.Match(lastName.Trim());
                if (!matchLastName3.Success)
                {
                    Regex regexLastName2 = new Regex(@"[ก-ฮA-Zะ-ไเ-ไ็่้๊๋์ํa-z.]+$");
                    Match matchLastName2 = regexLastName2.Match(lastName.Trim());
                    if (!matchLastName2.Success)
                    {
                        return Json(new { IsResult = false, Msg = $"ตรวจสอบนามสกุลผู้เอาประกัน", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                    }
                }
            }
            //end check lastName

            //var startCoverDateConvert = DateTime.ParseExact(startCoverDate, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-Us"));
            var startCoverDateConvert = startCoverDate;
            var birthDateConvert = DateTime.ParseExact(birthDate, "dd-MM-yyyy", new CultureInfo("en-Us"));

            DateTime today = DateTime.Today;
            double age = today.Year - birthDateConvert.Year;

            //check age min 1 Year
            if (today.Month < birthDateConvert.Month ||
               ((today.Month == birthDateConvert.Month) && (today.Day < birthDateConvert.Day)))
            {
                if (age == 1)
                {
                    age--;
                    if (age < 1)
                    {
                        return Json(new { IsResult = false, Msg = "ผู้ขอเอาประกันภัยต้องมีอายุตั้งแต่ 1 ปีขึ้นไป", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                    }
                }
            }

            //check age max 100 Year
            if (today.Month > birthDateConvert.Month ||
               ((today.Month == birthDateConvert.Month) && (today.Day > birthDateConvert.Day)))
            {
                if (age == 99)
                {
                    age++;
                    if (age > 99)
                    {
                        return Json(new { IsResult = false, Msg = "ผู้ขอเอาประกันภัยต้องมีอายุไม่เกิน 99 ปี", SessionTimeOut = false, JsonRequestBehavior.AllowGet });
                    }
                }
            }

            //replace mask phon
            var phone = mobilePhoneNumber.Trim().Replace("-", "");

            //get current  app
            var saleId = _context.usp_CovidApplicationByAppId_Select(appId).Select(o => o.SaleId).FirstOrDefault();

            var result = _context.usp_CovidApplicationByAppId_Update(appId, productId, startCoverDateConvert, firstName.Trim(), lastName.Trim(), identityCardNo,
                sexId, birthDateConvert, phone, email, lineId, address, subDistrictId, saleId, user.User_ID, occupationId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckCitizenId(string identityIDCard)
        {
            var result = _context.usp_CheckIdentityIDCard(identityIDCard).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDoc(string identityIDCard)
        {
            var result = _context.usp_CheckIdentityIDCard(identityIDCard).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAppDetail(int covidApplicationId)
        {
            var result = _context.usp_CovidApplicationByAppId_Select(covidApplicationId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteApplication(int appId)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var result = _context.usp_CovidApplicationDeleteByAppId_Update(appId, user.User_ID).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDistrict(int? provinceId)
        {
            try
            {
                using (var client = new AddressServiceClient())
                {
                    var districtList = client.GetDistrict(provinceId).ToList();
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetSubDistrict(int? districtId)
        {
            try
            {
                using (var client = new AddressServiceClient())
                {
                    var subDistrictList = client.GetSubDistrict(districtId).ToList();
                    return Json(subDistrictList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetPostCode(int subDistrictId)
        {
            try
            {
                using (var client = new AddressServiceClient())
                {
                    var postCode = client.GetZipCodeBySubDistrictID(subDistrictId).ZipCode;
                    return Json(postCode, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion api
    }
}