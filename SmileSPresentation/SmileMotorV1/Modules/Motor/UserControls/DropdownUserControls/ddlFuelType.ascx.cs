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
    public partial class ddlFuelType : System.Web.UI.UserControl
    {
        #region Declare

        private int _FuelType_ID;
        private string _FuelTypeDetail;
        private bool _AutoPostback;

        public int FuelType_ID
        {
            get
            {
                try
                {
                    _FuelType_ID = Convert.ToInt32(ddlFuelType1.SelectedValue);
                    return _FuelType_ID;
                }
                catch
                {
                    return -1;
                }
            }

            set
            {
                _FuelType_ID = value;
                ddlFuelType1.SelectedValue = value.ToString();
            }
        }

        public string FuelTypeDetail
        {
            get
            {
                _FuelTypeDetail = ddlFuelType1.SelectedItem.Text;
                return _FuelTypeDetail;
            }

            set
            {
                _FuelTypeDetail = value;
                ddlFuelType1.Text = value;
            }
        }

        public bool AutoPostback
        {
            get
            {
                // Get Auto Post Back DDL
                _AutoPostback = ddlFuelType1.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                // Set Auto Post Back DDL
                _AutoPostback = value;
                ddlFuelType1.AutoPostBack = value;
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

        public void DoLoadDropdownList()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            dt = db.GetFuelTypeByFuelTypeID();

            ddlFuelType1.Items.Clear();

            fm.LoadDropdownlist(ddlFuelType1, dt, "FuelTypeDetail", "FuelType_ID", true);
        }

        public void IsEnabled(bool _bool)
        {
            ddlFuelType1.Enabled = _bool;
        }

        #endregion Method
    }
}