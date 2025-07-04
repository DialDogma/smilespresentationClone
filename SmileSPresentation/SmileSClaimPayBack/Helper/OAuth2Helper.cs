using Newtonsoft.Json;
using RestSharp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SmileSClaimPayBack.Helper
{
    public static class OAuth2Helper
    {
        #region "Properties"
        // Client Id    
        public static string ClientId = Properties.Settings.Default.ClientId;

        // Callback Url
        public static string RedirectUri = Properties.Settings.Default.CallbackURL;

        // Required scopes
        private static string _scope = Properties.Settings.Default.Scope;

        // Identity Server Url
        public static string Issuer = Properties.Settings.Default.Issuer;

        // Token Endpoint
        private static string _tokenEndpoint = Issuer + "/connect/token";

        // Authorization Endpoint
        private static string _authorizeEndpoint = Issuer + "/connect/authorize";

        // User Info Endpoint
        private static string _userInfoEndpoint = Issuer + "/connect/userinfo";

        // Default Access Token Name
        private static string _accessTokenCookieName = Properties.Settings.Default.TokenName;

        // Default Refresh Token Name
        private static string _refreshTokenCookieName = Properties.Settings.Default.RefreshTokenName;

        // Logout Endpoint
        public static string LogoutUrl = Issuer + "/Account/Logout";

        // Change Password Endpoint
        public static string ChangePasswordUrl = Issuer + "Manage/ChangePassword";

        // Check Session 
        public static bool CheckSession = Properties.Settings.Default.CheckSession;
        #endregion "Properties"

        /// <summary>
        /// Get the authorization URL
        /// </summary>
        /// <param name="state">
        /// Current URL for callback redirection
        /// </param>
        /// <returns></returns>
        public static string GetAuthorizationRequest(string state = "")
        {

            // If use reference token, add "offline_access" to scope
            var scope = _scope;

            // Build the authorization URL
            var authorizationRequest = string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}&scope={3}",
                _authorizeEndpoint,
                ClientId,
                RedirectUri,
                scope);

            // Add state if it is not empty
            if (!string.IsNullOrWhiteSpace(state)) authorizationRequest += "&state=" + state;

            // Return the authorization URL
            return authorizationRequest;
        }

        /// <summary>
        /// Convert authorization code to access token
        /// </summary>
        /// <param name="code">Authorization code from IdentityServer</param>
        /// <returns><see cref="AccessTokenResponse"/> Object</returns>
        /// <exception cref="Exception"></exception>
        public static AccessTokenResponse AuthorizationCallback(string code)
        {

            // Restsharp client
            var client = new RestClient(_tokenEndpoint);

            // OAuth2 Token request parameter
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", RedirectUri);
            request.AddParameter("client_id", ClientId);

            // Get response and deserialize to Token object
            var response = client.Execute(request);
            var token = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

            // If not 
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception("Error retrieving token: " + token.Error);

            // If token is null
            if (string.IsNullOrWhiteSpace(token.AccessToken)) throw new Exception("Error retrieving token: Access Token is not valid");

            // Return the token
            return token;
        }

        /// <summary>
        /// Check access token from IdentityServer
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns>true when access token is valid, otherwise false </returns>
        public static LoginDetail GetUserProfile(string accessToken)
        {
            var result = new LoginDetail()
            {
                Result = LoginResult.ERROR,
                ErrorText = "Error retrieving user profile: Access Token is not valid"
            };

            // Restsharp client
            var client = new RestClient(_userInfoEndpoint);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + accessToken);

            // Check response status code
            var response = client.Execute(request);

            // If response is OK, token is valid
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception("Error retrieving user profile: " + response.Content);

            var userInfo = JsonConvert.DeserializeObject<UserInfoResponse>(response.Content);

            // Return the result
            return userInfo.ToLoginDetail();
        }

        /// <summary>
        /// Use refresh token to get new access token
        /// </summary>
        /// <param name="refreshToken">Refresh token from cookie</param>
        /// <returns>New <see cref="AccessTokenResponse"/>, response from IdentityServer</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="JsonSerializationException"></exception>
        public static AccessTokenResponse RefreshToken(string refreshToken)
        {
            // Restsharp client
            var client = new RestClient(_tokenEndpoint);

            // OAuth2 Token request parameter
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refreshToken);
            request.AddParameter("client_id", ClientId);

            // Get response and deserialize to Token object
            var response = client.Execute(request);
            var newToken = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

            // If not 
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception("Error retrieving token: " + newToken.Error);

            // If token is null
            if (string.IsNullOrWhiteSpace(newToken.AccessToken)) throw new Exception("Error retrieving token: Access Token is not valid");

            // Return the token
            return newToken;
        }

        /// <summary>
        /// Get the user cookies
        /// </summary>
        /// <returns>JWT of Access Token</returns>
        public static System.Web.HttpCookie GetCookie()
        {
            // Get cookie
            var cookie = HttpContext.Current.Request.Cookies[_accessTokenCookieName];

            // If cookie is not null
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value)) return cookie;

            return null;
        }

        /// <summary>
        /// Get the user cookies
        /// </summary>
        /// <returns>JWT of Refresh Token</returns>
        public static System.Web.HttpCookie GetRefreshCookie()
        {
            // Get cookie.
            var cookie = HttpContext.Current.Request.Cookies[_refreshTokenCookieName];

            // If cookie is not null.
            if (cookie != null) return cookie;

            return null;
        }

        public static System.Web.HttpCookie GetStateCookie()
        {
            // Get cookie
            var cookie = HttpContext.Current.Request.Cookies["session_state"];

            // If cookie is not null
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value)) return cookie;

            return null;
        }

        public static string GetState()
        {
            var stateCookie = GetStateCookie();

            return stateCookie?.Value;
        }

        /// <summary>
        /// Set the user cookies
        /// </summary>
        /// <param name="name">Cookie name</param>
        /// <param name="value">Cookie value</param>
        /// <param name="expires">Cookie expires</param>
        private static void SetCookie(string name, string value, DateTime expires)
        {
            // Create cookie.
            var cookie = new System.Web.HttpCookie(name, value);
            cookie.Expires = expires;
            cookie.HttpOnly = true;

            // Set cookie.
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Set access token to the user cookies.
        /// </summary>
        /// <param name="token">
        /// <see cref="AccessTokenResponse"/> object
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetCookie(AccessTokenResponse token)
        {
            // If token is null.
            var jwt = GetJwt(token.AccessToken);

            var expried = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            expried = expried.AddSeconds(jwt.Payload.Exp.Value).ToLocalTime();

            SetCookie(_accessTokenCookieName, token.AccessToken, expried.AddMinutes(-5));
        }

        /// <summary>
        /// Set refresh token to the user cookies.
        /// </summary>
        /// <param name="token">
        /// <see cref="AccessTokenResponse"/> object
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetRefreshCookie(AccessTokenResponse token)
        {

            // If token is null.
            var jwt = GetJwt(token.AccessToken);

            // if refresh token is not set, make feature unavilable.
            if (string.IsNullOrWhiteSpace(_refreshTokenCookieName)) return;

            var expried = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            expried = expried.AddSeconds(jwt.Payload.Exp.Value).ToLocalTime();

            // Set cookie.
            SetCookie(_refreshTokenCookieName, token.RefreshToken, expried.AddMinutes(60));
        }

        public static void SetStateCookie(AccessTokenResponse token, string state)
        {
            // If state is null
            if (string.IsNullOrWhiteSpace(state)) throw new ArgumentNullException("Session State");

            // If token is null.
            var jwt = GetJwt(token.AccessToken);

            var expried = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            expried = expried.AddSeconds(jwt.Payload.Exp.Value).ToLocalTime();

            // Set cookie.
            SetCookie("session_state", state, expried.AddMinutes(60));

        }

        /// <summary>
        /// Clear all cookies
        /// </summary>
        public static void ClearCookies()
        {
            // Remove cookie
            RemoveCookie(_accessTokenCookieName);
            if (!string.IsNullOrEmpty(_refreshTokenCookieName)) RemoveCookie(_refreshTokenCookieName);
            RemoveCookie("session_state");
        }

        /// <summary>
        /// Remove cookie
        /// </summary>
        /// <param name="name">Cookie name</param>
        public static void RemoveCookie(string name)
        {
            var expires = DateTime.Now.AddYears(-1);
            SetCookie(name, null, expires);
        }

        /// <summary>
        /// Validate the token is expired
        /// </summary>
        /// <returns>
        /// true if token is expired, otherwise false
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ValidateTokenExpired()
        {
            var cookie = GetCookie();
            if (cookie is null) throw new NullReferenceException("Cookie is not setted");

            return ValidateTokenExpired(cookie.Value);
        }
        /// <summary>
        /// Validate the token is expired
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns>
        /// true if token is expired, otherwise false
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ValidateTokenExpired(string token)
        {
            return ValidateTokenExpired(token, -5);
        }

        /// <summary>
        /// Validate the token is expired
        /// </summary>
        /// <param name="token">Access token</param>
        /// <param name="calibrateMinute">Number of minute to adjust expired time</param>
        /// <returns>
        /// true if token is expired, otherwise false
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ValidateTokenExpired(string token, int calibrateMinute)
        {
            // Check token is setted.
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException("Token is not valid");

            var result = false;

            // Get JWT from token
            var jwt = GetJwt(token);

            // Get expired time
            var exp = jwt.Payload.Exp;

            // If exp is not null
            if (exp != null)
            {
                //Check expired time
                var expDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                expDate = expDate.AddSeconds(exp.Value).ToLocalTime();

                // If now is already pass expried, return true
                if (expDate.AddMinutes(calibrateMinute) < DateTime.Now) result = true;
            }

            return result;
        }

        // Validate near expried token
        public static bool ValidateRefreshToken(string token)
        {
            return ValidateTokenExpired(token, -15);
        }

        /// <summary>
        /// Get <see cref="JwtSecurityToken "/> from cookie
        /// </summary>
        /// <returns><see cref="JwtSecurityToken "/> objext</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static JwtSecurityToken GetJwt()
        {
            var cookie = GetCookie();
            if (cookie == null) throw new NullReferenceException("Token is not valid");

            var token = cookie.Value;

            return GetJwt(token);
        }

        /// <summary>
        /// Get <see cref="JwtSecurityToken "/> from token
        /// </summary>
        /// <returns><see cref="JwtSecurityToken "/> objext</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static JwtSecurityToken GetJwt(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException("Token is not valid");

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jwt;
        }
        /// <summary>
        /// Get roles in concatenate string from cookie 
        /// </summary>
        /// <returns>string of roles sperated by comma (,)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetRoles()
        {
            var cookie = GetCookie();
            if (cookie == null) throw new ArgumentNullException("Token is not valid");

            var token = cookie.Value;

            return GetRoles(token);
        }

        /// <summary>
        /// Get roles in concatenate string from token
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetRoles(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentNullException("Token is not valid");

            var jwt = GetJwt(accessToken);

            var roles = jwt.Claims.Where(x => x.Type == "role").Select(x => x.Value).ToArray();

            return string.Join(",", roles);
        }

        public static LoginDetail GetLoginDetail()
        {
            var cookie = GetCookie();
            if (!(cookie is null) || !string.IsNullOrWhiteSpace(cookie.Value))
            {
                return GetLoginDetail(cookie.Value);
            }

            return new LoginDetail()
            {
                Result = LoginResult.ERROR,
                ErrorText = "Token is not setted",
            };

        }

        public static LoginDetail GetLoginDetail(string accessToken)

        {
            var result = new LoginDetail
            {
                Result = LoginResult.ERROR,
                ErrorText = "Token is not setted",
            };

            try
            {
                if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentNullException("Token is not setted");

                var checkIdToken = ValidateTokenExpired(accessToken, -30);

                if (checkIdToken)
                {
                    // Get User's information from userinfo endpoint
                    result = GetUserProfile(accessToken);
                }

                else
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfoResponse>(JsonConvert.SerializeObject(GetJwt(accessToken).Payload));

                    result = userInfo.ToLoginDetail();
                }


            }
            catch (Exception ex)
            {
                result.ErrorText = ex.Message;
            }

            return result;
        }

        private static int CheckNullInt(object obj)
        {
            if (obj == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }


        private static string CheckNullString(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }

    }

    public class UserInfoResponse
    {
        [JsonProperty("sub")]
        public string Sub { get; set; }
        [JsonProperty("role")]
        public string[] Roles { get; set; }
        [JsonProperty("preferred_username")]
        public string PreferredUsername { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("email_verified")]
        public bool? EailVerified { get; set; }
        [JsonProperty("user_id")]
        public int? User_ID { get; set; }
        public string Username { get; set; }
        public int? Person_ID { get; set; }
        public int? Employee_ID { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Branch_ID { get; set; }
        public string BranchDetail { get; set; }
        public int? Department_ID { get; set; }
        public string DepartmentDetail { get; set; }
        public int? EmployeeTeam_ID { get; set; }
        public string EmployeeTeamDetail { get; set; }
        public int? EmployeePosition_ID { get; set; }
        public string EmployeePositionDetail { get; set; }
        public int? Organize_ID { get; set; }
        public string OrganizeCode { get; set; }
        public string OrganizeDetail { get; set; }
        public LoginDetail ToLoginDetail()
        {
            if (User_ID is null || Branch_ID is null || Department_ID is null) return new LoginDetail()
            {
                Result = LoginResult.ERROR,
                ErrorText = "ข้อมูลไม่ครบถ้วน"
            };

            return new LoginDetail()
            {
                Result = LoginResult.SUCCESS,
                ErrorText = "",
                UserName = Username,
                EmpCode = EmployeeCode,
                User_ID = User_ID ?? -1,
                Person_ID = Person_ID ?? -1,
                Employee_ID = Employee_ID ?? -1,
                FirstName = FirstName,
                LastName = LastName,
                EmployeePositionDetail = EmployeePositionDetail,
                EmployeeTeamDetail = EmployeeTeamDetail,
                BranchDetail = BranchDetail,
                DepartmentDetail = DepartmentDetail,
                EmployeeTeam_ID = EmployeeTeam_ID ?? -1,
                Department_ID = Department_ID ?? -1,
                Branch_ID = Branch_ID ?? -1,
                EmployeePosition_ID = EmployeePosition_ID ?? -1,
                OrganizeCode = OrganizeCode,
                OrganizeDetail = OrganizeDetail,
                Organize_ID = Organize_ID,
                EmployeeCode = EmployeeCode,
                FullName = FullName,
            };
        }
    }

    public class AccessTokenResponse
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; } = 0;

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}