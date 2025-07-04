using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ucVehicleYearDropdownlist : System.Web.UI.UserControl
    {
        #region Declare

        private bool _AutoPostback;
        private string _vehicleYear;

        /// <summary>
        /// Property Get/Set Vehicle Year
        /// </summary>
        public string VehicleYear
        {
            get
            {
                _vehicleYear = ddlVehicleYear.SelectedValue;
                return _vehicleYear;
            }
            set
            {
                _vehicleYear = value;
                ddlVehicleYear.SelectedValue = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlVehicleYear.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                _AutoPostback = value;
                ddlVehicleYear.AutoPostBack = value;
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
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownList(int count = 25)
        {
            List<ListItem> ylst = new List<ListItem>();

            int yy;

            // Set Date
            yy = cDate.ToDate(DateTime.Now).Year;

            if (ddlVehicleYear.Items.Count == 0)
            {
                //Year
                ListItem li = new ListItem();
                li.Text = "ปี";
                li.Value = "0";

                for (int i = 0; i <= count; i++)
                {
                    ylst.Add(new ListItem(Convert.ToString(yy - i), Convert.ToString(yy - i)));
                    //ylist.Add(yStart);
                }

                // Bind Year
                ddlVehicleYear.DataSource = ylst;
                ddlVehicleYear.DataBind();

                ddlVehicleYear.Items.Insert(0, li);
                ddlVehicleYear.SelectedIndex = 0;
            }
        }

        public void IsEnabled(bool _bool)
        {
            ddlVehicleYear.Enabled = _bool;
        }

        #endregion Method
    }
}