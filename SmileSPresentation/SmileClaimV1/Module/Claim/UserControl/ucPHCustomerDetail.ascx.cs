using SmileClaimV1.ApplicationService_PH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucPHCustomerDetail : System.Web.UI.UserControl
    {
        #region Declare
        private string applicationID;
        private string productID;
        private string insuranceCompanyID;
        private string statusID;

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

        public string ProductID
        {
            get
            {
                return productID;
            }

            set
            {
                productID = value;
            }
        }

        public string InsuranceCompanyID
        {
            get
            {
                return insuranceCompanyID;
            }

            set
            {
                insuranceCompanyID = value;
            }
        }

        public string StatusID
        {
            get
            {
                return statusID;
            }

            set
            {
                statusID = value;
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
        /// <param name="_appID"></param>
        public void DoLoadDetail(string _appID)
        {
            // Set Constructor PH Client
            ApplicationServiceClient client_PH = new ApplicationServiceClient();

            // Set Class
            usp_CustomerDetail_Select_Result lst = new usp_CustomerDetail_Select_Result();

            hdfApplicationID.Value = _appID;

            // Get Customer PH Data
            lst = client_PH.GetCustomerDetail(_appID);

            if (lst != null)
            {

                // Bind Data To Object
                lblAppID.Text = lst.ApplicationID.ToString();
                lblName.Text = lst.FullName.ToString();
                lblIdentityNumbers.Text = lst.IdentityNumber.ToString();
                lblBirthDate.Text = lst.BirthDate.Value.ToShortDateString();
                lblAge.Text = lst.Ages.ToString();
                lblStrartCoverDate.Text = lst.StartCoverDate.Value.ToShortDateString();
                lblStatus.Text = lst.Status.ToString();
                DiseaseExcept.Text = "-";
                lblRemarkDetail.Text = "-";
                lblRemarkDetail1.Text = "-";

                // Set Value
                ApplicationID = lst.ApplicationID.ToString();
                ProductID = "";
                InsuranceCompanyID = "";
                StatusID = "";
            }       
        }

        #endregion

    }
}