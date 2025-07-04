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
    public partial class ucJobTypeLevel : System.Web.UI.UserControl
    {

        #region Declare
        private int _OccupationLevel_ID;
        private string _OccupationLevelDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Occupation Level ID
        /// </summary>
        public int OccupationLevel_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _OccupationLevel_ID = Convert.ToInt32(ddlJobTypeLevel1.SelectedValue);
                    return _OccupationLevel_ID;
                }
                catch
                { return -1; }


            }

            set
            {
                // Set Value To DDL
                _OccupationLevel_ID = value;
                ddlJobTypeLevel1.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Occupation Level Detail
        /// </summary>
        public string OccupationLevelDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _OccupationLevelDetail = ddlJobTypeLevel1.SelectedItem.Text;
                    return _OccupationLevelDetail;
                }
                catch
                { return string.Empty; }

            }

            set
            {
                // Set Value To DDL
                _OccupationLevelDetail = value;
                ddlJobTypeLevel1.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                // Get DDL Auto Postback
                _AutoPostback = ddlJobTypeLevel1.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                // Set DDL Auto Postback
                _AutoPostback = value;
                ddlJobTypeLevel1.AutoPostBack = value;
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
        /// Do Load Dropdown List All
        /// </summary>
        public void DoLoadDropdownList()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Occupation Level
            dt = db.GetOccupationLevel_Select();

            // Load Dropdown
            fm.LoadDropdownlist(ddlJobTypeLevel1, dt, "OccupationLevelDetail", "OccupationLevel_ID");
        }

        /// <summary>
        /// Do Load Dropdown List By Occupation ID
        /// </summary>
        /// <param name="occupationID"></param>
        public void DoLoadDropdownlist(int occupationID)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Occupation Level
            dt = db.GetOccupationLevel_ByOccupation_Select(occupationID);

            // Clear
            ddlJobTypeLevel1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlJobTypeLevel1, dt, "OccupationLevelDetail", "OccupationLevel_ID", true);
        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void DoClear()
        {
            ddlJobTypeLevel1.Items.Clear();
            //ddlVehicleSegment.SelectedIndex = 1;
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlJobTypeLevel1.Enabled = _bool;
        }

        #endregion

    }
}