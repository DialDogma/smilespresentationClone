using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using SmileSDataCenterV2.Models;

namespace SmileSDataCenterV2.WebServices
{
    /// <summary>
    /// Summary description for SmileSLoginService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class SmileSLoginService : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [WebMethod]
        public LoginDetail Login(string username, string password)
        {
            LoginDetail result;
            //login via aspnetuser
            var manager = new UserManager();
            ApplicationUser user = manager.Find(username, password);
            if (user != null)
            {
                var context = new DataCenterV1Entities();
                result = new LoginDetail(context.usp_GetUserDetailByUserName(username).FirstOrDefault());
            }
            else
            {
                result = null;
            }

            return result;
        }

        [WebMethod]
        public LoginDetail GetLoginDetail(string username)
        {
            var context = new DataCenterV1Entities();
            var result = new LoginDetail(context.usp_GetUserDetailByUserName(username).FirstOrDefault());
            return result;
        }

        /// <summary>
        /// reset password datacenter
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [WebMethod]
        public bool ResetPassword(string username)
        {
            bool result;
            var user = GetLoginDetail(username).Id;

            UserManager manager = new UserManager();

            //remove old password fisrt!!
            IdentityResult RemoveResult = manager.RemovePassword(user);
            if (RemoveResult.Succeeded)
            {
                //when success remove old password,then add new password
                IdentityResult SetPasswordResult = manager.AddPassword(user, username);
                if (SetPasswordResult.Succeeded)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Reset password sss
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ResetPassword_SSS(string username)
        {
            bool result;

            var sssService = new SSSLoginService.SSSLoginServiceSoapClient();

            result = sssService.ChangePasswordSSS(username, username);

            return result;
        }

        /// <summary>
        /// Change password datacenter
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [WebMethod]
        public bool ChangePassword(string username, string newPassword)
        {
            bool result;
            var user = GetLoginDetail(username).Id;

            UserManager manager = new UserManager();

            //remove old password fisrt!!
            IdentityResult RemoveResult = manager.RemovePassword(user);
            if (RemoveResult.Succeeded)
            {
                //when success remove old password,then add new password
                IdentityResult SetPasswordResult = manager.AddPassword(user, newPassword);
                if (SetPasswordResult.Succeeded)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Change Password SSS
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [WebMethod]
        public bool ChangePassword_SSS(string username, string newPassword)
        {
            var sssService = new SSSLoginService.SSSLoginServiceSoapClient();
            bool result = sssService.ChangePasswordSSS(username, newPassword);
            return result;
        }
    }
}