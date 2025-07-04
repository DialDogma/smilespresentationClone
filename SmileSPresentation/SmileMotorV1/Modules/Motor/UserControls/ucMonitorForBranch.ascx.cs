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
    public partial class ucMonitorForBranch : System.Web.UI.UserControl
    {

        #region Declear

        int sumCell_1, sumCell_2, sumCell_3, sumCell_4, sumCell_5, sumCell_6, sumCell_7;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private int _branchID;

        /// <summary>
        /// Property Get/Set Start Date
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                // Get Date to UC
                if (ucDateStart.DateSelected.HasValue == true)
                {
                    return ucDateStart.DateSelected.Value;
                }
                else
                {
                    return null;
                }         
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
        public DateTime? EndDate
        {
            get
            {
                // Get Date to UC
                if (ucDateEnd.DateSelected.HasValue == true)
                {
                    return ucDateEnd.DateSelected.Value;
                }
                else
                {
                    return null;
                }                
            }

            set
            {
                // Set Date To UC
                _endDate = value.Value;
                ucDateEnd.DateSelected = value;
            }
        }

        /// <summary>
        /// Property Get/Set Branch ID
        /// </summary>
        public int BranchID
        {
            get
            {
                return _branchID;
            }

            set
            {
                _branchID = value;
                ddlBranch.BranchID = value;
            }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Load DDL And Set Defualt
                this.ddlBranch.DoLoadDropDownlist();
                SetBranchDefault();
            }

            // Load Gridview
            LoadGridview();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // ส่ง query string
                string id = dgvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string StatusWaitEdit = e.Row.Cells[8].Text; // cells รอแก้ไข

                sumCell_1 += Convert.ToInt32(e.Row.Cells[3].Text);
                sumCell_2 += Convert.ToInt32(e.Row.Cells[4].Text);
                sumCell_3 += Convert.ToInt32(e.Row.Cells[5].Text);
                sumCell_4 += Convert.ToInt32(e.Row.Cells[6].Text);
                sumCell_5 += Convert.ToInt32(e.Row.Cells[7].Text);
                sumCell_6 += Convert.ToInt32(e.Row.Cells[8].Text);
                sumCell_7 += Convert.ToInt32(e.Row.Cells[9].Text);

                // ถ้าจำนวน รอแก้ไข ไม่เท่ากับ 0
                if (StatusWaitEdit != "0")
                {
                    e.Row.ForeColor = Color.Red;
                }

                // EncryptID
                FunctionManager fm = new FunctionManager();

                string EncryptID = fm.Base64Encrypt(id);

                // Set Att On Row Click
                e.Row.Attributes.Add("onclick", "window.open('" + "Modules/Motor/frmShowDetailByEmpID.aspx?" + "empid=" + EncryptID + "')");
                e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "รวม :";
                e.Row.Cells[3].Text = sumCell_1.ToString();
                e.Row.Cells[4].Text = sumCell_2.ToString();
                e.Row.Cells[5].Text = sumCell_3.ToString();
                e.Row.Cells[6].Text = sumCell_4.ToString();
                e.Row.Cells[7].Text = sumCell_5.ToString();
                e.Row.Cells[8].Text = sumCell_6.ToString();
                e.Row.Cells[9].Text = sumCell_7.ToString();
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvMain.Rows)
            {
                // Select Row เปลี่ยนสีอะไรซักอย่าง น๊ะจ๊ะ
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
            FunctionManager fm = new FunctionManager();
            MotorDB mdb = new MotorDB();

            DateTime sDate ;
            DateTime eDate ;
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


            // Load Gridview
            fm.LoadGridview(dgvMain, mdb.GetDashboardForBranch(sDate, eDate, ddlBranch.BranchID), "Employee_ID");
        }

        /// <summary>
        /// Do Export To Excel
        /// </summary>
        public void DoExport()
        {
            MotorDB mdb = new MotorDB();
            ExcelManager em = new ExcelManager();

            // Get Date
            DateTime sDate = ucDateStart.DateSelected.Value;
            DateTime eDate = ucDateEnd.DateSelected.Value;
            
            // Export To Excel
            em.DatatableToExcel(HttpContext.Current, "Export", mdb.GetDashboardForBranch(sDate, eDate, ddlBranch.BranchID), "Detail");
        }

        /// <summary>
        /// Set Branch Default
        /// </summary>
        private void SetBranchDefault()
        {
            ddlBranch.BranchID = _branchID;
        }
        #endregion

    }
}