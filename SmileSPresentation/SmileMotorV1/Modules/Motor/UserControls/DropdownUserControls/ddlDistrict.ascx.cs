using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlDistrict : System.Web.UI.UserControl
    {

        #region Declare
        private string _District_ID;
        private string _DistrictDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set District ID
        /// </summary>
        public string District_ID
        {
            get
            {
                // Get Value From DDL
                _District_ID = ddlDistrict1.SelectedValue;
                return _District_ID;
            }

            set
            {
                // Set Value To DDL
                _District_ID = value;
                ddlDistrict1.SelectedValue = value;
            }
        }

        /// <summary>
        /// Property Get/Set District Detail
        /// </summary>
        public string DistrictDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _DistrictDetail = ddlDistrict1.SelectedItem.Text;
                    return _DistrictDetail;
                }
                catch
                { return string.Empty; }

            }

            set
            {
                // Set Value To DDL
                _DistrictDetail = value;
                ddlDistrict1.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                // Get Auto Postback
                _AutoPostback = ddlDistrict1.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                // Set Auto Postback
                _AutoPostback = value;
                ddlDistrict1.AutoPostBack = value;
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
            if (Page.IsPostBack == false)
            {

            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Do Load Dropdown List All
        /// </summary>
        public void DoLoadDropDownList()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Load Dropdown
            fm.LoadDropdownlist(ddlDistrict1, db.GetDistrict(), "DistrictDetail", "District_ID");
        }

        /// <summary>
        /// Do Load Dropdown List By Province
        /// </summary>
        /// <param name="provinceID">Province ID</param>
        public void DoLoadDropDownList(string provinceID)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get District 
            dt = db.GetDistrictByProvinceID(Convert.ToInt32(provinceID));

            // Clear
            ddlDistrict1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlDistrict1, dt, "DistrictDetail", "District_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlDistrict1.Enabled = _bool;
        }

        /// <summary>
        /// Do Clear
        /// </summary>
        public void DoClear()
        {
            ddlDistrict1.Items.Clear();
        }

        #endregion

    }
}