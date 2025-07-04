using AuthorizationServer.Contacts;
using MassTransit;
using Microsoft.AspNet.Identity;
using SmileSLogin.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmileSLogin.Helper
{
    public static class AuthMethod
    {
        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static LoginDetail Login(string username, string password)
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


        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static bool ChangePassword(string username, string newPassword)
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

        public static LoginDetail GetLoginDetail(string username)
        {
            var context = new DataCenterV1Entities();
            var result = new LoginDetail(context.usp_GetUserDetailByUserName(username).FirstOrDefault());
            return result;
        }

        public static bool ResetPassword_SSS(string username)
        {
            bool result;

            var sssService = new SSSLoginService.SSSLoginServiceSoapClient();

            result = sssService.ChangePasswordSSS(username, username);

            return result;
        }

        public static bool ChangePassword_SSS(string username, string newPassword)
        {
            var sssService = new SSSLoginService.SSSLoginServiceSoapClient();
            bool result = sssService.ChangePasswordSSS(username, newPassword);
            return result;
        }

        //public static bool ChangePassword_AuthServer(string username, string newPassword)
        //{
        //    ChangePasswordRequest request = CreateChangePasswordRequest(username, newPassword);

        //    IRequestClient<ChangePasswordRequest> client = MvcApplication.Bus.CreateRequestClient<ChangePasswordRequest>(new Uri(Properties.Settings.Default.AuthServerQueue));

        //    Response<ChangePasswordSuccess, ChangePasswordFail> response = client.GetResponse<ChangePasswordSuccess,ChangePasswordFail>(request).Result;

        //    if (response.Is(out Response<ChangePasswordSuccess> success))
        //        return true;
            
        //    return false;
        //}

        private static ChangePasswordRequest CreateChangePasswordRequest(string userName, string newPassword)
        {
            UserManager manager = new UserManager();

            ApplicationUser user = manager.FindByName(userName);

            return new ChangePasswordRequest
            {
                Id = user.Id,
                UserName = user.UserName,
                NewPassword = newPassword
            };
        }
    }
}