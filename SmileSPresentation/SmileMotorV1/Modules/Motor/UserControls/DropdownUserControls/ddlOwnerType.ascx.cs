using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlOwnerType : System.Web.UI.UserControl
    {

        #region Declare

        private int _ownerType_ID;
        private string _ownerTypeDetail;

        /// <summary>
        /// Property Get/Set Owner Type ID
        /// </summary>
        public int OwnerType_ID
        {
            get
            {
                try
                {
                    // Get Value From DLL
                    _ownerType_ID = Convert.ToInt32(ddlApplicationOwnerType.SelectedValue);
                    return _ownerType_ID;
                }
                catch
                {
                    return -1;
                }               
            }

            set
            {
                // Set Value From DLL
                _ownerType_ID = value;
                ddlApplicationOwnerType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Owner Type Detail
        /// </summary>
        public string OwnerTypeDetail
        {
            get
            {
                // Get Value From DLL
                _ownerTypeDetail = ddlApplicationOwnerType.SelectedItem.Text;
                return _ownerTypeDetail;
            }

            set
            {
                // Set Value From DLL
                _ownerTypeDetail = value;
                ddlApplicationOwnerType.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Dropdown List
        /// </summary>
        public DropDownList OwnerTypeEnable
        {
            get
            {
                return ddlApplicationOwnerType;
            }
            set
            {
                ddlApplicationOwnerType = value;
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
            DataTable dt = new DataTable();

            // Get Motor Application Owner Type
            dt = db.GetMotorApplicationOwnerTypeByID();

            // Clear
            ddlApplicationOwnerType.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlApplicationOwnerType, dt, "MotorApplicationOwnerTypeDetail", "MotorApplicationOwnerType_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="value"></param>
        public void IsEnabled(bool value)
        {
            ddlApplicationOwnerType.Enabled = value;
        }

        #endregion

    }
}