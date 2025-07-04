using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    string hciID = "";

            //    if (Request.QueryString["HCIID"] != "")
            //    {
            //        DoLoadDetail(int.Parse(hciID));
            //    }

            //    DoLoadDetail(60); // Mocha ไว้
            //}
        }

        /// <summary>
        /// Do Load Report Detail
        /// </summary>
        /// <param name="hciID"></param>
        //private void DoLoadDetail(int hciID)
        //{
        //    HCIServiceClient client = new HCIServiceClient();
        //    usp_HCIClaimForm_Select_Result detail = new usp_HCIClaimForm_Select_Result();

        //    detail = client.GetHCIClaimForm(hciID); // Mocha ไว้

        //    lbl0_company.Text = detail.InsuranceCompanyDetail.ToString();
        //    lbl0_Policy.Text = detail.ApplicationID.ToString();
        //    lbl0_MemberNo.Text = "-";

        //    lbl0_Hospital.Text = detail.OrganizeDetail.ToString();
        //    lbl0_Room.Text = "-";
        //    lbl0_Tel.Text = "-";
        //    lbl0_Fax.Text = "-";

        //    lbl0_NameSendFax.Text = "-";
        //    lbl0_TimeSend.Text = DateTime.Now.ToShortDateString();

        //    lbl1_Name.Text = detail.FullName.ToString();
        //    lbl1_BrithDate.Text = detail.BirthDate.ToString();
        //    lbl1_Age.Text = "-";
        //    lbl1_IDCard.Text = "บัตรประชาชน";
        //    lbl1_Card.Text = detail.IdentityNumber.ToString();
        //    lbl1_AddressDetail.Text = detail.Address.ToString();
        //    lbl1_TelNo.Text = detail.PhoneNo.ToString();
        //    lbl1_OtherInsurers.Text = "-";
        //    lbl1_specify.Text = "-";
        //    lbl1_Policy.Text = "-";

        //    lbl1_Treated.Text = "-";
        //    lbl1_Dateoftreatment.Text = "-";
        //    lbl1_CostTreatmentGiven.Text = "-";

        //    lbl1_NameInsured.Text = detail.FullName.ToString();
        //    lbl1_date_1.Text = DateTime.Now.Day.ToString();
        //    lbl1_month_1.Text = DateTime.Now.Month.ToString();
        //    lbl1_year_1.Text = DateTime.Now.Year.ToString();

        //    lbl1_NameWitness.Text = "-";
        //    lbl1_date_2.Text = DateTime.Now.Day.ToString();
        //    lbl1_month_2.Text = DateTime.Now.Month.ToString(); ;
        //    lbl1_year_2.Text = DateTime.Now.Year.ToString(); ;

        //}
    }
}