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
    public partial class ucProductTypeDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _ProductType_ID;
        private string _ProductTypeDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Product Type ID
        /// </summary>
        public int ProductType_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _ProductType_ID = Convert.ToInt32(ddlProductType.SelectedValue);
                    return _ProductType_ID;
                }
                catch
                { return -1; }


            }
            set
            {
                // Set Value To DDL
                _ProductType_ID = value;
                ddlProductType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Product Type Detail
        /// </summary>
        public string ProductTypeDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _ProductTypeDetail = ddlProductType.SelectedItem.Text;
                    return _ProductTypeDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                // Set Value To DDL
                _ProductTypeDetail = value;
                ddlProductType.SelectedItem.Text = value;
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
                _AutoPostback = ddlProductType.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                // Set Value To DDL
                _AutoPostback = value;
                ddlProductType.AutoPostBack = value;
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

            // Clear
            ddlProductType.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlProductType, db.GetProductType(), "ProductTypeDetail", "ProductType_ID", true);           
        }
        
        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlProductType.Enabled = _bool;
        }

        #endregion

    }
}