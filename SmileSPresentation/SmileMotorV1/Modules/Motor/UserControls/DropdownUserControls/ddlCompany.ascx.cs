using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucCompanyDropdownList : System.Web.UI.UserControl
    {

        #region Declare 
        private int _SiamSmileCompany_ID;
        private string SiamSmileCompanyDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Company ID
        /// </summary>
        public int SiamSmileCompany_ID
        {
            get
            {
                try
                {
                    _SiamSmileCompany_ID = Convert.ToInt32(ddlCompany.SelectedValue);
                    return _SiamSmileCompany_ID;
                }
                catch
                { return -1; }


            }

            set
            {
                _SiamSmileCompany_ID = value;
                ddlCompany.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Company Detail
        /// </summary>
        public string SiamSmileCompanyDetail1
        {
            get
            {
                try
                {
                    SiamSmileCompanyDetail = ddlCompany.SelectedItem.Text;
                    return SiamSmileCompanyDetail;
                }
                catch
                { return string.Empty; }

            }

            set
            {
                SiamSmileCompanyDetail = value;
                ddlCompany.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlCompany.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                _AutoPostback = value;
                ddlCompany.AutoPostBack = value;
            }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Load DDl
                DoLoadDropdownlist();
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        void DoLoadDropdownlist()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlCompany.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlCompany, db.GetSiamSmileCompany(), "SiamSmileCompanyDetail", "SiamSmileCompany_ID",true);

        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlCompany.Enabled = _bool;
        }

        #endregion

    }
}