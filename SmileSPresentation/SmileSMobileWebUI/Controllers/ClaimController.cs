using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmileSMobileWebUI.CommonService;
using SmileSMobileWebUI.MobileDataCenterService;
using SmileSMobileWebUI.PHClaimService;

namespace SmileSMobileWebUI.Controllers
{
    public class ClaimController : Controller
    {
        // GET: Claim
        public ActionResult Feedback()
        {
            var mobilekey = new SmileSMobileAppToken.AlignToken().GetAlignToken();
            ViewBag.MBKey = mobilekey;
            return View(ViewBag);
        }

        #region Show Claim Feedback

        /// <summary>
        /// show claim feed back by empCode
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowClaimFeedback(FormCollection form)
        {
            //Check EmpCode
            var empCode = Request.QueryString["empCode"];
            ViewBag.empCode = empCode;
            if (!string.IsNullOrEmpty(empCode))
            {
                var client = new MobileServiceClient();
                var obj = client.GetEmployee(empCode).ToString();

                ViewBag.obj = obj;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(empCode)");
                //throw new HttpException(404, "empCode not found.");
            }
            ViewBag.dcr = form["hidden_selected"];
            //get master filter list
            using (var client = new CommonServiceClient())
            {
                var masterlistresult = client.GetMasterFilterList(2).ToList();
                ViewBag.MasterFilterList = masterlistresult;
            }

            return View();
        }

        [HttpGet]
        public ActionResult TotalClaimFeedback(string dcr, string empCode)
        {
            using (var client = new ClaimServiceClient())
            {
                var result = client.GetClaimFeedbackTotal(empCode, dcr);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AppDetail(string dcr, string empCode, int pageSize, int indexStart, string sortField, string orderType)
        {
            using (var client = new ClaimServiceClient())
            {
                var result = client.GetClaimFeedbackAppDetail(empCode, dcr, pageSize, indexStart, sortField, orderType).ToList();

                return PartialView("ApplicationDetailPartialView", result);
            }
        }

        #endregion Show Claim Feedback
    }
}