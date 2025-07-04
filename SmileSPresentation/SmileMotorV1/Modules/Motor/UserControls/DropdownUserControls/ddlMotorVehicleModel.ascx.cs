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
    public partial class ucVehicleModelDropdownList : System.Web.UI.UserControl
    {

        #region Declare

        private int _VehicleModel_ID;
        private string _VehicleModelDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {

                _AutoPostback = ddlVehicleModel.AutoPostBack;
                return _AutoPostback;
            }

            set
            {

                _AutoPostback = value;
                ddlVehicleModel.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Property Get/Set Vehicle Model Detail
        /// </summary>
        public string VehicleModelDetail
        {
            get {
                try
                {
                    _VehicleModelDetail = ddlVehicleModel.SelectedItem.Text;
                    return _VehicleModelDetail;
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { 
                _VehicleModelDetail = value;
                ddlVehicleModel.SelectedItem.Text = value;
                }
        }

        /// <summary>
        /// Property Get/Set Vehicle Model ID
        /// </summary>
        public int VehicleModel_ID
        {
            get {
                try
                {
                    _VehicleModel_ID = Convert.ToInt32(ddlVehicleModel.SelectedValue);
                    return _VehicleModel_ID;
                }
                catch
                {
                    return -1;
                }
            }
            set { 
                _VehicleModel_ID = value;
                ddlVehicleModel.SelectedValue = value.ToString();
                }
        }

        #endregion


        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
               
            }

            this.ddlVehicleModel.AutoPostBack = this.AutoPostback;
        }
        #endregion


        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        /// <param name="vehicleSegment_id">Vehicle Segment ID</param>
        /// <param name="vehicleBrand_id">Vehicle Brand ID</param>
        public void DoLoadDropdownListByVehicleSegmentandVehicleBrand(int vehicleSegment_id = 0, int vehicleBrand_id = 0)
        {
            DataTable dt = new DataTable();
            FunctionManager fn = new FunctionManager();
            VehicleManager vm = new VehicleManager();

            // Get Vehicle Model
            dt = vm.GetVehicleModelByVehicleBrandAndVehicleSegment_ToDatatable(vehicleBrand_id, vehicleSegment_id);

            // Clear
            ddlVehicleModel.Items.Clear();

            // ถ้า dt row มากกว่า 0
            if (dt.Rows.Count > 0)
            {
                // Load Dropdown
                fn.LoadDropdownlist(ddlVehicleModel, dt, "VehicleModelDetail", "VehicleModel_ID",true);
            }
            else
            {
                ddlVehicleModel.SelectedValue = "0";
            }
        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void Doclear()
        {
            ddlVehicleModel.Items.Clear();
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlVehicleModel.Enabled = _bool;
        }
        #endregion

    }
}