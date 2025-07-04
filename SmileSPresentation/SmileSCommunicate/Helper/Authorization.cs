using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSCommunicate.Helper
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

        /// <summary>
        /// Remove QueryString By Key
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(key);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
        }

        [Obsolete("Unused method", true)]
        public static LoginDetail GetLoginDetail(HttpContextBase context)
        {
            throw new NotImplementedException();
            //var secretKey = Properties.Settings.Default.SecretKey;
            //var tokenName = Properties.Settings.Default.TokenName;

            ////get key
            //var c = context.Request.Cookies[tokenName];

            //var result = new LoginDetail();

            ////Decode token
            //try
            //{
            //    var payload = new JwtBuilder()
            //            .WithSecret(secretKey)
            //            .MustVerifySignature()
            //            .Decode<IDictionary<string, object>>(c.Value);

            //    //Valid Token,Get details
            //    result.Result = LoginResult.SUCCESS;
            //    result.ErrorText = "";
            //    result.UserName = (payload["Username"]).ToString();
            //    result.User_ID = Convert.ToInt32(payload["User_ID"]);
            //    result.Person_ID = Convert.ToInt32(payload["Person_ID"]);
            //    result.Employee_ID = Convert.ToInt32(payload["Employee_ID"]);
            //    result.FirstName = (payload["FirstName"]).ToString();
            //    result.LastName = (payload["LastName"]).ToString();
            //    result.EmployeePositionDetail = (payload["EmployeePositionDetail"]).ToString();
            //    result.EmployeeTeamDetail = (payload["EmployeeTeamDetail"]).ToString();
            //    result.BranchDetail = (payload["BranchDetail"]).ToString();
            //    result.DepartmentDetail = (payload["DepartmentDetail"]).ToString();
            //    result.EmployeeTeam_ID = Convert.ToInt32(payload["EmployeeTeam_ID"]);
            //    result.Department_ID = Convert.ToInt32(payload["Department_ID"]);
            //    result.Branch_ID = Convert.ToInt32(payload["Branch_ID"]);
            //    result.EmployeePosition_ID = Convert.ToInt32(payload["EmployeePosition_ID"]);
            //    result.OrganizeCode = ((payload["OrganizeCode"]) != null ? (payload["OrganizeCode"]).ToString() : "");
            //    result.OrganizeDetail = ((payload["OrganizeDetail"]) != null ? (payload["OrganizeDetail"]).ToString() : "");
            //    if (payload["Organize_ID"] != null)
            //    {
            //        result.Organize_ID = Convert.ToInt32(payload["Organize_ID"]);
            //    }
            //    result.EmployeeCode = (payload["EmployeeCode"]).ToString();
            //    result.FullName = (payload["FullName"]).ToString();
            //}

            ////Token Expire
            //catch (JWT.TokenExpiredException)
            //{
            //    //token has expire
            //    result.Result = LoginResult.ERROR;
            //    result.ErrorText = "cookie Expired";
            //}

            ////Invalid Signature
            //catch (JWT.SignatureVerificationException)
            //{
            //    //token has invalid signature
            //    result.Result = LoginResult.ERROR;
            //    result.ErrorText = "Invalid Signature";
            //}

            ////other
            //catch (Exception ex)
            //{
            //    result.Result = LoginResult.ERROR;
            //    result.ErrorText = ex.Message;
            //}

            //return result;
        }

        public static string GetChangePasswordURL()
        {
            return Properties.Settings.Default.ChangePasswordURL;
        }
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