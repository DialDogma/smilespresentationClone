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
    public partial class ucVehicleTypeDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _VehicleType_ID;
        private string _VehicleTypeDetail;
        private bool _autoPostback;

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get {
                _autoPostback = ddlVehicleType.AutoPostBack;
                return _autoPostback; 
            }
            set { 
                _autoPostback = value;
                ddlVehicleType.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Property Get/Set Vehicle Type ID
        /// </summary>
        public int VehicleType_ID
        {
            get { 
                    try{
                            _VehicleType_ID = Convert.ToInt32(ddlVehicleType.SelectedValue);
                            return _VehicleType_ID;
                    } 
                    catch{
                        return -1;
                    }
                }
            set { 
                _VehicleType_ID = value;
                ddlVehicleType.SelectedValue = value.ToString();
                }
        }

        /// <summary>
        /// Property Get/Set Vehicle Type Detail
        /// </summary>
        public string VehicleTypeDetail
        {
            get {
                try
                {
                    _VehicleTypeDetail = ddlVehicleType.SelectedItem.Text;
                    return _VehicleTypeDetail;
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { 
                _VehicleTypeDetail = value; 
                ddlVehicleType.Text = value; 
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
            // Set Auto Postback
            this.ddlVehicleType.AutoPostBack = this.AutoPostback;
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
            ddlVehicleType.Items.Clear();

            // Load Dropdown
            fn.LoadDropdownlist(ddlVehicleType, vm.GetVehicleType_ToDatatable(), "VehicleTypeDetail", "VehicleType_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlVehicleType.Enabled = _bool;
        }

        #endregion

    }
}