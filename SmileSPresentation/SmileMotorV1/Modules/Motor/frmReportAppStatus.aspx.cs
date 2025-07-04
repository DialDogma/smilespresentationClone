using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportAppStatus : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    LoadDDLStatus();
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            DoloadGridview();
        }

        protected void btnExportExcel_OnClick(object sender, EventArgs e)
        {
            DoExport();
        }

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;
            DoloadGridview();
        }

        #endregion Event

        #region Method

        private void LoadDDLStatus()
        {
            using (var db = new MotorV1Entities())
            {
                var lst = db.usp_MotorApplication_Status_Select(null).ToList();

                ddlStatus.DataSource = lst;
                ddlStatus.DataTextField = "MotorApplicationStatusDetail";
                ddlStatus.DataValueField = "MotorApplicationStatus_ID";
                ddlStatus.DataBind();
            }
        }

        private void DoExport()
        {
            using (var db = new MotorV1Entities())
            {
                var statusId = ddlStatus.SelectedValue;
                var lst = db.usp_MotorApplication_Report_AppByStatus_Select(Convert.ToInt32(statusId)).ToList();

                mFunction.ExportToExcel(HttpContext.Current, lst, "Application by Status", "รายงาน Application สถานะ" + ddlStatus.SelectedItem);
            }
        }

        private void DoloadGridview()
        {
            using (var db = new MotorV1Entities())
            {
                var statusId = ddlStatus.SelectedValue;
                var lst = db.usp_MotorApplication_Report_AppByStatus_Select(Convert.ToInt32(statusId)).ToList();

                var dt = mFunction.ToDataTable(lst);

                mFunction.LoadGridview(dgvMain, dt);
            }
        }

        #endregion Method
    }
}