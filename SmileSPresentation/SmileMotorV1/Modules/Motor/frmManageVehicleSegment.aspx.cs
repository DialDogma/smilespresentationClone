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
    public partial class frmManageVehicleSegment : System.Web.UI.Page
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearSelectIndex();
                LoadGirdView();
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

                    string segment_id = dgvMain.DataKeys[row.RowIndex].Values[0].ToString();

                    FunctionManager fm = new FunctionManager();

                    string EncryptSegmentID = fm.Base64Encrypt(segment_id);
                    btnEdit.Enabled = true;
                    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + EncryptSegmentID + "')");

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
            LoadGirdView();
            btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");

        }
        #endregion

        #region Method
        private void LoadGirdView()
        {

            DataTable dt = new DataTable();
            FunctionManager fn = new FunctionManager();
            VehicleManager vm = new VehicleManager();

            dt = fn.ToDataTable(vm.GetVehicleSegment());

            fn.LoadGridview(dgvMain, dt, "VehicleSegment_ID");

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
        //    dt.Columns.Add("Segment");
        //    dt.Columns.Add("Status");
        //    dt.Columns.Add("CreateBy");
        //    dt.Columns.Add("CreateDate");

        //    dt.Rows.Add("1", "รถเก๋ง", "True", "03525", "1-6-2560");
        //    dt.Rows.Add("2", "รถกระบะ", "True", "03525", "1-6-2560");
        //    dt.Rows.Add("3", "รถตู้", "True", "03525", "1-6-2560");
        //    dt.Rows.Add("4", "รถบรรทุก", "True", "03525", "1-6-2560");

        //    return dt;
        //}

        //private void AddAttributeButton()
        //{
        //    btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + _id + "','" + _userid + "')");
        //    btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
        //}

    }
}