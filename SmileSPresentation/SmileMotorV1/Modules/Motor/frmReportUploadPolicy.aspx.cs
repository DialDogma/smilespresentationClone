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
    public partial class frmReportUploadPolicy : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    ucDatepickerFrom.DateSelected = DateTime.Now;
                    ucDatepickerTo.DateSelected = DateTime.Now;
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Load Gridview
            LoadGridview();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Export
            DoExport();
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Pagang
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview();
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        private void LoadGridview()
        {
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();
            MotorDB db = new MotorDB();
            DateTime dateFrom, dateTo;

            // Get Date From UC
            if (ucDatepickerFrom.DateSelected.HasValue == true)
            {
                dateFrom = cDate.ToDate(ucDatepickerFrom.DateSelected.Value);
            }
            else
            {
                dateFrom = DateTime.Now;
            }

            if (ucDatepickerTo.DateSelected.HasValue == true)
            {
                dateTo = cDate.ToDate(ucDatepickerTo.DateSelected.Value);
            }
            else
            {
                dateTo = DateTime.Now;
            }

            // ถ้า date ไม่เท่ากับค่าว่าง
            if (dateFrom != null && dateTo != null)
            {
                dt = db.GetReportMotorUploadPolicy(dateFrom, dateTo);
            }

            // ถ้า date ว่าง
            else
            {
                dt = db.GetReportMotorUploadPolicy(null, null);
            }

            // โหลด
            fm.LoadGridview(dgvMain, dt);
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        private void DoExport()
        {
            MotorDB db = new MotorDB();
            ExcelManager em = new ExcelManager();
            DataTable dt = new DataTable();
            DateTime dateFrom, dateTo;

            // Get Date From UC
            if (ucDatepickerFrom.DateSelected.HasValue == true)
            {
                dateFrom = cDate.ToDate(ucDatepickerFrom.DateSelected.Value);
            }
            else
            {
                dateFrom = DateTime.Now;
            }

            if (ucDatepickerTo.DateSelected.HasValue == true)
            {
                dateTo = cDate.ToDate(ucDatepickerTo.DateSelected.Value);
            }
            else
            {
                dateTo = DateTime.Now;
            }

            // ถ้า date ไม่เท่ากับค่าว่าง
            if (dateFrom != null && dateTo != null)
            {
                dt = db.GetReportMotorUploadPolicy(dateFrom, dateTo);
            }

            // ถ้า date ว่าง
            else
            {
                dt = db.GetReportMotorUploadPolicy(null, null);
            }

            // Export
            em.DatatableToExcel(HttpContext.Current,
                "รายงานการ Upload กรมธรรม์ " + ucDatepickerFrom.DateSelected + " ถึง " + ucDatepickerTo.DateSelected,
                dt,
                "รายละเอียด");
        }

        #endregion Method
    }
}