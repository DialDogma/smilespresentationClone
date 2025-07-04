using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ucPayMethodType : System.Web.UI.UserControl
    {
        #region Declare

        private int _PayMethodType_ID;
        private string _PayMethodTypeDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlPayMethodType.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                _AutoPostback = value;
                ddlPayMethodType.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Property Get/Set Pay Method TypeID
        /// </summary>
        public int PayMethodTypeID
        {
            get
            {
                _PayMethodType_ID = Convert.ToInt32(ddlPayMethodType.SelectedValue);
                return _PayMethodType_ID;
            }

            set
            {
                _PayMethodType_ID = value;
                ddlPayMethodType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Pay Method Type Detail
        /// </summary>
        public string PayMethodTypeDetail
        {
            get
            {
                _PayMethodTypeDetail = ddlPayMethodType.SelectedValue;
                return _PayMethodTypeDetail;
            }

            set
            {
                _PayMethodTypeDetail = value;
                ddlPayMethodType.SelectedValue = value;
            }
        }

        #endregion Declare

        #region RaiseEvent

        public event EventHandler SelectChanged;

        protected virtual void OnSelectChanged(object sender, EventArgs e)
        {
            EventHandler handler = SelectChanged;

            _PayMethodType_ID = Convert.ToInt32(ddlPayMethodType.SelectedValue);

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
        public void DoLoadDropdownList()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlPayMethodType.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlPayMethodType, db.GetPayMethodType(), "PayMethodTypeDetail", "PayMethodType_ID",true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlPayMethodType.Enabled = _bool;
        }

        #endregion Method
    }
}