using SmileClaimV1.ApplicationService_PA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucPABenefit : System.Web.UI.UserControl
    {

        #region Declare
        private string _accident_Per_Time;
        private string _sick_Per_Time;

        /// <summary>
        /// Get/Set Accident Over OPD
        /// </summary>
        public string Accident_Per_Time
        {
            get
            {
                _accident_Per_Time = lblAccidentPerTime.Text;
                return _accident_Per_Time;
            }

            set
            {
                lblAccidentPerTime.Text = value;
            }
        }

        /// <summary>
        /// Get/Set Sick Over OPD
        /// </summary>
        public string Sick_Per_Time
        {
            get
            {
                _sick_Per_Time = lblSickPerTime.Text;
                return _sick_Per_Time;
            }

            set
            {

                lblSickPerTime.Text = value;
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
        /// Do Load Benefit PA Detail
        /// </summary>
        /// <param name="_appID"></param>
        public void DoLoadDetail(string _appID)
        {
            // Set Constructor PA Client
            ApplicationServiceClient client_PA = new ApplicationServiceClient();

            // Set Class
            usp_BenefitCoverage_ByBenefitID_Select_Result Accident = new usp_BenefitCoverage_ByBenefitID_Select_Result();
            usp_BenefitCoverage_ByBenefitID_Select_Result Sick = new usp_BenefitCoverage_ByBenefitID_Select_Result();

            // Get Benefit Data
            Accident = client_PA.GetCoverageByBenefitID(_appID, "2006").FirstOrDefault();
            Sick = client_PA.GetCoverageByBenefitID(_appID, "2020").FirstOrDefault();

            // Check Null Value
            if (Accident != null)
            {
                Accident_Per_Time = String.Format("{0:N}", Accident.Coverage);
            }
            else
            {
                Accident_Per_Time = "-";
            }

            if (Sick != null)
            {
                Sick_Per_Time = Sick.Coverage.ToString();
            }
            else
            {
                Sick_Per_Time = "-";
            }
                      
        }
        #endregion
    }
}