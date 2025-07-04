using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmEndorseChangeSource : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["mtid"] != "" && Request.QueryString["mtid"] != null)
                {
                    var motorEncode = Request.QueryString["mtid"];
                    hdfMotorID.Value = new ASCIIEncoding().GetString(Convert.FromBase64String(motorEncode));
                    var motorId = int.Parse(hdfMotorID.Value);

                    DoLoad(motorId);

                    var payId = ucAppDetailPayer.MotorApplication.Payer.Person_ID;
                    var periodType = ucAppDetailPayMethod.PeriodTypeId;
                    ucAddEditPayMethod.DoLoadDropdownlist();
                    ucAddEditPayMethod.DoLoadDropdownlist(payId);
                    ucAddEditPayMethod.DoloadBankAccount(payId);
                    ucAddEditPayMethod.DoLoad(motorId, payId);
                    ucAddEditPayMethod.SetHeaderText("");
                    ucAddEditPayMethod.VisibleWarningANDPeriodANDPremium(false);
                    ucAddEditPayMethod.IsEnabledPremium(false);
                    VisibledButton(periodType != 3);
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        #endregion Event

        #region Method

        private void DoLoad(int motor_id)
        {
            ucAppDetailApplication.DoLoad(motor_id);
            ucAppDetailVehicle.DoLoad(motor_id);
            ucAppDetailCustomer.DoLoad(motor_id);
            ucAppDetailAddressCustomer.DoLoad2(motor_id, 2);
            ucAppDetailPayer.DoLoad(motor_id);
            ucAppDetailAddressPayer.DoLoad2(motor_id, 3);
            ucAppDetailPayMethod.DoLoad(motor_id);
        }

        private void VisibledButton(bool value)
        {
            btnSave.Visible = value;
            btnCancel.Visible = value;
        }

        private void DoSave()
        {
            using (var db = new MotorV1Entities())
            {
                var motor_ID = int.Parse(hdfMotorID.Value);
                var encryptID = cFunction.Base64Encrypt(hdfMotorID.Value);
                var periodType = ucAddEditPayMethod.PeriodType_ID;
                var paymethodId = ucAddEditPayMethod.PayMethodType_ID;
                var bankaccountId = ucAddEditPayMethod.PersonBankAccount_ID;
                var premiumAmount = ucAddEditPayMethod.PremiumAmount;
                var premiumDeliverAmount = ucAddEditPayMethod.PremiumDeliverAmount;
                var dutyAmount = 0;
                var taxAmount = 0;
                var premiumTaxAmount = ucAddEditPayMethod.PremiumTaxAmount;

                var result = db.usp_EndorseChangePaymethod_Update(motor_ID
                                                                    , periodType
                                                                    , paymethodId
                                                                    , bankaccountId
                                                                    , premiumAmount
                                                                    , dutyAmount
                                                                    , taxAmount
                                                                    , premiumTaxAmount
                                                                    , premiumDeliverAmount
                                                                    , cFunction.GetCurrentLoginUser_ID(Page)).First();
                if (result.IsResult == false)
                {
                    cFunction.AlertJavaScript(Page, "เกิดข้อผิดพลาด!");
                }
                else
                {
                    Server.Transfer("frmAddEditApplicationComfirmed.aspx?appid=" + encryptID + "&disbtn=0");
                }
            }
        }

        #endregion Method

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            DoSave();
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            cFunction.CloseBrowser(Page);
        }
    }
}