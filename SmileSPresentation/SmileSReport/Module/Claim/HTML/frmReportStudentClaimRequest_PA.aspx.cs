using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSReport.PAClaimReportService;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportClaimRequest_PA:System.Web.UI.Page
    {
        protected void Page_Load(object sender,EventArgs e)
        {
            if(!IsPostBack)
            {
                string claimHeaderID = "";
                //Get ClaimHeaderID From QueryString
                if(Request.QueryString["CLID"] != null)
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

        public void DoLoad(string claimHeaderID)
        {
            using(var db = new ClaimServiceClient())
            {
                var currencyFormat = "#,##0.00";
                var result = db.ClaimExpensesPAReport(claimHeaderID).FirstOrDefault();

                if(result.ClaimNo != "")
                {
                    lblClaimHeaderId.Text = result.ClaimNo;
                }
                else
                {
                    lblClaimHeaderId.Text = "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;";
                }

                lblinsuranceCompanyHeader.Text = result.InsuranceCompany_id;
                lblAppId.Text = result.InsurancePolicyNum;
                lblDateActive.Text = result.DateReceive;
                lblSchoolName.Text = result.SchoolName;
                lblCusName.Text = result.CustomerFullName;
                if(result.Age != "")
                {
                    lblCusAge.Text = result.Age;
                }
                else
                {
                    lblCusAge.Text = "&emsp;";
                }
                if(result.IDPersonalCard != "")
                {
                    lblCusCardId.Text = result.IDPersonalCard;
                }
                else
                {
                    lblCusCardId.Text = "&emsp;&emsp;&emsp;&emsp;&emsp;";
                }
                if(result.StudentIDCard != "")
                {
                    lblCusSchoolCardId.Text = result.StudentIDCard;
                }
                else
                {
                    lblCusSchoolCardId.Text = "&emsp;&emsp;&emsp;&emsp;&emsp;";
                }
                if(result.CustGrade != "")
                {
                    lblSchoolClassDetail.Text = result.CustGrade;
                }
                else
                {
                    lblSchoolClassDetail.Text = "&emsp;&emsp;&emsp;";
                }
                if(result.PhoneNum != "")
                {
                    lblPhoneNum.Text = result.PhoneNum;
                }
                else
                {
                    lblPhoneNum.Text = "&emsp;&emsp;&emsp;&emsp;";
                }

                //lblSchoolClass.Text = result.

                lblFullAddressDetail.Text = result.Address;
                lblClaimAmount.Text = result.MoneyClaimExpenses.Value.ToString(currencyFormat);
                lblBillAmount.Text = result.Bill;
                lblClaimAmountText.Text = result.Spell;
                if(result.DateAccident != "")
                {
                    lblAccidentDate.Text = result.DateAccident;
                }
                else
                {
                    lblAccidentDate.Text = "&emsp;&emsp;&emsp;";
                }
                if(result.TimeAccident != "")
                {
                    lblAccidentTime.Text = result.TimeAccident;
                }
                else
                {
                    lblAccidentTime.Text = "&emsp;&emsp;&emsp;";
                }
                if(result.PlaceAccident != "")
                {
                    lblAccidentPlace.Text = result.PlaceAccident;
                }
                else
                {
                    lblAccidentPlace.Text = "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;";
                }
                if(result.AccidentDetail != "")
                {
                    lblICD10.Text = result.AccidentDetail;
                }
                else
                {
                    lblICD10.Text = "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;";
                }
            }
        }
    }
}