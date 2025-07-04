using SmileClaimV1.ApplicationService_PA;
using SmileClaimV1.HCIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim
{
    public partial class _09_DetailConfirmClaim : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string hospitalClaimInformID = Request.QueryString["hospitalClaimInformID"];

                // โหลดรายละเอียด
                DoLoadDetail(int.Parse(hospitalClaimInformID));
                setAttributeButton();
            }
        }

        private void DoLoadDetail(int hospitalClaimInformID)
        {

            HCIServiceClient HCIClient = new HCIServiceClient();
            ApplicationServiceClient clientPA = new ApplicationServiceClient();

            var detail = HCIClient.GetHCICustomerDetail(hospitalClaimInformID);

            ucClaimPrivilegeDetail.DoLoadDetail(detail.HospitalClaimInformID.Value);

            // Load Detail
            if (detail.ProductGroupID == 2) // PH
            {
                ucPHCustomerDetail.DoLoadDetail(detail.ApplicationID);
                ucPACustomerDetail.Visible = false;
            }

            if (detail.ProductGroupID == 3) // PA
            {
                ucPACustomerDetail.DoLoadDetail(detail.ReferenceID);
                ucPHCustomerDetail.Visible = false;
            }

        }

        private void setAttributeButton()
        {
            btnPrivilegeConfirmPrint.Attributes.Add("onclick", "javascript:window.open('http://49.231.178.253/SmilesReport/Module/Claim/HTML/frmClaimFormOPD.aspx')");
            btnPrint.Attributes.Add("onclick", "javascript:window.open('http://49.231.178.253/SmilesReport/Module/Claim/HTML/frmReportClaimVoucher.aspx?CLID=CL5710000057')");
        }

    }
}