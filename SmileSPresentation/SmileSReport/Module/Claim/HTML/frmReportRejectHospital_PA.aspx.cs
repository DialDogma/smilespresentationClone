using SmileSReport.PAClaimReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportHospitalReject_PA : System.Web.UI.Page
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
                //DoLoad("CLPA661100000195");
            }
        }
        public void DoLoad(string claimHeaderCode)
        {
            var wcfClient = new ClaimServiceClient();
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
                //lblInvoiceNo.Text = "";
                lblCustomerName.Text = obj.CustomerName == null ? "_____________________________" : obj.CustomerName;
                lblAdmitDate.Text = obj.DateIn == null ? "_____________________________" : obj.DateIn?.ToString(dateFormat);
                lblAmount.Text = obj.PaySS_Total?.ToString(currencyFormat) + " (" + obj.PaySS_TotalThaiBahtText.ToString() + ")";
                lblReceiveDocDate.Text = obj.ReceiveDocDate == null ? "_____________________________" : obj.ReceiveDocDate?.ToString(dateFormat);
                lblUnCoverAmount.Text = obj.Amount_UnPay?.ToString(currencyFormat) + " (" + obj.Amount_UnPayThaiBahtText.ToString() + ")";
            }
        }
    }
}