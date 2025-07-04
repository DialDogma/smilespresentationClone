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
    public partial class ucVehicleSegmentDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _VehicleSegment_ID;
        private string _VehicleSegmentDetail;
        private bool _autopostback;

        /// <summary>
        /// Property Get/Set AutoPostback
        /// </summary>
        public bool AutoPostback
        {
            get {
                _autopostback = ddlVehicleSegment.AutoPostBack;
                return _autopostback; 
            }
            set { _autopostback = value;
            ddlVehicleSegment.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Property Get/Set Vehicle Segment Detail
        /// </summary>
        public string VehicleSegmentDetail
        {
            get {
                try
                {
                    // Get Value From DDL
                    _VehicleSegmentDetail = ddlVehicleSegment.SelectedItem.Text;
                    return _VehicleSegmentDetail;
                }
                catch
                {
                    return string.Empty;
                }

            }
            set
            {
                // Set Value To DDL
                _VehicleSegmentDetail = value;
                ddlVehicleSegment.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Vehicle Segment ID
        /// </summary>
        public int VehicleSegment_ID
        {
            get {
                try
                {
                    // Get Value From DDL
                    _VehicleSegment_ID = Convert.ToInt32(ddlVehicleSegment.SelectedValue);
                    return _VehicleSegment_ID;
                }
                catch
                {
                    return -1;
                }
            }
            set {
                // Set Value To DDL
                _VehicleSegment_ID = value;
                ddlVehicleSegment.SelectedValue = value.ToString();
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
        /// <param name="vehicleType_id"></param>
        public void DoLoadDropdownListByVehicleTypeID(int vehicleType_id = 0)
        {
            DataTable dt = new DataTable();
            FunctionManager fn = new FunctionManager();
            VehicleManager vm = new VehicleManager();

            // Get Vehicle Segment
            dt = vm.GetVehicleSegmentByVehicleTypeID_ToDatatable(vehicleType_id);

            // Clear
            ddlVehicleSegment.Items.Clear();

            // ถ้า dt มากกว่า 0
            if (dt.Rows.Count > 0)
            {
                // Load Dropdown
                fn.LoadDropdownlist(ddlVehicleSegment, dt, "VehicleSegmentDetail", "VehicleSegment_ID",true);
            }
            else
            {
                ddlVehicleSegment.SelectedValue = "0";
            }
            
        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void DoClear()
        {
            ddlVehicleSegment.Items.Clear();
            //ddlVehicleSegment.SelectedIndex = 1;
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlVehicleSegment.Enabled = _bool;
        }
        #endregion


    }
}