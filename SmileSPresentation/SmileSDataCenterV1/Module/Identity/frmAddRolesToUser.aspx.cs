using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSDataCenterV1.SmileSIdentityService;

namespace SmileSDataCenterV1.Module.Identity
{
    public partial class frmAddRolesToUser : System.Web.UI.Page
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
                        message == "AddRoleSuccess" ? "This User has been add Roles Successfull."
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
                BindDataTo_cbl();
                plhRoleSelect.Visible = true;
            }
        }

        protected void btnAddUserToRoles_Click(object sender, EventArgs e)
        {
            var deleteresult = siClient.Set_AspNetUserRoles_Delete_AllRolesFromUser(SearchUser.Value);
            if (deleteresult)
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
                var result = siClient.Set_AspNetUsersRoles_Insert_RolesToUser(SearchUser.Value, AllRoles);
                if (result)
                {
                    Response.Redirect(Page.ResolveUrl("/Module/Identity/frmAddRolesToUser?m=AddRoleSuccess"));
                }
                else
                    Response.Redirect(Page.ResolveUrl("/Module/Identity/frmAddRolesToUser?m=chkDataFail"));
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