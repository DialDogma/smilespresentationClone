using SmileMotorV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmNoticeEndCover : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            //check role = MotorDeveloper OR MotorUnderwrite
            if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
            {
                //Something..
            }
            else
            {
                Response.Redirect("frmSuccess.aspx?msg=2");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DoloadGridview();
        }

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;
            DoloadGridview();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DoExport();
        }

        #endregion Event

        #region MyRegion

        private void DoExport()
        {
            using (var db = new MotorV1Entities())
            {
                var lst = new List<usp_MotorApplication_Report_EndCoverNotice_Select_Result>();

                if (HttpContext.Current.User.IsInRole("MotorDeveloper"))
                {
                    //TODO:
                    lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, 2, 7, null, 60).ToList();
                }
                if (HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    //TODO:
                    lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, 2, 7, null, 60).ToList();
                }
                if (HttpContext.Current.User.IsInRole("MotorUser"))
                {
                    //TODO:
                    lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, 2, 7, cFunction.GetCurrentBranchID(Page), 60).ToList();
                }

                mFunction.ExportToExcel(HttpContext.Current, lst, "App ใกล้หมดอายุ", "รายงาน Application ใกล้หมดอายุ");
            }
        }

        private void DoloadGridview()
        {
            using (var db = new MotorV1Entities())
            {
                var lst = new List<usp_MotorApplication_Report_EndCoverNotice_Select_Result>();

                if (HttpContext.Current.User.IsInRole("MotorDeveloper"))
                {
                    //TODO:
                    lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, 2, 7, null, 60).ToList();
                }
                if (HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    //TODO:
                    lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, 2, 7, null, 60).ToList();
                }
                if (HttpContext.Current.User.IsInRole("MotorUser"))
                {
                    //TODO:
                    lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, 2, 7, cFunction.GetCurrentBranchID(Page), 60).ToList();
                }

                //var firstPeriod = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);

                var dt = mFunction.ToDataTable(lst);

                mFunction.LoadGridview(dgvMain, dt);
            }
        }

        #endregion MyRegion
    }
}