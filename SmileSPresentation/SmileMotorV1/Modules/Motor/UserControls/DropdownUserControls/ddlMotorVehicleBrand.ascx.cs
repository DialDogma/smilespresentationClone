using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucVehicleBrandDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _vehicleBrand_ID;
        private string _vehicleBrandDetail;
        private bool _autopostback;

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                // Get Value From DDL
                _autopostback = ddlVehicleBrand.AutoPostBack;
                return _autopostback;
            }
            set
            {
                // Set Value To DDL
                _autopostback = value;
                ddlVehicleBrand.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Property Get/Set Vehicle Brand Detail
        /// </summary>
        public string VehicleBrandDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _vehicleBrandDetail = ddlVehicleBrand.SelectedItem.Text;
                    return _vehicleBrandDetail;
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                // Set Value To DDL
                _vehicleBrandDetail = value;
                ddlVehicleBrand.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Vehicle Brand ID
        /// </summary>
        public int VehicleBrand_ID
        {
            get
            {

                try
                {
                    // Get Value From DDL
                    _vehicleBrand_ID = Convert.ToInt32(ddlVehicleBrand.SelectedValue);
                    return _vehicleBrand_ID;
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                // Set Value To DDL
                _vehicleBrand_ID = value;
                ddlVehicleBrand.SelectedValue = value.ToString();
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

        }
        #endregion

        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownList()
        {
            FunctionManager fn = new FunctionManager();
            VehicleManager vm = new VehicleManager();

            // Clear
            ddlVehicleBrand.Items.Clear();

            // Load Dropdown
            fn.LoadDropdownlist(ddlVehicleBrand, vm.GetVehicleBrand_ToDatatable(), "VehicleBrandDetail", "VehicleBrand_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlVehicleBrand.Enabled = _bool;
        }

        #endregion

    }
}