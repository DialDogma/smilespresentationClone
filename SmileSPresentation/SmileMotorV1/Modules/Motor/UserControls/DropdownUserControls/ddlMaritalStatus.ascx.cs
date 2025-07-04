using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucMaritalStatus : System.Web.UI.UserControl
    {

        #region Declare
        private int _MaritalStatus_ID;
        private string _MaritalStatusDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Marital Status ID
        /// </summary>
        public int MaritalStatus_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _MaritalStatus_ID = Convert.ToInt32(ddlMaritalStatus.SelectedValue);
                    return _MaritalStatus_ID;
                }
                catch
                { return -1; }


            }
            set
            {
                // Set Value To DDL
                _MaritalStatus_ID = value;
                ddlMaritalStatus.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Marital Status Detail
        /// </summary>
        public string MaritalStatusDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _MaritalStatusDetail = ddlMaritalStatus.SelectedItem.Text;
                    return _MaritalStatusDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                // Set Value To DDL
                _MaritalStatusDetail = value;
                ddlMaritalStatus.SelectedItem.Text = value;
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
                _AutoPostback = ddlMaritalStatus.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                // Set Value To DDL
                _AutoPostback = value;
                ddlMaritalStatus.AutoPostBack = value;
            }
        }

        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }
        #endregion

        #region Method

        /// <summary>
        /// Do Load Dropdown list
        /// </summary>
        public void DoLoadDropdownList()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlMaritalStatus.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlMaritalStatus, db.GetMaritalStatus_Select(), "MaritalStatusDetail", "MaritalStatus_ID",true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlMaritalStatus.Enabled = _bool;
        }
        #endregion

    }
}