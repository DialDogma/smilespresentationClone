using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucPersonSearch : System.Web.UI.UserControl
    {

        #region Declear
        public event EventHandler eSelectIndexPersonID;
        private int _PersonID;

        /// <summary>
        /// Property Get/Set Person ID
        /// </summary>
        public int PersonID
        {
            get
            {
                return _PersonID;
            }

            set
            {
                _PersonID = value;
            }
        }

        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            btnSearch.CssClass = "btn btn-info btn-sm";
            btnClose.CssClass = "btn btn-danger btn-xs";
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow row in dgvMain.Rows)
            {
                // Select Index
                if (row.RowIndex == dgvMain.SelectedIndex)
                {

                    //txtPersonID.Text = dgvMain.DataKeys[row.RowIndex].Values[0].ToString();
                    txtPersonID.Text = dgvMain.SelectedRow.Cells[1].Text;
                    _PersonID = Convert.ToInt32(dgvMain.DataKeys[row.RowIndex].Values[0].ToString());
                }
            }

            // Clear Selected
            ClearSelectedIndex();
            txtPersonID.Focus();

            EventHandler handler;
            handler = eSelectIndexPersonID;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Set Att On Row Click
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Text ว่าง
            if (txtSearch.Text.Trim() == string.Empty)
            {
               
                FunctionManager fm = new FunctionManager();
                fm.AlertJavaScript(this.Page, "กรุณาระบุคำค้น!");

            }
            // Text ไม่ว่าง
            else
            {
                LoadGridView();
                ModalPopupExtender1.Show();
            }

        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridView();
            ModalPopupExtender1.Show();
        }
        #endregion

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        private void LoadGridView()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Person
            dt = db.GetPersonSearch(txtSearch.Text.Trim());

            // Load Gridview
            fm.LoadGridview(dgvMain, dt,"Person_ID");

        }

        /// <summary>
        /// Clear Select Index DGV
        /// </summary>
        private void ClearSelectedIndex()
        {
            // Clear
            txtSearch.Text = string.Empty;
            dgvMain.SelectedIndex = -1;
            dgvMain.DataBind();
        }

        #endregion

    }
}