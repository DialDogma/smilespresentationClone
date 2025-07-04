using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSReport.Model;
using SmileSReport.PHClaimReportService;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmReportRequestHistory : System.Web.UI.Page
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
                string empCode = "";
                //Get ClaimHeaderID From QueryString
                if (Request.QueryString["empCode"] != "")
                {
                    //Reset Controls
                    empCode = Request.QueryString["empCode"];
                }
                DoLoad(claimHeaderID, empCode);
            }
        }

        public void DoLoad(string claimHeaderId, string empCode)
        {
            using (var db = new ClaimReportServiceClient())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    var result = db.ReportClaimRequestHistory(claimHeaderId);
                    lblDatePrint.Text = dateNow.ToShortDateString();
                    lblHospitalName.Text = result.Hospital;
                    lblCustomerFullName.Text = result.CustName;
                    lblANHN.Text = result.AN + result.HN;
                    lblDateIn.Text = result.AdmitDate.Value.ToShortDateString();
                    lblDateCut.Text = result.LeaveDate.Value.ToShortDateString();
                    lblEmpName.Text = db.GetEmployeeDetail(empCode).EmployeeName; //TODO
                    lblDept.Text = "ฝ่ายพิจารณาสินไหม";
                }
                catch (Exception e)
                {
                    //  Block of code to handle errors
                }
            }
        }
    }
}