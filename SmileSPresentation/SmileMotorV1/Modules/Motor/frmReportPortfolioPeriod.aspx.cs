using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportPortfolioPeriod : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    ucDateFrom.DateSelected = DateTime.Now;
                    UcDateTo.DateSelected = DateTime.Now;
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;
            // Load Gridview
            LoadGridview();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            // Load Gridview
            LoadGridview();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Export
            DoExport();
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        private void LoadGridview()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DateTime date_From, date_To;

            if (ucDateFrom.DateSelected.HasValue == true)
            {
                date_From = cDate.ToDate(ucDateFrom.DateSelected.Value);
            }
            else
            {
                date_From = DateTime.Now;
            }

            if (UcDateTo.DateSelected.HasValue == true)
            {
                date_To = cDate.ToDate(UcDateTo.DateSelected.Value);
            }
            else
            {
                date_To = DateTime.Now;
            }

            // Load Detail
            fm.LoadGridview(dgvMain, db.GetMotorApplicationPortfolioPeriod(date_From, date_To));
        }

        /// <summary>
        /// Do Export To Excel
        /// </summary>
        private void DoExport()
        {
            MotorDB db = new MotorDB();
            ExcelManager em = new ExcelManager();
            DataTable dt = new DataTable();
            DateTime date_From, date_To;

            if (ucDateFrom.DateSelected.HasValue == true)
            {
                date_From = cDate.ToDate(ucDateFrom.DateSelected.Value);
            }
            else
            {
                date_From = DateTime.Now;
            }

            if (UcDateTo.DateSelected.HasValue == true)
            {
                date_To = cDate.ToDate(UcDateTo.DateSelected.Value);
            }
            else
            {
                date_To = DateTime.Now;
            }

            // Load Data To Data Table
            dt = db.GetMotorApplicationPortfolioPeriod(date_From, date_To);

            // Export To Excel
            em.DatatableToExcel(HttpContext.Current,
                "รายงานตามงวดที่คิดผลงาน วันที่ " + date_From.ToShortDateString() + " ถึง " + date_To.ToShortDateString(),
                dt,
                "รายละเอียด");
        }

        #endregion Method
    }
}