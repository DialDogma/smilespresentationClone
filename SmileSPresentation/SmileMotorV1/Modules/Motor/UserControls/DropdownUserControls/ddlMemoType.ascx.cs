using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;


namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ucMemoTypeDropdownlist : System.Web.UI.UserControl
    {

#region Declare

        private int _memoTypeID;
        private string _memoTypeDetail;
        private bool _autoPostback;

        /// <summary>
        /// Property Get/Set Memo Type ID
        /// </summary>
        public int MemoTypeID
        {
            get
            {
                try 
                {
                    // Get Value From DDL
                    _memoTypeID = Convert.ToInt32(ddlMemoType1.SelectedValue);
                    return _memoTypeID;
                }
                catch
                {
                    return -1;
                }
            }

            set
            {
                // Set Value From DDL
                _memoTypeID = value;
                ddlMemoType1.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Memo Type Detail
        /// </summary>
        public string MemoTypeDetail
        {
            get 
            {
                try
                {
                    // Get Value From DDL
                    _memoTypeDetail = ddlMemoType1.SelectedItem.Text;
                    return _memoTypeDetail;
                }
                catch
                {
                    return string.Empty;
                }
            }

            set 
            {
                // Set Value From DDL
                _memoTypeDetail = value;
                ddlMemoType1.SelectedItem.Text = value;
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
                _autoPostback = ddlMemoType1.AutoPostBack;
                return _autoPostback;
            }
            set 
            {
                // Set Value From DDL
                _autoPostback = value;
                ddlMemoType1.AutoPostBack = value;
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

        public void OnSelectIndexChanged(object sender, EventArgs e)
        {
            // Set Value To Property
            _memoTypeID = Convert.ToInt32(ddlMemoType1.SelectedValue);
            _memoTypeDetail = ddlMemoType1.SelectedItem.Text;
        }


        #endregion


#region Method

        /// <summary>
        /// Do Load Dropdown List All
        /// </summary>
        public void DoLoadDropDownlistAll() 
        {
            FunctionManager fm = new FunctionManager();
            MotorDB db = new MotorDB();

            // Clear
            ddlMemoType1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlMemoType1, db.GetApplicationMemoType(), "MotorApplicationMemoTypeDetail", "MotorApplicationMemoType_ID", true,"ทั้งหมด");
        }

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropDownlist()
        {
            FunctionManager fm = new FunctionManager();
            MotorDB db = new MotorDB();

            // Clear
            ddlMemoType1.Items.Clear();

            // Load Dropdown 
            fm.LoadDropdownlist(ddlMemoType1, db.GetApplicationMemoType(), "MotorApplicationMemoTypeDetail", "MotorApplicationMemoType_ID");
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool) 
        {
            ddlMemoType1.Enabled = _bool;
        }

#endregion

    }
}