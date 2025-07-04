using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportEndCoverNotice : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    //LOAD DROPDOWNLIST
                    ddlBranch1.DoLoadDropDownlist("ทั้งหมด");
                    ddlPeriodType1.DoLoadDropdownList("ทั้งหมด");
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview();
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            LoadGridview();
        }

        protected void btnExportExcel_OnClick(object sender, EventArgs e)
        {
            DoExport();
        }

        #endregion Event

        #region Method

        private void LoadGridview()
        {
            var branchId = ddlBranch1.BranchID;
            var periodTypeId = ddlPeriodType1.PeriodType_ID;
            var noticeDays = int.Parse(ddlEndCoverNotice.SelectedValue);

            using (var db = new MotorV1Entities())
            {
                var fm = new SmileSMotorClassLibrary.FunctionManager();

                var lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, periodTypeId, 7, branchId, noticeDays).ToList();

                fm.LoadGridview(dgvMain, fm.ToDataTable(lst), "ApplicationID");
            }
           ;
        }

        private void DoExport()
        {
            var branchId = ddlBranch1.BranchID;
            var noticeDays = int.Parse(ddlEndCoverNotice.SelectedValue);
            var periodTypeId = ddlPeriodType1.PeriodType_ID;

            using (var db = new MotorV1Entities())
            {
                var fm = new SmileSMotorClassLibrary.FunctionManager();

                var lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, periodTypeId, 7, branchId, noticeDays).ToList();

                // Export To Excel
                mFunction.ExportToExcel(HttpContext.Current, lst, "รายงาน App ใกล้หมดอายุ", "รายงาน App ใกล้หมดอายุ " + noticeDays + " วัน");
            }
        }

        #endregion Method
    }
}