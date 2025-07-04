using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucCustomerTitle : System.Web.UI.UserControl
    {

        #region Declare
        private int _Title_ID;
        private string _TitleDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Title ID
        /// </summary>
        public int Title_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _Title_ID = Convert.ToInt32(ddlTitle.SelectedValue);
                    return _Title_ID;
                }
                catch
                { return -1; }


            }

            set
            {
                // Set Value To DDL
                _Title_ID = value;
                ddlTitle.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Title Detail
        /// </summary>
        public string TitleDetail
        {
            get
            {
                try
                {
                    // Get Value To DDL
                    _TitleDetail = ddlTitle.SelectedItem.Text;
                    return _TitleDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                // Set Value To DDL
                _TitleDetail = value;
                ddlTitle.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                // Get Auto Post Back DDL
                _AutoPostback = ddlTitle.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                // Set Auto Post Back DDL
                _AutoPostback = value;
                ddlTitle.AutoPostBack = value;
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
            ddlTitle.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlTitle, db.GetTitle(null, (int)MappingCode.ePersonType.Individual), "TitleDetail", "Title_ID", true);
        }

        public void DoLoadDropdownList(int _personType_ID)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlTitle.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlTitle, db.GetTitle(null, _personType_ID), "TitleDetail", "Title_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlTitle.Enabled = _bool;
        }
        #endregion

    }
}