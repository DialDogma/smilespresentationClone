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
    public partial class ucJobType : System.Web.UI.UserControl
    {

        #region Declare
        private int _Occupation_ID;
        private string _OccupationDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Occupation ID
        /// </summary>
        public int Occupation_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _Occupation_ID = Convert.ToInt32(ddlJobType.SelectedValue);
                    return _Occupation_ID;
                }
                catch
                { return -1; }


            }
            set
            {
                // Set Value To DDL
                _Occupation_ID = value;
                ddlJobType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Occupation Detail
        /// </summary>
        public string OccupationDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _OccupationDetail = ddlJobType.SelectedItem.Text;
                    return _OccupationDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                // Set Value To DDL
                _OccupationDetail = value;
                ddlJobType.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlJobType.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                _AutoPostback = value;
                ddlJobType.AutoPostBack = value;
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
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Occupation
            dt = db.GetOccupation_Select();

            // Clear
            ddlJobType.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlJobType, dt, "OccupationDetail", "Occupation_ID",true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlJobType.Enabled = _bool;
        }
        #endregion

    }
}