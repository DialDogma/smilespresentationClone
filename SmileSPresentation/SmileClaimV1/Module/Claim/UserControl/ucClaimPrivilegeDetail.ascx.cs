using SmileClaimV1.HCIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucClaimPrivilegeDetail : System.Web.UI.UserControl
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnHistory_Click(object sender, EventArgs e)
        {
            ModalHistory.Show();
        }
        #endregion


        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hospitalClaimInformID"></param>
        public void DoLoadDetail(int hospitalClaimInformID)
        {
            HCIServiceClient HCIClient = new HCIServiceClient();
            usp_HCIDetail_select_Result detail = new usp_HCIDetail_select_Result();

            detail = HCIClient.GetHCIDetail(hospitalClaimInformID);

            lblPrivilegeNo.Text = detail.HospitalClaimInformCode.ToString();
            lblStatus.Text = detail.HCIStatusDetail.ToString();
            lblClaimNo.Text = detail.ClaimHeaderID.ToString();
            lblHospital.Text = detail.OrganizeDetail;
            lblCreateDate.Text = detail.CreatedDate.Value.ToShortDateString();
            lblCreateBy.Text = detail.CreatedBy;

            lblClaimAdmitType.Text = detail.ClaimAdmitTypeDetail.ToString();
            lblClaimAdmitDate.Text = detail.DateIssue.Value.ToShortDateString();
            lblClaimType.Text = detail.ClaimTypeDetail.ToString();
            lblChiefComplain.Text = detail.ChiefComplainDetail.ToString();
            lblMoreDetail.Text = detail.Remark.ToString();

        }


        #endregion


    }
}