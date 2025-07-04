using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPACommunity.Models;
using SmileSPACommunity.Helper;

namespace SmileSPACommunity.Controllers
{
    [Authorization]
    public class ApproveController : Controller
    {
        private PACommunityEntities _db;

        public ApproveController()
        {
            _db = new PACommunityEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: Approve
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PAMonitorApprove()
        {
            ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.AppRoundStatus = _db.usp_ApplicationRoundStatus_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            return View();
        }

        public ActionResult PANewAppApprove(string appID = null)
        {
            //DeCode
            var appIDBase64EncodedBytes = Convert.FromBase64String(appID);
            var deCode_AppID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);

            int d_AppId = Convert.ToInt32(deCode_AppID);
            ViewBag.ApplicationID = d_AppId;
            //ViewBag.OrganizeTypes = _db.usp_OrganizeType_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.OrganizeGroups = _db.usp_OrganizeGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.Provinces = _db.usp_Province_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.Products = _db.usp_Product_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.Titles = _db.usp_Title_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ContractAgentType = _db.usp_ApplicationAgentType_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.Round = _db.usp_ApplicationRound_Select(d_AppId, null, null, null, 0, 999, null, null, null).ToList();
            ViewBag.CancelCauses = _db.usp_ApplicationRoundUnApproveCause_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.RoundStatusId = _db.usp_ApplicationRound_Select(d_AppId, null, null, null, 0, 999, null, null, null).SingleOrDefault().ApplicationRoundStatusId;
            ViewBag.LinkCashReceive = Properties.Settings.Default.CashReceive_Payment_Path;
            ViewBag.OrganizeBank = _db.usp_OrganizeBank_Select(null, 0, 9999, null, null, null).ToList();

            return View();
        }

        public ActionResult EndorseCancelPolicyApprove(string RequestCancelApplicationId = null)
        {
            //DeCode
            var RequestCancelApplicationIdBase64EncodedBytes = Convert.FromBase64String(RequestCancelApplicationId);
            var deCode_RequestCancelApplicationId = System.Text.Encoding.UTF8.GetString(RequestCancelApplicationIdBase64EncodedBytes);

            int d_RequestCancelApplicationId = Convert.ToInt32(deCode_RequestCancelApplicationId);
            ViewBag.RequestCancelApplicationId = d_RequestCancelApplicationId;
            ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 2, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult EndorseCancelCustomerApprove(string RequestCancelCustomerHeaderId = null)
        {
            //DeCode
            var RequestCancelCustomerHeaderIdBase64EncodedBytes = Convert.FromBase64String(RequestCancelCustomerHeaderId);
            var deCode_RequestCancelCustomerHeaderId = System.Text.Encoding.UTF8.GetString(RequestCancelCustomerHeaderIdBase64EncodedBytes);

            int d_RequestCancelCustomerHeaderId = Convert.ToInt32(deCode_RequestCancelCustomerHeaderId);
            ViewBag.RequestCancelCustomerHeaderId = d_RequestCancelCustomerHeaderId;

            ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 3, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult EndorseApplicationContactApprove(string RequestEndorseContactAndAccountId = null)
        {
            //DeCode
            var RequestEndorseContactAndAccountIdBase64EncodedBytes = Convert.FromBase64String(RequestEndorseContactAndAccountId);
            var deCode_RequestEndorseContactAndAccountId = System.Text.Encoding.UTF8.GetString(RequestEndorseContactAndAccountIdBase64EncodedBytes);

            int d_RequestEndorseContactAndAccountId = Convert.ToInt32(deCode_RequestEndorseContactAndAccountId);

            ViewBag.RequestEndorseContactAndAccountId = d_RequestEndorseContactAndAccountId;

            ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 4, 0, 9999, null, null, null).ToList();
            return View();
        }

        public JsonResult GetApplicationRound(string appRoundStatusId, int? branchID = null, string startCoverDate = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            DateTime? s_CoverDate = null;

            if (startCoverDate != null && startCoverDate != "")
            {
                s_CoverDate = GlobalFunction.ConvertDatePickerToDate_BE(startCoverDate);
            }

            var result = _db.usp_ApplicationRound_Select_V2(null, branchID == -1 ? null : branchID, appRoundStatusId, null, s_CoverDate, pageStart, pageSize, sortField, orderType, search).ToList();

            //var result = _db.usp_ApplicationRound_Select(null, branchID == -1 ? null : branchID, appRoundStatusId, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationRoundDetail(int? applicationID, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
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

        public JsonResult DoUpdateStatusRoundPolicy(int applicationRoundId, int applicationRoundStatusId, int applicationRoundUnApproveCauseId, string applicationRoundUnApproveRemark)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_ApplicationRound_ApprovePolicy_Update(applicationRoundId, applicationRoundStatusId, applicationRoundUnApproveCauseId, applicationRoundUnApproveRemark, userID).Single();

            if (result.IsResult == true)
            {
                var rs_CustomerApplication = _db.usp_CustomerApplication_PAC_Person_InsertOrUpdate(applicationRoundId, userID);
            }

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoApproveRequestCancelApplication(int requestCancelApplicationId, int approveStatusId, int? approveCauseId = null, string approveRemark = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_RequestCancelApplication_Approve_Update(requestCancelApplicationId, approveStatusId, approveCauseId, approveRemark, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoApproveRequestCancelCustomer(int requestCancelCustomerHeaderId, int approveStatusId, int? approveCauseId = null, string approveRemark = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_RequestCancelCustomer_Approve_Update(requestCancelCustomerHeaderId, approveStatusId, approveCauseId, approveRemark, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestCancelCustomerHeaderById(int requestCancelCustomerHeaderId)
        {
            var rs = _db.usp_RequestCancelCustomerHeader_Select(requestCancelCustomerHeaderId, null, null, 0, 9999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEndorseContactAndAccountDetail(int referenceID)
        {
            var rs = _db.usp_RequestEndorseContactAndAccount_SelectById(referenceID, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveApproveContactAndAccount(int endorseContactandAccountID, int approveStatusId, int? approveCauseId = null, string approveRemark = null)
        {
            var userID = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var rs = _db.usp_RequestEndorseContactAndAccount_Approve_Update(endorseContactandAccountID, approveStatusId, approveCauseId == -1 ? null : approveCauseId, approveRemark, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}