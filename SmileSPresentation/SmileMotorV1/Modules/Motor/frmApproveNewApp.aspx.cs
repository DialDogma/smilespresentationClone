using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmApproveNewApp : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    // Check Query String "motorid"
                    if (Request.QueryString["motorid"] != string.Empty)
                    {
                        var motorId = GetMotorAppID();
                        // Load uc Detail And Setup
                        this.ucAppDetail.DoLoad(motorId, true);
                        this.ucAppDetail.DoSetVisibleButtonEditPeron(true);
                        CheckStatusApp(motorId);
                    }
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            var mm = new MotorManager();

            // Get Motor Application ID To Variable
            var appID = GetMotorAppID();

            // Motor Application ID ไม่เท่ากับ 0
            if (GetMotorAppID() != 0)
            {
                // Validate
                if (IsValidate() == true)
                {
                    var customerId = this.ucAppDetail.GetCustomerId(appID);
                    var payerId = this.ucAppDetail.GetPayerId(appID);

                    // Update Motor Application Status
                    var result = mm.UpdateMotorApplicationStatus(MappingCode.eMotorApplicationStatus.Approve, appID, cFunction.GetCurrentLoginUser_ID(this));

                    DoInsertCustomerApplication(appID, customerId, "C");
                    DoInsertCustomerApplication(appID, payerId, "P");

                    // Get Result Update Status
                    GetResultUpdateStatus(result, appID);

                    // Disable Button
                    DoDisableButton();
                }
            }

            // Load uc Detail
            this.ucAppDetail.DoLoad(appID, true);

            //COMMENT 8-3-2019 15.51น.
            //var encryptID = cFunction.Base64Encrypt(appID.ToString());
            ////Redirect to print covernote
            //Response.Redirect("frmReportApplicationCoverNote?motorid=" + encryptID);
        }

        protected void btnPending_Click(object sender, EventArgs e)
        {
            MotorManager mm = new MotorManager();

            // Get Motor Application ID To Variable
            int appID = GetMotorAppID();

            // Motor Application ID ไม่เท่ากับ 0
            if (GetMotorAppID() != 0)
            {
                // Validate
                if (IsValidate() == true)
                {
                    int result;

                    // Update Motor Application Status
                    result = mm.UpdateMotorApplicationStatus(MappingCode.eMotorApplicationStatus.WaitEdit, appID, cFunction.GetCurrentLoginUser_ID(this));

                    // Get Result Update Status
                    GetResultUpdateStatus(result, appID);

                    // Disable Button
                    DoDisableButton();
                }
            }

            // Load uc Detail
            this.ucAppDetail.DoLoad(appID, true);
        }

        protected void btnUnApprove_Click(object sender, EventArgs e)
        {
            MotorManager mm = new MotorManager();

            // Get Motor Application ID To Variable
            int appID = GetMotorAppID();

            // Motor Application ID ไม่เท่ากับ 0
            if (GetMotorAppID() != 0)
            {
                // Validate
                if (IsValidate() == true)
                {
                    int result;

                    // Update Motor Application Status
                    result = mm.UpdateMotorApplicationStatus(MappingCode.eMotorApplicationStatus.UnApprove, appID, cFunction.GetCurrentLoginUser_ID(this));

                    // Get Result Update Status
                    GetResultUpdateStatus(result, appID);

                    // Disable Button
                    DoDisableButton();
                }
            }

            // Load uc Detail
            this.ucAppDetail.DoLoad(appID, true);
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            MotorManager mm = new MotorManager();

            // Get Motor Application ID To Variable
            int appID = GetMotorAppID();

            // Motor Application ID ไม่เท่ากับ 0
            if (GetMotorAppID() != 0)
            {
                // Validate
                if (IsValidate() == true)
                {
                    // Update Motor Application Status
                    var result = mm.UpdateMotorApplicationStatus(MappingCode.eMotorApplicationStatus.Cancel, appID, cFunction.GetCurrentLoginUser_ID(this));

                    // Get Result Update Status
                    GetResultUpdateStatus(result, appID);

                    // Disable Button
                    DoDisableButton();
                }
            }

            // Load uc Detail
            this.ucAppDetail.DoLoad(appID, true);
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Get Motor App ID
        /// </summary>
        /// <returns></returns>
        private int GetMotorAppID()
        {
            int result;

            // Check Query String "motorid"
            if (Request.QueryString["motorid"] != string.Empty)
            {
                // Get Query String "motorid"
                string code = (Request.QueryString["motorid"]);

                // ถอดรหัส Base64
                result = Convert.ToInt32(cFunction.Base64Decrypt(code));
            }
            else
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Get Result Update Status
        /// </summary>
        /// <param name="result"></param>
        /// <param name="appID"></param>
        private void GetResultUpdateStatus(int result, int appID)
        {
            // Result = AppID ?
            if (result == appID)
            {
                // Alert บันทึกรายการสำเร็จ
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.success, "บันทึกรายการสำเร็จ");
            }
            //
            else
            {
                // Alert บันทึกรายการไม่สำเร็จ
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "บันทึกรายการไม่สำเร็จ");
            }
        }

        /// <summary>
        /// ควรจะย้ายไปอยู่ใน class
        /// </summary>
        /// <returns></returns>
        private bool IsValidate()
        {
            if ((this.ucAppDetail.GetMotorApplicationStatus_ID() != (int)MappingCode.eMotorApplicationStatus.WaitApprove)
                && (this.ucAppDetail.GetMotorApplicationStatus_ID() != (int)MappingCode.eMotorApplicationStatus.EditWaitApprove)
                && (this.ucAppDetail.GetMotorApplicationStatus_ID() != 10)) // 10 Status รอคืนสถานะ
            {
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "สถานะไม่ใช่รอการอนุมัติ");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Do Disable Button
        /// </summary>
        private void DoDisableButton()
        {
            btnApprove.Visible = false;
            btnUnApprove.Visible = false;
            btnPending.Visible = false;
            btnCancel.Visible = false;
        }

        private void CheckStatusApp(int motorId)
        {
            using (var db = new MotorV1Entities())
            {
                var result = db.usp_MotorApplication_SelectStatusByID(motorId).First();

                if (result.MotorApplicationStatus_ID == 10)
                {
                    btnPending.Visible = false;
                    btnUnApprove.Visible = false;
                }
                else
                {
                    btnCancel.Visible = false;
                }
            }
        }

        private void DoInsertCustomerApplication(int appId, int personId, string custType)
        {
            var userID = cFunction.GetCurrentLoginUser_ID(this.Page);
            using (var db = new MotorV1Entities())
            {
                var rs = db.usp_CustomerApplication_Motor_Person_InsertOrUpdate(appId, personId, custType, userID);
            }
        }

        #endregion Method
    }
}