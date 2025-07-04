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
    public partial class ucActiveStatusDropdownlist : System.Web.UI.UserControl
    {
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
        /// โหลด Dropdown list
        /// </summary>
        public void LoadDropdownlist()
        {
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            //add columns
            dt.Columns.Add("IsActiveDetail");
            dt.Columns.Add("IsActive", typeof(bool));

            //add row
            dt.Rows.Add("เปิดการใช้งาน", true);
            dt.Rows.Add("ปิดการใช้งาน", false);

            //clear items
            ddlActiveStatus.Items.Clear();

            //load dropdownist by function LoadDropdownlist
            fm.LoadDropdownlist(ddlActiveStatus, dt, "IsActiveDetail", "IsActive",true);

        }

        /// <summary>
        /// เปิด ปิด Dropdownlist
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlActiveStatus.Enabled = _bool;
        }

        #endregion

    }
}