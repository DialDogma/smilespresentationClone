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
    public partial class frmManageVehicleModel : System.Web.UI.Page
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                this.ddlMotorVehicleType.AutoPostback = true;
                //this.ddlMotorVehicleSegment1.Autopostback = true;
                btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            ClearSelectIndex();
            LoadGridView();
        }

        protected void ddlMotorVehicleType1_SelectChanged(object sender, EventArgs e)
        {
            //ddlMotorVehicleSegment1.Autopostback = true;
            ddlMotorVehicleSegment.DoClear();
            ddlMotorVehicleSegment.DoLoadDropdownListByVehicleTypeID(ddlMotorVehicleType.VehicleType_ID);

            //Label1.Text = Convert.ToString( ddlMotorVehicleType1.VehicleType_ID);
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

                    string vehiclemodel = dgvMain.DataKeys[row.RowIndex].Values[0].ToString();
                    FunctionManager fm = new FunctionManager();

                    string EncryptVehicleModelID = fm.Base64Encrypt(vehiclemodel);
                    btnEdit.Enabled = true;
                    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + EncryptVehicleModelID + "')");
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
            FunctionManager fn = new FunctionManager();
            VehicleManager _vehicle = new VehicleManager();

            dt = fn.ToDataTable(_vehicle.GetVehicleModelByVehicleBrandAndVehicleSegment(ddlMotorVehicleBrand.VehicleBrand_ID, ddlMotorVehicleSegment.VehicleSegment_ID));

            fn.LoadGridview(dgvMain, dt, "VehicleModel_ID");
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