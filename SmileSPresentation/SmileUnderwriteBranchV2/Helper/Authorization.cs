using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmileUnderwriteBranchV2.Helper
{
    public class Authorization : ActionFilterAttribute, IActionFilter
    {
        public string Roles;

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var state = HttpContext.Current.Request.Url;

            //Get token name
            var tokenName = Properties.Settings.Default.TokenName;

            try
            {
                // Read Token cookie
                var cookie = OAuth2Helper.GetCookie();

                // Read Token SessionTCookie
                var sessionCookie = OAuth2Helper.GetStateCookie();

                // Read Token RefreshCookie
                var refreshCookie = OAuth2Helper.GetRefreshCookie();
                var expired = false;
                if (cookie == null)
                {
                    expired = true;
                }
                else
                {
                    expired = OAuth2Helper.ValidateTokenExpired();
                }
                if (sessionCookie == null)
                {
                    throw new Exception("invaild Session");
                }
                if (refreshCookie == null)
                {
                    throw new Exception("invaild RefreshToken");
                }

                if (expired && sessionCookie != null)
                {
                    var tokens = OAuth2Helper.RefreshToken(refreshCookie.Value);
                    OAuth2Helper.SetCookie(tokens);
                    OAuth2Helper.SetRefreshCookie(tokens);
                    OAuth2Helper.SetStateCookie(tokens, sessionCookie.Value);

                    cookie = OAuth2Helper.GetCookie();
                }

                if (cookie is null || string.IsNullOrWhiteSpace(cookie.Value)) throw new Exception("Cookie is not setted");

                var token = cookie.Value;

                // Validate Expired
                if (OAuth2Helper.ValidateTokenExpired(token)) throw new Exception("Token is expired");

                // Validate required refresh token
                if (OAuth2Helper.ValidateRefreshToken(token)) CheckRefreshToken();

                // Validate Roles
                var jwt = OAuth2Helper.GetJwt(token);
                var cookieRoles = OAuth2Helper.GetRoles(token);

                if (Roles != null && !IsAuthen(Roles, cookieRoles))
                {
                    filterContext.Result = new RedirectToRouteResult(
                      new RouteValueDictionary
                      {
                            {"Controller", "Auth"},
                            {"Action", "Unauthorize"}
                      });
                }
            }
            catch
            {
                //Redirect to login page
                OAuth2Helper.ClearCookies();
                filterContext.Result = new RedirectResult(OAuth2Helper.GetAuthorizationRequest(state.AbsoluteUri));
            }

            OnActionExecuting(filterContext);
        }

        private string CheckRefreshToken()
        {
            var refreshToken = OAuth2Helper.GetRefreshCookie().Value;
            // Refresh token
            var jsonTokens = OAuth2Helper.RefreshToken(refreshToken);

            // Set new token
            OAuth2Helper.SetCookie(jsonTokens);
            OAuth2Helper.SetRefreshCookie(jsonTokens);
            OAuth2Helper.SetStateCookie(jsonTokens, OAuth2Helper.GetState());

            return jsonTokens.AccessToken;
        }

        /// <summary>
        /// Check role in session is authen
        /// </summary>
        /// <param name="roleToCheck"></param>
        /// <param name="sessionRole"></param>
        /// <returns></returns>
        private bool IsAuthen(string roleToCheck, string sessionRole)
        {
            var result = false;

            var lstRoleToCheck = roleToCheck.Split(',').ToList();

            var lstSessionRole = sessionRole.Split(',').ToList();

            //intersec
            var intersectCount = lstRoleToCheck.Intersect(lstSessionRole).Count();

            result = (intersectCount > 0) ? true : false;

            return result;
        }

        public enum LoginResult
        {
            SUCCESS,
            ERROR
        }

        public class LoginDetail
        {
            public LoginResult Result { get; set; }
            public string ErrorText { get; set; }
            public string UserName { get; set; }
            public int User_ID { get; set; }
            public int Person_ID { get; set; }
            public int Employee_ID { get; set; }
            public string EmpCode { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmployeePositionDetail { get; set; }
            public string EmployeeTeamDetail { get; set; }
            public string BranchDetail { get; set; }
            public string DepartmentDetail { get; set; }
            public int EmployeeTeam_ID { get; set; }
            public int Department_ID { get; set; }
            public int Branch_ID { get; set; }
            public int EmployeePosition_ID { get; set; }
            public string FullName { get; set; }
            public int? Organize_ID { get; set; }
            public string OrganizeCode { get; set; }
            public string OrganizeDetail { get; set; }
            public string EmployeeCode { get; set; }
        }
    }
}