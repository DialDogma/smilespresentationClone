using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucProductGroupDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _ProductGroup_ID;
        private string _ProductGroupDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Product Group ID
        /// </summary>
        public int ProductGroup_ID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ddlProductGroup.SelectedValue);
                }
                catch
                { return -1; }
            }

            set
            {
                _ProductGroup_ID = value;
            }
        }

        /// <summary>
        /// Property Get/Set Product Group Detail
        /// </summary>
        public string ProductGroupDetail
        {
            get
            {
                return _ProductGroupDetail;
            }

            set
            {
                _ProductGroupDetail = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                return _AutoPostback;
            }

            set
            {
                _AutoPostback = value;
            }
        }
        #endregion


        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load Dropdown
                DoLoadDropdownList();
            }

            // Set Auto Postback
            this.ddlProductGroup.AutoPostBack = this.AutoPostback;
        }

        #endregion


        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        void DoLoadDropdownList()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlProductGroup.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlProductGroup, db.GetProductGroup(), "ProductGroupDetail", "ProductGroup_ID",true);
            
        }

        /// <summary>
        /// Set Eanable DDL
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlProductGroup.Enabled = _bool;
        }

        #endregion
    }
}