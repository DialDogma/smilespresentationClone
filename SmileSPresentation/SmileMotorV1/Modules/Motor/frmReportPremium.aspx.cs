using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportPremium : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    ddlFirstDayOfMonth.DoLoadDropDownList();
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;
            DoloadGridview();
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            DoloadGridview();
        }

        protected void btnExportExcel_OnClick(object sender, EventArgs e)
        {
            DoExport();
        }

        #endregion Event

        #region MyRegion

        private void DoExport()
        {
            using (var db = new MotorV1Entities())
            {
                //var firstPeriod = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);
                //var lst = db.usp_Premium_Select(firstPeriod).ToList();

                //mFunction.ExportToExcel(HttpContext.Current, lst, "รายงานการรับชำระเบี้ย", "รายงานการรับชำระเบี้ย");
            }
        }

        private void DoloadGridview()
        {
            using (var db = new MotorV1Entities())
            {
                //var firstPeriod = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);
                //var lst = db.usp_Premium_Select(firstPeriod).ToList();

                //var dt = mFunction.ToDataTable(lst);

                //mFunction.LoadGridview(dgvMain, dt);
            }
        }

        #endregion MyRegion
    }
}