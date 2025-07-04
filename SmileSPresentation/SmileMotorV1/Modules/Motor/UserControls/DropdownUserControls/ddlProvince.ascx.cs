using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlProvince : System.Web.UI.UserControl
    {


        #region Declare
        private string _Province_ID;
        private string _ProvinceDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Province ID
        /// </summary>
        public string Province_ID
        {
            get
            { 
                // Get Value From DDL
                _Province_ID = ddlProvince1.SelectedValue;
                    return _Province_ID;
            }
            set
            {
                // Set Value From DDL
                _Province_ID = value;
                ddlProvince1.SelectedValue = value;
            }
        }

        /// <summary>
        /// Property Get/Set Province Detail
        /// </summary>
        public string ProvinceDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _ProvinceDetail = ddlProvince1.SelectedItem.Text;
                    return _ProvinceDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                // Set Value From DDL
                _ProvinceDetail = value;
                ddlProvince1.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlProvince1.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                _AutoPostback = value;
                ddlProvince1.AutoPostBack = value;
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
                // Set DDL Auto Postback
                this.ddlProvince1.AutoPostBack = this.AutoPostback;
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

            // Get Province
            dt = db.GetProvince();

            // Clear
            ddlProvince1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlProvince1, dt , "ProvinceDetail", "Province_ID",true);        
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlProvince1.Enabled = _bool;
        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void DoClear()
        {
            ddlProvince1.Items.Clear();
        }

        #endregion


    }
}