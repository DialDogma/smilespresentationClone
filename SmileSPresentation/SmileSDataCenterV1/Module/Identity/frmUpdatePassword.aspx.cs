using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSDataCenterV1.SmileSIdentityService;

namespace SmileSDataCenterV1.Module.Identity
{
    public partial class frmUpdatePassword : System.Web.UI.Page
    {

        #region Declared
        //declared service client from SmileSWCFDataCenter 
        public SmileIdentityServiceClient siClient = new SmileIdentityServiceClient();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //render success message and response page back
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    Form.Action = ResolveUrl("~/Module/frmAddNewUser.aspx");
                    SuccessMessage =
                        message == "ResetPasswordS" ? "This User has been create Successfull."
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                    ErrorMessage =
                        message == "chkDataFail" ? "กรุณาตรวจสอบข้อมูล!!"
                        : String.Empty;
                    errorMessage.Visible = !String.IsNullOrEmpty(ErrorMessage);
                }
            }

        }

        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            dgvSearchUser.DataSource = null;
            dgvSearchUser.DataBind();
            var result = siClient.Get_AspNetUsers_SelectBy(SearchUser.Value);
            if (result != null)
            {
                dgvSearchUser.DataSource = result;
                dgvSearchUser.DataBind();
            }
        }

        protected void dgvSearchUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            var user = dgvSearchUser.SelectedRow.Cells[0].Text;
            if (user != null)
            {
                plhChangePasswordform.Visible = true;
            }
        }

        protected void btnChangePassword_click(object sender, EventArgs e)
        {

            if (txtPassword.Text != "" && txtConfirmPassword.Text != "" && txtOldPassword.Text != null)
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    var result = siClient.Set_AspNetUsers_ChangePassword(SearchUser.Value, txtOldPassword.Text.Trim(), txtConfirmPassword.Text.Trim());
                    if (result)
                    {
                        Response.Redirect(Page.ResolveUrl("/Module/Identity/frmResetPassword?m=ResetPasswordS"));
                    }
                    else
                        Response.Redirect(Page.ResolveUrl("/Module/Identity/frmResetPassword?m=chkDataFail"));
                }
                else
                    lblPasswordCheck.Text = "กรุณากรอกรหัสใหม่ทั้ง2ช่องให้ตรงกัน";
            }
            else
                lblPasswordCheck.Text = "กรุณากรอกรหัสทั้่ง3ช่องให้ครบถ้วน";
        }
        #endregion

        #region Message
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected string ErrorMessage
        {
            get;
            private set;
        }
        #endregion

    }
}