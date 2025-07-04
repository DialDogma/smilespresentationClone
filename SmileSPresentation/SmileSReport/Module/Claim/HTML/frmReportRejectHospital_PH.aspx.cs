using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSReport.PHClaimReportService;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportHospitalReject_PH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string claimHeaderCode = "";
                //Get ClaimHeaderID From QueryString
                if (Request.QueryString["CLID"] != "")
                {
                    //Reset Controls
                    claimHeaderCode = Request.QueryString["CLID"];
                }
                DoLoad(claimHeaderCode);

                //data test
                //DoLoad("CL6607000181");
            }
        }

        public void DoLoad(string claimHeaderCode)
        {
            var wcfClient = new ClaimReportServiceClient();
            var obj = wcfClient.ClaimRejectHospitalReport(claimHeaderCode).FirstOrDefault();
            var dateTitleFormat = "dd/MM/yyyy";
            var dateFormat = "dd-MM-yyyy";
            var currencyFormat = "#,##0.00";

            if (obj == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ไม่พบข้อมูล !!!')", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closeTab", "window.close();", true);
            }
            else
            {
                lblPrintFromDate.Text = DateTime.Now.ToString(dateTitleFormat);
                lblHospitalName.Text = obj.HospitalName == null ? "_____________________________" : obj.HospitalName;
                lblInvoiceNo.Text = string.IsNullOrEmpty(obj.InvoiceNo) ? "_____________________________" : obj.InvoiceNo;
                lblCustomerName.Text = obj.CustomerName == null ? "_____________________________" : obj.CustomerName;
                lblAdmitDate.Text = obj.AdmitDate == null ? "_____________________________" : obj.AdmitDate?.ToString(dateFormat);
                lblAmount.Text = obj.PaySS_Total?.ToString(currencyFormat) + " (" + obj.PaySS_TotalThaiBahtText.ToString() + ")";
                lblReceiveDocDate.Text = obj.ReceiveDocDate == null ? "_____________________________" : obj.ReceiveDocDate?.ToString(dateFormat);
                lblUnCoverAmount.Text = obj.UnPay_Total?.ToString(currencyFormat) + " (" + obj.UnPay_TotalThaiBahtText.ToString() + ")";
            }
        }
    }
}