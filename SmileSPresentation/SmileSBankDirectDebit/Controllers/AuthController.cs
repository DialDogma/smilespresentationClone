using SmileSBankDirectDebit.Helper;
using System.Web.Mvc;

namespace SmileSBankDirectDebit.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Logout()
        {
            OAuth2Helper.ClearCookies();
            var logoutUrl = OAuth2Helper.LogoutUrl;
            return Redirect(logoutUrl);
        }

        public ActionResult UnAuthorize()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            var changePasswordURL = OAuth2Helper.ChangePasswordUrl;
            return Redirect(changePasswordURL);
        }

        // Sign in callback
        public ActionResult SignInCallback(string code, string session_state, string state = "")
        {
            try
            {
                var token = OAuth2Helper.AuthorizationCallback(code);
                OAuth2Helper.GetUserProfile(token.AccessToken);

                OAuth2Helper.SetCookie(token);
                OAuth2Helper.SetRefreshCookie(token);
                OAuth2Helper.SetStateCookie(token, session_state);

                if (!string.IsNullOrEmpty(state)) return Redirect(state);

                return Redirect("~/");

            }
            catch
            {
                OAuth2Helper.ClearCookies();
                return Redirect(OAuth2Helper.GetAuthorizationRequest());
            }

        }

        // Post logout callback
        public ActionResult SignOutCallback()
        {
            OAuth2Helper.ClearCookies();
            Response.StatusCode = 201;
            return new EmptyResult();
        }
    }
}
