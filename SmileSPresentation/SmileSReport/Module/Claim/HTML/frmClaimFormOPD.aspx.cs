using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class frmClaimFormOPD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string hciID = "";

                //if (Request.QueryString["HCIID"] != "")
                //{
                //    DoLoadDetail(int.Parse(hciID));
                //}

                //DoLoadDetail(60); // Mocha ไว้ (60)
            }
        }

        /// <summary>
        /// Do Load Report Detail
        /// </summary>
        /// <param name="hciID"></param>
        //private void DoLoadDetail(int hciID)
        //{
        //    HCIServiceClient HCIClient = new HCIServiceClient();
        //    usp_HCIClaimForm_Select_Result detail = new usp_HCIClaimForm_Select_Result();

        //    detail = HCIClient.GetHCIClaimForm(hciID); // Mocha ไว้ (60)

        //    lbl0_PolicyNumber.InnerText = detail.ApplicationID.ToString();
        //    lbl0_Hospital.InnerText = detail.OrganizeDetail.ToString();
        //    lbl0_HN.InnerText = "-";
        //    lbl0_Phone.InnerText = "-";
        //    lbl0_Fax.InnerText = "-";
        //    lbl0_DateTreatment.InnerText = DateTime.Now.ToShortDateString();
        //    lbl0_TimeTreatment.InnerText = DateTime.Now.ToShortTimeString();
        //    lbl0_Sender.InnerText = "-";

        //    lbl1_Name.InnerText = detail.FullName.ToString();
        //    lbl1_BirthDate.InnerText = detail.BirthDate.ToString();
        //    lbl1_Age.InnerText = "-";
        //    lbl1_Ocpation.InnerText = "-";
        //    lbl1_Card.InnerText = "บัตรประจำตัวประชาชน";
        //    lbl1_CardNo.InnerText = detail.IdentityNumber.ToString();
        //    lbl1_Address.InnerText = detail.Address.ToString();
        //    lbl1_Phone.InnerText = detail.PhoneNo.ToString();
        //    lbl1_Insure.InnerText = "-";
        //    lbl1_Policy_No.InnerText = "-";
        //    lbl1_TypeInsurance.InnerText = "-";
        //    lbl1_AccidentDetail.InnerText = "-";
        //    lbl1_DateAccident.InnerText = "-";
        //    lbl1_TimeAccident.InnerText = "-";
        //    lbl1_PlaceAccident.InnerText = "-";

        //    lbl2_CheifComplaint.InnerText = detail.ChiefComplainDetail.ToString();
        //    lbl2_Alc.InnerText = "-";
        //    lbl2_Lasr_Special.InnerText = "-";
        //    lbl2_Last_Date.InnerText = "-";
        //    lbl2_Last_Hospital.InnerText = "-";
        //    lbl2_Last_LicNo.InnerText = "-";
        //    lbl2_Last_Phone.InnerText = "-";
        //    lbl2_Last_Physician.InnerText = "-";
        //    lbl2_Last_Sign.InnerText = "-";
        //    lbl2_Medi_Inves_Other.InnerText = "-";
        //    lbl2_No1_SympAccDate.InnerText = "-";
        //    lbl2_No2_IllInjDate.InnerText = "-";
        //    lbl2_No3a_PresentIll.InnerText = "-";
        //    lbl2_No3b_Pertinent.InnerText = "-";
        //    lbl2_No4_Diagnosis1.InnerText = "-";
        //    lbl2_No4_Diagnosis2.InnerText = "-";
        //    lbl2_No4_Icd_1.InnerText = "-";
        //    lbl2_No4_Icd_2.InnerText = "-";
        //    lbl2_No5_Suture.InnerText = "-";
        //    lbl2_No5_TreatmentOther.InnerText = "-";
        //    lbl2_No6_Lmp.InnerText = "-";
        //    lbl2_No6_YesTime.InnerText = "-";
        //    lbl2_No7_Comment.InnerText = "-";

        //}
    }
}