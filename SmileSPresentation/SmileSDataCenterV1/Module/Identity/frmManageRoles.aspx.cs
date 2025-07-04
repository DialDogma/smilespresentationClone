using System;
using SmileSDataCenterV1.SmileSIdentityService;

namespace SmileSDataCenterV1.Module.Identity
{
    public partial class frmManageRoles : System.Web.UI.Page
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
                lblResult.Text = "";
                BindGridviewData();
            }
        }

        /// <summary>
        /// add role event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddRole_Click(object sender, EventArgs e)
        {
            //call service Set_AspNetRoles_Insert and send role name to create role
            var result = siClient.Set_AspNetRoles_Insert(txtAddRole.Text.Trim());
            //check if result = true
            if (result)
            {
                lblResult.ForeColor = System.Drawing.Color.Green;
                lblResult.Text = "เพิ่ม Role " + txtAddRole.Text + " สำเร็จ";
            }
            else
            {
                lblResult.ForeColor = System.Drawing.Color.Yellow;
                lblResult.Text = "!!!เพิ่ม Role ไม่สำเร็จ กรุณาลองใหม่อีกครั้ง!!!";
            }
        }

        /// <summary>
        /// select gridview event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgvAllRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var roleSelect = dgvAllRoles.SelectedRow.Cells[0].Text;
            //check if gridview is select value
            if (roleSelect != null)
            {
                plhButton.Visible = true;
            }
        }

        /// <summary>
        /// delete role event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deleteRole(object sender, EventArgs e)
        {
            // store role selected from gridview
            var roleSelect = dgvAllRoles.SelectedRow.Cells[0].Text;
            //check if not select null
            if (roleSelect != null)
            {
                //call service client Set_AspNetRoles_DeleteRole and send parameter role name
                var result = siClient.Set_AspNetRoles_DeleteRole(roleSelect);
                //check if return true
                if (result)
                {
                    lblResult.ForeColor = System.Drawing.Color.Red;
                    lblResult.Text = "ลบ Role " + txtAddRole.Text + " สำเร็จ";
                }
                else
                {
                    lblResult.ForeColor = System.Drawing.Color.Yellow;
                    lblResult.Text = "ลบ Role ไม่สำเร็จ กรุณาลองอีกครั้ง!!!";
                }
            }
        }

        #endregion

        #region Method
        /// <summary>
        /// bind data to gridview
        /// </summary>
        private void BindGridviewData()
        {
            //call service client Get_AspNetRoles_SelectAll and return all role from db
            var result = siClient.Get_AspNetRoles_SelectAll();
            //bind data to gridview 
            dgvAllRoles.DataSource = result;
            dgvAllRoles.DataBind();
        }
        #endregion

    }
}