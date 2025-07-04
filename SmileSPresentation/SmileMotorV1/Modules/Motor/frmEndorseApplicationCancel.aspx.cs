using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmEndorseApplicationCancel : System.Web.UI.Page
    {
        #region Declare

        private int _motorApplication_ID;
        private int _motorApplicationStatus_ID;

        /// <summary>
        /// Property Get/Set Motor Application ID
        /// </summary>
        public int MotorApplication_ID
        {
            get
            {
                return _motorApplication_ID;
            }

            set
            {
                _motorApplication_ID = value;
            }
        }

        /// <summary>
        /// Property Get/Set Motor Application Status ID
        /// </summary>
        public int MotorApplicationStatus_ID
        {
            get
            {
                return _motorApplicationStatus_ID;
            }

            set
            {
                _motorApplicationStatus_ID = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    // Setup uc Visible
                    SetupUserControl(false);

                    // Setup Btn Visible
                    SetupButton(false);

                    //Load DDL
                    LoadDropdownCause();

                    //ถ้ามีการเลือกข้อมูล
                    if (dgvMain.SelectedIndex != -1)
                    {
                        dgvMain_SelectedIndexChanged(new object(), new EventArgs());
                    }

                    btnWaitCancel.Width = Unit.Pixel(180);
                    btnReverse.Width = Unit.Pixel(180);
                    btnCancel.Width = Unit.Pixel(180);

                    ucDatepicker.DateSelected = DateTime.Now;
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //โหลด grigview
            ClearSelectIndex();

            // Setup uc Visible
            SetupUserControl(false);

            // Setup Btn Visible
            SetupButton(false);

            // Load
            LoadGridView();
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //เปลี่ยนหน้า paging
            dgvMain.PageIndex = e.NewPageIndex;
            ClearSelectIndex();
            LoadGridView();
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //attributes onclick Select on row
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //foreach row ใน dgvMain
            foreach (GridViewRow row in dgvMain.Rows)
            {
                //ถ้า rowIndex เท่ากับ SelectIndex ที่เลือก
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    //set สีเป็นสีแดงๆ set tooltip = string.empty
                    row.BackColor = ColorTranslator.FromHtml("#CC3333");
                    row.ToolTip = string.Empty;

                    // เก็บ applicationID จาก dataKeys ไว้ใน app_id
                    var dataKeyAppId = dgvMain.DataKeys[row.RowIndex]?.Values;
                    if (dataKeyAppId != null)
                    {
                        hdfAppID.Value = dataKeyAppId?[0].ToString();

                        // Set Application ID
                        _motorApplication_ID = Convert.ToInt32(hdfAppID.Value);

                        //โหลด uc appdatail โดย appid
                        this.ucAppDetailCustomer.DoLoad(_motorApplication_ID);
                        this.ucAppDetailPayer.DoLoad(_motorApplication_ID);
                        this.ucAppDetailVehicle.DoLoad(_motorApplication_ID);
                        this.ucAppDetailApplication.DoLoad(_motorApplication_ID);
                    }

                    //แสดง uc
                    this.ucAppDetailCustomer.Visible = true;
                    this.ucAppDetailPayer.Visible = true;
                    this.ucAppDetailVehicle.Visible = true;
                    this.ucAppDetailApplication.Visible = true;
                    this.btnDetail.Visible = true;

                    // เก็บ Status ID
                    int status = ucAppDetailApplication.MotorApplicationStatus_ID;
                    _motorApplicationStatus_ID = status;

                    if (status == (int)MappingCode.eMotorApplicationStatus.Approve)
                    {
                        this.lblDateCancel.Visible = true;
                        this.ucDatepicker.Visible = true;
                        this.lblDateCancel0.Visible = true;
                        this.ddlCancelCause.Visible = true;
                        this.txtRemark.Visible = true;
                        this.btnWaitCancel.Visible = true;
                        this.btnReverse.Visible = false;
                        this.btnCancel.Visible = true;
                    }
                    else if (status == (int)MappingCode.eMotorApplicationStatus.WaitCancel)
                    {
                        this.lblDateCancel.Visible = true;
                        this.ucDatepicker.Visible = true;
                        this.lblDateCancel0.Visible = true;
                        this.ddlCancelCause.Visible = true;
                        this.txtRemark.Visible = true;
                        this.btnWaitCancel.Visible = false;
                        this.btnReverse.Visible = true;
                        this.btnCancel.Visible = true;
                    }
                    else
                    {
                        this.lblDateCancel.Visible = false;
                        this.ucDatepicker.Visible = false;
                        this.lblDateCancel0.Visible = false;
                        this.ddlCancelCause.Visible = false;
                        this.txtRemark.Visible = false;
                        this.btnWaitCancel.Visible = false;
                        this.btnReverse.Visible = false;
                        this.btnCancel.Visible = false;
                    }
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
                }

                FunctionManager fm = new FunctionManager();
                string motorApplicationCode = fm.Base64Encrypt(_motorApplication_ID.ToString());

                // Set Att Button
                btnDetail.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code=" + motorApplicationCode + "')");
            }
        }

        protected void btnWaitCancel_OnClick(object sender, EventArgs e)
        {
            DoUpdate(Convert.ToInt32(hdfAppID.Value), (int)MappingCode.eMotorApplicationStatus.WaitCancel);
            LoadGridView();
            dgvMain_SelectedIndexChanged(new object(), new EventArgs());
        }

        /// <summary>
        /// BOOM เอง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReverse_Click(object sender, EventArgs e)
        {
            //Update Status 10 (10 คือ สถานะรอคืนสภาพ)
            using (var db = new MotorV1Entities())
            {
                var appId = Convert.ToInt32(hdfAppID.Value);
                var statusId = 10; //Status รอคืนสภาพ
                var userId = cFunction.GetCurrentLoginUser_ID(this);
                var causeId = Convert.ToInt32(ddlCancelCause.SelectedValue);
                var remark = txtRemark.Text.Trim();

                db.usp_MotorApplication_Status_UpdateV2(appId, statusId, userId, causeId, remark, null);
                LoadGridView();
                dgvMain_SelectedIndexChanged(new object(), new EventArgs());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                DoUpdate(Convert.ToInt32(hdfAppID.Value), (int)MappingCode.eMotorApplicationStatus.Cancel);
                LoadGridView();
                dgvMain_SelectedIndexChanged(new object(), new EventArgs());
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load GridView
        /// </summary>
        private void LoadGridView()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            //function get โดยคำค้นหา เก็บไว้ใน data table
            dt = db.GetMotorApplicationByInsuranceCompanyID(null, txtSearch.Text.Trim());

            //
            fm.LoadGridview(dgvMain, dt, "ApplicationID");
        }

        private void LoadDropdownCause()
        {
            using (var db = new MotorV1Entities())
            {
                var lst = db.usp_MotorApplication_CancelCuase_Select().ToList();

                ddlCancelCause.DataSource = lst;
                ddlCancelCause.DataTextField = "MotorApplicationCancelCuaseDetail";
                ddlCancelCause.DataValueField = "MotorApplicationCancelCuase_ID";
                ddlCancelCause.DataBind();

                //ADD INDEX 0
                ddlCancelCause.Items.Insert(0, new ListItem("**โปรดระบุ**", "-1"));
            }
        }

        /// <summary>
        /// Do Update Motor Application Status
        /// </summary>
        /// <param name="motorApplication_ID"></param>
        /// <param name="Status"></param>
        private void DoUpdate(int motorApplication_ID, int Status)
        {
            //MotorDB db = new MotorDB();
            using (var db = new MotorV1Entities())
            {
                // Check Button To Update - เช็คค่าจากปุ่มที่กด เพื่อเลือก Update
                // รอยกเลิก
                if (Status == (int)MappingCode.eMotorApplicationStatus.WaitCancel)
                {
                    var appId = Convert.ToInt32(hdfAppID.Value);
                    var updateStatus = (int)MappingCode.eMotorApplicationStatus.WaitCancel;
                    var userId = cFunction.GetCurrentLoginUser_ID(this);
                    var causeId = Convert.ToInt32(ddlCancelCause.SelectedValue);
                    var remark = txtRemark.Text.Trim();

                    var result = db.usp_MotorApplication_Status_UpdateV2(appId, updateStatus, userId, causeId, remark, null);
                }
                // สถานะ Cancel
                else
                {
                    var updateStatus_Cancel = (int)MappingCode.eMotorApplicationStatus.Cancel; //ยกเลิก
                    var updateStatus_ScheduledCancel = 12;//มีกำหนดยกเลิก
                    var appId = motorApplication_ID;
                    var userId = cFunction.GetCurrentLoginUser_ID(this);
                    var causeId = Convert.ToInt32(ddlCancelCause.SelectedValue);
                    var remark = txtRemark.Text.Trim();
                    // Get Date Current
                    var dateCurrent = DateTime.Now;
                    // Get Date From Uc Picker
                    var dateCancel = ucDatepicker.DateSelected.Value;

                    //IF Date Cancel > Date Current THEN UPDATE STATUS มีกำหนดยกเลิก(ID 12)
                    if (dateCancel > dateCurrent)
                    {
                        var result = db.usp_MotorApplication_Status_UpdateV2(appId, updateStatus_ScheduledCancel, userId, causeId, remark, dateCancel).First();
                    }
                    else
                    {
                        var result = db.usp_MotorApplication_Status_UpdateV2(appId, updateStatus_Cancel, userId, causeId, remark, dateCancel).First();
                    }
                }
            }
        }

        /// <summary>
        /// Clear Select Index DGV
        /// </summary>
        private void ClearSelectIndex()
        {
            dgvMain.SelectedIndex = -1;
            this.ucAppDetailCustomer.Visible = false;
            this.ucAppDetailPayer.Visible = false;
            this.ucAppDetailVehicle.Visible = false;
            this.ucAppDetailApplication.Visible = false;
        }

        /// <summary>
        /// Set Up Defualt Visible User Control
        /// </summary>
        /// <param name="value"></param>
        private void SetupUserControl(bool value)
        {
            this.ucAppDetailCustomer.Visible = value;
            this.ucAppDetailPayer.Visible = value;
            this.ucAppDetailVehicle.Visible = value;
            this.ucAppDetailApplication.Visible = value;
            this.ucDatepicker.Visible = value;
            ddlCancelCause.Visible = value; //ADD 6-7-2018 by boom
            txtRemark.Visible = value;  //ADD 6-7-2018 by boom
        }

        /// <summary>
        /// Set Up Defualt Visible Button And Label
        /// </summary>
        /// <param name="value"></param>
        private void SetupButton(bool value)
        {
            this.lblDateCancel.Visible = value;
            this.lblDateCancel0.Visible = value;
            this.btnDetail.Visible = value;
            this.btnWaitCancel.Visible = value;
            this.btnReverse.Visible = value;
            this.btnCancel.Visible = value;
        }

        #endregion Method

        private bool IsValidate()
        {
            if (ddlCancelCause.SelectedValue == "-1")
            {
                cFunction.AlertJavaScript(Page, "กรุณาเลือกสาเหตุการยกเลิก !");

                return false;
            }

            if (ucDatepicker.DateSelected == null)
            {
                cFunction.AlertJavaScript(Page, "กรุณาเลือกวันที่ยกเลิก");
                return false;
            }

            return true;
        }
    }
}