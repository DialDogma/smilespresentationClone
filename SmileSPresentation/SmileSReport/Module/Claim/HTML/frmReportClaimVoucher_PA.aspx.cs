using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSReport.PAClaimReportService;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportClaimVoucher_PA:System.Web.UI.Page
    {
        protected void Page_Load(object sender,EventArgs e)
        {
            if(!IsPostBack)
            {
                string claimHeaderID = "";
                //Get ClaimHeaderID From QueryString
                if(Request.QueryString["CLID"] != "")
                {
                    //Reset Controls
                    claimHeaderID = Request.QueryString["CLID"];
                    DoLoad(claimHeaderID);
                }
            }
        }

        public void DoLoad(string claimHeaderId)
        {
            using(var db = new ClaimServiceClient())
            {
                var currencyFormat = "#,##0.00";
                var result = db.ClaimVoucherReport(claimHeaderId).FirstOrDefault();

                lblHospitalName.Text = result.Hospital;
                lblCustomerName.Text = result.Customer;
                lblReference.Text = "AN." + result.AN + " HN." + result.HN + " VN." + result.VN;
                lblProduct.Text = result.Product;
                lblDateBetween.Text = result.fDateBetween;
                lblFirst.Text = result.ClaimType;
                lblClaimListFirst.Text = result.Net.Value.ToString(currencyFormat);
                lblCanClaimFirst.Text = result.Pay.Value.ToString(currencyFormat);
                lblClaimExcessFirst.Text = result.UnPay.Value.ToString(currencyFormat);
                lblCanClaimDiscount.Text = result.Discount;
                lblClaimExcessDiscount.Text = result.Discount;
                lblClaimListTotal.Text = result.Net.Value.ToString(currencyFormat);
                lblCanClaimTotal.Text = result.Pay.Value.ToString(currencyFormat);
                lblClaimExcessTotal.Text = result.UnPay.Value.ToString(currencyFormat);
                lblTotalFirstResult.Text = result.Pay_Total.Value.ToString(currencyFormat);
                lblTotalSecondResult.Text = result.Pay_Total.Value.ToString(currencyFormat);
                lblTotalThidResult.Text = result.CustomerPay.Value.ToString(currencyFormat);
                lblApproveName.Text = result.FullName;
                lblCustomerName2.Text = result.Customer;
                lblContactInfo.Text = result.ContactInfo;
                lblContactAddress1.Text = result.ContactAddress;
                lblContactAddress2.Text = result.ContactAddress2;
                lblContactAddress3.Text = result.ContactAddress3;
                lblRemark.Text = result.UnpayRemark;
                lblComment.Text = result.fClaimHeader;
            }
        }
    }
}