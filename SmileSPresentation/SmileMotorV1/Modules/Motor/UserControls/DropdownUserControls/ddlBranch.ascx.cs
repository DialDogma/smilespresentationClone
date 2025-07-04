using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucBranchDropdownlist : System.Web.UI.UserControl
    {

#region Declare
        private int _branchID;
        private string _branchDetail;
        private bool _autoPostback;

        /// <summary>
        /// Property Get/Set Branch ID
        /// </summary>
        public int BranchID
        {
            get 
            {
                try
                {
                    _branchID = Convert.ToInt32(ddlBranch.SelectedValue);
                    return _branchID;
                }
                catch
                { return -1; }

                
            }

            set 
            { 
                _branchID = value;
                ddlBranch.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Branch Detail
        /// </summary>
        public string BranchDetail
        {
            get
            {
                try
                {
                    _branchDetail = ddlBranch.SelectedItem.Text;
                    return _branchDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                _branchDetail = value;
                ddlBranch.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _autoPostback = ddlBranch.AutoPostBack;
                return _autoPostback;
            }

            set
            {
                _autoPostback = value;
                ddlBranch.AutoPostBack = value;
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
            // Get DDL Value To Property
            _branchID = Convert.ToInt32(ddlBranch.SelectedValue);
            _branchDetail = ddlBranch.SelectedItem.Text;
        }

#endregion

#region Method

        /// <summary>
        /// Do Load Branch into Dropdownlist
        /// </summary>
        public void DoLoadDropDownlist(string firstItemText = "")
        {
            FunctionManager fm = new FunctionManager();
            DataCenterDB dc = new DataCenterDB();

            // Clear
            ddlBranch.Items.Clear();

            if (firstItemText != "")
            {
                // Load Dropdown
                fm.LoadDropdownlist(ddlBranch, dc.GetBranch(), "BranchDetail", "Branch_ID", true,firstItemText);
            }
            else
            {
                // Load Dropdown
                fm.LoadDropdownlist(ddlBranch, dc.GetBranch(), "BranchDetail", "Branch_ID", true);
            }

            
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlBranch.Enabled = _bool;
        }

        /// <summary>
        /// Setup Default Branch
        /// </summary>
        public void SetDefault()
        {
            BranchID = cFunction.GetCurrentBranchID(this.Page);
        }

#endregion
            
    }
}