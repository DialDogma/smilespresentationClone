using SmileClaimV1.ApplicationService_PA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucPACustomerDetail : System.Web.UI.UserControl
    {

        #region Declare
        private string applicationID;
        private string customerID;

        /// <summary>
        /// Get / Set Application ID
        /// </summary>
        public string ApplicationID
        {
            get
            {
                applicationID = hdfApplicationID.Value;
                return applicationID;
            }

            set
            {
                hdfApplicationID.Value = value;
            }
        }

        /// <summary>
        /// Get / Set Customer ID
        /// </summary>
        public string CustomerID
        {
            get
            {
                customerID = hdfCustomerID.Value;
                return customerID;
            }

            set
            {
                hdfCustomerID.Value = value;
            }
        }

        #endregion


        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion


        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="customerID"></param>
        public void DoLoadDetail(string customerID)
        {
            // Set Constructor PH Client
            ApplicationServiceClient client_PA = new ApplicationServiceClient();

            // Set Class
            usp_CustomerDetail_Select_Result lst = new usp_CustomerDetail_Select_Result();

            lst = client_PA.GetCustomerDetail(customerID).FirstOrDefault();
            hdfCustomerID.Value = customerID;
            hdfApplicationID.Value = lst.App_id;
            // Bind Data To Object
            lblSchoolYear.Text = lst.ApplicationYear.ToString();
            lblName.Text = lst.FullName.ToString();
            lblAppID.Text = hdfApplicationID.Value;
            lblCustomerType.Text = lst.CustomerType.ToString();
            lblSchoolName.Text = lst.School.ToString();
            lblLevelRoom.Text = lst.LevelRoom.ToString();
            lblPosition.Text = lst.Position.ToString();
            lblStartCoverDate.Text = lst.StartCoverDate.Value.ToShortDateString();
            lblEndCoverDate.Text = lst.EndCoverDate.Value.ToShortDateString();
        }

        #endregion

    }
}