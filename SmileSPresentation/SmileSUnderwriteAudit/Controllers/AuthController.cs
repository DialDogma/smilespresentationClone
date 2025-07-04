using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
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
            var changePasswordURL = Properties.Settings.Default.ChangePasswordURL;
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

        [HttpGet]
        public JsonResult CheckCookie()
        {
            var status = false;

            // Get Tokens Name
            var tokensName = Properties.Settings.Default.TokenName;
            try
            {
                //read token cookie
                var refreshCookie = OAuth2Helper.GetRefreshCookie();
                var sessionCookie = OAuth2Helper.GetStateCookie();
                if (!CheckExpired())
                {
                    return Json(new CookieSession(true), JsonRequestBehavior.AllowGet);
                }

                if (refreshCookie != null && sessionCookie != null)
                {
                    var jwt = OAuth2Helper.RefreshToken(refreshCookie.Value);
                    OAuth2Helper.SetCookie(jwt);
                    OAuth2Helper.SetRefreshCookie(jwt);
                    OAuth2Helper.SetStateCookie(jwt, sessionCookie.Value);
                    status = true;
                }
            }
            catch (Exception)
            {
                status = false;
            }

            return Json(new CookieSession(status), JsonRequestBehavior.AllowGet);
        }

        private bool CheckExpired()
        {
            var cookie = OAuth2Helper.GetCookie();
            var refreshCookie = OAuth2Helper.GetRefreshCookie();
            var sessionCookie = OAuth2Helper.GetStateCookie();

            if (cookie == null || refreshCookie == null || sessionCookie == null)
            {
                return true;
            }

            if (OAuth2Helper.ValidateTokenExpired())
            {
                return true;
            }

            return false;
        }
    }
}