using SmileSReport.PHClaimReportService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportClaim
{
    public partial class frmReportClaim : System.Web.UI.Page
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

        public void DoLoad(string claimHeaderID)
        {
            var wcfClient = new ClaimReportServiceClient();
            var obj = wcfClient.GetClaimVoucherReport(claimHeaderID);
            var lst = wcfClient.GetClaimVoucherReportDetail(claimHeaderID);
            var dateFormat = "dd/MM/yyyy";
            var currencyFormat = "#,##0.00";

            if (obj.PaySS_Total == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ไม่พบข้อมูล !!!')", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closeTab", "window.close();", true);
            }
            else
            {
                //header
                lblHeader.Text = "หนังสือรับรองการจ่าย";
                lblCreateDate.Text = DateTime.Today.ToShortDateString();
                lblHospitalName.Text = obj.Hospital;
                lblCustomerName.Text = obj.CustomerName;
                lblReference.Text = obj.Reference;
                lblProduct.Text = obj.Product;
                lblDateBetween.Text = obj.DateIssue.Value.ToString(dateFormat) + " - " + obj.DateEnd.Value.ToString(dateFormat) + " " + obj.DateBetween;
                lblAppID.Text = obj.App_id;
                lblIDCard.Text = obj.CustomerZCard;

                //detail 1
                lblFirst.Text = lst[0].Benefit;
                lblClaimListFirst.Text = lst[0].Net.Value.ToString(currencyFormat);
                lblCanClaimFirst.Text = lst[0].Pay.Value.ToString(currencyFormat);
                lblClaimExcessFirst.Text = lst[0].UnPay.Value.ToString(currencyFormat);
                //detail 2
                lblSecend.Text = lst[1].Benefit;
                lblClaimListSecond.Text = lst[1].Net.Value.ToString(currencyFormat);
                lblCanClaimSecond.Text = lst[1].Pay.Value.ToString(currencyFormat);
                lblClaimExcessSecond.Text = lst[1].UnPay.Value.ToString(currencyFormat);
                //detail 3
                lblThird.Text = lst[2].Benefit;
                lblClaimListThird.Text = lst[2].Net.Value.ToString(currencyFormat);
                lblCanClaimThird.Text = lst[2].Pay.Value.ToString(currencyFormat);
                lblClaimExcessThird.Text = lst[2].UnPay.Value.ToString(currencyFormat);
                //detail 4
                lblForth.Text = lst[3].Benefit;
                lblClaimListForth.Text = lst[3].Net.Value.ToString(currencyFormat);
                lblCanClaimForth.Text = lst[3].Pay.Value.ToString(currencyFormat);
                lblClaimExcessForth.Text = lst[3].UnPay.Value.ToString(currencyFormat);
                //detail 5
                lblFifth.Text = lst[4].Benefit;
                lblClaimListFifth.Text = lst[4].Net.Value.ToString(currencyFormat);
                lblCanClaimFifth.Text = lst[4].Pay.Value.ToString(currencyFormat);
                lblClaimExcessFifth.Text = lst[4].UnPay.Value.ToString(currencyFormat);
                //detail 6
                lblSixth.Text = lst[5].Benefit;
                lblClaimListSixth.Text = lst[5].Net.Value.ToString(currencyFormat);
                lblCanClaimSixth.Text = lst[5].Pay.Value.ToString(currencyFormat);
                lblClaimExcessSixth.Text = lst[5].UnPay.Value.ToString(currencyFormat);

                if (lst.Count() >= 7)
                {
                    //detail 7
                    TRSeventh.Visible = true;
                    lblSeventh.Text = lst[6].Benefit;
                    lblClaimListSeventh.Text = lst[6].Net.Value.ToString(currencyFormat);
                    lblCanClaimSeventh.Text = lst[6].Pay.Value.ToString(currencyFormat);
                    lblClaimExcessSeventh.Text = lst[6].UnPay.Value.ToString(currencyFormat);
                }
                else
                {
                    TRSeventh.Visible = false;
                }

                //total payment
                lblCanClaimDiscount.Text = obj.Discount.Value.ToString(currencyFormat);
                lblClaimExcessDiscount.Text = obj.Discount.Value.ToString(currencyFormat);
                lblClaimListTotal.Text = obj.Net.Value.ToString(currencyFormat);
                lblCanClaimTotal.Text = obj.Pay.Value.ToString(currencyFormat);
                lblClaimExcessTotal.Text = obj.UnPay.Value.ToString(currencyFormat);
                //total text
                lblTotalFirst.Text = obj.TextAmount1;
                lblTotalSecond.Text = obj.TextAmount2;
                lblTotalThird.Text = obj.TextAmount3;
                lblTotalFourth.Text = obj.TextAmount4;
                lblTotalFifth.Text = obj.TextAmount5;
                lblTotalFirstResult.Text = obj.AmountTotal1.Value.ToString(currencyFormat);
                lblTotalSecondResult.Text = obj.AmountTotal2.Value.ToString(currencyFormat);
                lblTotalThidResult.Text = obj.AmountTotal3.Value.ToString(currencyFormat);
                lblTotalFourthResult.Text = obj.AmountTotal4.Value.ToString(currencyFormat);
                lblTotalFifthResult.Text = obj.AmountTotal5.Value.ToString(currencyFormat);

                lblCustomerCompensate.Text = obj.CustomerCompensate;
                //signature
                lblApproveName.Text = obj.ApproveName;
                lblCustomerName2.Text = obj.CustomerName;
                //address text
                lblContactInfo.Text = obj.ContactInfo;
                lblContactAddress1.Text = obj.ContactAddress1;
                lblContactAddress2.Text = obj.ContactAddress2;
                lblContactAddress3.Text = obj.ContactAddress3;
                //footer info
                if (obj.OverFromCompany != "")
                {
                    lblOverFromCompany.Text = "พิจารณาต่อจาก " + obj.OverFromCompany;
                }
                lblRemark.Text = obj.Remark;
                lblRemark.Style.Add("line-height", "1.5");
                lblComment.Text = obj.Comment;
            }
        }
    }
}