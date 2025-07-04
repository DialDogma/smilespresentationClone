using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportNewApplicationV2 : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    ddlBranch.DoLoadDropDownlist("ทั้งหมด");
                    ddlMotorApplicationStatus1.DoLoadDropdownlistAll();
                    ucDatepickerFrom.DateSelected = DateTime.Now;
                    ucDatepickerTo.DateSelected = DateTime.Now;
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            LoadGridview();
        }

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview();
        }

        protected void btnExportExcel_OnClick(object sender, EventArgs e)
        {
            Export();
        }

        #endregion Event

        #region Method

        private void LoadGridview()
        {
            using (var db = new MotorV1Entities())
            {
                var dateFrom = ucDatepickerFrom.DateSelected.Value;
                var dateTo = ucDatepickerTo.DateSelected.Value;
                var statusId = Convert.ToInt32(ddlMotorApplicationStatus1.ddl.SelectedValue);
                var conditionId = Convert.ToInt32(ddlCondition.SelectedValue);
                var branchId = ddlBranch.BranchID;
                int? empId = null;
                if (ucEmployeeSearch1.EmployeeCode != "")
                {
                    empId = ucEmployeeSearch1.Employee.Employee_ID;
                }

                var lst = db.usp_MotorApplication_Report_NewAppV2_Select(dateFrom, dateTo, statusId, conditionId, branchId, empId).ToList();

                var dt = mFunction.ToDataTable(lst);
                mFunction.LoadGridview(dgvMain, dt);
            }
        }

        private void Export()
        {
            using (var db = new MotorV1Entities())
            {
                var dateFrom = ucDatepickerFrom.DateSelected.Value;
                var dateTo = ucDatepickerTo.DateSelected.Value;
                var statusId = Convert.ToInt32(ddlMotorApplicationStatus1.ddl.SelectedValue);
                var conditionId = Convert.ToInt32(ddlCondition.SelectedValue);
                var branchId = ddlBranch.BranchID;
                int? empId = null;
                if (ucEmployeeSearch1.EmployeeCode != "")
                {
                    empId = ucEmployeeSearch1.Employee.Employee_ID;
                }

                var lst = db.usp_MotorApplication_Report_NewAppV2_Select(dateFrom, dateTo, statusId, conditionId, branchId, empId).ToList();

                mFunction.ExportToExcel(HttpContext.Current, lst, "รายงาน NewApplication", "รายงาน NewApplication " + ddlCondition.SelectedItem);
            }
        }

        #endregion Method
    }
}