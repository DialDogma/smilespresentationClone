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
    public partial class frmReportAppNotConfirmFirstPeriod : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    //TODO:SOMETHING
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
            LoadGridview();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            //Load Gridview
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
            FunctionManager fm = new FunctionManager();
            MotorDB db = new MotorDB();
            DataTable dt = new DataTable();

            // Get Data Report
            dt = db.GetMotorApplicationStatusApprovedFirstPeriod(null,
                                                                 null,
                                                                 null,
                                                                 null,
                                                                 null,
                                                                 null,
                                                                 false);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt);
        }

        /// <summary>
        /// Do Export To Excel
        /// </summary>
        private void DoExport()
        {
            MotorDB db = new MotorDB();
            DataTable dt = new DataTable();
            ExcelManager em = new ExcelManager();

            // Get Data Report
            dt = db.GetMotorApplicationStatusApprovedFirstPeriod(null,
                                                                 null,
                                                                 null,
                                                                 null,
                                                                 null,
                                                                 null,
                                                                 false);

            // Export To Excel
            em.DatatableToExcel(HttpContext.Current, "รายงาน App ที่ยังไม่ยืนยันการชำระงวดแรก", dt, "รายละเอียด");
        }

        #endregion Method
    }
}