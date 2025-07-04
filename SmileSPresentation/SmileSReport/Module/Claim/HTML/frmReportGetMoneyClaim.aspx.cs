using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using SmileSReport.Model;
using SmileSReport.PHClaimReportService;
using Image = System.Web.UI.WebControls.Image;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportGetMoneyClaim : System.Web.UI.Page
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

        public void DoLoad(string claimHeaderId)
        {
            using (var db = new ClaimReportServiceClient())
            {
                var result = db.ReportClaimGetMoneyClaim(claimHeaderId);

                //if (result == null && result.PaySS_Total == null)
                if (result == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ไม่พบข้อมูล !!!')", true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "closeTab", "window.close();", true);
                }
                else
                {
                    lblPrintDate.Text = DateTime.Now.ToShortDateString();
                    //lblCusName.Text = result.CustName;
                    //lblCusName.Text = "............................................";
                    lblAppId.Text = result.App_id;
                    if (result.AmountNet != null)
                        {
                        lblSumClaimMoney.Text = (string.Format(("{0:n}"), result.AmountNet));
                        lblSumClaimMoneyWrite.Text = GlobalFunction.ThaiBahtText(result.AmountNet.ToString());
                    }
                    else
                    {
                        lblSumClaimMoney.Text = "0";
                        lblSumClaimMoneyWrite.Text = GlobalFunction.ThaiBahtText(0.ToString());
                    }
                    lblInsuranceCompany.Text = result.InsuranceCompany;
                    lblICD10.Text = result.ICD10;
                    lblHospitalDetail.Text = result.HospitalName;
                    //if (result.HospitalName == "")
                    //{
                    //    lblWriteAt.Text = "....................";
                    //}
                    //else
                    //{
                    lblWriteAt.Text = result.HospitalName;
                    //}

                    //var thumbnail = CreateThumbnail(100,100,"/Image/ViriyaLogo.png");
                    //if (result.InsuranceCompanyCode == "100000000005")
                    //{
                    //    imgLogo.ImageUrl = "/Image/bangkokSahaLogo.png";
                    //}
                    //else if (result.InsuranceCompanyCode == "100000000002")
                    //{
                    //    imgLogo.ImageUrl = "/Image/ViriyaLogo.png";
                    //}
                }
            }
        }
    }
}