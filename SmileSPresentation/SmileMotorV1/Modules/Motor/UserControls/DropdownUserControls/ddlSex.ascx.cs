using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlSex : System.Web.UI.UserControl
    {

        #region DeClare
        private int _sexID;
        private string _sexDetail;
        private bool _autoPostback;

        /// <summary>
        /// Property Get/Set Sex ID
        /// </summary>
        public int SexID
        {
            get
            {
                try
                {
                    _sexID = Convert.ToInt32(ddlCustomerSex.SelectedValue);
                    return _sexID;
                }
                catch
                { return -1; }


            }
            set
            {
                _sexID = value;
                ddlCustomerSex.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Sex Detail
        /// </summary>
        public string SexDetail
        {
            get
            {
                try
                {
                    _sexDetail = ddlCustomerSex.SelectedItem.Text;
                    return _sexDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                _sexDetail = value;
                ddlCustomerSex.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _autoPostback = ddlCustomerSex.AutoPostBack;
                return _autoPostback;
            }
            set
            {
                _autoPostback = value;
                ddlCustomerSex.AutoPostBack = value;
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

            // Clear
            ddlCustomerSex.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlCustomerSex, db.GetSex(null), "SexDetail", "Sex_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlCustomerSex.Enabled = _bool;
        }
        #endregion

    }
}