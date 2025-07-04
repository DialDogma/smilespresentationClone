using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucMotorClaimTypeDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _MotorClaimType_ID;
        private string _MotorClaimTypeDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Motor Claim Type sID
        /// </summary>
        public int MotorClaimType_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _MotorClaimType_ID = Convert.ToInt32(ddlMotorClaimType.SelectedValue);
                    return _MotorClaimType_ID;
                }
                catch
                { return -1; }


            }
            set
            {
                // Set Value To DDL
                _MotorClaimType_ID = value;
                ddlMotorClaimType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Motor Claim Type Detail
        /// </summary>
        public string MotorClaimTypeDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _MotorClaimTypeDetail = ddlMotorClaimType.SelectedItem.Text;
                    return _MotorClaimTypeDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                // Set Value To DDL
                _MotorClaimTypeDetail = value;
                ddlMotorClaimType.SelectedItem.Text = value;
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
                _AutoPostback = ddlMotorClaimType.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                // Set Value To DDL
                _AutoPostback = value;
                ddlMotorClaimType.AutoPostBack = value;
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
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlMotorClaimType.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlMotorClaimType, db.GetMotorApplicationClaimType(null), "MotorApplicationClaimTypeDetail", "MotorApplicationClaimType_ID", true);
        }

        /// <summary>
        /// Set Eanable DDL
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlMotorClaimType.Enabled = _bool;
        }

        /// <summary>
        /// Set Default DDl Claim Type
        /// </summary>
        public void SetDefault()
        {
            ddlMotorClaimType.SelectedValue = "3"; //set ให้เป็น ซ่อมอู่
        }

        #endregion

    }
}