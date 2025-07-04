using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMiscellaneous.Models;
using SmileSMiscellaneous.Helper;

namespace SmileSMiscellaneous.Controllers
{
    [Authorization]
    public class ApproveController : Controller
    {
        private MiscellaneousDBContext _db;

        public ApproveController()
        {
            _db = new MiscellaneousDBContext();
        }

        // GET: Approve
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnderwriteApprove(string appID = null)
        {
            //DeCode
            int? d_AppId = null;

            if (appID != null)
            {
                //DeCode
                var appIDBase64EncodedBytes = Convert.FromBase64String(appID);
                var deCode_AppID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);

                d_AppId = Convert.ToInt32(deCode_AppID);
            }
            ViewBag.AppId = d_AppId;
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.CancelCause = _db.usp_ApplicationUnApproveCause_Select(null, 0, 9999, null, null, null).ToList();

            return View();
        }

        private void DoInsertOrUpdateCustomerApplication(int appId, string CustType)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs_CustomerApplication = _db.usp_CustomerApplication_MI_Person_InsertOrUpdate(appId, CustType, userID);
        }

        public ActionResult SaveUnderwriteApprove(int appId, int approveStatusId, int? unappCauseId = null, string remark = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_Application_Approve_Update(appId, approveStatusId, unappCauseId, remark, userID).Single();

            if (rs.IsResult == true)
            {
                if (approveStatusId == 5)
                {
                    //Add CustomerApplication
                    DoInsertOrUpdateCustomerApplication(appId, "C");
                    DoInsertOrUpdateCustomerApplication(appId, "P");
                }
            }

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            if (approveStatusId == 5)   //อนุมัติ
            {
                //Insert Log
                var rs_log = InsertTransaction(Convert.ToInt32(appId), 81);     //81 อนุมัติ Applciation
            }
            else if (approveStatusId == 4)  //ไม่อนุมัติ
            {
                //Insert Log
                var rs_log = InsertTransaction(Convert.ToInt32(appId), 82);     //82 ไม่อนุมัติ Applciation
            }

            return Json(lstArr, JsonRequestBehavior.AllowGet);
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
    }
}