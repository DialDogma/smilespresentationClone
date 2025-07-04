using SmileClaimV1.ApplicationService_PH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucPHBenefit : System.Web.UI.UserControl
    {

        #region Declare
        private string _OPD_Accident_Over;
        private string _OPD_Sick_Over;
        private string _OPD_Sick_Over_Count;

        /// <summary>
        /// Get/Set Accident Over OPD
        /// </summary>
        public string OPD_Accident_Over
        {
            get
            {
                _OPD_Accident_Over = lblAccidentOverOPD.Text;
                return _OPD_Accident_Over;
            }

            set
            {
                lblAccidentOverOPD.Text = value;
            }
        }

        /// <summary>
        /// Get/Set Sick Over OPD
        /// </summary>
        public string OPD_Sick_Over
        {
            get
            {
                _OPD_Sick_Over = lblSickOverOPD.Text;
                return _OPD_Sick_Over;
            }

            set
            {

                lblSickOverOPD.Text = value;
            }
        }

        /// <summary>
        /// Get/Set Sick Over OPD Count
        /// </summary>
        public string OPD_Sick_Over_Count
        {
            get
            {
                _OPD_Sick_Over_Count = lblSickOverCount.Text;
                return _OPD_Sick_Over_Count;
            }

            set
            {
                lblSickOverCount.Text = value;
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
        /// Do Load Detail
        /// </summary>
        /// <param name="_appID"></param>
        public void DoLoadDetail(string _appID)
        {
            // Set Constructor PH Client
            ApplicationServiceClient client_PH = new ApplicationServiceClient();

            // Set Class
            usp_BenefitCoverrage_ByBenefitID_Select_Result Accident = new usp_BenefitCoverrage_ByBenefitID_Select_Result();
            usp_BenefitCoverrage_ByBenefitID_Select_Result Sick = new usp_BenefitCoverrage_ByBenefitID_Select_Result();

            // Get PH Benefit Data
            Accident = client_PH.GetCoverageByBenefitID(_appID, "1300").FirstOrDefault();
            Sick = client_PH.GetCoverageByBenefitID(_appID, "1400").FirstOrDefault();

            // Check Null Value
            if (Accident != null)
            {
                OPD_Accident_Over = String.Format("{0:N}", Accident.PricePerUnit);
            }
            else
            {
                OPD_Accident_Over = "-";
            }

            if (Sick != null)
            {
                OPD_Sick_Over = String.Format("{0:N}", Sick.PricePerUnit);
                OPD_Sick_Over_Count = Sick.MaxQuantity.ToString();
            }
            else
            {
                OPD_Sick_Over = "-";
                OPD_Sick_Over_Count = "-";
            }

        }
        #endregion
    }
}