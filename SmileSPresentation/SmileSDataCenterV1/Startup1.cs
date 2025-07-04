using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(SmileSDataCenterV1.Startup1))]

namespace SmileSDataCenterV1
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            //ใช้ cookie Authentication และตั้งค่าภายใน cookie
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                //ตั้งค่า Authenticationtype เป็น Application Cookie
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //ตั้งค่า loginpath เป็นหน้า login
                LoginPath = new PathString("/Login"),
                //ตั้งค่ารูปแบบการ timeout ของ cookie
                SlidingExpiration = true,
                //ตั้งค่าเวลา timeout 
                ExpireTimeSpan = TimeSpan.FromHours(24)
            });
        }
    }
}
