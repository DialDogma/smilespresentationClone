using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucBankCompany : System.Web.UI.UserControl
    {

        #region Declare
        private int _Bank_ID;
        private string _BankDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Bank ID
        /// </summary>
        public int Bank_ID
        {
            get
            {
                try
                {
                    _Bank_ID = Convert.ToInt32(ddlBankCompany.SelectedValue);
                    return _Bank_ID;
                }
                catch
                { return -1; }

            }
            set
            {
                _Bank_ID = value;
                ddlBankCompany.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Bank Detail
        /// </summary>
        public string BankDetail
        {
            get
            {
                try
                {
                    _BankDetail = ddlBankCompany.SelectedItem.Text;
                    return _BankDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                _BankDetail = value;
                ddlBankCompany.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlBankCompany.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                _AutoPostback = value;
                ddlBankCompany.AutoPostBack = value;
            }
        }

        #endregion

        #region RaiseEvent
        public event EventHandler eBankCompanySelect;
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void ddlBankCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            EventHandler handler;
            _Bank_ID = Convert.ToInt32(ddlBankCompany.SelectedValue);

            //Event handler 
            handler = eBankCompanySelect;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region Method

        /// <summary>
        /// โหลด dropdownlist ธนาคาร
        /// </summary>
        public void DoloadDropdownList(MappingCode.eProductGroup eProductGroup)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // เคลียร์ items
            ddlBankCompany.Items.Clear();

            // โหลด
            fm.LoadDropdownlist(ddlBankCompany, db.GetFinancialBank(), "BankDetail", "Bank_ID", true);

        }

        /// <summary>
        /// โหลด ddl โดย person_id groupby bankdetail
        /// </summary>
        /// <param name="_person_ID"></param>
        /// <param name="_bank_ID"></param>
        public void DoloadDropdownList_ByPersonID_GroupByBankDetail(int _person_ID, MappingCode.eProductGroup eProductGroup, int? _bank_ID = null)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
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

            // เคลียร์ items
            ddlBankCompany.Items.Clear();

            // โหลด
            fm.LoadDropdownlist(ddlBankCompany, db.GetBankAccountByPersonIDGroupByBankDetail(_person_ID, _bank_ID, null,null,null, productGroupID), "BankDetail", "Bank_ID", true);
        }

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoloadDropdownListAll(MappingCode.eProductGroup eProductGroup)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

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
            // เคลียร์ items
            ddlBankCompany.Items.Clear();

            // โหลด
            fm.LoadDropdownlist(ddlBankCompany, db.GetFinancialBank(productGroupID), "BankDetail", "Bank_ID");
        }
        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlBankCompany.Enabled = _bool;
        }

        #endregion

    }
}