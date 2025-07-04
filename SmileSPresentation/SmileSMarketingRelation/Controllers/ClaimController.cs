using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMarketingRelation.Models;
using SmileSMarketingRelation.Helper;

namespace SmileSMarketingRelation.Controllers
{
    //[Authorization]
    public class ClaimController : Controller
    {
        #region Context

        private SSSDBContext _contextPH;
        private SSSPADBContext _contextPA;

        public ClaimController()

        {
            _contextPH = new SSSDBContext();
            _contextPA = new SSSPADBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _contextPH.Dispose();
            _contextPA.Dispose();
        }

        #endregion Context


        // GET: Claim
        public ActionResult Index()
        {
           

            return View();
        }

        [Authorization]
        public ActionResult ReportClaimPH()
        {
            ViewBag.ClaimType = _contextPH.usp_GetClaimType_Select(null).ToList();
            return View();
        }

        [Authorization]
        public ActionResult ReportClaimPA()
        {
            ViewBag.ClaimStyle = _contextPA.usp_GetClaimTypeByCodeGroup_Select("4100").ToList();
            ViewBag.CliamType = _contextPA.usp_GetClaimTypeByCodeGroup_Select("4000").ToList();
            return View();
        }


#region "Method"

        //ExportReportPH
        public ActionResult ExportReportPH(FormCollection form)
        {
            var df = form["dtpDateFrom"];
            var dt = form["dtpDateTo"];
            var claimAdmintType = form["ddlClaimAdmitType"];

            DateTime? dfrom = GlobalFunction.ConvertDatePickerToDate_BE(df);
            DateTime? dto = GlobalFunction.ConvertDatePickerToDate_BE(dt);

            var result = GlobalFunction.ToDataTable(_contextPH.usp_ReportClaim_PH_Select(claimAdmintType,dfrom,dto).ToList());

            ExcelManager.ExportToExcel(this.HttpContext, "Report_ClaimPH", result, "Report_ClaimPH");
            return View();
        }

        //ExportReportPA
        public ActionResult ExportReportPA(FormCollection form)
        {
            var df = form["dtpDateFrom"];
            var dt = form["dtpDateTo"];
            var claimAdmintType = form["ddlClaimAdmitType"];

            DateTime? dfrom = GlobalFunction.ConvertDatePickerToDate_BE(df);
            DateTime? dto = GlobalFunction.ConvertDatePickerToDate_BE(dt);

            var result = GlobalFunction.ToDataTable(_contextPA.usp_ReportClaim_PA_Select(claimAdmintType, dfrom, dto).ToList());

            ExcelManager.ExportToExcel(this.HttpContext, "Report_ClaimPA", result, "Report_ClaimPA");
            return View();
        }


        //GetClaimAdmitType
        public ActionResult GetClaimAdmitType(string claimType)
        {

            if (claimType == "")
            {
                claimType = null;
            }

            var result = _contextPH.usp_GetClaimAdmitType_select(claimType).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

#endregion


    }
}