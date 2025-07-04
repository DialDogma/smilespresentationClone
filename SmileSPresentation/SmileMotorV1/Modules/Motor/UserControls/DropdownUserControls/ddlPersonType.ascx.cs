using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlPersonType : System.Web.UI.UserControl
    {

        #region DeClare
        private int _personTypeID;
        private string _personTypeDetail;
        private bool _autoPostback;

        /// <summary>
        /// Property Get/Set PersonType ID
        /// </summary>
        public int PersonTypeID
        {
            get
            {
                try
                {
                    _personTypeID = Convert.ToInt32(ddlPersonType1.SelectedValue);
                    return _personTypeID;
                }
                catch
                { return -1; }


            }
            set
            {
                _personTypeID = value;
                ddlPersonType1.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set PersonTypeDetail
        /// </summary>
        public string PersonTypeDetail
        {
            get
            {
                try
                {
                    _personTypeDetail = ddlPersonType1.SelectedItem.Text;
                    return _personTypeDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                _personTypeDetail = value;
                ddlPersonType1.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _autoPostback = ddlPersonType1.AutoPostBack;
                return _autoPostback;
            }
            set
            {
                _autoPostback = value;
                ddlPersonType1.AutoPostBack = value;
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

        #region RaisEvent
        public event EventHandler eSelectChanged;


        protected void ddlPersonType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = eSelectChanged;

            if (handler != null)
            {
                handler(this, e);
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
            ddlPersonType1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlPersonType1, db.GetPersonType(null), "PersonTypeDetail", "PersonType_ID", true);
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool"></param> 
        public void IsEnabled(bool _bool)
        {
            ddlPersonType1.Enabled = _bool;
        }
        #endregion

    }
}
