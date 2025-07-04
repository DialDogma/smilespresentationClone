using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Wordprocessing;
using SmileClaimV2.Models;
using SmileClaimV2.Helper;
using Serilog;
using RestSharp;
using Newtonsoft.Json;

namespace SmileClaimV2.Controllers {
    [Authorization]
    public class PAController : Controller {
        private readonly CultureInfo dateTH = new CultureInfo("th-TH");
        #region Declare

        //**start set context
        private readonly SSSPAEntities _context;

        private readonly HospitalClaimInformV1Entities _hospitalClaimInform;
        private readonly DataCenterV1Entities _dataCenterV1centerV1;
        private const string _controllerName = nameof(PAController);

        public PAController() {
            _context = new SSSPAEntities();
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

        // GET: School PA
        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019  sub menu ประกันอุบัติเหตุนักเรียน
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPA,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult Index() {
            //ViewBag.Year = DateTime.Now.ToString("yyyy", dateTH);
            return View();
        }

        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019
        //GET: PA Student
        /// <summary>
        ///
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="schoolName"></param>
        /// <param name="productName"></param>
        /// <param name="s">Subdistrict</param>
        /// <param name="d">District</param>
        /// <param name="p">Province</param>
        /// <returns></returns>
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPA,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetPA(string appid, string schoolName, string productName, string s, string d, string p) {
            // decode
            var data = Convert.FromBase64String(appid);
            var appId = Encoding.UTF8.GetString(data);

            //GET QUANTITY STUDENT AND SET TEXT WHEN EMPTY TABLE
            var resultQuantity = _context.usp_CustomerDetailForReportQuantity(appId).FirstOrDefault();
            if (resultQuantity != null) {
                if (resultQuantity.ImportCurrent == resultQuantity.ALL) {
                    ViewBag.TextEmptyTable = "ไม่พบรายชื่อกรุณาสำรองจ่าย";
                }
                else if (resultQuantity.ImportCurrent == 0 || resultQuantity.ImportCurrent != resultQuantity.ALL) {
                    ViewBag.TextEmptyTable = "อยู่ระหว่างนำเข้ารายชื่อกรุณาสำรองจ่าย";
                }
            }
            else {
                ViewBag.TextEmptyTable = "ไม่พบรายชื่อกรุณาสำรองจ่าย";
            }

            //SET VIEWBAG
            ViewBag.AppID = appId;
            ViewBag.School = schoolName;
            ViewBag.SubDistrict = s;
            ViewBag.District = d;
            ViewBag.Province = p;
            ViewBag.Product = productName;

            return View();
        }

        public ActionResult CountClaim(string cusId, string dateIssue) {
            var customer = cusId;
            var datetime = Convert.ToDateTime(dateIssue);
            //Count Claim
            var Count_Result = _context.usp_ClaimPARepeatedlyByCustomerDetail_id_Select(customer, datetime).FirstOrDefault();

            return Json(Count_Result, JsonRequestBehavior.AllowGet);
        }

        //GET:Detail
        //ADD ROLE SmileClaimV2_Operation 12.00 9-4-2019
        //[Authorization(Roles = "Developer,System_Admin,SmileClaimV2_Hospital,SmileClaimV2_Operation")] //Old Role Update 2025-04-29
        [Authorization(Roles = "SmileClaimV2_Hospital,SmileClaimV2_HospitalPA,SmileClaimV2_Operation,System_Admin,Developer")]
        public ActionResult GetDetail(string cusId, string pGroupId) {
            // decode

            byte[] dataCusId = Convert.FromBase64String(cusId);
            cusId = Encoding.UTF8.GetString(dataCusId);
            ViewBag.cusId = cusId;

            byte[] dataPGroupId = Convert.FromBase64String(pGroupId);
            pGroupId = Encoding.UTF8.GetString(dataPGroupId);
            ViewBag.pGroupId = pGroupId;
            //claimType
            ViewBag.listClaimType = _dataCenterV1centerV1.usp_ClaimType_Select(Convert.ToInt32(pGroupId), true).ToList();
            //detail
            ViewBag.Detail = _context.usp_CustomerDetail_PA_Select(cusId).FirstOrDefault();
            ViewBag.EffectiveDate = ViewBag.Detail.EffectiveDate.ToString("dd/MM/yyyy", dateTH);
            ViewBag.StartCoverDate = ViewBag.Detail.StartCoverDate.ToString("dd/MM/yyyy", dateTH);
            ViewBag.EndCoverDate = ViewBag.Detail.EndCoverDate.ToString("dd/MM/yyyy", dateTH);
            ViewBag.InsuranceCompanyId = ViewBag.Detail.InsuranceCompany_id;

            //benefit
            //viewbag != string
            var appId = @ViewBag.Detail.ApplicationID;
            ViewBag.b2006 = _context.usp_BenefitCoverage_ByBenefitID_Select((string)appId, "2006").First(); //2006ค่ารักษาพยาบาลต่อครั้ง

            //Get Except //pg_Id 3 ข้อมูลสำหรับ PA
            ViewBag.DE = _hospitalClaimInform.usp_Except_Select(3, null).ToList();

            //GET LOGIN DETAIL AND GET ROLES USER //ADD 11-4-2562
            //var loginDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            //var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(loginDetail.UserName);

            //Update 2025-05-06
            var cookie = OAuth2Helper.GetCookie();
            var userRole = OAuth2Helper.GetRoles(cookie.Value);

            var roleCallClaim = "SmileClaimV2_Operation";
            ViewBag.IsCallClaim = userRole.Contains(roleCallClaim);

            //remove session HospitalClaimInformPA
            Session.Remove("HospitalClaimInformPA");
            return View();
        }

        [HttpPost]
        public ActionResult CreateClaimInform(FormCollection form) {
            //Create HCI
            var claimTypeId = Convert.ToInt32(form["claim_type_hidden"]);
            var claimAdmitTypeId = Convert.ToInt32(form["claim_admittype_hidden"]);
            var chiefComplainId = Convert.ToInt32(form["chief_complain"]);
            var dateHappen = form["date_happen"];
            var dateIssue = form["date_in"];
            var timeHappen = form["time_happen"];
            var timeIssue = form["time_in"];
            var remark = form["remark"];
            var customerPhoneNo = form["phone_number"];
            var officerDepartment = form["officerDepartment"]; //ADD 2-11-2561
            var officerName = form["officerName"]; //ADD 2-11-2561
            //const int hciStatusId = 2; //สถานะเช็คสิทธิ์
            const int hciStatusId = 4; //สถานะยกเลิกเคลม

            const int OPD_All = 0; //PA ไม่มี OPD
            const int OPD_Used = 0; //PA ไม่มี OPD
            const int OPD_Net = 0; //PA ไม่มี OPD
            const int opdBenefitAll = 0; //PA ไม่มี OPD
            const int remain = 0; //PA ไม่มี OPD
            const int currentuse = 0; //PA ไม่มี OPD

            var hospitalId = GlobalFunction.GetLoginDetail(this.HttpContext).Organize_ID;//Convert.ToInt32(Session["Organize_ID"]);
            var referenceId = form["cusId"];
            var applicationId = form["appId"];
            const int productGroupId = 3; // 3 = PA
            var createdById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID; // Convert.ToInt32(Session["User_ID"]);
            var cause = form["cause"];
            var dHappen = GlobalFunction.ConvertDatePickerToDate_BE(dateHappen, timeHappen);
            var dIssue = GlobalFunction.ConvertDatePickerToDate_BE(dateIssue, timeIssue);
            var year = form["year"];
            var name = _context.usp_CustomerDetail_PA_Select(referenceId).Select(x => x.sName).FirstOrDefault();

            var hospitalClaimInform = new usp_HospitalClaimInform_Insert_Result();
            if (Session["HospitalClaimInformPA"] != null) {
                // ดึงข้อมูล JSON จาก Session และแปลงกลับเป็นอ็อบเจ็กต์ hospitalClaimInform
                hospitalClaimInform = JsonConvert.DeserializeObject<usp_HospitalClaimInform_Insert_Result>(Session["HospitalClaimInformPA"].ToString());
            }
            else {
                hospitalClaimInform = _hospitalClaimInform.usp_HospitalClaimInform_Insert(hospitalId
                                                                                             , hciStatusId
                                                                                             , chiefComplainId
                                                                                             , claimTypeId
                                                                                             , claimAdmitTypeId
                                                                                             , dHappen
                                                                                             , dIssue
                                                                                             , OPD_All
                                                                                             , OPD_Used
                                                                                             , OPD_Net
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

            //Create ClaimPA
            var hospitalCode = GlobalFunction.GetLoginDetail(this.HttpContext).OrganizeCode; //Session["Organize_Code"];
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            var branchCode = _dataCenterV1centerV1.usp_Branch_Select(branchId).First().tempcode;
            var claimHeaderId = _context.usp_ClaimHeader_Insert(referenceId //CustomerDetail_id
                                                                  , hospitalCode.ToString() //HospitalCode **required
                                                                  , hciDetail.ClaimAdmitTypeCode // ClaimType ใน PA คือClaimAdmitType
                                                                  , hciDetail.ClaimTypeCode //ClaimStyle_id ใน PA คือ claimType ******
                                                                  , hciDetail.DateHappen.Value
                                                                  , hciDetail.DateIssue //DateIn **required
                                                                  , hciDetail.DateIssue //DateOut **required //วันเดียวกัน
                                                                  , hciDetail.DateIssue.Value //DateNotice **required
                                                                  , hciCustomerDetail.ProductCode
                                                                  , hciCustomerDetail.InsuranceCompanyCode
                                                                  , false //IsContinueClaim
                                                                  , "" //ContinueFromClaim_id
                                                                  , "" //AccidentCause_id
                                                                  , "" //AccidentDetail
                                                                  , hciDetail.ChiefComplainCode
                                                                  , "" //ICD10_1
                                                                  , "" //ICD10_2
                                                                  , "" //ICD10_3
                                                                  , remark //Remark
                                                                  , "3510" //3510ใช้สิทธิ์แล้วรอจ่าย รพ.
                                                                  , empCode //CreatedBy_id *required
                                                                  , null//ApprovedBy_id *
                                                                  , null  //ApprovedDate
                                                                  , false //IsExgratia
                                                                  , "" //DenyCause_id
                                                                  , 0 //Amount_Total
                                                                  , 0 //Amount_Pay
                                                                  , 0 //Amount_UnPay
                                                                  , 0 //Amount_Compensate_in
                                                                  , 0 //Amount_Compensate_out
                                                                  , 0 //Amount_Compensate
                                                                  , 0 //Amount_Net
                                                                  , "" //Unpay_Remark
                                                                  , null //ClaimheaderGroup_id
                                                                  , branchCode //BranchID (BranchCode)
                                                                  , "" //ClaimPayBy_id
                                                                  , hciDetail.HospitalClaimInformCode
                                                                  , hciCustomerDetail.InsuranceCompanyDetail
                                                                  ).First();

            var checkExistClaimHeaderID = _hospitalClaimInform.HospitalClaimInform
                                        .Where(w => w.ClaimHeaderID == claimHeaderId)
                                        .FirstOrDefault();
            if (checkExistClaimHeaderID != null) {
                // ส่งค่าแจ้งว่า ClaimHeaderID ซ้ำ
                Session["HospitalClaimInformPA"] = JsonConvert.SerializeObject(hospitalClaimInform);
                return Json(new { success = false, duplicateClaim = true, message = "บันทึกข้อมูลไม่สำเร็จ กรุณากดปุ่ม ยืนยันการใช้สิทธิ์ อีกครั้ง" });
            }
            else {
                Session.Remove("HospitalClaimInformPA");
            }
            if (claimHeaderId != "") {
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
                /*TODO: LogError*/
                hospitalClaimInform.HospitalClaimInformID = null;  //return null
                return Json(hospitalClaimInform.HospitalClaimInformID, JsonRequestBehavior.AllowGet);
            }
            try {
                if ((claimHeaderId.Trim() != "") && (hciDetail.ClaimTypeCode.Trim() == "4110") &&
                    (hciDetail.ClaimAdmitTypeCode.Trim() == "4001") &&
                    (hciCustomerDetail.InsuranceCompanyCode.Trim() == "100000000041")) { //เช็คเคลมโรงพยาบาล(วางบิล), ค่ารักษาอุบัติเหตุ,บริษัท สยามสไมล์ ประกันภัย จำกัด (มหาชน)
                    SendSMIRequestPA(claimHeaderId, 5, 5, 11, createdById.ToString(), hciDetail.DateHappen.Value,false);// รหัสเคลม,5 = กดยืนยันการใช้สิทธิ์,5 = HCI OPD, 11 = ส่งท.บ.,ผู้ทำรายการ,วันที่เกิดเหตุ
                }
                return new PHController().SendSMSOPD(dateIssue, name, customerPhoneNo, empCode, hospitalClaimInform.HospitalClaimInformID, _controllerName);

            }
            catch (Exception ex) {
                Log.Error(ex, "{_controllerName} - [CreateClaimInform] Sent SMS OPD Error: {Message}", _controllerName, ex.Message);
                return Json(hospitalClaimInform.HospitalClaimInformID, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult CheckDuplicate(string customerId, string chiefComplainCode, string dateHappen, string dateIn) {
            var customer = customerId;
            var datetime = DateTime.Now;
            var dHappen = GlobalFunction.ConvertDatePickerToDate_BE(dateHappen);
            var dIn = GlobalFunction.ConvertDatePickerToDate_BE(dateIn);

            var chiefComplainId = Convert.ToInt32(chiefComplainCode);
            //Count Claim
            var CustomerCount = _hospitalClaimInform.usp_HCIPA_CheckDuplicate_Select(customer, datetime, chiefComplainId, dHappen, dIn).Count();

            return Json(CustomerCount, JsonRequestBehavior.AllowGet);
        }

        #endregion Action

        #region GetData

        public JsonResult GetDatatableSchool(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string provinceId, int? applicationYear = null) {
            var list = _context.usp_ApplicationSearch_Select(applicationYear, null, provinceId, null, null, pageStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableStudent(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string appId) {
            var list = _context.usp_CustomerSearch_PA_Select(appId, pageStart, pageSize, sortField, orderType, search).ToList();
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
            var list = _context.usp_AccidentCause_Select(pageStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableClaimHistoryPA(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string customerId) {
            var list = _context.usp_ClaimHeader_PA_Select(customerId, pageStart, pageSize, sortField, orderType, search).ToList();
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

        #region Send SMI Request PA
        public JsonResult SendSMIRequestPA(string claimCode, int tPAStatusId, int groupClaimTypeId, int sMIStatusId, string createdByUserCode, DateTime dateHappen, bool isResend) {
            Log.Information("[PA] [SendSMIRequestPA] - Start Send SMI Request PA - Parameter: ClaimCode = {claimCode},TPAStatusId = {tPAStatusId},GroupClaimTypeId = {groupClaimTypeId},SMIStatusId = {sMIStatusId},CreatedByUserCode = {createdByUserCode},DateHappen = {dateHappen}"
                , claimCode, tPAStatusId, groupClaimTypeId, sMIStatusId, createdByUserCode, dateHappen);

            //Call Api ssspa_api
            string EndPoint = Properties.Settings.Default.SSSPA_Api.Trim() + "/Claim/insert/claim/pa";
            var client = new RestClient(EndPoint);
            var param = new { claimCode = claimCode, tPAStatusId = tPAStatusId, groupClaimTypeId = groupClaimTypeId, sMIStatusId = sMIStatusId, createdByUserCode = createdByUserCode, dateHappen = dateHappen, isResend = isResend };
            //Add Json Body to Request
            var request = new RestRequest().AddJsonBody(param);

            //Post Request
            try {
                Log.Debug("[PA] - [SendSMIRequestPA] Sending Request - [Parameter = {@param}]", param);
                var response = client.Post(request);
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                bool isSuccess = jsonResponse?.isSuccess ?? false;
                if (isSuccess) {
                    //Log Success
                    Log.Information("[PA] - [SendSMIRequestPA] Success - [Parameter = {@param}]", param);
                    return Json(claimCode, JsonRequestBehavior.AllowGet);
                }
                else {
                    // Log error and handle the failure
                    throw new Exception($"[PA] - [SendSMIRequestPA] API call failed: {response.StatusCode}, Error: {response.ErrorMessage}");
                }
            }
            catch (Exception ex) {
                Log.Error("[PA] - [SendSMIRequestPA] Exception: {Message}", ex.Message);
                var result = _context.usp_ClaimExportTransactionFail_Insert(claimCode, tPAStatusId, groupClaimTypeId, sMIStatusId, createdByUserCode, dateHappen.Date).FirstOrDefault();
                if (result.IsResult == false) {
                    Log.Error("[PA] - [SendSMIRequestPA] Database insert failed. Result: {Result} Error: {Msg}", result.Result, result.Msg);
                    return Json(new { success = false, message = result.Msg }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Check Policy
        public JsonResult CheckPolicy(int productTypeId, string claimDate, string customerDetailCode) {
            Log.Information("[PA] [CheckPolicy] - Checking Policy PA - Parameter: ProductTypeId = {productTypeId},ClaimDate = {claimDate},CustomerDetailCode = {customerDetailCode}"
                , productTypeId, claimDate, customerDetailCode);
            Log.Debug("[PA] [CheckPolicy] - Checking Policy PA - Parameter: ProductTypeId = {productTypeId},ClaimDate = {claimDate},CustomerDetailCode = {customerDetailCode}"
                , productTypeId, claimDate, customerDetailCode);
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
            request.AddQueryParameter("CustomerDetailCode", customerDetailCode);

            string token = Properties.Settings.Default.SmileInsuranceToken;
            request.AddHeader("Authorization", $"Bearer {token}");

            // Log parameter object
            var param = new { productTypeId = productTypeId, claimDate = formattedDate, customerDetailCode = customerDetailCode };
            Log.Debug("[PA] - [CheckPolicy] Checked Policy PA - [Parameter = {@param}]", param);

            // Send the GET request
            var response = client.Execute(request);
            if (response.IsSuccessful) {
                var apiResponse = JsonConvert.DeserializeObject<SMIResponse>(response.Content);
                // Log response success
                Log.Information("[PA] - [CheckPolicy] Checked Policy PA Success - [Parameter = {@param}]", param);
                return Json(apiResponse.isSuccess, JsonRequestBehavior.AllowGet);
            }
            else {
                // Log error and handle the failure
                Log.Error("[PA] - [CheckPolicy] API call failed. Status code: {StatusCode}, Error: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
                return Json(new { success = false, message = response.ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            

        }
        #endregion

    }
}