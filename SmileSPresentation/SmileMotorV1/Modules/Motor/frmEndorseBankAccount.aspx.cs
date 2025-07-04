using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileMotorV1.CommonUserControls;
using SmileSMotorClassLibrary;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmEndorseBankAccount : System.Web.UI.Page
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

                    //ถ้ามีการเลือกข้อมูล
                    if (dgvMain.SelectedIndex != -1)
                    {
                        dgvMain_SelectedIndexChanged(new object(), new EventArgs());
                    }
                    btnEdit.Width = Unit.Pixel(50);
                    btnCancel.Width = Unit.Pixel(50);
                    btnSave.Width = Unit.Pixel(180);
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ucAlert.SetMessage(ucAlert.AlertStatus.clear, "");
            //โหลด grigview
            ClearSelectIndex();

            // Setup uc Visible
            SetupUserControl(false);

            // Setup Btn Visible
            SetupButton(false);

            VisibleBankAccount(false);

            SetupButton(false);
            // Load
            LoadGridView();
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //เปลี่ยนหน้า paging
            dgvMain.PageIndex = e.NewPageIndex;

            this.ucAlert.SetMessage(ucAlert.AlertStatus.clear, "");

            ClearSelectIndex();
            // Setup uc Visible
            SetupUserControl(false);

            // Setup Btn Visible
            SetupButton(false);

            VisibleBankAccount(false);

            SetupButton(false);

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
                        MotorDB db = new MotorDB();

                        // Set Application ID
                        hdfAppID.Value = dataKeyAppId?[0].ToString();
                        _motorApplication_ID = Convert.ToInt32(hdfAppID.Value);

                        var paymethodType = db.GetMotorApplicationPremium(_motorApplication_ID).PayMethodType_ID;
                        if (paymethodType == 3) //3 คือหักผ่านธนาคาร
                        {
                            //โหลด uc appdatail โดย appid
                            this.ucAppDetailCustomer.DoLoad(_motorApplication_ID);
                            this.ucAppDetailPayer.DoLoad(_motorApplication_ID);
                            this.ucAppDetailVehicle.DoLoad(_motorApplication_ID);
                            this.ucAppDetailApplication.DoLoad(_motorApplication_ID);
                            this.ucAppDetailPayMethod.DoLoad((_motorApplication_ID));
                            LoadGridViewBankAccount(MappingCode.eProductGroup.Motor);
                            ddlBankCompany1.DoloadDropdownList(MappingCode.eProductGroup.Motor);
                            hdfBankID.Value = Convert.ToString(ucAppDetailPayMethod.BankID);
                            hdfBankAccountNo.Value = ucAppDetailPayMethod.BankAccountDetail;
                            ddlBankCompany1.Bank_ID = ucAppDetailPayMethod.BankID;
                            txtBankAccountNo.Text = ucAppDetailPayMethod.BankAccountDetail;
                            //แสดง uc
                            SetupUserControl(true);
                            VisibleBankAccount(true);
                            EnableBankAccount(false);
                            this.btnDetail.Visible = true;

                            // เก็บ Status ID
                            int status = ucAppDetailApplication.MotorApplicationStatus_ID;
                            _motorApplicationStatus_ID = status;

                            if (status == (int)MappingCode.eMotorApplicationStatus.Approve)
                            {
                                btnEdit.Visible = true;
                                btnCancel.Visible = true;
                            }
                            this.ucAlert.SetMessage(ucAlert.AlertStatus.clear, "");
                        }
                        else
                        {
                            this.ucAppDetailCustomer.Visible = false;
                            this.ucAppDetailPayer.Visible = false;
                            this.ucAppDetailVehicle.Visible = false;
                            this.ucAppDetailApplication.Visible = false;
                            this.btnSave.Visible = false;
                            this.btnEdit.Visible = false;
                            this.btnCancel.Visible = false;
                            this.btnDetail.Visible = true;
                            this.ucAppDetailPayMethod.Visible = true;
                            this.ucAppDetailPayMethod.DoLoad((_motorApplication_ID));
                            VisibleBankAccount(false);
                            dgvBankAccount.Visible = false;
                            //แจ้งเตือนไม่พบข้อมูล
                            this.ucAlert.SetMessage(ucAlert.AlertStatus.danger, "ไม่พบข้อมูลวิธีการชำระเงิน : หักผ่านธนาคาร");
                        }
                    }
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
                }

                SmileSMotorClassLibrary.FunctionManager fm = new SmileSMotorClassLibrary.FunctionManager();
                string motorApplicationCode = fm.Base64Encrypt(_motorApplication_ID.ToString());

                // Set Att Button
                btnDetail.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code=" + motorApplicationCode + "')");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                var db = new MotorV1Entities();
                var result = db.usp_MotorApplication_EndroseBankAccount(Convert.ToInt32(hdfAppID.Value)
                                                                        , ddlBankCompany1.Bank_ID
                                                                        , txtBankAccountNo.Text.Trim()
                                                                        , cFunction.GetCurrentLoginUser_ID(Page)).FirstOrDefault();

                if (result != null && result.IsResult == true)
                {
                    ucAlert.SetMessage(ucAlert.AlertStatus.success, result.Msg);
                    dgvMain.SelectedIndex = -1;
                }
                else
                {
                    if (result != null) ucAlert.SetMessage(ucAlert.AlertStatus.danger, result.Msg);
                    dgvMain.SelectedIndex = -1;
                }

                //โหลด grigview
                ClearSelectIndex();

                // Setup uc Visible
                SetupUserControl(false);

                // Setup Btn Visible
                SetupButton(false);

                VisibleBankAccount(false);
                SetupButton(false);

                // Load
                LoadGridView();
            }
        }

        protected void dgvBankAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridViewBankAccount(MappingCode.eProductGroup.Motor);
        }

        protected void btnEdit_OnClick(object sender, EventArgs e)
        {
            ddlBankCompany1.IsEnabled(true);
            txtBankAccountNo.Enabled = true;
            btnSave.Visible = true;
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            ddlBankCompany1.IsEnabled(false);
            txtBankAccountNo.Enabled = false;
            btnSave.Visible = false;
            ddlBankCompany1.Bank_ID = Convert.ToInt32(hdfBankID.Value);
            txtBankAccountNo.Text = hdfBankAccountNo.Value;
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load GridView
        /// </summary>
        private void LoadGridView()
        {
            MotorDB db = new MotorDB();
            SmileSMotorClassLibrary.FunctionManager fm = new SmileSMotorClassLibrary.FunctionManager();
            DataTable dt = new DataTable();

            //function get โดยคำค้นหา เก็บไว้ใน data table
            dt = db.GetMotorApplicationByInsuranceCompanyID(null, txtSearch.Text.Trim());

            //
            fm.LoadGridview(dgvMain, dt, "ApplicationID");
        }

        private void LoadGridViewBankAccount(MappingCode.eProductGroup eProductGroup)
        {
            DataCenterDB db = new DataCenterDB();
            SmileSMotorClassLibrary.FunctionManager fm = new SmileSMotorClassLibrary.FunctionManager();
            DataTable dt = new DataTable();
            int? productGroupID;

            switch (eProductGroup)
            {
                case MappingCode.eProductGroup.NotAvailable:
                    productGroupID = null;
                    break;

                default:
                    productGroupID = (int)eProductGroup;
                    break;
            }

            // Get Bank Account
            dt = db.GetBankAccountByPersonIDandBankID(ucAppDetailPayer.MotorApplication.Payer.Person_ID, null, null, null, null, productGroupID);

            fm.LoadGridview(dgvBankAccount, dt);
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
            this.ucAppDetailPayMethod.Visible = value;
            dgvBankAccount.Visible = value;
        }

        /// <summary>
        /// Set Up Defualt Visible Button And Label
        /// </summary>
        /// <param name="value"></param>
        private void SetupButton(bool value)
        {
            btnDetail.Visible = value;
            btnSave.Visible = value;
            btnEdit.Visible = value;
            btnCancel.Visible = value;
        }

        private void VisibleBankAccount(bool value)
        {
            lblAccount.Visible = value;
            ddlBankCompany1.Visible = value;
            lblBank.Visible = value;
            txtBankAccountNo.Visible = value;
        }

        private void EnableBankAccount(bool value)
        {
            ddlBankCompany1.IsEnabled(value);
            txtBankAccountNo.Enabled = value;
        }

        private bool IsValidate()
        {
            if (ddlBankCompany1.Bank_ID == -1 || ddlBankCompany1.Bank_ID == 2)
            {
                ucAlert.SetMessage(ucAlert.AlertStatus.danger, "กรุณาเลือกธนาคาร");
                return false;
            }
            else if (txtBankAccountNo.Text.Trim() == "")
            {
                ucAlert.SetMessage(ucAlert.AlertStatus.danger, "กรุณาระบุเลขที่บัญชี");
                return false;
            }
            else if (ddlBankCompany1.Bank_ID == Convert.ToInt32(hdfBankID.Value) && txtBankAccountNo.Text.Trim() == hdfBankAccountNo.Value)
            {
                ucAlert.SetMessage(ucAlert.AlertStatus.warning, "ไม่พบการแก้ไขข้อมูล");
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion Method
    }
}