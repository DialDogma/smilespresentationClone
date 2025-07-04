using System;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SmileSDataCenterV1.Module
{
    public partial class frmChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FailureText.Text = "";
            }
        }

        protected void ChangePassword(object sender, EventArgs e)
        {
            //ประกาศค่า usermanager จาก identity
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
            //ประกาศค่า userid เพื่อเก็บค่า user id โดยค้นหาจาก username ที่ล็อกอินในปัจจุบัน
            var userID = userManager.FindByName(Page.User.Identity.Name).Id;
            //ค้นหา user และ password จาก usermanager โดยใช้ฟังก์ชั่น "Find" 
            var user = userManager.Find(Page.User.Identity.Name, OldPassword.Value);
            //ถ้า new password ค่าตรงกับ confirm password
            if (NewPassword.Value == ConfirmNewPassword.Value)
            {
                if (user != null)
                {
                    //ประกาศค่า IdentityResult เพื่อใช้เก็บค่าผลลัพภ์ในการเปลี่ยน password 
                    //ทำการเปลี่ยน password โดยฟังกชั่น ChangePassword ส่งพารามิเตอร์ usarID,พาสเวิร์ดเดิม,พาสเวิร์ดใหม่
                    IdentityResult result = userManager.ChangePassword(userID, OldPassword.Value, ConfirmNewPassword.Value);
                    //เช็คถ้า IdentityResult สำเร็จ
                    if (result.Succeeded)
                    {
                        FailureText.ForeColor = System.Drawing.Color.Green;
                        FailureText.Text = "เปลี่ยน Password สำเร็จ";
                    }
                    else
                    {
                        FailureText.ForeColor = System.Drawing.Color.Red;
                        FailureText.Text = "เปลี่ยนระบบไม่สำเร็จ กรุณาติดต่อผู้พัฒนาระบบ!!";
                    }
                    
                }
                else
                {
                    FailureText.ForeColor = System.Drawing.Color.Red;
                    FailureText.Text = "รหัสผ่านเก่าไม่ตรงกับฐานข้อมูล!!";
                }
            }
            else
            {
                FailureText.ForeColor = System.Drawing.Color.Red;
                FailureText.Text = "รหัสผ่านใหม่ไม่ตรงกัน!!";
            }
        }
    }
}