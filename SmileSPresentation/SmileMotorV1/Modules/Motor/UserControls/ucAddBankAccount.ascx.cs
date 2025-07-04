using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddBankAccount : System.Web.UI.UserControl
    {
        #region Declare

        private int _person_ID;

        public int Person_ID
        {
            get
            {
                _person_ID = Convert.ToInt32(hdfPerson_ID.Value);
                return _person_ID;
            }
            set
            {
                _person_ID = value;
                hdfPerson_ID.Value = Convert.ToString(_person_ID);
            }
        }

        public Button ButtonAddAccount
        {
            get
            {
                return btnAdd;
            }

            set
            {
                btnAdd = value;
            }
        }

        #endregion Declare

        #region RaiseEvent

        public event EventHandler eClickBankAccountSave;

        #endregion RaiseEvent

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set button
            btnAdd.CssClass = "btn btn-success btn-xs";
            if (Page.IsPostBack == false)
            {
                this.ddlBankCompany1.DoloadDropdownListAll(MappingCode.eProductGroup.Motor);

                //btnAdd.Enabled = false;
            }

            // Set Width button
            //this.btnAdd.Width = Unit.Percentage(30);
            this.btnSave.Width = Unit.Percentage(30);
            this.btnCancel.Width = Unit.Percentage(30);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // valid
            if (Isvalidate() == true)
            {
                DoSave();
                DoClear();

                EventHandler handler;
                handler = eClickBankAccountSave;

                if (handler != null)
                {
                    handler(this, e);
                }
            }
            else
            {
                mpe.Show();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DoClear();
            mpe.Hide();
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridView(MappingCode.eProductGroup.Motor);
            mpe.Show();
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Validate Bank Account Name
        /// </summary>
        /// <returns></returns>
        private bool Isvalidate()
        {
            TransactionManager tm = new TransactionManager();
            string result;

            // Valid Value Text Box
            result = tm.Isvalidate_AddBankAccountNo(txtBankAccountName.Text, Convert.ToInt32(hdfPerson_ID.Value), Convert.ToInt32(ddlBankCompany1.Bank_ID), txtBankAccountNo.Text, true);

            // Check Text Result
            if (result != "")
            {
                lblWarning.Text = result;
                return false;
            }

            //TODO: validate Class

            return true;
        }

        /// <summary>
        /// Do Save Bank Account
        /// </summary>
        private void DoSave()
        {
            //Dosave
            DataCenterDB db = new DataCenterDB();

            db.AddBankAccountNo(txtBankAccountName.Text, txtBankAccountNo.Text, Convert.ToInt32(hdfPerson_ID.Value), ddlBankCompany1.Bank_ID, cFunction.GetCurrentLoginUser_ID(this.Page));
        }

        /// <summary>
        /// Do Clear Text
        /// </summary>
        private void DoClear()
        {
            txtBankAccountNo.Text = "";
            txtBankAccountName.Text = "";
            lblWarning.Text = "";
        }

        /// <summary>
        /// Do Load Bank Account
        /// </summary>
        /// <param name="_personID">Person ID</param>
        public void Doload(int _personID)
        {
            // Load UC
            hdfPerson_ID.Value = Convert.ToString(_personID);
            this.ucPersonDetail_mini1.DoLoad(_personID);
            LoadGridView(MappingCode.eProductGroup.Motor);
        }

        /// <summary>
        /// Load Bank Account To Gridview
        /// </summary>
        private void LoadGridView(MappingCode.eProductGroup eProductGroup)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
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
            dt = db.GetBankAccountByPersonIDandBankID(Convert.ToInt32(hdfPerson_ID.Value), null, null, null, null, productGroupID);

            fm.LoadGridview(dgvMain, dt);
        }

        public void IsEnableButtonAddAccount(bool value)
        {
            btnAdd.Enabled = value;
        }

        #endregion Method
    }
}