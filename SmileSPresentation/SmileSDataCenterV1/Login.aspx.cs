using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace SmileSDataCenterV1
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// login event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                //ประกาศค่า usermanager จาก identity
                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                //ค้นหา user และ password จาก usermanager โดยใช้ฟังก์ชั่น "Find" 
                var user = userManager.Find(Username.Value, Password.Value);
                //ถ้า "user" ไม่เท่ากับค่าว่าง
                if (user != null)
                {
                    //ประกาศค่า Authentication Manager
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    //สร้าง identity โดย usermanager ส่งพารามิเตอร์ "user" และ authentication type(ในที่นี้เป็นแบบ cookie) 
                    //โดยฟังก์ชั่น "CreateIdentity" เก็บไว้ใน "userIdentity"
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    //ล๊อกอินโดย authentication Manager ส่งพารามิเตอร์ "AuthenticationProperties" และ "userIdentity" โดยฟังก์ชั่น "SignIn"
                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                    //ส่งต่อไปที่หน้า Default.aspx
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    //error cause
                    FailureText.Text = "ชื่อผู้ใช้งาน หรือ รหัสผ่าน ไม่ถูกต้อง!";
                }
            }
        }
    }
}