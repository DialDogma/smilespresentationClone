using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSDataCenterV1.SmileSIdentityService;

namespace SmileSDataCenterV1.Module.Identity
{
    public partial class frmAddRoleToUsers : System.Web.UI.Page
    {

        #region Declared
        //declared service client from SmileSWCFDataCenter 
        public SmileIdentityServiceClient siClient = new SmileIdentityServiceClient();
        #endregion

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
                        message == "CreateUserSuccess" ? "This User has been create Successfull."
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                    ErrorMessage =
                        message == "chkDataFail" ? "กรุณาตรวจสอบข้อมูล!!"
                        : String.Empty;
                    errorMessage.Visible = !String.IsNullOrEmpty(ErrorMessage);
                }
            }
        }



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