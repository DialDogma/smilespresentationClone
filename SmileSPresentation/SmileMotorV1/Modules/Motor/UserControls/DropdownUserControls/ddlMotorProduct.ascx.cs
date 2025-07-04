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
    public partial class ucProductDropdownList : System.Web.UI.UserControl
    {

        #region Declare
        private int _Product_ID;
        private string _ProductDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Dropdown Product
        /// </summary>
        public DropDownList ddlProduct
        {
            get
            {
                return ddlProduct1;
            }
            set
            {
                ddlProduct1 = value;
            }
        }

        /// <summary>
        /// Property Get/Set Product ID
        /// </summary>
        public int Product_ID
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _Product_ID = Convert.ToInt32(ddlProduct1.SelectedValue);
                    return _Product_ID;
                }
                catch
                { return -1; }


            }
            set
            {
                // Set Value To DDL
                _Product_ID = value;
                ddlProduct1.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Product Detail
        /// </summary>
        public string ProductDetail
        {
            get
            {
                try
                {
                    // Get Value From DDL
                    _ProductDetail = ddlProduct1.SelectedItem.Text;
                    return _ProductDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                // Set Value To DDL
                _ProductDetail = value;
                ddlProduct1.SelectedItem.Text = value;
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
                _AutoPostback = ddlProduct1.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                // Set Value To DDL
                _AutoPostback = value;
                ddlProduct1.AutoPostBack = value;
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

            // Load Dropdown
            fm.LoadDropdownlist(ddlProduct1, db.GetProduct(), "ProductDetail", "Product_ID", true);
        }

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        /// <param name="insuranceCompanyID">Insurance Company ID</param>
        /// <param name="productTypeID">Product Type ID</param>
        public void DoLoadDropdownList(int insuranceCompanyID, int productTypeID)
        {
            DataTable dt = new DataTable();
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Get Product
            dt = db.GetProductByProductGroupIDAndInsuranceCompanyID(productTypeID, insuranceCompanyID);

            // Clear
            ddlProduct1.Items.Clear();

            // ถ้า dt row มากกว่า 0
            if (dt.Rows.Count > 0)
            {
                // Load Dropdown
                fm.LoadDropdownlist(ddlProduct1, dt, "ProductDetail", "Product_ID", true);
            }
            else
            {
                // Load Dropdown
                fm.LoadDropdownlist(ddlProduct1, dt, "ProductDetail", "Product_ID", true);
            }
        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void DoClear()
        {
            ddlProduct1.Items.Clear();
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlProduct1.Enabled = _bool;
        }

        #endregion

    }
}