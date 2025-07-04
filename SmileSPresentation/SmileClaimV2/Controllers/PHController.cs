using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using SmileClaimV2.Helper;
using SmileClaimV2.Models;
using static SmileClaimV2.Models.SmsModel;

namespace SmileClaimV2.Controllers {
    [Authorization]
    public class PHController : Controller {
        //private readonly CultureInfo _dateTh = new CultureInfo("th-TH");
        //private readonly CultureInfo _dateEn = new CultureInfo("en-EN");

        #region Declare

        //**start set context
        private readonly SSSEntities _context;

        private readonly HospitalClaimInformV1Entities _hospitalClaimInform;
        private readonly DataCenterV1Entities _dataCenterV1centerV1;
        private const string _controllerName = nameof(PHController);


        public PHController() {
            _context = new SSSEntities();
            _hospitalClaimInform = new HospitalClaimInformV1Entities();
            _dataCenterV1centerV1 = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing) {
            _context.Dispose();
            _hospitalClaimInform.Dispose();
            _dataCenterV1centerV1.Dispose();
        }

        //**end set context

        #endregion Declare

        #region Action

        // GET: PH
        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019  sub menu ประกันบุคคล
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPH,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult Index() {
            return View();
        }

        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPH,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetOPDRemain(string appId, string dtHappen) {
            var decode = Convert.FromBase64String(appId);
            appId = Encoding.UTF8.GetString(decode);
            var dt = GlobalFunction.ConvertDatePickerToDate_BE(dtHappen);
            var result = _hospitalClaimInform.usp_CustomerBenefit_OPD_Select(appId, dt).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountClaim(string appId, string dateIssue) {
            var app = appId;
            var datetime = Convert.ToDateTime(dateIssue);
            //Count Claim
            var Count_Result = _context.usp_ClaimPHRepeatedlyByAppCode_Select(app, datetime).FirstOrDefault();

            return Json(Count_Result, JsonRequestBehavior.AllowGet);
        }

        //GET:Detail
        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPH,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetDetail(string appId, string pGroupId) {
            // decode
            var dataAppId = Convert.FromBase64String(appId);
            appId = Encoding.UTF8.GetString(dataAppId);
            ViewBag.appId = appId;

            var dataPGroupId = Convert.FromBase64String(pGroupId);
            pGroupId = Encoding.UTF8.GetString(dataPGroupId);
            ViewBag.pGroupId = pGroupId;
            //claimType
            ViewBag.listClaimType = _dataCenterV1centerV1.usp_ClaimType_Select(Convert.ToInt32(pGroupId), true).ToList();
            //detail
            ViewBag.Detail = _context.usp_CustomerDetail_Select(appId).FirstOrDefault();
            ViewBag.StartCoverDate = ViewBag.Detail.StartCoverDate.ToString("dd/MM/yyyy");
            ViewBag.StartCoverDateEN = ViewBag.Detail.StartCoverDate.ToString("MM/dd/yyyy");
            ViewBag.BirthDate = ViewBag.Detail.BirthDate.ToString("dd/MM/yyyy");
            ViewBag.InsuranceCompanyId = ViewBag.Detail.InsuranceCompanyID;
            //benefit
            //  ViewBag.accident = _context.usp_BenefitCoverrage_ByBenefitID_PH_Select(appId, "1300").First();
            //  ViewBag.sick = _context.usp_BenefitCoverrage_ByBenefitID_PH_Select(appId, "1400").FirstOrDefault();

            //Get Except //pg_Id 2 ข้อมูลสำหรับ PA
            ViewBag.DE = _hospitalClaimInform.usp_Except_Select(2, null).ToList();

            //GET LOGIN DETAIL AND GET ROLES USER //ADD 11-4-2562
            //var loginDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            //var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(loginDetail.UserName);
            //Update 2025-05-06
            var cookie = OAuth2Helper.GetCookie();
            var userRole = OAuth2Helper.GetRoles(cookie.Value);

            var roleCallClaim = "SmileClaimV2_Operation";
            ViewBag.IsCallClaim = userRole.Contains(roleCallClaim);

            //check product 901-902
            string ProduceID = @ViewBag.Detail.ProductID;
            var IsOPDByProductCode = _hospitalClaimInform.usp_OPDByProductCode_select(ProduceID).FirstOrDefault().Value;
            ViewBag.IsOPDByProductCode = IsOPDByProductCode;

            //remove session HospitalClaimInformPH
            Session.Remove("HospitalClaimInformPH");

            return View();
        }

        [HttpPost]
        public ActionResult CreateClaimInform(FormCollection form) {
            //Create HCI
            var claimTypeId = Convert.ToInt32(form["claim_type_hidden"]);
            var claimAdmitTypeId = Convert.ToInt32(form["claim_admittype"]);
            var chiefComplainId = Convert.ToInt32(form["chief_complain"]);
            var dateHappen = form["date_happen"];
            var dateIssue = form["date_in"];
            var birthDate = form["birthdate"];
            var startCover = form["startcoverdateTH"];
            var timeHappen = form["time_happen"];
            var timeIssue = form["time_in"];
            var remark = form["remark"];
            var customerPhoneNo = form["phone_number"];
            var officerDepartment = form["officerDepartment"]; //ADD 2-11-2561
            var officerName = form["officerName"]; //ADD 2-11-2561
            //var hciStatusId = 2; //STATUS 2 เช็คสิทธิ์
            var hciStatusId = 4; //สถานะยกเลิกเคลม
            var referenceId = form["appId"];
            var applicationId = form["appId"];
            var name = _context.usp_CustomerDetail_Select(applicationId).Select(x => x.sName).FirstOrDefault();

            var opdBenefitAll = 0;
            var remain = 0;
            var currentuse = 0;

            var hospitalId = GlobalFunction.GetLoginDetail(this.HttpContext).Organize_ID; //Convert.ToInt32(Session["Organize_ID"]);
            const int productGroupId = 2; // 2 = PH
            var createdById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID; //Convert.ToInt32(Session["User_ID"]);
            var cause = form["cause"];
            var dHappen = GlobalFunction.ConvertDatePickerToDate_BE(dateHappen, timeHappen);
            var dIssue = GlobalFunction.ConvertDatePickerToDate_BE(dateIssue, timeIssue);
            var dBrithDate = GlobalFunction.ConvertDatePickerToDate_BE(birthDate, "00:00:00");
            var dStartCover = GlobalFunction.ConvertDatePickerToDate_BE(startCover, "00:00:00");

            var AgeAtClaim = _context.func_CalculateAge(dBrithDate, DateTime.Now).First();
            var AgeAppAtClaim = _context.func_CalculateAge(dStartCover, DateTime.Now).First();
            var year = "-";

            if (claimAdmitTypeId != 7) //7 คือ opdอุบัติเหตุ
            {
                var result = _hospitalClaimInform.usp_CustomerBenefit_OPD_Select(applicationId, dHappen).FirstOrDefault();

                opdBenefitAll = result.OPD_Max.Value;
                remain = result.OPD_Remain.Value;
                currentuse = (result.OPD_Use.Value + 1);
            }

            var hospitalClaimInform = new usp_HospitalClaimInform_Insert_Result();
            if (Session["HospitalClaimInformPH"] != null) {
                // ดึงข้อมูล JSON จาก Session และแปลงกลับเป็นอ็อบเจ็กต์ hospitalClaimInform
                hospitalClaimInform = JsonConvert.DeserializeObject<usp_HospitalClaimInform_Insert_Result>(Session["HospitalClaimInformPH"].ToString());
            }
            else {
                hospitalClaimInform = _hospitalClaimInform.usp_HospitalClaimInform_Insert(hospitalId
                                                                                                 , hciStatusId
                                                                                                 , chiefComplainId
                                                                                                 , claimTypeId
                                                                                                 , claimAdmitTypeId
                                                                                                 , dHappen
                                                                                                 , dIssue
                                                                                                 , null
                                                                                                 , null
                                                                                                 , null
                                                                                                 , remark
                                                                                                 , opdBenefitAll
                                                                                                 , currentuse
                                                                                                 , remain
                                                                                                 , customerPhoneNo
                                                                                                 , createdById
                                                                                                 , referenceId
                                                                                                 , applicationId
                                                                                                 , productGroupId
                                                                                                 , cause
                                                                                                 , year
                                                                                                 , officerName
                                                                                                 , officerDepartment).First();
            }
             
            //Get HCI Detail
            var hciDetail = _hospitalClaimInform.usp_HCIDetail_select(hospitalClaimInform.HospitalClaimInformID).First();

            //Get HCI CUstomerDetail
            var hciCustomerDetail = _hospitalClaimInform.usp_HCICustomerDetail_select(hospitalClaimInform.HospitalClaimInformID).First();

            //Create ClaimPH
            var hospitalCode = GlobalFunction.GetLoginDetail(this.HttpContext).OrganizeCode;// Convert.ToString(Session["Organize_Code"]);
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode; // Convert.ToString(Session["Employee_Code"]);
            var claimHeaderId = _context.usp_ClaimHeader_Insert(
                                                                hciCustomerDetail.ApplicationID
                                                                , hospitalCode //HospitalCode
                                                                , false //IsContinue
                                                                , "" //ContinueFromClaimID
                                                                , hciDetail.ClaimTypeCode
                                                                , hciDetail.ClaimAdmitTypeCode
                                                                , hciDetail.DateHappen.Value
                                                                , hciDetail.DateIssue.Value //DateNotice
                                                                , hciCustomerDetail.ProductCode
                                                                , hciCustomerDetail.InsuranceCompanyCode
                                                                , empCode //AgentID
                                                                , hciDetail.ChiefComplainCode
                                                                , "" //ChiefComplainAnswer
                                                                , "" //ICD10_1
                                                                , "" //ICD10_2
                                                                , "" //ICD10_3
                                                                , remark //Remark
                                                                , "3510" //3510ใช้สิทธิ์แล้วรอจ่าย รพ.
                                                                , AgeAtClaim.Y //AgeAtClaim_Year
                                                                , AgeAtClaim.M //AgeAtClaim_Month
                                                                , AgeAtClaim.D //AgeAtClaim_Day
                                                                , AgeAppAtClaim.Y //AgeAppAtClaim_Year
                                                                , AgeAppAtClaim.M //AgeAppAtClaim_Month
                                                                , AgeAppAtClaim.Y //AgeAppAtClaim_Day
                                                                , empCode//CreateByID *required
                                                                , null //IsWalkoutClaim
                                                                , "" //OverFromCompany
                                                                , false //IsChiefComplainWarning
                                                                , false //IsContinueClaimWarinng
                                                                , false //IsExgratia
                                                                , false //IsNoDCRWarning
                                                                , "" //DenyCauseID
                                                                , "" //ReceiptStatusID
                                                                , "" //SendInsuranceStatusID
                                                                , "" //PayHospitalCashHeaderID
                                                                , null //ClaimHeaderGroupID
                                                                , null //IsPayCenter
                                                                , "" //ApprovedByID
                                                                , null //ApprovedDate
                                                                , "" //ClaimPayByID
                                                                , hciDetail.HospitalClaimInformCode
                                                                , hciCustomerDetail.InsuranceCompanyDetail).First();
            var checkExistClaimHeaderID = _hospitalClaimInform.HospitalClaimInform
                                        .Where(w => w.ClaimHeaderID == claimHeaderId)
                                        .FirstOrDefault();
            if (checkExistClaimHeaderID != null) {
                // ส่งค่าแจ้งว่า ClaimHeaderID ซ้ำ
                Session["HospitalClaimInformPH"] = JsonConvert.SerializeObject(hospitalClaimInform);
                return Json(new { success = false, duplicateClaim = true, message = "บันทึกข้อมูลไม่สำเร็จ กรุณากดปุ่ม ยืนยันการใช้สิทธิ์ อีกครั้ง" });
            }
            else {
                Session.Remove("HospitalClaimInformPH");
            }
            //Update History
            _ = _context.usp_CustomerClaimHistory_Update(applicationCode: applicationId, dateCheck: hciDetail.DateHappen.Value).First();

            //claimHeaderId not string empty
            if (claimHeaderId.Trim() != "") {
                //Update OPD Count -- CL
                _ = _context.usp_ClaimHeaderOPDCount_UpdateV2(claimCode: claimHeaderId).First();

                //UpdateClaimID
                var updateHCI = _hospitalClaimInform.usp_HCI_UpdateClaimID_update(hospitalClaimInform.HospitalClaimInformID, claimHeaderId); // service 32
                if (updateHCI != null) {
                    /*TODO: log Id*/
                }
                else {
                    var resultUpdate = _context.usp_HCI_UpdateStatusClaimNotUse(claimHeaderId);
                    var resultUpdateSt = _hospitalClaimInform.usp_HCIHospitalClaimInform_UpdateStatus(hospitalClaimInform.HospitalClaimInformID, 4); //: Status ยกเลิก

                    hospitalClaimInform.HospitalClaimInformID = null;  //return null
                    return Json(hospitalClaimInform.HospitalClaimInformID, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                _hospitalClaimInform.usp_HCIHospitalClaimInform_UpdateStatus(hospitalClaimInform.HospitalClaimInformID, 4); // : Status ไม่ใช้สิทธิ์

                hospitalClaimInform.HospitalClaimInformID = null;  //return null
                return Json(hospitalClaimInform.HospitalClaimInformID, JsonRequestBehavior.AllowGet);
            }
            try {
                if ((claimHeaderId.Trim() != "") && (hciDetail.ClaimTypeCode.Trim() == "1000") &&
                    (hciDetail.ClaimAdmitTypeCode.Trim() == "2001" || hciDetail.ClaimAdmitTypeCode.Trim() == "2003") &&
                    (hciCustomerDetail.InsuranceCompanyCode.Trim() == "100000000041")) { //เช็คเคลมโรงพยาบาล,OPD ทั่วไป(H) หรือ OPD อุบัติเหตุ(H),บริษัท สยามสไมล์ ประกันภัย จำกัด (มหาชน)
                    SendSMIRequestPH(claimHeaderId, 5, 5, 11, createdById.ToString(), hciDetail.DateHappen.Value, false);// รหัสเคลม,5 = กดยืนยันการใช้สิทธิ์,5 = HCI OPD, 11 = ส่งท.บ.,ผู้ทำรายการ,วันที่เกิดเหตุ
                }
                return SendSMSOPD(dateIssue, name, customerPhoneNo, empCode, hospitalClaimInform.HospitalClaimInformID, _controllerName);
            }
            catch (Exception ex) {
                Log.Error(ex, "{_controllerName} - [CreateClaimInform] Sent SMS OPD Error: {Message}", _controllerName, ex.Message);
                return Json(hospitalClaimInform.HospitalClaimInformID, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckDuplicate(string appId, string chiefComplainCode, string dateHappen, string dateIn)
        {
            var application = appId;
            var submitConfirmTimes = DateTime.Now;
            var dHappen = GlobalFunction.ConvertDatePickerToDate_BE(dateHappen);
            var dIn = GlobalFunction.ConvertDatePickerToDate_BE(dateIn);

            var chiefComplainId = Convert.ToInt32(chiefComplainCode);

            //Count Claim
            var AppCount = _hospitalClaimInform.usp_HCIPH_CheckDuplicate_Select(application, submitConfirmTimes, chiefComplainId, dHappen, dIn).Count();

            return Json(AppCount, JsonRequestBehavior.AllowGet);
        }

        #endregion Action

        #region GetData

        public JsonResult GetDatatablePerson(int draw, int pageSize, int pageStart, string sortField, string orderType, string search) {
            var list = _context.usp_CustomerSearch_Select(pageStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableCause(int draw, int pageSize, int pageStart, string sortField, string orderType, string search) {
            var list = _context.usp_ExceptDisease_Select(pageStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableClaimHistory(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string appId) {
            var list = _context.usp_ClaimHeaderAccidentOnly_Select(appId, pageStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ActionResult CustomerMemoByAppCode(string appId) {
            var list = _context.usp_CustomerMemoByAppCode_Select(appId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion GetData

        public JsonResult SendSMSOPD(string dateIssue, string name, string customerPhoneNo, string empCode, int? hospitalClaimInformID, string controllerName) {
            Log.Debug("{controllerName} [CreateClaimInform] - Start Send SMS OPD ", controllerName);

            string msg = Properties.Settings.Default.SMSPrivilegeOPDMessage.Trim();
            msg = string.Format(msg, dateIssue, name);
            string headerNameAPISMS = Properties.Settings.Default.HeaderNameAPISMS.Trim();
            string headerValueAPISMS = Properties.Settings.Default.HeaderValueAPISMS.Trim();

            //regex pattern
            var reg = new Regex(@"[!""#฿$%&'()*+,-./:;<=>?@A-Zก-๙\[\\\]^_`a-z{|}~]");

            string phoneNumber = "";
            phoneNumber = reg.Replace(customerPhoneNo, "");

            if (phoneNumber.Length != 10) {
                //return เบอร์โทรไม่ถูกต้อง
                Log.Error("{controllerName} - [CreateClaimInform] Please Check Phone Number {0}", controllerName, phoneNumber);
                return Json(new { IsResult = false, Msg = String.Format("กรุณาตรวจสอบหมายเลขโทรศัพท์ {0}", phoneNumber) }, JsonRequestBehavior.AllowGet);
            }

            //Call Service Send SMS
            string EndPoint = Properties.Settings.Default.SMSUrl.Trim() + "/api/sms/SendSMSV2";
            var client = new RestClient(EndPoint);
            //41 SMS แจ้งใช้สิทธิ์ OPD
            var param = new { SMSTypeId = 41, Message = msg, PhoneNo = customerPhoneNo, CreatedById = empCode };
            //Add Json Body to Request
            var request = new RestRequest().AddJsonBody(param);
            //Add Header Token
            request.AddHeader(headerNameAPISMS, headerValueAPISMS);
            //Post Request
            Log.Debug("{controllerName} - [CreateClaimInform] Send SMS OPD [param = {@param}]", controllerName, param);
            var response = client.Post(request);
            //Deserialize JSON
            SMS_Result sms = JsonConvert.DeserializeObject<SMS_Result>(response.Content);
            Log.Debug("{controllerName} - [CreateClaimInform] Send SMS OPD result: {@sms}", controllerName, sms);

            //Log
            Log.Information("{controllerName} - [CreateClaimInform] Sent SMS OPD Success", controllerName);
            return Json(hospitalClaimInformID, JsonRequestBehavior.AllowGet);
        }
        #region Send SMI Request PH
        public JsonResult SendSMIRequestPH(string claimCode, int tPAStatusId, int groupClaimTypeId, int sMIStatusId, string createdByUserCode, DateTime dateHappen,bool isResend) {
            Log.Information("[PH] [SendSMIRequestPH] - Start Send SMI Request PH - Parameter: ClaimCode = {claimCode},TPAStatusId = {tPAStatusId},GroupClaimTypeId = {groupClaimTypeId},SMIStatusId = {sMIStatusId},CreatedByUserCode = {createdByUserCode},DateHappen = {dateHappen}"
                , claimCode, tPAStatusId, groupClaimTypeId, sMIStatusId, createdByUserCode, dateHappen);
            //Call Api sss_api
            string EndPoint = Properties.Settings.Default.SSS_Api.Trim() + "/Claim/insert/claim/ph";
            var client = new RestClient(EndPoint);
            var param = new { claimCode = claimCode, tPAStatusId = tPAStatusId, groupClaimTypeId = groupClaimTypeId, sMIStatusId = sMIStatusId, createdByUserCode = createdByUserCode, dateHappen = dateHappen , isResend = isResend };
            //Add Json Body to Request
            var request = new RestRequest().AddJsonBody(param);

            //Post Request
            try {
                Log.Debug("[PH] - [SendSMIRequestPH] Sending Request - [Parameter = {@param}]", param);
                var response = client.Post(request);
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                bool isSuccess = jsonResponse?.isSuccess ?? false;
                if (isSuccess) {
                    //Log Success
                    Log.Information("[PH] - [SendSMIRequestPH] Success - [Parameter = {@param}]", param);
                    return Json(claimCode, JsonRequestBehavior.AllowGet);
                }
                else {
                    // Log error and handle the failure
                    throw new Exception($"[PH] - [SendSMIRequestPH] API call failed: {response.StatusCode}, Error: {response.ErrorMessage}");
                }
            }
            catch (Exception ex) {
                Log.Error("[PH] - [SendSMIRequestPH] Exception: {Message}", ex.Message);
                var result = _context.usp_ClaimExportTransactionFail_Insert(claimCode, tPAStatusId, groupClaimTypeId, sMIStatusId, createdByUserCode, dateHappen.Date).FirstOrDefault();
                if (result.IsResult == false) {
                    Log.Error("[PH] - [SendSMIRequestPH] Database insert failed. Result: {Result} Error: {Msg}", result.Result, result.Msg);
                    return Json(new { success = false, message = result.Msg }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
            
        }
        #endregion

        #region Check Policy
        public JsonResult CheckPolicy(int productTypeId, string claimDate,string applicationCode) {
            Log.Information("[PH] [CheckPolicy] - Checking Policy PH - Parameter: ProductTypeId = {productTypeId},ClaimDate = {claimDate},ApplicationCode = {applicationCode}"
                , productTypeId, claimDate, applicationCode);
            Log.Debug("[PH] [CheckPolicy] - Checking Policy PH - Parameter: ProductTypeId = {productTypeId},ClaimDate = {claimDate},ApplicationCode = {applicationCode}"
                , productTypeId, claimDate, applicationCode);
            DateTime gregorianDate = DateTime.ParseExact(claimDate, "dd/MM/yyyy", null).AddYears(-543);
            string formattedDate = gregorianDate.ToString("yyyy-MM-dd");
            //Call Api sss_api
            string EndPoint = Properties.Settings.Default.SmileInsurance_Api.Trim() + $"/api/v3/PolicyCoverage/check/policy/filter";
            // Create RestClient
            var client = new RestClient(EndPoint);

            // Create RestRequest
            var request = new RestRequest(Method.GET);
            // Add query parameters
            request.AddQueryParameter("ProductTypeId", productTypeId.ToString());
            request.AddQueryParameter("ClaimDate", formattedDate);
            request.AddQueryParameter("ApplicationCode", applicationCode);

            string token = Properties.Settings.Default.SmileInsuranceToken;
            request.AddHeader("Authorization", $"Bearer {token}");

            // Log parameter object
            var param = new { productTypeId = productTypeId, claimDate = formattedDate, applicationCode = applicationCode };
            Log.Debug("[PH] - [CheckPolicy] Checked Policy PH - [Parameter = {@param}]", param);

            // Send the GET request
            var response = client.Execute(request);
            if (response.IsSuccessful) {
                var apiResponse = JsonConvert.DeserializeObject<SMIResponse>(response.Content);
                // Log response success
                Log.Information("[PH] - [CheckPolicy] Checked Policy PH Success - [Parameter = {@param}]", param);
                return Json(apiResponse.isSuccess, JsonRequestBehavior.AllowGet);
            }
            else {
                // Log error and handle the failure
                Log.Error("[PH] - [CheckPolicy] API call failed. Status code: {StatusCode}, Error: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
                return Json(new { success = false, message = response.ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            

        }
        #endregion


    }
}