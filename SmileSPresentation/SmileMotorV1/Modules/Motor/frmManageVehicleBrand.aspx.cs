using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SmileSMotorClassLibrary;
using System.Drawing;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmManageVehicleBrand : System.Web.UI.Page
    {
        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //AddAttributeButton();
                LoadGridView();
                btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
            }

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearSelectIndex();
            LoadGridView();

        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void OnSelectIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvMain.Rows)
            {
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#CC3333");
                    row.ToolTip = string.Empty;

                    string vehiclebrand = dgvMain.DataKeys[row.RowIndex].Values[0].ToString();

                    FunctionManager fm = new FunctionManager();

                    string EncryptVehicleBrandID = fm.Base64Encrypt(vehiclebrand);
                    btnEdit.Enabled = true;
                    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + EncryptVehicleBrandID + "')");

                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
                }
            }
        }
        #endregion

        #region Method
        private void LoadGridView()
        {
            DataTable dt = new DataTable();
            FunctionManager fm = new FunctionManager();
            VehicleManager vm = new VehicleManager();

            dt = fm.ToDataTable(vm.GetVehicleBrand(1));

            fm.LoadGridview(dgvMain, dt, "VehicleBrand_ID");
        }

        private void ClearSelectIndex()
        {
            dgvMain.SelectedIndex = -1;
            btnEdit.Enabled = false;
            btnEdit.Attributes.Remove("onclick");
        }

        


        #endregion

    }
}