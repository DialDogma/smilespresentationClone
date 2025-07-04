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
    public partial class ddlZebra : System.Web.UI.UserControl
    {
        #region Declare

        private int _zebra_ID;
        private string _zebraNo;

        /// <summary>
        /// Property Get/Set Owner Type ID
        /// </summary>
        public int Zebra_ID
        {
            get
            {
                try
                {
                    // Get Value From DLL
                    _zebra_ID = Convert.ToInt32(ddlApplicationZebra.SelectedValue);
                    return _zebra_ID;
                }
                catch
                {
                    return -1;
                }
            }

            set
            {
                // Set Value From DLL
                _zebra_ID = value;
                ddlApplicationZebra.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Owner Type Detail
        /// </summary>
        public string ZebraNo
        {
            get
            {
                // Get Value From DLL
                _zebraNo = ddlApplicationZebra.SelectedItem.Text;
                return _zebraNo;
            }

            set
            {
                // Set Value From DLL
                _zebraNo = value;
                ddlApplicationZebra.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Dropdown List
        /// </summary>
        public DropDownList ZebraEnable
        {
            get
            {
                return ddlApplicationZebra;
            }
            set
            {
                ddlApplicationZebra = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Dropdown list
        /// </summary>
        public void DoLoadDropdownList()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Motor Application Owner Type
            dt = db.GetZebra();

            // Clear
            ddlApplicationZebra.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlApplicationZebra, dt, "ZebraOwnerDetail", "Zebra_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="value"></param>
        public void IsEnabled(bool value)
        {
            ddlApplicationZebra.Enabled = value;
        }

        #endregion Method
    }
}