using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMiscellaneous.Models;
using SmileSMiscellaneous.Helper;
using System.Net;
using SmileSMiscellaneous.PremiumOrder;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;

//using SmileSMiscellaneous.Helper

namespace SmileSMiscellaneous.Controllers
{
    [Helper.Authorization]
    public class ApplicationController : Controller
    {
        private MiscellaneousDBContext _db;

        public ApplicationController()
        {
            _db = new MiscellaneousDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: Application
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewApp(string appId = null, string appCode = null, string IGCode = null,string leadID = null)
        {
            int? d_AppId = null;
            string d_igCode = null;
            var EmployeeCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            if (appCode != null)
            {
                //DeCode
                var appCodeBase64EncodedBytes = Convert.FromBase64String(appCode);
                var appCodeStr = System.Text.Encoding.UTF8.GetString(appCodeBase64EncodedBytes);
                
                var rs = _db.usp_Application_Select(null, appCodeStr, 0, 999, null, null, null).Single();

                d_AppId = rs.ApplicationId;              
            }

            if (appId != null)
            {
                //DeCode
                var appIDBase64EncodedBytes = Convert.FromBase64String(appId);
                var appIdStr = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);

                d_AppId = Int32.Parse(appIdStr);
            }

            if (IGCode != null)
            {
                //DeCode
                var igCodeBase64EncodedBytes = Convert.FromBase64String(IGCode);
                d_igCode = System.Text.Encoding.UTF8.GetString(igCodeBase64EncodedBytes);

            }


            ViewBag.IGCode = d_igCode;
            ViewBag.AppId = d_AppId;
            ViewBag.ProductType = _db.usp_ProductType_Select(0, 999, null, null, null).ToList();
            ViewBag.EmployeeCode = EmployeeCode;
            ViewBag.LeadCode = leadID;

            return View();
        }

        public ActionResult DetailApp(string appId = null)
        {
            //DeCode
            var appIDBase64EncodedBytes = Convert.FromBase64String(appId);
            var deCode_AppID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);
            var EmployeeCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            
            int d_AppId = Convert.ToInt32(deCode_AppID);

            ViewBag.AppId = d_AppId;
            ViewBag.EmployeeCode = EmployeeCode;

            return View();
        }

        public ActionResult SyncCashReceive(string appId = null)
        {
            return View();
        }
        public ActionResult GetDataApplicationFull(int? appId = null)
        {
            var rs = _db.usp_ApplicationFullDetail_Select(appId, 0, 999, null, null, null).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataApplication(int? appId = null)
        {
            var rs = _db.usp_Application_Select(appId, null, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataCustomer(int? appId = null)
        {
            int c;

            c = _db.usp_ApplicationInsured_Select(appId, 0, 999, null, null, null).Count();

            if (c > 0)
            {
                var rs = _db.usp_ApplicationInsured_Select(appId, 0, 999, null, null, null).Single();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAddress(int? appId = null, int? addressTypeId = null)
        {
            int c;

            c = _db.usp_Address_Select(appId, addressTypeId, 0, 999, null, null, null).Count();

            if (c > 0)
            {
                var rs = _db.usp_Address_Select(appId, addressTypeId, 0, 999, null, null, null).Single();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDataPayer(int? appId = null)
        {
            int c;

            c = _db.usp_ApplicationPayer_Select(appId, 0, 999, null, null, null).Count();

            if (c > 0)
            {
                var rs = _db.usp_ApplicationPayer_Select(appId, 0, 999, null, null, null).Single();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDataHeir(int? appId = null, int? draw = null, int? pageStart = null, int? pageSize = null,
                                             string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ApplicationHeir_Select(appId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDocument(int? appId = null, string documentTypeIdList = null, string remark = null, int? draw = null, int? pageStart = null, int? pageSize = null,
                                             string sortField = null, string orderType = null, string search = null)
        {
            int c;

            c = _db.usp_Document_Select(appId, documentTypeIdList, pageStart, pageSize, sortField, orderType, search).Count();
            if (c == 0)
            {
                var userid = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var i_rs = _db.usp_Document_Insert(appId, documentTypeIdList, remark, userid).Single();

                if (i_rs.IsResult == false)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            var result = _db.usp_Document_Select(appId, documentTypeIdList, pageStart, pageSize, sortField, orderType, search).ToList().OrderBy(x => x.DocumentTypeId);

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SentApplication(int? appId = null)
        {
            int createUserId;
            createUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_Application_Send_Update(appId, createUserId, null).Single();

            if (rs.IsResult == true)
            {
                //Insert Log
                var rs_log = InsertTransaction(Convert.ToInt32(appId), 80);     //80 ส่งตรวจ Application
            }

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PremiumReceiveByMatch()
        {
            int createUserId;
            createUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_PremiumReceiveByMatch_Insert(createUserId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public usp_ApplicationTransaction_Insert_Result InsertTransaction(int applicationId, int transactionTypeId)
        {
            string jsonOutput;
            int createUserId;

            createUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            jsonOutput = new JsonLog().GetJsonOutputByApplicationId(applicationId);

            var result = _db.usp_ApplicationTransaction_Insert(applicationId, transactionTypeId, createUserId, jsonOutput).Single();

            return result;
        }

        [HttpPost]
        public ActionResult InsertOrUpdateApplication(FormCollection form)
        {
            int? applicationId = null;
            int? productId = null;
            int? countCustomer = null;
            Double sumPremiumAmount;
            DateTime? d_startCoverDate;
            DateTime? d_endCoverDate;
            int coveragePeriod;
            int? agentUserId;
            int zebraId;
            int createUserId;
            string inCode = null;
            string billNo = null;

            if (form["hdfappID_policy"] != null && form["hdfappID_policy"] != "")
            {
                applicationId = Convert.ToInt32(form["hdfappID_policy"]);
            }

            inCode = form["txtInCode"].ToString();
            productId = Convert.ToInt32(form["ddlProduct"]);
            countCustomer = Convert.ToInt32(form["txtCountCustomer"]);
            sumPremiumAmount = Convert.ToDouble(form["txtPremiumAmount"]);
            d_startCoverDate = GlobalFunction.ConvertDatePickerToDate_BE(form["dtpStartCoverDate"]);
            d_endCoverDate = GlobalFunction.ConvertDatePickerToDate_BE(form["dtpEndCoverDate"]);
            coveragePeriod = Convert.ToInt32(form["txtDuration"]);
            agentUserId = _db.usp_UserIDByEmployeeCode_Select(form["ddlAgnt"]).Single();
            zebraId = Convert.ToInt32(form["ddlZebra"]);
            createUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            billNo = form["txtBillNo"].ToString();

            usp_Application_InsertOrUpdate_Result rs = new usp_Application_InsertOrUpdate_Result();

            rs = _db.usp_Application_InsertOrUpdate(applicationId, productId, countCustomer, sumPremiumAmount, d_startCoverDate, d_endCoverDate, coveragePeriod,
                                                        agentUserId, zebraId, createUserId, null, inCode, billNo).Single();

            if (applicationId == null)
            {
                if (rs.IsResult == true)
                {
                    //Insert Log
                    var rs_log = InsertTransaction(Convert.ToInt32(rs.Result), 79);     //79 สร้าง Application ใหม่
                }
            }

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertOrUpdateCustomer(FormCollection form)
        {
            int? applicationId = null;
            int? title = null;
            string fName = null;
            string lName = null;
            DateTime? d_birthDate = null;
            string idCard = null;
            string passport = null;
            string mobilePhone = null;
            string remark = null;
            int createdUserId;
            int addressTypeId = 2; //ผู้เอาประกัน
            string address1 = null;
            string address2 = null;
            string subDistrictId = null;
            string zipCode = null;

            applicationId = Convert.ToInt32(form["hdfappID_Customer"]);
            title = Convert.ToInt32(form["ddlTitle_C"]);
            fName = form["txtFname_C"];
            lName = form["txtLname_C"];

            string dbirthDate = form["txtAgeCustomer"];

            if (dbirthDate != null && dbirthDate != "")
            {
                d_birthDate = GlobalFunction.ConvertDatePickerToDate_BE(form["txtAgeCustomer"]);
            }

            idCard = form["txtZcardCustomer"];

            if (idCard != null || idCard != "")
            {
                idCard = idCard.Replace("-", "");
            }

            passport = form["txtPassCustomer"];
            mobilePhone = form["txtTelCustomer"];

            if (mobilePhone != null || mobilePhone != "")
            {
                mobilePhone = mobilePhone.Replace("-", "");
            }
            remark = form["txtRemarkCust"];

            createdUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            address1 = form["txtAddress_Cust1"];
            address2 = form["txtAddress_Cust2"];
            subDistrictId = form["ddlSubDistrict_Cust"];
            zipCode = form["txtZipCode_Cust"];

            var rs = _db.usp_CustomeMember_InsertOrUpdate(applicationId, title, fName, lName, d_birthDate, idCard, passport, mobilePhone, remark, createdUserId).Single();

            if (rs.IsResult == false)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rs_address = _db.usp_Address_InsertOrUpdate(applicationId, addressTypeId, address1, address2, subDistrictId, zipCode, createdUserId).Single();
                return Json(rs_address, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InsertOrUpdatePayer(FormCollection form)
        {
            int? applicationId = null;
            int? title = null;
            string fName = null;
            string lName = null;
            string idCard = null;
            string passport = null;
            string mobilePhone = null;
            bool isSamePayer;
            int createdUserId;
            int addressTypeId = 3; //ผู้ชำระเบี้ย
            string address1 = null;
            string address2 = null;
            string subDistrictId = null;
            string zipCode = null;

            applicationId = Convert.ToInt32(form["hdfappID_payer"]);
            title = Convert.ToInt32(form["ddlTitle_P"]);
            fName = form["txtFname_P"];
            lName = form["txtLname_P"];
            idCard = form["txtZcardPayer"];

            if (idCard != null || idCard != "")
            {
                idCard = idCard.Replace("-", "");
            }

            passport = form["txtPassPayer"];
            mobilePhone = form["txtTelPayer"];

            if (mobilePhone != null || mobilePhone != "")
            {
                mobilePhone = mobilePhone.Replace("-", "");
            }

            if (form["hdfIsSamePayer"] == "0")
            {
                isSamePayer = false;
            }
            else
            {
                isSamePayer = true;
            }

            createdUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            address1 = form["txtAddress_Payer1"];
            address2 = form["txtAddress_Payer2"];
            subDistrictId = form["ddlSubDistrict_Payer"];
            zipCode = form["txtZipCode_Payer"];

            var rs = _db.usp_ApplicationPayer_InsertOrUpdate(applicationId, title, fName, lName, idCard, passport, mobilePhone, isSamePayer, createdUserId).Single();

            if (rs.IsResult == true)
            {
                var rs_address = _db.usp_Address_InsertOrUpdate(applicationId, addressTypeId, address1, address2, subDistrictId, zipCode, createdUserId).Single();
                return Json(rs_address, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InsertHeir(int appId, int title, string fName = null, string lName = null, string idCard = null, string passport = null, int? relationTypeId = null)
        {
            int createdUserId;
            createdUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            string IdCard = null;

            if (idCard != null || idCard != "")
            {
                IdCard = idCard.Replace("-", "");
            }

            var rs = _db.usp_ApplicationHeir_Insert(appId, title, fName, lName, IdCard, passport, null, relationTypeId, createdUserId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteHeir(int applciationHeirId)
        {
            int createdUserId;
            createdUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_ApplicationHeir_Delete(applciationHeirId, createdUserId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataApplicationByINCode(string inCode = null)
        {
            var rs = _db.usp_ApplicationByINCode_Select(inCode).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPremiumOrderByIgCode(string igCode)
        {
            var order = new Order();
            var apiUrl = Properties.Settings.Default.PremiumOrderAPIURL;
            string url = String.Format(apiUrl + "Order/GetNewOrderByIGCode?igCode={0}", igCode);

            try
            {
                var client = new RestClient();
                var request = new RestRequest(url);

                // request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");

                var response = client.ExecuteAsync(request);
                order = JsonConvert.DeserializeObject<Order>(response.Result.Content);

                return Json(order.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetPremiumPayerByINCode(string inCode)
        {
            var payer = new Payer();
            var apiUrl = Properties.Settings.Default.PremiumOrderAPIURL;
            string url = String.Format(apiUrl + "Order/GetPayerByINCode?inCode={0}", inCode);
            
            try
            {
                var client = new RestClient();
                var request = new RestRequest(url);

             // request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");

                var response = client.ExecuteAsync(request);
                payer = JsonConvert.DeserializeObject<Payer>(response.Result.Content);

                return Json(payer.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PDPAPostData(string appId = null, string Mode = null)
        {            
            var EmployeeCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            int d_AppId = Convert.ToInt32(appId);
            var rs = _db.usp_ApplicationFullDetail_Select(d_AppId, 0, 999, null, null, null).Single();
            var Title = _db.usp_Title_Select(rs.PayerTitleID, null, null, null, null,null,null).Single();
            var PDPAUrl = Properties.Settings.Default.PDPAURL;
            
            ViewBag.AppId = d_AppId;
            ViewBag.Url = PDPAUrl;

            ViewBag.ProjectId = 18;
            ViewBag.ApplicationCode = rs.ApplicationCode;
            ViewBag.ApplicationStatusId = rs.ApplicationStatusId;
            ViewBag.ApplicationStatusName = rs.ApplicationStatus;
            ViewBag.CustFirstName = rs.CustTitle + rs.CustFirstName;
            ViewBag.CustLastName = rs.CustLastName;
            ViewBag.CustomerIdentitycardNo = (rs.CustIdCardNumber == null || rs.CustIdCardNumber == "" ? rs.CustPassportNumber : rs.CustIdCardNumber);
            ViewBag.PayerFirstName = Title.TitleDetail + rs.PayerFirstName;
            ViewBag.PayerLastName = rs.PayerLastName;
            ViewBag.PayerIdentitycardNo = (rs.PayerIdCardNumber == "" || rs.PayerIdCardNumber == null ? rs.PayerPassportNumber : rs.PayerIdCardNumber);                        
            ViewBag.IsSamePerson =  (rs.IsSameCustomer  == true ? "true" :"false");   
            ViewBag.Mode = Mode;
            ViewBag.EmployeeCode = EmployeeCode;

            return View();
        }


        public JsonResult GetPDPAByAppId(string AppCode)
        {        
            //------------------------getApi
            var apiUrl = Properties.Settings.Default.PDPAApiURL;
            string url = String.Format(apiUrl + "/Home/smilepdpaexist?applicationcode={0}", AppCode); 
            try
            {
                var model = new responsemodel();
                var client = new RestClient();
                var request = new RestRequest(url);           
                var response = client.ExecuteAsync(request);
                model = JsonConvert.DeserializeObject<responsemodel>(response.Result.Content);             
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPDPADateByID (string appid)
        {
            int d_AppId = Convert.ToInt32(appid);
            var rs = _db.usp_ApplicationFullDetail_Select(d_AppId, 0, 999, null, null, null).Single();
             return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCheckDatePDPA()
        {
            var Paramertername = "Check_PDPA_CreatedDate";
            var rs = _db.usp_ProgramConfig_Select(Paramertername, 0, 999, null, null, null).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoSaveCRMLead(string AppCode,int ProjectId , string LeadCode)
        {
            bool rs;
            try
            {
                if (string.IsNullOrEmpty(LeadCode))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

               _db.usp_CRMLead_Insert(AppCode, ProjectId, LeadCode);
                rs = true;
               
            }
            catch (Exception)
            {
                rs = false;
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

    }
}