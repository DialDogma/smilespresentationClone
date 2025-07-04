using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ucPeriodType : System.Web.UI.UserControl
    {
        #region Declare

        private int _periodType_ID;
        private string _periodTypeDetail;
        private bool _autopostback;

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool Autopostback
        {
            get
            {
                _autopostback = ddlPeriodType.AutoPostBack;
                return _autopostback;
            }
            set
            {
                _autopostback = value;
                ddlPeriodType.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Property Get/Set Period Type ID
        /// </summary>
        public int PeriodType_ID
        {
            get
            {
                _periodType_ID = Convert.ToInt32(ddlPeriodType.SelectedValue);
                return _periodType_ID;
            }

            set
            {
                _periodType_ID = value;
                ddlPeriodType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Period Type Detail
        /// </summary>
        public string PeriodTypeDetail
        {
            get
            {
                _periodTypeDetail = ddlPeriodType.SelectedValue;
                return _periodTypeDetail;
            }

            set
            {
                _periodTypeDetail = value;
                ddlPeriodType.SelectedValue = value;
            }
        }

        #endregion Declare

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

        #endregion RaiseEvent

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
        public void DoLoadDropdownList(string firstItemText = "")
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlPeriodType.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlPeriodType, db.GetPeriodType(), "PeriodTypeDetail", "PeriodType_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlPeriodType.Enabled = _bool;
        }
        public void FindByValue(string Id)
        {
            ddlPeriodType.SelectedItem.Selected = false;
            ddlPeriodType.Items.FindByValue(Id).Selected = true;
        }
        #endregion Method
    }
}