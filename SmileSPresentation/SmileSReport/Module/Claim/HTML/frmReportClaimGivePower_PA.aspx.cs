using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.PeerResolvers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSReport.PAClaimReportService;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportClaimGivePower_PA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string claimHeaderID = "";
                //Get ClaimHeaderID From QueryString
                if (Request.QueryString["CLID"] != "")
                {
                    //Reset Controls
                    claimHeaderID = Request.QueryString["CLID"];
                    DoLoad(claimHeaderID);
                }
            }
        }

        public void DoLoad(string claimHeaderID)
        {
            using (var db = new ClaimServiceClient())
            {
                var result = db.ClaimBookAssignPowerReport(claimHeaderID).FirstOrDefault();
                lblFullname.Text = " ..............................";
                //lblFirstName.Text = result.FirstName;
                //lblLastName.Text = result.LastName;
                lblHospital.Text = result.Location;
                lblAppId.Text = result.AppID;
                lblAddressNo.Text = result.AddressNo;
                lblTambol.Text = result.AddressTambol;
                lblAmphur.Text = result.AddressAmphur;
                lblProvince.Text = result.AddressProvince;
                lblCustomerCardId.Text = result.CustomerCardID;
                lblInsuranceCompany.Text = result.InsuranceCompany;
            }
        }
    }
}