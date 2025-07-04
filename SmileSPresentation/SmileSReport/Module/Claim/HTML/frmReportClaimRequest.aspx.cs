using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSReport.PHClaimReportService;

namespace Web
{
    public partial class frmReportClaimRequest : System.Web.UI.Page
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
                }

                DoLoad(claimHeaderID);
            }
        }

        private void DoLoad(string claimHeaderID)
        {
            var wcfClient = new ClaimReportServiceClient();
            var obj = wcfClient.GetClaimRequestReport(claimHeaderID);
            var dateFormat = "dd/MM/yyyy";
            var currencyFormat = "#,##0.00";

            if (obj.PaySS_Total == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ไม่พบข้อมูล !!!')", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closeTab", "window.close();", true);
            }
            else
            {
                lblClaimID.Text = obj.ClaimHeaderID;
                lblProvince.Text = obj.Province;
                lblPeriod.Text = obj.StartCoverDate.Value.ToString(dateFormat);
                lblAgentName.Text = obj.CodeEmployeeName;
                lblProduct.Text = obj.Product;
                lblCompany.Text = obj.InsuranceCompany;
                lblCustName.Text = obj.CustName;
                lblAge.Text = obj.Age.ToString();
                lblAppID.Text = obj.App_id;
                lblAddressDetail.Text = obj.FullAddress;
                lblOccapation.Text = obj.Occupation;
                lblPhone.Text = obj.MobilePhoneNumber;
                lblworkname.Text = obj.workBuildingName;
                lblworkPhone.Text = obj.WorkPhoneNumber;
                lblDateHappen.Text = obj.DateHappen.Value.ToString(dateFormat);
                lbldateIn.Text = obj.Date_Issue.Value.ToString(dateFormat);
                lblDateOut.Text = obj.Date_End.Value.ToString(dateFormat);
                lblHospital.Text = obj.HospitalDetail;
                lblAccountName.Text = obj.PayerName;
                lblBank.Text = obj.Bank;
                lblBranch.Text = obj.BranchPaid;
                lblAccountNo.Text = obj.Account_id;
                lblnet.Text = obj.net.Value.ToString(currencyFormat);
                //lblPay.Text = obj.Pay_Total.Value.ToString(currencyFormat); //Kittisak Phanukitphaiboon 20221125
                lblPay.Text = (obj.PaySS_Total == null ? "0" : obj.PaySS_Total.Value.ToString(currencyFormat));
                lblBSID.Text = obj.ClaimHeaderGroup_id;
                lblClaimType.Text = obj.ClaimAdmitType;
                lblOPDNo.Text = obj.OPDCount.ToString();
                lblChiefComplain.Text = obj.ChiefComplain;
                lblICD10.Text = obj.Detail_Eng;
                lblDiagnosis.Text = obj.Detail_Thai;
                lblTransfer.Text = "";
            }
        }
    }
}