using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSDataCenterV1.SmileSIdentityService;

namespace SmileSDataCenterV1.Module.Identity
{
    public partial class frmUserManager : System.Web.UI.Page
    {
        #region Declared
        //declared service client from SmileSWCFDataCenter 
        public SmileIdentityServiceClient siClient = new SmileIdentityServiceClient();
        #endregion

        #region event
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindDataTo_cbl();

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

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            bool result = false;
            result = siClient.Get_SSSPersonUser_SelectBy(Username.Value);

            if (result)
            {
                plhRoleSelect.Visible = true;
                lblUserCheck.ForeColor = System.Drawing.Color.Green;
                lblUserCheck.Text = "สามารถใช้ Username นี้ได้";
            }
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != "" && txtConfirmPassword.Text != "")
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    string AllRoles = null;
                    List<string> roles = new List<string>();
                    foreach (var item in cblRolesSelect.Items)
                    {
                        ListItem itm = ((ListItem)item);
                        if (itm.Selected == true)
                        {
                            roles.Add(itm.ToString());
                        }
                    }
                    AllRoles = string.Join(",", roles);
                    var result = siClient.Set_AspNetUsers_Insert(Username.Value, txtConfirmPassword.Text.Trim(), AllRoles);
                    if (result)
                    {
                        Response.Redirect(Page.ResolveUrl("/Module/Identity/frmAddNewUser?m=CreateUserSuccess"));
                    }
                    else
                        Response.Redirect(Page.ResolveUrl("/Module/Identity/frmAddNewUser?m=chkDataFail"));
                }
                else
                {
                    lblPasswordCheck.Text = "กรุณาตรวจสอบ Password ทั้ง2ช่องให้ตรงกัน!!";
                }
            }
            else
            {
                lblPasswordCheck.Text = "กรุณากรอก Password ให้ครบทั้ง2ช่อง!!";
            }
        }

        #endregion

        #region Method
        public void BindDataTo_cbl()
        {
            var result = siClient.Get_AspNetRoles_SelectAll();
            cblRolesSelect.DataSource = result;
            cblRolesSelect.DataTextField = "Name";
            cblRolesSelect.DataValueField = "ID";
            cblRolesSelect.DataBind();
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