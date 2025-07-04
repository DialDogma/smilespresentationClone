using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.WebPages;
using Newtonsoft.Json;
using QRCoder;
using SmileClaimV2.Helper;
using SmileClaimV2.Models;

namespace SmileClaimV2.Controllers
{
    [Authorization]
    public class HCIController : Controller
    {
        private readonly CultureInfo dateTH = new CultureInfo("th-TH");

        // GET: HCI

        #region Declare

        private readonly SSSEntities _context;
        private readonly SSSPAEntities _pacontext;
        private readonly HospitalClaimInformV1Entities _hcicontext;
        private readonly DownloadFiles obj;

        public HCIController()
        {
            _context = new SSSEntities();
            _pacontext = new SSSPAEntities();
            _hcicontext = new HospitalClaimInformV1Entities();
            obj = new DownloadFiles();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _hcicontext.Dispose();
            _pacontext.Dispose();
        }

        #endregion Declare

        #region Action

        public ActionResult Index()
        {
            return View();
        }

        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019  MENU ค้นหาเช็คสิทธิ์โรงพยาบาล
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPA,SmileClaimV2_HospitalPH,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetHospital()
        {
            return View();
        }

        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019  MENU ยกเลิกการใช้สิทธิ์
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPA,SmileClaimV2_HospitalPH,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetHospitalCancel()
        {
            return View();
        }

        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Manager,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Manager,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetHospitalMonitor()
        {
            ViewBag.URL_Claim_PH = Properties.Settings.Default.URL_Claim_PH;
            ViewBag.URL_Claim_PA = Properties.Settings.Default.URL_Claim_PA;
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Manager,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Manager,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult StaticHCI()
        {
            return View();
        }

        public ActionResult DownloadDocument()
        {
            var filesCollection = obj.GetFilesInfo();
            return View(filesCollection);
        }

        public FileResult DownloadFile(string fileID)
        {
            var currentFileId = Convert.ToInt32(fileID);
            var filesCollection = obj.GetFilesInfo();
            var currentFile = filesCollection.First(f => f.FileId == currentFileId);
            var fileName = currentFile.FilePath;
            var fileDownloadName = currentFile.FileName;

            return File(fileName, MediaTypeNames.Application.Octet, fileDownloadName);
        }

        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Manager,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPA,SmileClaimV2_HospitalPH,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetHCIDetail(string hciId)

        {
            ViewBag.hciId = hciId;

            // decode
            byte[] data = Convert.FromBase64String(hciId);
            var DecodeValue = Encoding.UTF8.GetString(data);
            //DecodeValue length equal 15 > HCI CODE
            var Id = DecodeValue.Length == 15 ? _hcicontext.usp_HCIIDByHCICode_Select(DecodeValue).First().HospitalClaimInformID : int.Parse(DecodeValue);

            ViewBag.Detail = _hcicontext.usp_HCIDetail_select(Id).FirstOrDefault();

            ViewBag.CustomerDetail = _hcicontext.usp_HCICustomerDetail_select(Id).FirstOrDefault();

            ViewBag.listCancelCause = JsonConvert.SerializeObject(_hcicontext.usp_HCI_CancelCause().ToList());

            //GET LOGIN DETAIL AND GET ROLES USER
            //var loginDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            //var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(loginDetail.UserName);
            //Update 2025-05-06
            var cookie = OAuth2Helper.GetCookie();
            var userRole = OAuth2Helper.GetRoles(cookie.Value);

            var roleCallClaim = "SmileClaimV2_Operation";
            ViewBag.IsCallClaim = userRole.Contains(roleCallClaim);

            return View();
        }

        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Manager,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPA,SmileClaimV2_HospitalPH,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetHCIDetailForCancel(string hciId)

        {
            ViewBag.hciId = hciId;

            // decode
            byte[] data = Convert.FromBase64String(hciId);
            var DecodeValue = Encoding.UTF8.GetString(data);
            //DecodeValue length equal 15 > HCI CODE
            var Id = DecodeValue.Length == 15 ? _hcicontext.usp_HCIIDByHCICode_Select(DecodeValue).First().HospitalClaimInformID : int.Parse(DecodeValue);

            ViewBag.Detail = _hcicontext.usp_HCIDetail_select(Id).FirstOrDefault();

            ViewBag.CustomerDetail = _hcicontext.usp_HCICustomerDetail_select(Id).FirstOrDefault();

            ViewBag.listCancelCause = JsonConvert.SerializeObject(_hcicontext.usp_HCI_CancelCause().ToList());

            //GET LOGIN DETAIL AND GET ROLES USER
            //var loginDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            //var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(loginDetail.UserName);

            //Update 2025-05-06
            var cookie = OAuth2Helper.GetCookie();
            var userRole = OAuth2Helper.GetRoles(cookie.Value);

            var roleCallClaim = "SmileClaimV2_Operation";
            ViewBag.IsCallClaim = userRole.Contains(roleCallClaim);

            return View();
        }

        public ActionResult CancelHCI(FormCollection form)
        {
            //string hciId, int cancelCauseId
            //CancelHCI
            var hospitalClaimInformId = Convert.ToInt32(form["hciId"]);
            var cancelById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID; //Convert.ToInt32(Session["User_ID"]);
            var cancelCauseId = Convert.ToInt32(form["cancelCauseId"]);

            var cancel_HCI = _hcicontext.usp_HCI_Cancel(hospitalClaimInformId, cancelById, cancelCauseId).FirstOrDefault();

            return Json(cancel_HCI.HospitalClaimInformID, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ค้นหาโรคและข้อยกเว้น
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult SearchExcept(string search)
        {
            var result = _hcicontext.usp_Except_Search(search).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ค้นหาโรคยกเว้นเฉพาะบุคคล
        /// </summary>
        /// <param name="search"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult SearchDeseaseExcept(string appId, string search)
        {
            var result = _context.usp_CustomerDeseaseExcept_Search(appId, search).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Action

        #region GetData

        /// <summary>
        /// เช็คสเตตัสเคลมก่อนยกเลิก
        /// </summary>
        /// <param name="claimCode"></param>
        /// <returns></returns>
        public ActionResult CheckClaimStatus(string claimCode)
        {
            //CHECK CLAIM PA
            var IsClaimPA = claimCode.Contains("CLPA");

            //TRUE
            if (IsClaimPA)
            {
                //CALL SP FOR CHECK CLAIM PA
                var result = _pacontext.usp_ClaimHeaderPAByStatus_Select(claimCode).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //CALL SP FOR CHECK CLAIM PH
                var result = _context.usp_ClaimHeaderByStatus_Select(claimCode).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string dateStart, string dateEnd, string search, int? claimAdmitTypeId = null, int? productGroupId = null, int? claimTypeId = null, int? HCIStatusId = null)
        {
            var dStart = GlobalFunction.ConvertDatePickerToDate_BE(dateStart);
            var dEnd = GlobalFunction.ConvertDatePickerToDate_BE(dateEnd);

            var sessoionOrganizeID = GlobalFunction.GetLoginDetail(this.HttpContext).Organize_ID; //Session["Organize_ID"];
            int? hospitalId = null;
            if (sessoionOrganizeID != null)
            {
                hospitalId = int.Parse(sessoionOrganizeID.ToString());
            }

            var list = _hcicontext.usp_HCI_select(hospitalId, dStart, dEnd, claimTypeId, claimAdmitTypeId, productGroupId, HCIStatusId, search, pageStart, pageSize, sortField, orderType).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStaticDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string dateStart, string dateEnd)
        {
            var dStart = GlobalFunction.ConvertDatePickerToDate_BE(dateStart);
            var dEnd = GlobalFunction.ConvertDatePickerToDate_BE(dateEnd);

            var list = _hcicontext.usp_HCI_Static_Select(dStart, dEnd, pageStart, pageSize, sortField, orderType).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion GetData

        #region Report

        public ActionResult GetOPDDetail(string hciId)
        {
            // decode
            byte[] data = Convert.FromBase64String(hciId);
            var Id = Convert.ToInt32(Encoding.UTF8.GetString(data));

            var result = _hcicontext.usp_HCIClaimForm_Select(Id).First();
            if (result != null)
            {
                ViewBag.HCIClaimForm = result;
            }
            return View();
        }

        public ActionResult GetGuaranteeMedical()
        {
            return View();
        }

        public class HospitalInfo
        {
            public string HospitalClaimInformCode { get; set; }
            public string FullName { get; set; }
            public string ClaimHeaderID { get; set; }
            public string OrganizeDetail { get; set; }
        }

        public ActionResult GetOPDAndGuaranteeReport(string hciId, string page4)
        {
            //DECODE
            byte[] data = Convert.FromBase64String(hciId);
            var Id = Convert.ToInt32(Encoding.UTF8.GetString(data));

            ViewBag.Id = Id;

            var success = Boolean.TryParse(page4, out var p4);

            //GET RESULT HCI CLAIMFORM
            var result = _hcicontext.usp_HCIClaimForm_Select(Id).First();
            //GET RESULT HCI CUSTOMER DETAIL
            var hciCustomerDetail = _hcicontext.usp_HCICustomerDetail_select(Id).First();

            //GET HCI FOR GENERATE QR CODE
           
            var hospital = string.Join("|", new string[]
            {   
                    result.ClaimHeaderID ?? string.Empty,
                    result.HospitalClaimInformCode ?? string.Empty,
            });

            var jsonData = !string.IsNullOrEmpty(hospital) ? hospital : "";
            var qrcode = QRCodeGeneratorHelper.GenerateQRCodeAsBase64(jsonData);
            ViewBag.QRcodePH = qrcode;    

            //SET APP_ID
            var appId = result.ApplicationID;
            //CheckDate
            bool IsCheckDate30 = false;
            bool IsCheckDate120 = false;
            DateTime startDate = hciCustomerDetail.StartCoverDate.Value;
            DateTime dHappen = hciCustomerDetail.DateHappen.Value;
            DateTime newDateHappen = new DateTime(dHappen.Year, dHappen.Month, dHappen.Day, 0, 0, 0);
            var d30 = startDate.AddDays(30 - 1);
            var d120 = startDate.AddDays(120 - 1);
            //check 30
            if (newDateHappen >= startDate && newDateHappen <= d30)
            {
                IsCheckDate30 = true;
            }
            else
            {
                //check 120
                if (newDateHappen >= startDate && newDateHappen <= d120) IsCheckDate120 = true;
            }

            ViewBag.IsCheckDate30 = IsCheckDate30;
            ViewBag.IsCheckDate120 = IsCheckDate120;

            //หาอายุช่วงวันเกิด - วันเคลม
            var IsAgeGreaterThanOrEqualTo20 = false;
            //var age = CalculateAgeFromDateOfBirthToDateofClaim(hciCustomerDetail.BirthDate.Value, hciCustomerDetail.CreatedDate.Value);
            //if (age >= 20) IsAgeGreaterThanOrEqualTo20 = true;
            ViewBag.IsAgeGreaterThanOrEqualTo20 = IsAgeGreaterThanOrEqualTo20;

            ////GET OPD
            //var opdRemain = _context.usp_GetOPDRemain(appId, hciCustomerDetail.DateHappen).First();
            ////SET  OPD TO VIEWBAG
            //ViewBag.OPDRemain = opdRemain ?? 0;

            //GET BENEFIT AND SET TO VIEWBAG
            ViewBag.accident = _context.usp_BenefitCoverrage_ByBenefitID_PH_Select(appId, "1300").First();
            ViewBag.sick = _context.usp_BenefitCoverrage_ByBenefitID_PH_Select(appId, "1400").FirstOrDefault();
            //RESULT
            ViewBag.HCIClaimForm = result;
            ViewBag.HCICustomerDetail = hciCustomerDetail;
            //SET CREATE DATE
            ViewBag.CreateDate = result.CreatedDate.Value.ToString("d MMMM yyyy", dateTH);
            ViewBag.CreateTime = result.CreatedDate.Value.ToString("HH:mm");
            //SET CURRENT DATE
            ViewBag.CurrentDate = DateTime.Now.ToString("d MMMM yyyy", dateTH);
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm");
            //SET BIRTH DATE
            ViewBag.BirthDate = ViewBag.HCIClaimForm.BirthDate.ToString("dd MMMM yyyy", dateTH);
            //page4
            ViewBag.Page4 = p4;
            //06074 : 16092021 : Add Claim ID for Report.
            ViewBag.ClaimHeaderId = hciCustomerDetail.ClaimHeaderID;
            return View();
        }

        public ActionResult GetOPDAndGuaranteeReport_PA(string hciId)
        {
            //DECODE
            byte[] data = Convert.FromBase64String(hciId);
            var Id = Convert.ToInt32(Encoding.UTF8.GetString(data));
            //GET RESULT HCI CLAIMFORM
            var result = _hcicontext.usp_HCIClaimForm_Select(Id).First();
            //SET CREATE DATE
            ViewBag.CreateDate = result.CreatedDate.Value.ToString("d MMMM yyyy", dateTH);
            ViewBag.CreateTime = result.CreatedDate.Value.ToString("HH:mm");
            //SET CURRENT DATE
            ViewBag.CurrentDate = DateTime.Now.ToString("d MMMM yyyy", dateTH);
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm");
            //GET RESULT HCI CUSTOMER DETAIL
            var hciCustomerDetail = _hcicontext.usp_HCICustomerDetail_select(Id).First();
            //GET RESULT CUSTOMER DETAIL
            var customerDetail = _pacontext.usp_CustomerDetail_PA_Select(hciCustomerDetail.ReferenceID).First();

            //GET HCI FOR GENERATE QR CODE
            var hospitalString = string.Join("|", new string[]
             {
                result.ClaimHeaderID ?? string.Empty,
                result.HospitalClaimInformCode ?? string.Empty,
             });

            var jsonData = !string.IsNullOrEmpty(hospitalString) ? hospitalString : "";
            var qrcode = QRCodeGeneratorHelper.GenerateQRCodeAsBase64(jsonData);
            ViewBag.QRCodePA = qrcode;

            //SET APP_ID
            var appId = result.ApplicationID;
            //GET BENEFIT AND SET TO VIEWBAG
            ViewBag.b2006 = _pacontext.usp_BenefitCoverage_ByBenefitID_Select(appId, "2006").First();
            //RESULT
            ViewBag.HCIClaimForm = result;
            ViewBag.HCICustomerDetail = hciCustomerDetail;
            ViewBag.CustomerDetail = customerDetail;
            //SET BIRTH DATE
            //ViewBag.BirthDate = ViewBag.HCIClaimForm.BirthDate.ToString("dd MMMM yyyy", dateTH);

            //06074 : 16092021 : Add Claim ID for Report.
            ViewBag.ClaimHeaderId = hciCustomerDetail.ClaimHeaderID;
            return View();
        }

        public ActionResult GetOPDAndGuaranteeReport_PA_New(string hciId, string page5)
        {
            //DECODE
            byte[] data = Convert.FromBase64String(hciId);
            var Id = Convert.ToInt32(Encoding.UTF8.GetString(data));
            var success = Boolean.TryParse(page5, out var p5);
            //GET RESULT HCI CLAIMFORM
            var result = _hcicontext.usp_HCIClaimForm_Select(Id).First();
            //SET CREATE DATE
            ViewBag.CreateDate = result.CreatedDate.Value.ToString("d MMMM yyyy", dateTH);
            ViewBag.CreateTime = result.CreatedDate.Value.ToString("HH:mm");
            //SET CURRENT DATE
            ViewBag.CurrentDate = DateTime.Now.ToString("d MMMM yyyy", dateTH);
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm");
            //GET RESULT HCI CUSTOMER DETAIL
            var hciCustomerDetail = _hcicontext.usp_HCICustomerDetail_select(Id).First();
            //GET RESULT CUSTOMER DETAIL
            var customerDetail = _pacontext.usp_CustomerDetail_PA_Select(hciCustomerDetail.ReferenceID).First();

            //GET HCI FOR GENERATE QR CODE
            var hospitalString = string.Join("|", new string[]
             {
                result.ClaimHeaderID ?? string.Empty,
                result.HospitalClaimInformCode ?? string.Empty,
             });

            var jsonData = !string.IsNullOrEmpty(hospitalString) ? hospitalString : "";
            var qrcode = QRCodeGeneratorHelper.GenerateQRCodeAsBase64(jsonData);
            ViewBag.QRCodePA = qrcode;

            //SET APP_ID
            var appId = result.ApplicationID;
            //GET BENEFIT AND SET TO VIEWBAG
            ViewBag.b2006 = _pacontext.usp_BenefitCoverage_ByBenefitID_Select(appId, "2006").First();
            //RESULT
            ViewBag.HCIClaimForm = result;
            ViewBag.HCICustomerDetail = hciCustomerDetail;
            ViewBag.CustomerDetail = customerDetail;
            //SET BIRTH DATE
            //ViewBag.BirthDate = ViewBag.HCIClaimForm.BirthDate.ToString("dd MMMM yyyy", dateTH);

            //06074 : 16092021 : Add Claim ID for Report.
            ViewBag.ClaimHeaderId = hciCustomerDetail.ClaimHeaderID;
            //Page5
            ViewBag.Page5 = p5;
            return View();
        }

        [HttpPost]
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Manager,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Manager,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult ReportStatic(FormCollection form)
        {
            var dateStart = form["date_start"];
            var dateEnd = form["date_end"];
            var dStart = GlobalFunction.ConvertDatePickerToDate_BE(dateStart);
            var dEnd = GlobalFunction.ConvertDatePickerToDate_BE(dateEnd);

            var list = _hcicontext.usp_HCI_Static_Select(dStart, dEnd, 0, 1000000, "", "").Select(o => new { o.HospitalID, o.Hospital, o.เช็คสิทธิ์เคลม, o.ยกเลิกเคลม, o.ALL }).ToList();

            //Export To Excel
            ExcelManager.ExportToExcel(HttpContext, list, "สถิติการแจ้งใช้สิทธิ์", "รายงานสถิติการแจ้งใช้สิทธิ์ วันที่ " + dateStart + "-" + dateEnd);

            return RedirectToAction("StaticHCI", "HCI");
        }

        #endregion Report

        #region Method

        /// <summary>
        ///
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <param name="dateOfClaim"></param>
        /// <returns></returns>
        private int CalculateAgeFromDateOfBirthToDateofClaim(DateTime dateOfBirth, DateTime dateOfClaim)
        {
            int age = 0;
            age = dateOfClaim.Year - dateOfBirth.Year;
            //Go back to the year the person was born in case of a leap year (366days)
            if (dateOfClaim.DayOfYear < dateOfBirth.DayOfYear) age--;

            return age;
        }

        #endregion Method
    }
}