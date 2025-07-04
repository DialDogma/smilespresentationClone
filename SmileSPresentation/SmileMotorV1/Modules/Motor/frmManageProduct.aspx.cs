using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileSMotorClassLibrary;


namespace SmileMotorV1.Modules.Motor
{
    public partial class frmManageProduct : System.Web.UI.Page
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load Dropdown List
                ddlMotorProductType.DoLoadDropdownList();
                ddlMotorCompanyInsurance.DoLoadDropdownList();

                // Set Att Button
                btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Set Att On Row Click
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvProduct, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow row in dgvProduct.Rows)
            {
                // Row Select Index
                if (row.RowIndex == dgvProduct.SelectedIndex)
                {
                    FunctionManager fm = new FunctionManager();

                    // Set Row Color
                    row.BackColor = ColorTranslator.FromHtml("#CC3333");
                    row.ToolTip = string.Empty;

                    // Get Datakey On Row Index
                    string _id = dgvProduct.DataKeys[row.RowIndex].Values[0].ToString();

                    // Encrypt ID
                    string EncryptID = fm.Base64Encrypt(_id);

                    // Set Att Button
                    btnEdit.Enabled = true;
                    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + EncryptID + "')");
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            // Load Gridview
            ClearSelectIndex();
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

            // Get Product
            dt = db.GetProductByProductGroupIDAndInsuranceCompanyID(ddlMotorProductType.ProductType_ID, ddlMotorCompanyInsurance.InsuranceCompany_ID);

            // Load Gridview
            fm.LoadGridview(dgvProduct, dt, "Product_ID");
        }

        /// <summary>
        /// Clear Select Index DGV
        /// </summary>
        private void ClearSelectIndex()
        {
            dgvProduct.SelectedIndex = -1;
            btnEdit.Enabled = false;
            btnEdit.Attributes.Remove("onclick");
        }
        #endregion

    }
}