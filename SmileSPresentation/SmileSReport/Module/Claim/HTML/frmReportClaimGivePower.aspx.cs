using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSReport.PHClaimReportService;

namespace SmileSReport.Module.Claim.HTML
{
    public partial class WebForm2 : System.Web.UI.Page
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
            using (var db = new ClaimReportServiceClient())
            {
                try
                {
                    var result = db.ReportClaimGivePower(claimHeaderID);

                    if (result.Hospital != null)
                    {
                        lblHospitalHeader.Text = result.Hospital;
                    }
                    else
                    {
                        lblHospitalHeader.Text = "";
                    }
                    //เช็คอายุ วันเกิดถึงวันที่เคลม
                    var age = CalculateAgeFromDateOfBirthToDateofClaim(result.BirthDate.Value, result.CreatedDate.Value);
                }
                catch (Exception e)
                {
                    //  Block of code to handle errors
                }

                

                //อายุตั้งแต่ 20 ปี ให้ดึงข้อมูลส่วนตัวมาแสดง
                //if ((age >= 20))
                //{

                //07242 Chanadol koonkam 18/01/2566
                //    //lblFullname.Text = result.CustName;
                //    lblFullname.Text = result.CustName;
                //    //lblLastname.Text = result.LastName;
                //    //lblFullAddressDetail.Text = result.FullAddress;

                //    lblAddressNo.Text = result.เลขที่;
                //    lblAddressMoo.Text = result.หมู่;
                //    lblAdressTumbol.Text = result.ตำบล;
                //    lblAddressAmphoe.Text = result.อำเภอ;
                //    lblAddressProvince.Text = result.จังหวัด;

                //    //lblZCardId.Text = result.ZCard_id;
                //    string str = result.ZCard_id;
                //    char[] charArray = str.ToCharArray();
                //    string[] zCardId = str.Select(x => x.ToString()).ToArray();
                //    txtZCardIdNum0.Text = zCardId[0];
                //    txtZCardIdNum1.Text = zCardId[1];
                //    txtZCardIdNum2.Text = zCardId[2];
                //    txtZCardIdNum3.Text = zCardId[3];
                //    txtZCardIdNum4.Text = zCardId[4];
                //    txtZCardIdNum5.Text = zCardId[5];
                //    txtZCardIdNum6.Text = zCardId[6];
                //    txtZCardIdNum7.Text = zCardId[7];
                //    txtZCardIdNum8.Text = zCardId[8];
                //    txtZCardIdNum9.Text = zCardId[9];
                //    txtZCardIdNum10.Text = zCardId[10];
                //    txtZCardIdNum11.Text = zCardId[11];
                //    txtZCardIdNum12.Text = zCardId[12];
                //    lblAppId.Text = result.App_id;
                //    lblInsuranceCompany.Text = result.InsuranceCompany;
                //}
                //else
                //{
                //    lblFullname.Text = "";
                //    //lblLastname.Text = "";
                //    lblAddressNo.Text = " ";
                //    lblAddressMoo.Text = "";
                //    lblAdressTumbol.Text = "";
                //    lblAddressAmphoe.Text = "";
                //    lblAddressProvince.Text = "";
                //    //lblFullAddressDetail.Text = ".......................................................................\r\n.........................................................................................................";
                //    //lblZCardId.Text = ".............................................................";
                //    lblAppId.Text = "";
                //    lblInsuranceCompany.Text = "";
                //}

                //lblFirstname.Text = "........................................................................";
                //    lblLastname.Text = "";

                //    lblFullAddressDetail.Text = ".......................................................................\r\n.........................................................................................................";

                //    //lblZCardId.Text = ".............................................................";

                //    lblAppId.Text = ".........................";
                //    lblInsuranceCompany.Text = "..............................................................";
            }
        }

        /// <summary>
        /// calculate age
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <param name="dateOfClaim"></param>
        /// <returns></returns>
        private int CalculateAgeFromDateOfBirthToDateofClaim(DateTime dateOfBirth, DateTime dateOfClaim)
        {
            int age = 0;
            age = dateOfClaim.Year - dateOfBirth.Year;
            //Go back to the year the person was born in case of a leap year (366days)
            if (dateOfClaim.DayOfYear < dateOfBirth.DayOfYear) age--;

            return age;
        }
    }
}