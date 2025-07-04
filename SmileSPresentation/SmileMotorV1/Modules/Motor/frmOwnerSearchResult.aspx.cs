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
    public partial class frmOwnerSearchResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["txtsearch"] != string.Empty)
                {
                    
                }
                btnSearch.CssClass = "form-control btn-info";
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedID;
            int index = dgvMain.SelectedIndex;

            selectedID = dgvMain.Rows[index].Cells[0].Text; //dgvMain.DataKeys[index].Value.ToString();

            // Add script on page startup
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "ReturnValue(" +  selectedID + ");", true);
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }

        #region Method
        private void LoadGridView()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            dt = db.GetEmployeeSearch(txtSearch.Text.Trim());

            if (dt.Rows.Count < 20)
            {
                fm.LoadGridview(dgvMain, dt);
            }            
        }
        #endregion


    }
}