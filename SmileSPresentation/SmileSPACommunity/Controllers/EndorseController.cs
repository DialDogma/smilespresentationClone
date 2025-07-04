using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Web;
using System.IO;
using System.Data.Entity.Core.Objects;

namespace SmileSPACommunity.Controllers
{
    [Authorization]
    public class EndorseController : Controller
    {
        private PACommunityEntities _db;

        public EndorseController()
        {
            _db = new PACommunityEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: EndorsePolicy
        public ActionResult EndorseCancelPolicy(string RequestCancelApplicationId = null)
        {
            if (RequestCancelApplicationId == null)
            {
                ViewBag.RequestCancelApplicationID = null;
            }
            else
            {
                //deCode
                var RequestCancelApplicationIdBase64EncodedBytes = Convert.FromBase64String(RequestCancelApplicationId);
                var deCode_RequestCancelApplicationId = System.Text.Encoding.UTF8.GetString(RequestCancelApplicationIdBase64EncodedBytes);

                ViewBag.RequestCancelApplicationID = deCode_RequestCancelApplicationId;
            }

            ViewBag.BranchID = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            ViewBag.CancelCause = _db.usp_ApplicationCancelCause_Select(null, 0, 999, null, null, null).ToList();
            return View();
        }

        // GET: EndorsePolicy
        public ActionResult EndorseCancelCustomerDetail(string RefHeadCancelCust = null)
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "PACommunity_MO" }; //MO
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PACommunity_PAUnderwrite" }; //ฝ่าย PA Underwrite

            ViewBag.Role = rolelist;

            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 999, null, null, null).ToList();
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 999, null, null, null).ToList();
            }

            string deApp = null;
            if (RefHeadCancelCust != null)
            {
                //deCode
                var RequestCancelApplicationIdBase64EncodedBytes = Convert.FromBase64String(RefHeadCancelCust);
                var deCode_ApplicationCode = System.Text.Encoding.UTF8.GetString(RequestCancelApplicationIdBase64EncodedBytes);

                deApp = deCode_ApplicationCode;
            }

            //ViewBag.BranchID = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            ViewBag.CancelCause = _db.usp_CustomerDetailCancelCause_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.AppCode = deApp;
            return View();
        }

        //[Authorization(Roles = "PACommunity_MO, PACommunity_PAUnderwrite, Developer")]
        public ActionResult EndorseCancelPolicyMonitor()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "PACommunity_MO" }; //MO
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PACommunity_PAUnderwrite" }; //ฝ่าย PA Underwrite

            ViewBag.Role = rolelist;

            ViewBag.RoleList = role;

            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 999, null, null, null).ToList();
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 999, null, null, null).ToList();
            }
            ViewBag.ApproveStatus = _db.usp_ApproveStatus_Select(null, 0, 9999, null, null, null).ToList();

            return View();
        }

        public ActionResult EndorseCancelCustomerDetailMonitor()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "PACommunity_MO" }; //MO
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PACommunity_PAUnderwrite" }; //ฝ่าย PA Underwrite

            ViewBag.Role = rolelist;
            ViewBag.RoleList = role;
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 999, null, null, null).ToList();
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 999, null, null, null).ToList();
            }

            ViewBag.CancelStatus = _db.usp_ApproveStatus_Select(null, null, null, null, null, null).ToList();

            return View();
        }

        public ActionResult EndorseApplicationContact(string RequestEndorseContactAndAccountId = null)
        {
            if (RequestEndorseContactAndAccountId == null)
            {
                ViewBag.RequestEndorseContactAndAccountId = null;
            }
            else
            {
                //deCode
                var RequestEndorseContactAndAccountIdBase64EncodedBytes = Convert.FromBase64String(RequestEndorseContactAndAccountId);
                var deCode_RequestEndorseContactAndAccountId = System.Text.Encoding.UTF8.GetString(RequestEndorseContactAndAccountIdBase64EncodedBytes);

                ViewBag.RequestEndorseContactAndAccountId = deCode_RequestEndorseContactAndAccountId;
            }

            ViewBag.Titles = _db.usp_Title_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.OrganizeBank = _db.usp_OrganizeBank_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }
        public JsonResult GetApplicationDetail(int? applicationID = null, string applicationStatusId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? branchID = null)
        {
            int? StatusId = null;
            if (applicationStatusId == "-1" || applicationStatusId == null)
            {
                StatusId = null;
            }
            else
            {
                StatusId = Convert.ToInt32(applicationStatusId);
            }

            var result = _db.usp_Application_Select(null, branchID, StatusId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestCancelApplicationDetail(string approveStatusId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, string branchID = null)
        {
            int? StatusId = null;
            int? c_branchID = null;
            if (approveStatusId == "-1" || approveStatusId == null)
            {
                StatusId = null;
            }
            else
            {
                StatusId = Convert.ToInt32(approveStatusId);
            }

            if (branchID != "-1" && branchID != null)
            {
                c_branchID = Convert.ToInt32(branchID);
            }

            var result = _db.usp_RequestCancelApplication_Select(null, null, c_branchID, StatusId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentRequestCancelDetail(int referenceId, int documentTypeId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_DocumentRequestCancel_Select(referenceId, documentTypeId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestCancelApplicationById(int requestCancelApplicationId)
        {
            var rs = _db.usp_RequestCancelApplication_Select(requestCancelApplicationId, null, null, null, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRequestCancelApplication(int applicationId)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_RequestCancelApplication_Insert(applicationId, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateRequestCancelApplication(int requestCancelApplicationId, int applicationCancelCauseId, string remark)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_RequestCancelApplication_Sent_Update(requestCancelApplicationId, applicationCancelCauseId, remark, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRequestCancelHeader(int applicationId)
        {
            var resultArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_RequestCancelCustomerHeader_Insert(applicationId, userID).Single();

            resultArr[0] = rs.IsResult.Value.ToString();
            resultArr[1] = rs.Result;
            resultArr[2] = rs.Msg;

            return Json(resultArr, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetApplicationCancel(int? refId, string searchStr, int? branchId, int? causeid, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null)
        {
            usp_RequestCancelCustomerHeader_Select_Result rs = new usp_RequestCancelCustomerHeader_Select_Result();
            var result = _db.usp_RequestCancelCustomerHeader_Select(refId, branchId == -1 ? null : branchId, causeid == -1 ? null : causeid, pageStart, pageSize, sortField, orderType, searchStr).ToList();

            var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCancelCustomerDetail(int? refId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_RequestCancelCustomerDetail_Select(null, refId, pageStart, pageSize == -1 ? null : pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsCustomerCancelOnProcess(int CustomerDetailId)
        {
            var lstArr = new string[3];

            var rs = _db.usp_RequestCancelCustomerDetail_Validate(CustomerDetailId).SingleOrDefault();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerRound(int appid, int? branchId)
        {
            var result = _db.usp_ApplicationRound_Select(appid, branchId == -1 ? null : branchId, null, null, null, null, null, null, null).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoSaveRequestCancelCusDetail(int requestCancelCusId, int CusDetailID, int CusCauseID, string remark)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_RequestCancelCustomerDetail_Insert(requestCancelCusId, CusDetailID, CusCauseID, remark, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoSendUpdateRequestCancelCustomer(int requestCancelCusId, FormCollection form)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var effDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["effectDate"]));

            var rs = _db.usp_RequestCancelCustomer_Sent_Update(requestCancelCusId, effDate, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoInsertRequestEndorseContactAndAccount(int applicationId)
        {
            var userID = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var rs = _db.usp_RequestEndorseContactAndAccount_Insert(applicationId, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoSentEndorseContactandAccount(int referanceId, int toContactTitleId, string toContactFirstName, string toContactLastName,
                                                            string toContactMobile, int toBankId, string toAccountNo, string toAccountName, string toIdCardNumber = null,
                                                            string toPassportNumber = null)
        {
            var userID = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var t = toContactMobile.Replace("-", "");

            toIdCardNumber = toIdCardNumber == "" ? null : toIdCardNumber;

            toPassportNumber = toPassportNumber == "" ? null : toPassportNumber;

            var rs = _db.usp_RequestEndorseContactAndAccount_Update(referanceId, toContactTitleId, toContactFirstName, toContactLastName, t, toBankId, toAccountNo, toAccountName, toIdCardNumber, toPassportNumber, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContactandAccountByAppID(int applicationId)
        {
            var rs = _db.usp_ApplicationContact_Select(applicationId, null, null, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDocumentRequestEndorseContactAndAccount(int referenceId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_DocumentRequestEndorseContactAndAccount_SelectById(referenceId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEndorseContactandAccount(int referenceId)
        {
            var rs = _db.usp_RequestEndorseContactAndAccount_SelectById(referenceId, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}