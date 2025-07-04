using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddEditPayMethod : System.Web.UI.UserControl
    {
        #region Declare

        private int _ProductID;
        private int _PeriodType_ID;
        private int _PayMethodType_ID;
        private int _PersonBank_ID;
        private int _PersonBankAccount_ID;
        private float _PremiumAmount;
        private float _PremiumTaxAmount;
        private float _PremiumDeliverAmount;

        /// <summary>
        /// BankID
        /// </summary>
        public int PersonBank_ID
        {
            get
            {
                _PersonBank_ID = ddlBankCompany.Bank_ID;
                return _PersonBank_ID;
            }

            set
            {
                _PersonBank_ID = value;
                ddlBankCompany.Bank_ID = value;
            }
        }

        /// <summary>
        /// Property Get/Set Product ID
        /// </summary>
        public int ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }

        /// <summary>
        /// Property Get/Set Period Type ID
        /// </summary>
        public int PeriodType_ID
        {
            get
            {
                _PeriodType_ID = Convert.ToInt32(ddlPeriodType.PeriodType_ID);
                return _PeriodType_ID;
            }

            set
            {
                _PeriodType_ID = value;
                ddlPeriodType.PeriodType_ID = value;
            }
        }

        /// <summary>
        /// Property Get/Set Pay Method Type ID
        /// </summary>
        public int PayMethodType_ID
        {
            get
            {
                _PayMethodType_ID = ddlPayMethodType.PayMethodTypeID;
                return _PayMethodType_ID;
            }

            set
            {
                _PayMethodType_ID = value;
                ddlPayMethodType.PayMethodTypeID = value;
            }
        }

        /// <summary>
        /// Property Get/Set Person Bank Account ID
        /// </summary>
        public int PersonBankAccount_ID
        {
            get
            {
                try
                {
                    _PersonBankAccount_ID = ddlBankAccountNo.PersonBankAccountID;

                    if (_PersonBankAccount_ID == -1)
                    {
                        _PersonBankAccount_ID = 1;
                    }
                    return _PersonBankAccount_ID;
                }
                catch
                {
                    return 1;
                }
            }

            set
            {
                _PersonBankAccount_ID = value;
                ddlBankAccountNo.PersonBankAccountID = value;
            }
        }

        /// <summary>
        /// Property Get/Set Premium Amount
        /// </summary>
        public float PremiumAmount
        {
            get
            {
                // Text ไม่ว่าง
                if (txtPremiumAmount.Text.Trim() != "")
                {
                    _PremiumAmount = Convert.ToSingle(txtPremiumAmount.Text);
                    return _PremiumAmount;
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                _PremiumAmount = value;
                txtPremiumAmount.Text = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Premium Tax Amount
        /// </summary>
        public float PremiumTaxAmount
        {
            get
            {
                // Text ไม่ว่าง
                if (txtPremiumAmount.Text.Trim() != "")
                {
                    _PremiumTaxAmount = Convert.ToSingle(txtPremiumTaxAmount.Text);
                    return _PremiumTaxAmount;
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                _PremiumTaxAmount = value;
                txtPremiumTaxAmount.Text = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Premium Deliver Amount
        /// </summary>
        public float PremiumDeliverAmount
        {
            get
            {
                // Text ไม่ว่าง
                if (txtPremiumAmount.Text.Trim() != "")
                {
                    _PremiumDeliverAmount = Convert.ToSingle(txtPremiumDeliverAmount.Text);
                    return _PremiumDeliverAmount;
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                _PremiumDeliverAmount = value;
                txtPremiumDeliverAmount.Text = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Payer ID
        /// </summary>
        public int PayerID
        {
            get
            {
                return Convert.ToInt32(hdfPayerID.Value);
            }

            set
            {
                hdfPayerID.Value = value.ToString();
            }
        }

        #endregion Declare

        #region RaiseEvent

        public event EventHandler ePeriodTypeSelectChanged;

        #endregion RaiseEvent

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Auto Postback DDL
                this.ddlPayMethodType.AutoPostback = true;
                this.ddlPeriodType.Autopostback = true;
                this.ddlBankCompany.AutoPostback = true;
            }
        }

        protected void ddlPayMethodType_SelectChanged(object sender, EventArgs e)
        {
            DataCenterDB db = new DataCenterDB();

            // Pay Method Type เป็น 3
            if (ddlPayMethodType.PayMethodTypeID == 3)
            {
                // โหลด DDL
                ddlBankCompany.DoloadDropdownList_ByPersonID_GroupByBankDetail(PayerID, MappingCode.eProductGroup.Motor);
                ddlBankCompany_eBankCompanySelect(new object(), new EventArgs());
                IsEnableDDL(true);
                ucAddBankAccount.ButtonAddAccount.Enabled = true;
            }
            // ถ้าเป็น เงินสด
            else if (ddlPayMethodType.PayMethodTypeID == (int)MappingCode.ePayMethodType.Cash)
            {
                // โหลด DDL
                ddlBankCompany.DoloadDropdownList_ByPersonID_GroupByBankDetail(PayerID, MappingCode.eProductGroup.Motor);
                ddlBankCompany.Bank_ID = (int)MappingCode.eBank.NotAvailable;

                ddlBankCompany_eBankCompanySelect(new object(), new EventArgs());

                ddlBankAccountNo.ddl.SelectedIndex = 1;
                IsEnableDDL(false);
                ucAddBankAccount.ButtonAddAccount.Enabled = false;
                //ADD BY BOOM 6-3-2562  14.25น.
                ddlPayMethodType.IsEnabled(false);
            }
            // ไม่ใช่
            else
            {
                // โหลด และ Set DDL
                ddlBankCompany.DoloadDropdownListAll(MappingCode.eProductGroup.Motor);
                ddlBankCompany.Bank_ID = (int)MappingCode.eBank.NotAvailable;
                ddlBankCompany_eBankCompanySelect(new object(), new EventArgs());
                ddlBankAccountNo.ddl.SelectedIndex = 0; // index 0 = n/a
                IsEnableDDL(false);
                ucAddBankAccount.ButtonAddAccount.Enabled = false;
                //ADD BY BOOM 6-3-2562  14.25น.
                ddlPayMethodType.IsEnabled(false);
            }
        }

        protected void ddlPeriodType_SelectChanged(object sender, EventArgs e)
        {
            EventHandler handler;

            _PeriodType_ID = ddlPeriodType.PeriodType_ID;

            // Handler Event
            handler = ePeriodTypeSelectChanged;

            if (handler != null)
            {
                handler(this, e);
            }

            //ถ้ามีการเลือกงวดชำระตามเงื่อนไข ดังนี้
            if (_PeriodType_ID == 2) // 2 คือ งวดชำระรายเดือน
            {
                ddlPayMethodType.PayMethodTypeID = 3; // 3 คือ หักผ่านธนาคาร
                ddlPayMethodType_SelectChanged(new object(), new EventArgs()); //เรียก Event ddlPayMethodType_SelectChanged
            }
            else if (_PeriodType_ID == 3) // 3 คืองวดชำระแบบรายปี
            {
                ddlPayMethodType.PayMethodTypeID = 2; // 2 คือ การชำระเงินสด
                ddlPayMethodType_SelectChanged(new object(), new EventArgs()); //เรียก Event ddlPayMethodType_SelectChanged
            }
            else
            {
                ddlPayMethodType.PayMethodTypeID = -1; // -1 คือโปรดระบุ
                ddlPayMethodType_SelectChanged(new object(), new EventArgs()); //เรียก Event ddlPayMethodType_SelectChanged
            }
        }

        protected void ddlBankCompany_eBankCompanySelect(object sender, EventArgs e)
        {
            // ไม่เลือก Bank Account
            if (ddlBankCompany.Bank_ID == -1)
            {
                if (ddlBankAccountNo.ddl.Items.Count > 0)
                {
                    ddlBankAccountNo.ddl.SelectedIndex = 0;
                }
            }
            // เลือก Back Account
            else
            {
                ddlBankAccountNo.DoloadDropdownList(MappingCode.eProductGroup.Motor, ucAddBankAccount.Person_ID, ddlBankCompany.Bank_ID);
            }
            ucAddBankAccount.ButtonAddAccount.Enabled = true; //เปิดปุ่ม เพิ่มเลขบัญชี
        }

        protected void ucAddBankAccount_eClickBankAccountSave(object sender, EventArgs e)
        {
            // โหลด DDL
            DoLoadDropdownlist(ucAddBankAccount.Person_ID);
        }

        #endregion Event

        #region method

        public void SetHeaderText(string txt)
        {
            lblHeader.Text = txt;
        }

        public void VisibleWarningANDPeriodANDPremium(bool value)
        {
            lblPeriod.Visible = value;
            ddlPeriodType.Visible = value;
            trPremium.Visible = value;
            lblWarning.Visible = value;
        }

        public void EnabledButtonAddBankAccount(bool value)
        {
            ucAddBankAccount.IsEnableButtonAddAccount(value);
        }

        /// <summary>
        /// Load Dropdown List
        /// </summary>
        public void DoLoadDropdownlist()
        {
            // Load DDL
            ddlPayMethodType.DoLoadDropdownList();
            ddlPeriodType.DoLoadDropdownList();
        }

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        /// <param name="_person_ID">Person ID</param>
        public void DoLoadDropdownlist(int _person_ID)
        {
            // Set And Load DDL
            PayerID = _person_ID;
            ddlBankCompany.DoloadDropdownList_ByPersonID_GroupByBankDetail(_person_ID, MappingCode.eProductGroup.Motor);
            ucAddBankAccount.Doload(_person_ID);
            ddlBankAccountNo.DoloadDropdownList(MappingCode.eProductGroup.Motor, _person_ID);
        }

        /// <summary>
        /// Set Enable DDL / Text
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabled(bool value)
        {
            ddlPayMethodType.IsEnabled(value);
            ddlBankCompany.IsEnabled(value);
            ddlPeriodType.IsEnabled(value);
            ddlBankAccountNo.IsEnabled(value);
            txtPremiumAmount.Enabled = value;
            txtPremiumTaxAmount.Enabled = value;
            txtPremiumDeliverAmount.Enabled = value;
        }

        /// <summary>
        /// Set Enable DDL Period Type
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnablePeriodType(bool value)
        {
            ddlPeriodType.IsEnabled(value);
        }

        /// <summary>
        /// Set Enable Pay Method Period Type
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnablePaymethodPeriodType(bool value)
        {
            ddlPayMethodType.IsEnabled(value);
            ddlPeriodType.IsEnabled(value);
        }

        /// <summary>
        /// Set Enable
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnableDDL(bool value)
        {
            ddlBankCompany.IsEnabled(value);
            ddlBankAccountNo.IsEnabled(value);
        }

        /// <summary>
        /// Set Enable Text Premium
        /// </summary>
        /// <param name="value"></param>
        public void IsEnabledPremium(bool value)
        {
            txtPremiumAmount.Enabled = value;
            txtPremiumDeliverAmount.Enabled = value;
            txtPremiumTaxAmount.Enabled = value;
        }

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="motorApplicationID">Motor Application ID</param>
        /// <param name="personID">Person ID</param>
        public void DoLoad(int motorApplicationID, int personID)
        {
            MotorDB db = new MotorDB();
            DataCenterDB mdb = new DataCenterDB();
            Premium pm = new Premium();
            PersonBankAccount pb = new PersonBankAccount();
            int BankTransfer;
            // mapping method type
            BankTransfer = (int)MappingCode.ePayMethodType.BankTransfer;

            // get motor premium
            pm = db.GetMotorApplicationPremium(motorApplicationID);

            // get bank account
            pb = mdb.GetBankAccountByBackAccountID(personID, pm.PersonBankAccount_ID);

            // set DDL
            ddlPayMethodType.PayMethodTypeID = pm.PayMethodType_ID;
            ddlPayMethodType_SelectChanged(new object(), new EventArgs());
            ddlPeriodType.PeriodType_ID = pm.PeriodType_ID;
            ddlPayMethodType.IsEnabled(PeriodType_ID != 3);
            //ถ้า PayMethodType == หักผ่านธนาคาร
            if (pm.PayMethodType_ID == BankTransfer)
            {
                ddlBankCompany.Bank_ID = pb.Bank_ID;

                ddlBankAccountNo.DoloadDropdownList(MappingCode.eProductGroup.Motor, personID, pb.Bank_ID);

                ddlBankAccountNo.PersonBankAccountID = pb.PersonBankAccount_ID;
            }
            else
            {
                ucAddBankAccount.IsEnableButtonAddAccount(false);
            }

            // set defualt
            txtPremiumAmount.Text = pm.PremiumAmount.ToString("0,0.00");
            txtPremiumTaxAmount.Text = pm.PremiumTaxAmount.ToString("0,0.00");
            txtPremiumDeliverAmount.Text = pm.PremiumDeliverAmount.ToString("0,0.00");
        }

        /// <summary>
        /// Do Load Bank Account
        /// </summary>
        /// <param name="_person_ID"></param>
        public void DoloadBankAccount(int _person_ID)
        {
            ucAddBankAccount.Doload(_person_ID);
        }

        /// <summary>
        /// Do Load Premium By Product
        /// </summary>
        /// <param name="product_ID"></param>
        /// <param name="period_ID"></param>
        public void DoLoadPremiumByProduct(int product_ID, int? period_ID = null)
        {
            Product pd = new Product();
            ProductManager pm = new ProductManager();

            pd = pm.GetProduct(product_ID);

            // รายเดือน
            if (period_ID == 2)
            {
                txtPremiumAmount.Text = pd.PremiumPerMonth.ToString("0,0.00");
                txtPremiumTaxAmount.Text = pd.PremiumPerMonth.ToString("0,0.00");
                txtPremiumDeliverAmount.Text = pd.PremiumPerMonth.ToString("0,0.00");
            }
            // รายปี
            else if (period_ID == 3)
            {
                txtPremiumAmount.Text = pd.PremiumPerYear.ToString("0,0.00");
                txtPremiumTaxAmount.Text = pd.PremiumPerYear.ToString("0,0.00");
                txtPremiumDeliverAmount.Text = pd.PremiumPerYear.ToString("0,0.00");
            }
            else
            {
                txtPremiumAmount.Text = "00.00";
                txtPremiumTaxAmount.Text = "00.00";
                txtPremiumDeliverAmount.Text = "00.00";
            }
        }

        /// <summary>
        /// Validate Pay Method
        /// </summary>
        /// <returns></returns>
        public string ValidatePayMethod()
        {
            string result;
            TransactionManager tm = new TransactionManager();

            // Valid From Class
            result = tm.Validate_PayMethod(PayMethodType_ID,
            PeriodType_ID,
            ddlBankCompany.Bank_ID,
            ddlBankAccountNo.PersonBankAccountID.ToString(),
            PremiumAmount,
            PremiumTaxAmount,
            PremiumDeliverAmount);

            return result;
        }

        /// <summary>
        /// Do Clear DDL Bank
        /// </summary>
        public void DoClearDDLBankCompany()
        {
            ddlBankCompany.Bank_ID = -1;
            ddlBankCompany_eBankCompanySelect(new object(), new EventArgs());
        }

        /// <summary>
        /// Do Clear Text And DDL
        /// </summary>
        public void DoClear()
        {
            this.ddlPayMethodType.PayMethodTypeID = -1;
            ddlPayMethodType_SelectChanged(new object(), new EventArgs());
            this.ddlPeriodType.PeriodType_ID = -1;
            this.txtPremiumAmount.Text = "0";
            this.txtPremiumDeliverAmount.Text = "0";
            this.txtPremiumTaxAmount.Text = "0";
        }

        /// <summary>
        /// สำหรับเทส
        /// </summary>
        public void SetDataTest()
        {
            ddlPeriodType.PeriodType_ID = 3;

            ddlPeriodType_SelectChanged(new Object(), new EventArgs());
            //ddlPayMethodType.PayMethodTypeID = 2;

            //ddlPayMethodType_SelectChanged(new Object(), new EventArgs());
            //ddlBankCompany.Bank_ID = 1;

            //ddlBankCompany_eBankCompanySelect(new Object(), new EventArgs());
            //ddlBankAccountNo.PersonBankAccountID = 1;

            //txtPremiumAmount.Text = "7,056.00";
            //txtPremiumTaxAmount.Text = "7,056.00";
            //txtPremiumDeliverAmount.Text = "7,056.00";
        }

        #endregion method
    }
}