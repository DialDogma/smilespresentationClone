using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSPACommunity.Controllers
{
    [Authorization]
    public class PACommunityController : Controller
    {
        private int c_numberic = 0;
        private int c_string = 0;

        private PACommunityEntities _db;

        public PACommunityController()
        {
            _db = new PACommunityEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        private string CheckNull(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return "";
            }
        }

        public ActionResult PACommunityDetail(string applicationID = null)
        {
            int? d_applicationid;

            //deCode
            var appIDBase64EncodedBytes = Convert.FromBase64String(applicationID);
            var deCode_appID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);

            d_applicationid = Convert.ToInt32(deCode_appID);

            ViewBag.ApplicationId = d_applicationid;
            ViewBag.LinkCashReceive = Properties.Settings.Default.CashReceive_Payment_Path;

            return View();
        }

        // GET: PACommunity
        public ActionResult AddCustomerDetail(string ApplicationRoundId, string CustomerDetailId = null)
        {
            int? d_appRoundID;
            int? d_customerDatailID = null;

            //deCode
            var appRoundIDBase64EncodedBytes = Convert.FromBase64String(ApplicationRoundId);
            var deCode_appRoundID = System.Text.Encoding.UTF8.GetString(appRoundIDBase64EncodedBytes);

            d_appRoundID = Convert.ToInt32(deCode_appRoundID);

            if (CustomerDetailId != "null")
            {
                //deCode
                var CustDetailIDBase64EncodedBytes = Convert.FromBase64String(CustomerDetailId);
                var deCode_CustDetailID = System.Text.Encoding.UTF8.GetString(CustDetailIDBase64EncodedBytes);

                d_customerDatailID = Convert.ToInt32(deCode_CustDetailID);
            }

            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.ApplicationRoundId = d_appRoundID;
            ViewBag.CustomerDetailId = d_customerDatailID;
            ViewBag.Titles = _db.usp_Title_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.Occupation = _db.usp_Occupation_Select(null, 0, 999, null, null, null).ToList();

            return View();
        }

        public ActionResult ControlCustomerDetail(string applicationRoundId)
        {
            //deCode
            var appRoundIDBase64EncodedBytes = Convert.FromBase64String(applicationRoundId);
            var deCode_appRoundID = System.Text.Encoding.UTF8.GetString(appRoundIDBase64EncodedBytes);

            int? d_AppRoundId = Convert.ToInt32(deCode_appRoundID);
            ViewBag.ApplicationRoundID = d_AppRoundId;
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            return View();
        }

        public ActionResult PACommunityMonitor()
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
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
                ViewBag.Role = rolelist;
                ViewBag.AppStatus = _db.usp_ApplicationStatus_Select(null, 0, 999, null, null, null).ToList();
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            //ถ้าเป็น สิทธิ์ของ MO ให้แสดงแค่สาขาตัวเอง
            else if (rolelist.Intersect(roleMO).Count() > 0)
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Role = rolelist;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
                ViewBag.AppStatus = _db.usp_ApplicationStatus_Select(null, 0, 999, null, null, null).ToList();
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            else
            {
                return RedirectToAction("UnAuthorize", "Auth");
            }
        }

        public ActionResult PAMonitorUnapprove()
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
                ViewBag.AppStatus = _db.usp_ApplicationStatus_Select(null, 0, 999, null, null, null).ToList();
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            //ถ้าเป็น สิทธิ์ของ MO ให้แสดงแค่สาขาตัวเอง
            else if (rolelist.Intersect(roleMO).Count() > 0)
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Role = rolelist;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
                ViewBag.AppStatus = _db.usp_ApplicationStatus_Select(null, 0, 999, null, null, null).ToList();
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            else
            {
                return RedirectToAction("UnAuthorize", "Auth");
            }
        }

        public ActionResult AddPACoumunityNewApp(string appID = null)
        {
            int? d_AppId;

            if (appID == null)
            {
                d_AppId = null;

                ViewBag.BranchID = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branch = GlobalFunction.GetLoginDetail(this.HttpContext).BranchDetail;
            }
            else
            {
                //deCode
                var appIDBase64EncodedBytes = Convert.FromBase64String(appID);
                var deCode_AppID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);

                d_AppId = Convert.ToInt32(deCode_AppID);
            }

            ViewBag.AppID = d_AppId;
            ViewBag.ContractAgentType = _db.usp_ApplicationAgentType_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.Province = _db.usp_Province_Select(null, 0, 999, null, null, null).ToList();
            //ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.OrganizeGroup = _db.usp_OrganizeGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.Titles = _db.usp_Title_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.Products = _db.usp_Product_Select(null, 0, 999, null, null, null).ToList();

            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.OrganizeBank = _db.usp_OrganizeBank_Select(null, 0, 9999, null, null, null).ToList();

            return View();
        }

        public ActionResult ApplicationSearch()
        {
            return View();
        }

        /// <summary>
        /// นำเข้าเลขกรมธรรม์
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportPolicyNumber()
        {
            return View();
        }

        /// <summary>
        /// บันทึกเลขกรมธรรม์
        /// </summary>
        /// <returns></returns>
        public ActionResult PolicyNumberManagement()
        {
            //select province
            ViewBag.Provinces = _db.usp_Province_Select(null, 0, 99, null, null, null).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult ImportCustomerDetail(string applicationRoundId)
        {
            //deCode
            var appRoundIDBase64EncodedBytes = Convert.FromBase64String(applicationRoundId);
            var deCode_appRoundID = System.Text.Encoding.UTF8.GetString(appRoundIDBase64EncodedBytes);

            int? d_AppRoundId = Convert.ToInt32(deCode_appRoundID);

            ViewBag.ApplicationRoundID = d_AppRoundId;
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            //GetApplicationRound to viewbag

            return View();
        }

        public JsonResult GetRoundByApplicationId(int applicatonId)
        {
            var rs = _db.usp_ApplicationRound_Select(applicatonId, null, null, null, 0, 999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeType(int organizeGroupId)
        {
            var rs = _db.usp_OrganizeType_Select(null, organizeGroupId, 0, 9999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDetail(int applicatonId, int? applicatonRoundId = null, int? CustomerDetailId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_CustomerDetail_Select(CustomerDetailId, applicatonId, null, applicatonRoundId, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerByCustomerid(int CustomerDetailId)
        {
            var rs = _db.usp_CustomerDetail_Select(CustomerDetailId, null, null, null, null, null, null, null, null, null).Single();
            var i = rs.ApplicationCode;
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoDeleteCustomerDetail(int CustomerDetailId)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_CustomerDetail_Delete(CustomerDetailId, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoDeleteCustomerByApplicationRoundId(int ApplicationRoundId)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_CustomerDetail_DeleteByApplicationRoundId(ApplicationRoundId, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoUpdateCustomerDetail(int CustomerDetailId
          , int TitleId, string FirstName, string LastName
          , string IdCardNumber, string PassPortNumber, string BirthDate
          , int AgeAtRegister_Y, int AgeAtRegister_M, int AgeAtRegister_D
          , int OccupationId, string MobileNumber, int CustomerDetailTypeId
          )
        {
            DateTime? a_BirthDate = GlobalFunction.ConvertDatePickerToDate_BE(BirthDate);
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_CustomerDetail_Update(CustomerDetailId, TitleId, FirstName, LastName,
                IdCardNumber, PassPortNumber, a_BirthDate
                , AgeAtRegister_D, AgeAtRegister_M, AgeAtRegister_Y
                , OccupationId, MobileNumber, CustomerDetailTypeId, userID
                ).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoSaveCustomerDetail(int ApplicationRoundId
            , int TitleId, string FirstName, string LastName
            , string MobileNumber, string IdCardNumber, string PassPortNumber
            , int OccupationId, int CustomerDetailTypeId, string BirthDate
    )
        {
            var tmCode = new ObjectParameter("Result", typeof(string));

            using (var db = new PACommunityEntities())
            {
                //Gen Code Tmp
                db.usp_GenerateCode("ISCD", 6, tmCode);
            }

            var tmp_Code = tmCode.Value.ToString();

            DateTime? a_BirthDate = GlobalFunction.ConvertDatePickerToDate_BE(BirthDate);
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_CustomerDetail_Insert(ApplicationRoundId, tmp_Code, TitleId, FirstName
                , LastName, MobileNumber, IdCardNumber, PassPortNumber
                , OccupationId, CustomerDetailTypeId, a_BirthDate, userID
                ).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalculateAgeXProduct(string DateFrom, string DateTo, int ProductId)
        {
            var lstArr = new string[4];

            DateTime? a_DateFrom = GlobalFunction.ConvertDatePickerToDate_BE(DateFrom);
            DateTime? a_DateTo = GlobalFunction.ConvertDatePickerToDate_BE(DateTo);

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_CalculateAgeXProduct_Select(a_DateFrom, a_DateTo, ProductId
                ).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalculateAge(string DateFrom, string DateTo)
        {
            DateTime? a_DateFrom = GlobalFunction.ConvertDatePickerToDate_BE(DateFrom);
            DateTime? a_DateTo = GlobalFunction.ConvertDatePickerToDate_BE(DateTo);
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_CalculateAge_Select(a_DateFrom, a_DateTo).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductByID(int productId)
        {
            var rs = _db.usp_Product_Select(productId, 0, 1, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationRoundByID(int applicationRoundID)
        {
            var rs = _db.usp_ApplicationRound_SelectById(applicationRoundID, 0, 99, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoSendPolicy(int applicationRoundId)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_ApplicationRound_SentPolicy_Update(applicationRoundId, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationDetailMonitor(string applicationStatusId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, string branchID = null)
        {
            int? StatusId = null;
            if (applicationStatusId != "-1" && applicationStatusId != null)
            {
                StatusId = Convert.ToInt32(applicationStatusId);
            }

            int? i_branchId = null;

            if (branchID != "-1" && branchID != null)
            {
                i_branchId = Convert.ToInt32(branchID);
            }

            var result = _db.usp_Application_Select(null, i_branchId, StatusId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PDPAExistByApplicationID(string ApplicationID)
        {
            var result = GlobalFunction.CheckSmilePDPAExist(ApplicationID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDetailByName(string customerName = null, string idCardOrPassport = null
                                                    , int? draw = null, int? pageSize = null, int? pageStart = null
                                                 , string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_CustomerDetail_SelectByName(customerName == "" ? null : customerName, idCardOrPassport == "" ? null : idCardOrPassport, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoSaveApplicationDetail(int companyId, string applicationName, string startCoverDate, string endCoverDate,
                                                    int titleId, string firstName, string lastName, string mobileNumber, int productId,
                                                    int branchId, int bankId, string accountName, string accountNo, bool isContact, string idCard, string passport)
        {
            //var result = "";
            var lstArr = new string[3];

            string t;

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var a_startDate = GlobalFunction.ConvertDatePickerToDate_BE(startCoverDate);
            var a_endDate = GlobalFunction.ConvertDatePickerToDate_BE(endCoverDate);

            t = mobileNumber.Replace("-", "");

            var result = _db.usp_ApplicationDetail_InsertV2(companyId, applicationName, a_startDate
                                                            , a_endDate, productId, branchId, titleId
                                                            , firstName, lastName, t, bankId, accountName
                                                            , accountNo, idCard, passport, isContact
                                                            , userID).Single();

            if (result.IsResult == true)
            {
                var rs_CustomerApplication = _db.usp_CustomerApplication_PAC_Organize_InsertOrUpdate(Convert.ToInt32(result.Result), userID);
            }

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoSaveApplicationRound(int applicationID, int customerBuy, int customerFree, string effectiveDate, double premiumSum)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var e_effectiveDate = GlobalFunction.ConvertDatePickerToDate_BE(effectiveDate);

            var result = _db.usp_ApplicationRound_Insert(applicationID, customerBuy, customerFree, premiumSum, e_effectiveDate, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoUpdateApplicationRound(int applicationRoundID, int customerBuy, int customerFree, string effectiveDate, double premiumSum)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var e_effectiveDate = GlobalFunction.ConvertDatePickerToDate_BE(effectiveDate);

            //e_effectiveDate = e_effectiveDate.Value.AddYears(543);

            var result = _db.usp_ApplicationRound_Update(applicationRoundID, customerBuy, customerFree, premiumSum, e_effectiveDate, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeDetail(string q)
        {
            var rs = _db.usp_Employee_Select(null, null, 0, 10, null, null, q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganize(string q, int? organizeTypeId, string provinceId, int? organizeGroupId)
        {
            //PACommunityEntities db = new PACommunityEntities();
            var result = _db.usp_Organize_Select(null, organizeGroupId == -1 ? null : organizeGroupId, organizeTypeId == -1 ? null : organizeTypeId, provinceId, 0, 10, null, null, q).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeById(int organizeID)
        {
            var result = _db.usp_Organize_Select(organizeID, null, null, null, 0, 10, null, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationDetail(int applicationID)
        {
            usp_Application_Select_Result rs = new usp_Application_Select_Result();

            var c = _db.usp_Application_Select(applicationID, null, null, 0, 1, null, null, null).Count();

            if (c == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = _db.usp_Application_Select(applicationID, null, null, 0, 1, null, null, null).Single();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetContractCommunity(int applicationID)
        {
            usp_ApplicationContact_Select_Result rs = new usp_ApplicationContact_Select_Result();

            var c = _db.usp_ApplicationContact_Select(applicationID, null, null, 0, 1, null, null, null).Count();

            if (c == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = _db.usp_ApplicationContact_Select(applicationID, null, null, 0, 1, null, null, null).Single();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerDetailByTmpCodeCount(string tmpCode)
        {
            var rs = _db.usp_TmpApplicationRoundImportCount_Select(tmpCode, 0, 1, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProgramConfig(string parameterName)
        {
            var result = _db.usp_ProgramConfig_Select(parameterName, 0, 1, null, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContractProject(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null)
        {
            var result = _db.usp_ApplicationAgent_Select(applicationID, null, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoSaveContractAgent(int applicationID, int ContractProjectTypeID, int employeeID, double percent)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_ApplicationAgent_Insert(applicationID, ContractProjectTypeID, percent, employeeID, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoDeleteContractAgent(int applicationAgentId)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_ApplicationAgent_Delete(applicationAgentId, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoEditApplicationDetail(int applicationId, int companyId, string applicationName, int applicationStatusID,
                                                    string startCoverDate, string endCoverDate, int titleId, string firstName, string lastName,
                                                    string mobileNumber, int productId, int branchId,
                                                    int bankId, string accountName, string accountNo, bool isContact, string idCard, string passport)
        {
            var lstArr = new string[3];

            string t;

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var a_startDate = GlobalFunction.ConvertDatePickerToDate_BE(startCoverDate);
            var a_endDate = GlobalFunction.ConvertDatePickerToDate_BE(endCoverDate);

            t = mobileNumber.Replace("-", "");

            var result = _db.usp_ApplicationDetail_UpdateV2(applicationId, companyId, applicationName
                                                            , applicationStatusID, a_startDate
                                                            , a_endDate, productId, branchId
                                                            , titleId, firstName, lastName
                                                            , t, bankId, accountName, accountNo
                                                            , idCard, passport, isContact
                                                            , userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null)
        {
            var result = _db.usp_Document_Select(applicationID, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHistoryApplicationDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null)
        {
            var result = _db.usp_ApplicationTransactionHeader_Select(applicationID, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHistoryImportCustomerDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null)
        {
            var result = _db.usp_ApplicationRoundImport_Select(applicationID, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationRoundDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null)
        {
            var result = _db.usp_ApplicationRound_Select(applicationID, null, null, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPremiumDetail(int? applicationID, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_Premium_Select(applicationID, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationRoundDetailFixUnApprove(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null, int? branchId = null)
        {
            var result = _db.usp_ApplicationRound_Select(applicationID
                , branchId, "5", null, pageStart, pageSize, sortField, orderType, search)
                .ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationSubRoundDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationRoundID = null)
        {
            var result = _db.usp_ApplicationSubRound_Select(applicationRoundID, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportCustomerDetail(HttpPostedFileBase file, int applicatonRoundId)
        {
            var lst = GetExcelImportCustomerDetail(file);

            //var db = new PACommunityEntities();
            var lst_detail = lst.Item1;

            var lstArr = new string[3];
            var lstValidate = new string[4];

            var tmCode = new ObjectParameter("Result", typeof(string));

            using (var db = new PACommunityEntities())
            {
                //Gen Code Tmp
                db.usp_GenerateCode("IMCD", 6, tmCode);
            }

            var tmp_Code = tmCode.Value.ToString();

            //Loop Insert Data
            foreach (var item in lst_detail)
            {
                try
                {
                    var rs = _db.usp_TmpApplicationRoundImport_Insert(applicatonRoundId, tmp_Code, item.title
                        , item.firstName, item.lasttName, item.moblienumber, item.zCardID, item.passport, item.occupation
                        , item.customerDetailType, Convert.ToInt32(item.birthdate_Day), Convert.ToInt32(item.birthdate_Month)
                        , Convert.ToInt32(item.birthdate_Year)).Single();

                    lstArr[0] = rs.IsResult.Value.ToString();
                    lstArr[1] = rs.Result.ToString();
                    lstArr[2] = rs.Msg.ToString();

                    if (lstArr[0] == "False")
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }

            //Insert Validate
            var rs_validate = _db.usp_TmpApplicationRoundImport_Validate(applicatonRoundId, tmp_Code).Single();

            lstValidate[0] = rs_validate.IsResult.Value.ToString();
            lstValidate[1] = rs_validate.Result.ToString();
            lstValidate[2] = rs_validate.Msg.ToString();
            lstValidate[3] = tmp_Code;

            return Json(lstValidate, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTmpCustomerDetailOverView(int applicatonRoundId, string tmpCode, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_TmpApplicationRoundImport_OverView_Select(applicatonRoundId, tmpCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTmpCustomerDetailError(int applicatonRoundId, string tmpCode, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_TmpApplicationRoundImport_Error_Select(applicatonRoundId, tmpCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoConfirmImportCustomerDetail(int applicatonRoundId, string tmpCode)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_TmpApplicationRoundImport_ToCustomerDetail_Insert(applicatonRoundId, tmpCode, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoDeleteCustomerDetailImport(int applicatonRoundId, string tmpCode)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_TmpApplicationRoundImport_Delete(applicatonRoundId, tmpCode).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidateforImport(HttpPostedFileBase file)
        {
            var lst = GetExcelImportCustomerDetail(file);

            var lst_detail = lst.Item1;

            string result = "";

            int c = 1;

            foreach (var item in lst_detail)
            {
                c += 1;
                if (item.firstName.Count() > 250)
                {
                    result = "รบกวนตรวจสอบชื่อ บรรทัดที่ " + c;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (item.lasttName.Count() > 250)
                {
                    result = "รบกวนตรวจสอบนามสกุล บรรทัดที่ " + c;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (item.zCardID == "" && item.passport == "")
                {
                    result = "รบกวนตรวจสอบเลขบัตรประชาชน หรือ Passport ต้องใส่อย่างใดอย่างนึง บรรทัดที่ " + c;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (item.zCardID != "")
                {
                    if (IsNumeric(item.zCardID) == false)
                    {
                        result = "รบกวนตรวจสอบเลขบัตรประชาชน ต้องเป็นตัวเลขเท่านั้น บรรทัดที่ " + c;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (item.zCardID.Count() != 13)
                        {
                            result = "รบกวนตรวจสอบเลขบัตรประชาชน ต้องครบ 13 หลัก บรรทัดที่ " + c;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                if (IsNumeric(item.birthdate_Day) == false)
                {
                    result = "รบกวนตรวจสอบวันเกิด ต้องเป็นตัวเลขเท่านั้น บรรทัดที่ " + c;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (IsNumeric(item.birthdate_Month) == false)
                {
                    result = "รบกวนตรวจสอบเดือนเกิด ต้องเป็นตัวเลขเท่านั้น บรรทัดที่ " + c;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (IsNumeric(item.birthdate_Year) == false)
                {
                    result = "รบกวนตรวจสอบปีเกิด ต้องเป็นตัวเลขเท่านั้น บรรทัดที่ " + c;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (item.occupation == "")
                {
                    result = "รบกวนตรวจสอบอาชีพ บรรทัดที่ " + c;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (item.moblienumber != "")
                {
                    if (IsNumeric(item.moblienumber) == false)
                    {
                        result = "รบกวนตรวจสอบเบอร์โทร ต้องเป็นตัวเลขเท่านั้น บรรทัดที่ " + c;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (item.moblienumber.Count() != 10)
                        {
                            result = "รบกวนตรวจสอบเบอร์โทร ต้องครบ 10 หลัก บรรทัดที่ " + c;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public Tuple<List<ImportCustomerDetail>> GetExcelImportCustomerDetail(HttpPostedFileBase file)
        {
            List<ImportCustomerDetail> detaillist = new List<ImportCustomerDetail>();

            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {
                        if (ws.Cells[ri, 1].Value == null)
                        {
                            ws.DeleteRow(nr);
                        }
                        else
                        {
                            detaillist.Add(new ImportCustomerDetail()
                            {
                                title = ws.Cells[ri, 1].Value.ToString(),
                                firstName = CheckNull(ws.Cells[ri, 2].Value),
                                lasttName = CheckNull(ws.Cells[ri, 3].Value),
                                zCardID = CheckNull(ws.Cells[ri, 4].Value),
                                passport = CheckNull(ws.Cells[ri, 5].Value),
                                birthdate_Day = CheckNull(ws.Cells[ri, 6].Value),
                                birthdate_Month = CheckNull(ws.Cells[ri, 7].Value),
                                birthdate_Year = CheckNull(ws.Cells[ri, 8].Value),
                                occupation = CheckNull(ws.Cells[ri, 9].Value),
                                moblienumber = CheckNull(ws.Cells[ri, 10].Value),
                                customerDetailType = ws.Cells[ri, 11].Value.ToString()
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return System.Tuple.Create(detaillist);
        }

        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public ActionResult CheckApplicationBypass(int appId)
        {
            var obj = _db.usp_ApplicationByPass_Check(appId).Single();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoUpdatePolicyNumber(int applicatonId, string policyNumber)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_ApplicationPolicyNo_Update(applicatonId, policyNumber, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidatePolicyNoDownloadQueueReport()
        {
            if (Session["DownloadExcel_PolicyNoQueueReport"] != null)
            {
                byte[] data = Session["DownloadExcel_PolicyNoQueueReport"] as byte[];
                string excelName = $"รายงานผลการตรวจสอบไฟล์เลขกรมธรรม์-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        //Get excel import
        public ActionResult ImportExcelPolicyFile(HttpPostedFileBase file)
        {
            var lstArr = new string[3];
            var lstValidate = new string[4];

            var tmCode = new ObjectParameter("Result", typeof(string));

            var lst = GetExcelImportPolicyNumber(file);

            if (lst == null)
            {
                lstValidate[0] = "False";
                lstValidate[1] = "0";
                lstValidate[2] = "ข้อมูลไม่ถูกต้องกรุณาตรวจสอบไฟล์";
                lstValidate[3] = "";
                return Json(lstValidate, JsonRequestBehavior.AllowGet);
            }

            var lst_Policy = lst.Item1;

            using (var db = new PACommunityEntities())
            {
                //Gen Code Tmp
                db.usp_GenerateCode_ForReportGroup("Tmp", tmCode);
            }
            var tmp_Code = tmCode.Value.ToString();
            //Loop Insert Data
            foreach (var item in lst_Policy)
            {
                try
                {
                    var rs = _db.usp_TmpApplicationPolicyImport_Insert(tmp_Code, item.ApplicationCode == "" ? null : item.ApplicationCode, item.PolicyNumber == "" ? null : item.PolicyNumber).Single();
                    string message = rs.Msg.ToString();
                    if (message.Trim().Contains("@ApplicationCode is not null"))
                    { message = "มีรายการที่ไม่มี Application Code"; }
                    if (message.Trim().Contains("@Policyno is not null"))
                    { message = "มีรายการที่ไม่มี เลข Policy"; }
                    lstArr[0] = rs.IsResult.Value.ToString();
                    lstArr[1] = rs.Result.ToString();
                    lstArr[2] = message;

                    if (lstArr[0] == "False")
                    {
                        return Json(lstArr, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            //Insert Validate
            var rs_validate = _db.usp_TmpApplicationPolicyImport_Validate(tmp_Code).SingleOrDefault();

            lstValidate[0] = rs_validate.IsResult.Value.ToString();
            lstValidate[1] = rs_validate.Result.ToString();
            lstValidate[2] = rs_validate.Msg.ToString();
            lstValidate[3] = tmp_Code;

            return Json(lstValidate, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTmpPolicyNumberList(string tmpCode, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_TmpApplicationPolicyImport_OverView_Select(tmpCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                { "recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                { "data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAppDetailForEditPolicyNo(int? appId = null, int? provinceId = null, string appName = null, string appCode = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ApplicationPolicyNo_Select(appId,
                                                            provinceId == -1 ? null : provinceId,
                                                            appName,
                                                            appCode,
                                                            pageStart,
                                                            pageSize,
                                                            sortField,
                                                            orderType,
                                                            search).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                { "recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                { "data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public Tuple<List<ImportPolicyNumberList>> GetExcelImportPolicyNumber(HttpPostedFileBase file)
        {
            List<ImportPolicyNumberList> policyNumberlist = new List<ImportPolicyNumberList>();

            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {
                        //if (ws.Cells[ri, 1].Value == null)
                        //{
                        //    ws.DeleteRow(nr);
                        //}
                        //else
                        //{
                        var app = ws.Cells[ri, 1].Value;
                        var poli = ws.Cells[ri, 2].Value;
                        string app_str = "";
                        string poli_str = "";
                        if (app != null)
                        { app_str = ws.Cells[ri, 1].Value.ToString(); }
                        if (poli != null)
                        { poli_str = ws.Cells[ri, 2].Value.ToString(); }
                        ImportPolicyNumberList item = new ImportPolicyNumberList();
                        item.ApplicationCode = app_str;
                        item.PolicyNumber = poli_str;
                        policyNumberlist.Add(item);
                        //}
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return System.Tuple.Create(policyNumberlist);
        }

        public JsonResult DoConfirmImportPolicyList(string tmpCode)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_TmpApplicationPolicyImport_ToApplication_Update(tmpCode, userID).SingleOrDefault();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportPolicyNoQueueReport(FormCollection form)
        {
            //await Task.Yield();
            var tempID = form["tempID"];
            var data_sheet1 = _db.usp_TmpApplicationPolicyImport_OverView_Select(tempID, null, null, null, null, null).Select(s => new
            {
                s.ApplicationCode,
                s.PolicyNo,
                สถานะการนำเข้า = s.IsError == "True" ? "ผ่าน" : "ไม่ผ่าน",
                สาเหตุ = s.ValidateMsg
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
                Session["DownloadExcel_FilePolicyNumberListReport"] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadPolicyNoQueueReport()
        {
            if (Session["DownloadExcel_FilePolicyNumberListReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FilePolicyNumberListReport"] as byte[];
                string excelName = $"รายงานตรวจสอบรายการอัพเดตเลขกรมธรรม์-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public JsonResult ValidatePDPAPost(int AppID)
        {
            var resultValidate = new Dictionary<string, dynamic>();

            var response = _db.usp_ApplicationPostPDPA_Select(AppID).FirstOrDefault();
            var cDetail = _db.usp_CustomerDetail_Select(null, AppID, null, null, null, null, null, null, null, null).AsQueryable().Count();

            if (response != null && cDetail > 0)
            {
                resultValidate["status"] = true;
                resultValidate["title"] = "แจ้งเตือน";
                resultValidate["type"] = "success";
                resultValidate["text"] = "ทำรายการสำเร็จ";
            }
            else
            {
                resultValidate["status"] = false;
                resultValidate["title"] = "แจ้งเเตือน";
                resultValidate["type"] = "error";
                resultValidate["text"] = "ทำรายการไม่สำเร็จ";
            }
            return Json(resultValidate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PDPAPost(int AppID, string Mode)
        {
            var response = _db.usp_ApplicationPostPDPA_Select(AppID).FirstOrDefault();

            ViewBag.Url = $"{Properties.Settings.Default.PDPAURL_Redirect}";
            ViewBag.AppId = AppID;
            ViewBag.ProjectId = response.ProjectId;
            ViewBag.ApplicationCode = response.ApplicationCode;
            ViewBag.ApplicationStatusId = response.ApplicationStatusId;
            ViewBag.ApplicationStatusName = response.ApplicationStatusName;
            ViewBag.PolicyName = response.PolicyName;
            ViewBag.CustomerCodeList = response.CustomerCodeList;
            ViewBag.CustomerIdentitycardNo = response.CustomerIdentitycardNo;
            ViewBag.CoordinatorIdentitycardNo = response.CoordinatorIdentitycardNo;
            ViewBag.IsSamePerson = response.IsSamePerson.ToString();
            ViewBag.Mode = Mode;
            ViewBag.EmployeeCode = response.EmployeeCode;
            ViewBag.CreateByUserId = response.CreatedByUserId;

            return View();
        }
    }
}