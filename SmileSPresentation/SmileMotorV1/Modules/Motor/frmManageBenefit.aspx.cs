using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmManageBenefit : System.Web.UI.Page
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load Gridview
                LoadGridView();

                // Load Dropdown List
                ddlMotorProductType1.DoLoadDropdownList();

                // Add Att Button
                btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
            }
           
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Set Att Click On Row 
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvMain.Rows)
            {
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    // Set BG Color And Tooltip
                    row.BackColor = ColorTranslator.FromHtml("#CC3333");
                    row.ToolTip = string.Empty;

                    // Get Datakey On Row Index
                    string _id = dgvMain.DataKeys[row.RowIndex].Values[0].ToString();

                    FunctionManager fm = new FunctionManager();

                    // Encrypt Datakey ID
                    string EncryptID = fm.Base64Encrypt(_id);

                    // Set Enable And Att
                    btnEdit.Enabled = true;
                    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + EncryptID + "')");

                   
                }
                else
                {
                    // Set BG Color And Tooltip
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            // Clear Select Index DGV
            ClearSelectIndex();

            // Load Gridview
            LoadGridView();
        }
        #endregion

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        public void LoadGridView()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Benefit
            dt = db.GetBenefit();

            // Load Gridview
            fm.LoadGridview(dgvMain, dt, "Benefit_ID");
        }

        /// <summary>
        /// Clear Select Index DGV
        /// </summary>
        private void ClearSelectIndex()
        {
            dgvMain.SelectedIndex = -1;
            btnEdit.Enabled = false;
            btnEdit.Attributes.Remove("onclick");
        }


        #endregion

    }
}