using SmileClaimV1.ClaimService_PH;
using SmileClaimV1.HCIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim
{
    public partial class _04_Detail : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //test visible ucPH & ucPA
                // Get Query String
                string appID = Request.QueryString["appID"];
                string productGroupID = Request.QueryString["pgroup"];

                hdfProductGroupID.Value = productGroupID;

                // Load DDL And Detail
                ucOpenClaim.DoLoadDropdownList(Convert.ToInt32(productGroupID));
                DoLoadDetail(appID, productGroupID);
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string resultValidate = ucOpenClaim.IsValidate();

            if (resultValidate == string.Empty)
            {
                // Create Claim Header
                DoCreateClaimHeader();
            }
            else
            {
                ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.danger, resultValidate);
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        ///
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="phID"></param>
        private void DoLoadDetail(string appID, string productGroupID)
        {
            if (productGroupID == "2") // 2 = PH --- 3= PA
            {
                //
                ucPACustomerDetail.Visible = false;
                ucPABenefit.Visible = false;

                //
                ucPHCustomerDetail.Visible = true;
                ucPHBenefit.Visible = true;

                //
                ucPHCustomerDetail.DoLoadDetail(appID);
                ucPHBenefit.DoLoadDetail(appID);
            }
            else
            {
                //
                string cusID = Request.QueryString["cusID"];

                //
                ucPACustomerDetail.Visible = true;
                ucPABenefit.Visible = true;

                //
                ucPHCustomerDetail.Visible = false;
                ucPHBenefit.Visible = false;

                //
                ucPACustomerDetail.DoLoadDetail(cusID);
                ucPABenefit.DoLoadDetail(appID);
            }
        }

        /// <summary>
        /// Do Create Claim Header
        /// </summary>
        private void DoCreateClaimHeader()
        {
            ClaimService_HCI.ClaimServiceClient clientClaimHCI = new ClaimService_HCI.ClaimServiceClient();

            int productGroupID = int.Parse(hdfProductGroupID.Value);

            if (hdfProductGroupID.Value == "2") // PH
            {
                //ucOpenClaim.ClaimAdmitTypeID = 2; // mocup

                int? result = clientClaimHCI.CreateClaimInform(ucOpenClaim.ClaimTypeID,
                ucOpenClaim.ClaimAdmitTypeID,
                ucOpenClaim.ClaimAdmitTypeID,
                productGroupID,
                2, // สถานะเช็คสิทธื์เคลม
                1, // Mocup
                1,// 1 person id test = n/a
                DateTime.Now,
                DateTime.Now,
                ucOpenClaim.MoreDetail,
                ucOpenClaim.PhoneNumber,
                ucPHCustomerDetail.ApplicationID,
                ucPHCustomerDetail.ApplicationID,
                10,
                5,
                15);

                if (result != null)
                {
                    ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.success, "บันทึกยืนยันใช้สิทธิ์เรียบร้อยแล้ว");
                    DoDisable();
                }
                else
                {
                    ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.danger, "เกิดข้อผิดพลาด!");
                }
            }

            if ((hdfProductGroupID.Value == "3")) // PA
            {
                //ucOpenClaim.ClaimAdmitTypeID = 4; // mocup

                int? result = clientClaimHCI.CreateClaimInform(ucOpenClaim.ClaimTypeID,
                ucOpenClaim.ClaimAdmitTypeID,
                ucOpenClaim.ClaimAdmitTypeID,
                productGroupID,
                2, // สถานะเช็คสิทธื์เคลม
                1, // Mocup
                1,// 1 person id test = n/a
                DateTime.Now,
                DateTime.Now,
                ucOpenClaim.MoreDetail,
                ucOpenClaim.PhoneNumber,
                ucPACustomerDetail.CustomerID,
                ucPACustomerDetail.ApplicationID,
                10,
                5,
                15);

                if (result != null)
                {
                    ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.success, "บันทึกยืนยันใช้สิทธิ์เรียบร้อยแล้ว");
                    DoDisable();
                }
                else
                {
                    ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.danger, "เกิดข้อผิดพลาด!");
                }
            }
        }

        private void DoDisable()
        {
            ucOpenClaim.DoDisable(false);
            btnConfirm.Enabled = false;
        }

        #endregion Method
    }
}