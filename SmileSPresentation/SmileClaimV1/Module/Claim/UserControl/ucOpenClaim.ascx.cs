using SmileClaimV1.ClaimSeviceDataCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSGlobalClassLibrary.Class;
using System.Data;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucOpenClaim : System.Web.UI.UserControl
    {

        #region Declare
        private string phoneNumber;
        private DateTime dateIssue;
        private int claimTypeID;
        private int claimAdmitTypeID;
        private int chiefComplainID;
        private string moreDetail;

        /// <summary>
        /// Get / Set Phone Number
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                phoneNumber = txtPhoneNumber.Text.Trim();
                return phoneNumber;
            }

            set
            {
                txtPhoneNumber.Text = value;
            }
        }

        /// <summary>
        /// Get / Set Date Issue
        /// </summary>
        public DateTime DateIssue
        {
            get
            {
                dateIssue = ucDatepicker.DateSelected;
                return dateIssue;
            }

            set
            {
                ucDatepicker.DateSelected = value;
            }
        }

        /// <summary>
        /// Get / Set Claim Type
        /// </summary>
        public int ClaimTypeID
        {
            get
            {
                claimTypeID = int.Parse(ddlClaimType.SelectedValue);
                return claimTypeID;
            }

            set
            {
                ddlClaimType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Get / Set Claim Admit Type
        /// </summary>
        public int ClaimAdmitTypeID
        {
            get
            {
                claimAdmitTypeID = int.Parse(ddlClaimAdmitType.SelectedValue);
                return claimAdmitTypeID;
            }

            set
            {
                ddlClaimAdmitType.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// TODO : Get / Set Chief Complain ID
        /// </summary>
        public int ChiefComplainID
        {
            get
            {
                chiefComplainID = 1; // Mocup
                return chiefComplainID;
            }

            set
            {
                chiefComplainID = value;
            }
        }

        /// <summary>
        /// Get / Set More Detail
        /// </summary>
        public string MoreDetail
        {
            get
            {
                moreDetail = txtMoreDetail.Text;
                return moreDetail;
            }

            set
            {
                txtMoreDetail.Text = value;
            }
        }


        #endregion


        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void ddlClaimType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string claimTypeID = ddlClaimType.SelectedValue;
            if (claimTypeID != "-1")
            {
                LoadClaimAdmidType(Convert.ToInt32(claimTypeID));
            }
            else
            {
                DoClearClaimAdmitType();
            }

        }
        #endregion


        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownList(int productGroupID)
        {
            LoadClaimType(productGroupID);
            DoClearClaimAdmitType();
        }

        /// <summary>
        /// Do Load Detail
        /// </summary>
        public void DoLoadDetail()
        {
            //คือไร ? boom ถามเอง 
            // Get Claim Detail Data From WCF
            txtPhoneNumber.Text = "-";
            ucDatepicker.DateSelected = DateTime.Now;
            ddlClaimType.SelectedItem.Value = "";
            ddlClaimAdmitType.SelectedItem.Value = "";
            //selector.SelectedIndex = 0;
            txtMoreDetail.Text = "";
        }

        /// <summary>
        /// Load DDL
        /// </summary>
        private void LoadClaimType(int productGroupID)
        {
            FunctionManager fm = new FunctionManager();
            ClaimServiceClient client = new ClaimServiceClient();

            var lst = client.GetClaimType(productGroupID, true).ToList();
            fm.LoadDropdownlist(ddlClaimType, lst, "ClaimTypeDetail", "ClaimTypeID", true);

        }

        /// <summary>
        /// Load DDL
        /// </summary>
        private void LoadClaimAdmidType(int claimTypeID)
        {
            FunctionManager fm = new FunctionManager();
            ClaimServiceClient client = new ClaimServiceClient();

            var lst = client.GetClaimAdmidType(null, claimTypeID, true).ToList();

            fm.LoadDropdownlist<usp_ClaimAdmitType_Select_Result>(ddlClaimAdmitType, lst, "ClaimAdmitTypeDetail", "ClaimAdmitTypeID");

        }

        //private void LoadChiefComplain()
        //{
        //    ClaimServiceClient client = new ClaimServiceClient();

        //    var lst = client.GetChiefComplain(null, true);

        //    selector.DataSource = lst;
        //    selector.DataTextField = "ChiefComplainDetail";
        //    selector.DataValueField = "ChiefComplainID";

        //    ListItem requiredList;
        //    //Add Required list item
        //    requiredList = new ListItem();
        //    requiredList.Text = "--โปรดระบุ--";
        //    requiredList.Value = "-1";
        //    selector.Items.Insert(0, requiredList);
        //}

        private void DoClearClaimAdmitType()
        {
            ddlClaimAdmitType.Items.Clear();
            ddlClaimAdmitType.Items.Add(new ListItem("--โปรดระบุ--", "-1"));
        }
        #endregion

        public void DoDisable(bool val)
        {
            txtMoreDetail.Enabled = val;
            txtPhoneNumber.Enabled = val;
            ddlClaimAdmitType.Enabled = val;
            ddlClaimType.Enabled = val;
            ucDatepicker.IsEnabled(val);
            selector.Disabled = !val;
        }

        public string IsValidate()
        {
            string result = string.Empty;

            if (txtPhoneNumber.Text.Trim() == string.Empty)
            {
                result = "กรุณาระบุเบอร์โทรผู้ป่วย";
            }

            if (ddlClaimType.SelectedValue == "-1")
            {
                result = "กรุณาระบุประเภทการเคลม";
            }

            if (ddlClaimAdmitType.SelectedValue == "-1")
            {
                result = "กรุณาระบุประเภทการเข้ารักษา";
            }
            //TODO: Validate Chief Complain

            return result;
        }

    }
}