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
    public partial class frmManageVehicleType : System.Web.UI.Page
    {

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearSelectIndex();
                DoLoadDGV();
                btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
            }
            //dgvMain.DataSource = dt();
            //dgvMain.DataBind();

            //AddAttributeButton();

        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvMain.Rows)
            {
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#CC3333");
                    row.ToolTip = string.Empty;

                    string vehicletype_id = dgvMain.DataKeys[row.RowIndex].Values[0].ToString();

                    FunctionManager fm = new FunctionManager();

                    string EncryptVehicleTypeID = fm.Base64Encrypt(vehicletype_id);
                    btnEdit.Enabled = true;
                    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + EncryptVehicleTypeID + "')");

                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearSelectIndex();
            DoLoadDGV();
            btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
        }
        #endregion

        #region Method

        /// <summary>
        /// Load GridVeiw
        /// </summary>
        private void DoLoadDGV()
        {
            VehicleManager _v = new VehicleManager();
            FunctionManager fn = new FunctionManager();
            DataTable dt = new DataTable();

            dt = fn.ToDataTable(_v.GetVehicleType(0));

            fn.LoadGridview(dgvMain, dt, "VehicleType_ID");

        }

        private void ClearSelectIndex()
        {
            dgvMain.SelectedIndex = -1;
            btnEdit.Enabled = false;
            btnEdit.Attributes.Remove("onclick");
        }
        #endregion

        //private DataTable dt()
        //{
        //    DataTable dt;
        //    dt = new DataTable();

        //    dt.Columns.Add("ID");
        //    dt.Columns.Add("Type");
        //    dt.Columns.Add("Status");
        //    dt.Columns.Add("CreateBy");
        //    dt.Columns.Add("CreateDate");

        //    dt.Rows.Add("1", "รถยนต์", "True", "03525", "1-6-2560");
        //    dt.Rows.Add("2", "มอเตอร์ไซต์", "True", "03525", "1-6-2560");

        //    return dt;
        //}

        //private void AddAttributeButton()
        //{
        //    string _id = "testID123";
        //    string _userid = "testID123";
        //    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + _id + "','" + _userid + "')");
        //    btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
        //}


    }
}