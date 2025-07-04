using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucContractType : System.Web.UI.UserControl
    {
        #region Declare

        private int _contractType_ID;
        private string _contractTypeDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Contract Type ID
        /// </summary>
        public int ContractType_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _contractType_ID = Convert.ToInt32(ddlContractType.SelectedValue);
                    return _contractType_ID;
                }
                catch
                {
                    return -1;
                }
            }

            set
            {
                // Set Value
                _contractType_ID = value;
                ddlContractType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Contract Type Detail
        /// </summary>
        public string ContractTypeDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _contractTypeDetail = ddlContractType.SelectedItem.Text;
                    return _contractTypeDetail;
                }
                catch
                { return string.Empty; }
            }

            set
            {
                // Set Value To DDL
                _contractTypeDetail = value;
                ddlContractType.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                // Get Value From DDL
                _AutoPostback = ddlContractType.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                // Set Value To DDL
                _AutoPostback = value;
                ddlContractType.AutoPostBack = value;
            }
        }

        #endregion Declare

        #region RaiseEvent

        public event EventHandler eContractTypeSelectChanged;

        #endregion RaiseEvent

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void ddlContractType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = eContractTypeSelectChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropDownList()
        {
            FunctionManager fm = new FunctionManager();
            MotorDB db = new MotorDB();
            DataTable dt = new DataTable();

            // Get Motor Application Contract Type
            dt = db.GetMotorApplicationContractType();

            // Clear
            ddlContractType.Items.Clear();

            // Load Dropdown List
            fm.LoadDropdownlist(ddlContractType, dt, "MotorApplicationContractTypeDetail", "MotorApplicationContractType_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlContractType.Enabled = _bool;
        }

        /// <summary>
        ///RemoveItem
        /// </summary>
        /// <param name="val"></param>
        public void RemoveItemIndex(byte val)
        {
            ddlContractType.Items.RemoveAt(val);
        }

        #endregion Method
    }
}