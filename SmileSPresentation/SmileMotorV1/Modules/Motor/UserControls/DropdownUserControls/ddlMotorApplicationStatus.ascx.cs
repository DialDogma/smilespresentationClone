using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ddlMotorApplicationStatus : System.Web.UI.UserControl
    {

        #region Declare

        /// <summary>
        /// Property Get/Set Dropdown List
        /// </summary>
        public DropDownList ddl
        {
            get
            {
                return ddlStatus1;
            }
            set
            {
                ddlStatus1 = value;
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
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownlist()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlStatus1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlStatus1, db.GetApplicationStatus(), "MotorApplicationStatusDetail", "MotorApplicationStatus_ID",true);
        }

        /// <summary>
        /// Do Load Dropdown List All
        /// </summary>
        public void DoLoadDropdownlistAll()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlStatus1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlStatus1, db.GetApplicationStatus(), "MotorApplicationStatusDetail", "MotorApplicationStatus_ID", true, "ทั้งหมด");
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlStatus1.Enabled = _bool;
        }

        #endregion

    }
}