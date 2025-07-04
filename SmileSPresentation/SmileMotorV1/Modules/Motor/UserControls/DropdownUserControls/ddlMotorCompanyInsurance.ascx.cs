using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucCompanyInsuranceDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _InsuranceCompany_ID;
        private string _InsuranceCompanyDetail;
        private bool _autoPostback;

        /// <summary>
        /// Property Get/Set Insurance Company ID
        /// </summary>
        public int InsuranceCompany_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    return Convert.ToInt32(ddlCompanyInsurance.SelectedValue);
                }
                catch
                { return -1;  }
            }

            set
            {
                // Set Value To DDL
                _InsuranceCompany_ID = value;
                ddlCompanyInsurance.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Insurance Company Detail
        /// </summary>
        public string InsuranceCompanyDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _InsuranceCompanyDetail = ddlCompanyInsurance.SelectedItem.Text;
                    return _InsuranceCompanyDetail;
                }
                catch
                {
                    return string.Empty;
                }
                
            }

            set
            {
                // Set Value To DDL
                _InsuranceCompanyDetail = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                // Get Value From DDL
                _autoPostback = ddlCompanyInsurance.AutoPostBack;
                return _autoPostback;
            }

            set
            {
                // Set Value To DDL
                _autoPostback = value;
                ddlCompanyInsurance.AutoPostBack = value;
            }
        }

        #endregion

        #region RaiseEvent
        public event EventHandler SelectChanged;

        protected virtual void OnSelectChanged(object sender, EventArgs e)
        {
            EventHandler handler = SelectChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
            // Set DDL Auto Postback
            this.ddlCompanyInsurance.AutoPostBack = this.AutoPostback;
        }
        #endregion

        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownList()
        {
            DataCenterDB dbCenter = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlCompanyInsurance.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlCompanyInsurance, dbCenter.GetInsuranceCompany(null,true), "InsuranceCompanyDetail", "InsuranceCompany_ID",true);

        }

        /// <summary>
        /// Do Load Dropdown List All
        /// </summary>
        public void DoLoadDropdownListAll()
        {
            DataCenterDB dbCenter = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlCompanyInsurance.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlCompanyInsurance, dbCenter.GetInsuranceCompany(null,null), "InsuranceCompanyDetail", "InsuranceCompany_ID", true, "--ทั้งหมด--");

        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void DoClear()
        {
            ddlCompanyInsurance.Items.Clear();
        }

        /// <summary>
        /// Set Enable
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlCompanyInsurance.Enabled = _bool;
        }
        #endregion

    }
}