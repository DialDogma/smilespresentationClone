using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucMonitorForMotor : System.Web.UI.UserControl
    {

        #region Declare
        int sumCell_1, sumCell_2, sumCell_3, sumCell_4, sumCell_5, sumCell_6, sumCell_7;
        private DateTime _startDate;
        private DateTime _endDate;

        /// <summary>
        /// Property Get/Set Start Date
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                // Get Date From UC
                _startDate = ucDateStart.DateSelected.Value;
                return _startDate;
            }
            set
            {
                // Set Date To UC
                _startDate = value;
                ucDateStart.DateSelected = value;
            }
        }
        
        /// <summary>
        /// Property Get/Set End Date
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                // Get Date From UC
                _endDate = ucDateEnd.DateSelected.Value;
                return _endDate;
            }
            set
            {
                // Set Date To UC
                _endDate = value;
                ucDateEnd.DateSelected = value;
            }
        }
        
        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            // Load Gridview
            LoadGridview();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // set datakeys ใส่ branch id
                string branchid = dgvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string StatusWaitApprove = e.Row.Cells[3].Text; // cells รอการอนุมัติ

                sumCell_1 += Convert.ToInt32(e.Row.Cells[2].Text);
                sumCell_2 += Convert.ToInt32(e.Row.Cells[3].Text);
                sumCell_3 += Convert.ToInt32(e.Row.Cells[4].Text);
                sumCell_4 += Convert.ToInt32(e.Row.Cells[5].Text);
                sumCell_5 += Convert.ToInt32(e.Row.Cells[6].Text);
                sumCell_6 += Convert.ToInt32(e.Row.Cells[7].Text);
                sumCell_7 += Convert.ToInt32(e.Row.Cells[8].Text);

                // ถ้าจำนวน รอการอนุมัติ ไม่เท่ากับ 0 
                if (StatusWaitApprove != "0")
                {
                    e.Row.ForeColor = Color.Red;
                }

                //Encrypt
                FunctionManager fm = new FunctionManager();

                string EncryptBranchID = fm.Base64Encrypt(branchid);

                // add attributes open new window
                e.Row.Attributes.Add("onclick", "window.open('" + "Modules/Motor/frmMotorUnderwrite.aspx?" + "branch" + "=" + EncryptBranchID + "')");
                // mouse hover show tooltip
                e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                // Sumary
                e.Row.Cells[1].Text = "รวม :";
                e.Row.Cells[2].Text = sumCell_1.ToString();
                e.Row.Cells[3].Text = sumCell_2.ToString();
                e.Row.Cells[4].Text = sumCell_3.ToString();
                e.Row.Cells[5].Text = sumCell_4.ToString();
                e.Row.Cells[6].Text = sumCell_5.ToString();
                e.Row.Cells[7].Text = sumCell_6.ToString();
                e.Row.Cells[8].Text = sumCell_7.ToString();
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvMain.Rows)
            {
                // ใส่สีตอนกดเลือกอะไรสักอย่าง น๊ะจ๊ะ
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Export
            DoExport();
        }
        #endregion

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        public void LoadGridview()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            DateTime sDate;
            DateTime eDate;
            // Date start
            if (ucDateStart.DateSelected.HasValue == true)
            {
                sDate = ucDateStart.DateSelected.Value;
            }
            else
            {
                sDate = DateTime.Now;
            }

            //date end
            if (ucDateEnd.DateSelected.HasValue == true)
            {
                eDate = ucDateEnd.DateSelected.Value;
            }
            else
            {
                eDate = DateTime.Now;
            }

            // Get Dash Board Data
            dt = db.GetDashboardForMotor(sDate, eDate);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt, "Branch_ID");

        }

        /// <summary>
        /// Do Export To Excel
        /// </summary>
        public void DoExport()
        {
            MotorDB mdb = new MotorDB();
            ExcelManager em = new ExcelManager();

            // Get Date From UC
            DateTime sDate = ucDateStart.DateSelected.Value;
            DateTime eDate = ucDateEnd.DateSelected.Value;
            
            // Export To Excel
            em.DatatableToExcel(HttpContext.Current, "Export", mdb.GetDashboardForMotor(sDate, eDate), "Detail");
        }

        #endregion

    }
}