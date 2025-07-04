using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using SmileSReport.PAClaimReportService;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportGetMoneyClaim_PA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string claimHeaderID = "";
                //Get ClaimHeaderID From QueryString
                if (Request.QueryString["CLID"] != null)
                {
                    //Reset Controls
                    claimHeaderID = Request.QueryString["CLID"];
                    DoLoad(claimHeaderID);
                }
                else
                {
                    Response.StatusCode = 404;
                }
            }
        }

        public void DoLoad(string claimHeaderId)
        {
            using (var db = new ClaimServiceClient())
            {
                var currencyFormat = "#,##0.00";
                var result = db.ClaimPAReport(claimHeaderId).FirstOrDefault();
                lblPrintDate.Text = DateTime.Now.ToShortDateString();
                lblInsuranceCompany3.Text = result.InsuranceCompany;
                //lblCusName.Text = result.SchoolName;
                lblCusName.Text = "............................................";
                if (result.PolicyNo != null)
                {
                    lblAppId.Text = result.PolicyNo;
                }
                else
                {
                    lblAppId.Text = "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;";
                }

                lblSumClaimMoney.Text = result.Money.Value.ToString(currencyFormat);
                lblSumClaimMoneyWrite.Text = result.SpellMoney;
                lblInsuranceCompany.Text = result.InsuranceCompany;
                lblICD10.Text = "เลขที่เคลม " + result.ClaimID + " ผู้เอาประกัน " + result.CustomerDetailName;
                lblInsuranceCompany2.Text = result.InsuranceCompany;
                lblStartCoverDate.Text = result.StartDate;
                lblActionDate.Text = result.ActiveDate;
                lblEndCoverDate.Text = result.EndDate;
                lblHospitalDetail.Text = result.Hospital_Detail;
                if (result.ProductType != null)
                {
                    lblProductType.Text = result.ProductType;
                }
                else
                {
                    lblProductType.Text = "&emsp;&emsp;&emsp;&emsp;";
                }
                if (result.MoneyProduct != null)
                {
                    lblMoneyProduct.Text = result.MoneyProduct.ToString();
                }
                else
                {
                    lblMoneyProduct.Text = "&emsp;&emsp;&emsp;&emsp;";
                }
            }
        }
    }
}