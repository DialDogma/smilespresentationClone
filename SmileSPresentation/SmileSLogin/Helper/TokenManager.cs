using JWT.Algorithms;
using JWT.Builder;
using SmileSLogin.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace SmileSLogin.Helper
{
    public class TokenManager
    {
        /// <summary>
        /// secret key
        /// </summary>
        private string secretKey = "";

        /// <summary>
        /// Token name
        /// </summary>
        private string tokenName = "";

        public TokenManager()
        {
            secretKey = Properties.Settings.Default.SecretKey;
            tokenName = Properties.Settings.Default.TokenName;
        }

        /// <summary>
        /// Create token from loginDetail
        /// </summary>
        /// <param name="loginDetail"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Boolean CreateToken(LoginDetail loginDetail, HttpContextBase httpContext)
        {
            var token = GenerateToken(loginDetail);

            var expDays = Properties.Settings.Default.TokenExpireDays;

            //Add Cookie
            HttpCookie myCookie = new HttpCookie(Properties.Settings.Default.TokenName);
            myCookie.Value = token;
            myCookie.Expires = DateTime.Now.AddDays(expDays);
            httpContext.Response.Cookies.Add(myCookie);

            return true;
        }

        public string GenerateToken(LoginDetail loginDetail)
        {
            var expDays = Properties.Settings.Default.TokenExpireDays;
            var token = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secretKey)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(expDays).ToUnixTimeSeconds())
                    .AddClaim("Username", loginDetail.UserName)
                    .AddClaim("User_ID", loginDetail.User_ID)
                    .AddClaim("Person_ID", loginDetail.Person_ID)
                    .AddClaim("Employee_ID", loginDetail.Employee_ID)
                    .AddClaim("FirstName", loginDetail.FirstName)
                    .AddClaim("LastName", loginDetail.LastName)
                    .AddClaim("EmployeePositionDetail", loginDetail.EmployeePositionDetail)
                    .AddClaim("EmployeeTeamDetail", loginDetail.EmployeeTeamDetail)
                    .AddClaim("BranchDetail", loginDetail.BranchDetail)
                    .AddClaim("DepartmentDetail", loginDetail.DepartmentDetail)
                    .AddClaim("EmployeeTeam_ID", loginDetail.EmployeeTeam_ID)
                    .AddClaim("Branch_ID", loginDetail.Branch_ID)
                    .AddClaim("Department_ID", loginDetail.Department_ID)
                    .AddClaim("EmployeePosition_ID", loginDetail.EmployeePosition_ID)
                    .AddClaim("Organize_ID", loginDetail.Organize_ID)
                    .AddClaim("OrganizeDetail", loginDetail.OrganizeDetail)
                    .AddClaim("OrganizeCode", loginDetail.OrganizeCode)
                    .AddClaim("FullName", loginDetail.FirstName + " " + loginDetail.LastName)
                    .AddClaim("EmployeeCode", loginDetail.EmployeeCode)
                    .Build();

            //Insert in database
            var db = new Models.DataCenterV1Entities();
            db.usp_AddToken(token, DateTime.Now.AddDays(expDays), loginDetail.UserName);

            return token;
        }

        /// <summary>
        /// Get token detail / also check valid token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Token GetToken(HttpContextBase httpContext)
        {
            //Read Token from cookie
            var c = httpContext.Request.Cookies[tokenName];
            Token t = new Token();

            //If token not null
            if (c != null)
            {
                //If token valid
                try
                {
                    var payload = new JwtBuilder()
                            .WithSecret(secretKey)
                            .MustVerifySignature()
                            .Decode<IDictionary<string, object>>(c.Value);

                    //Valid Token,Get details
                    t.Result = TokenResult.SUCCESS;
                    t.ErrorText = "";
                    t.UserName = (payload["Username"]).ToString();
                    t.User_ID = Convert.ToInt32(payload["User_ID"]);
                    t.Person_ID = Convert.ToInt32(payload["Person_ID"]);
                    t.Employee_ID = Convert.ToInt32(payload["Employee_ID"]);
                    t.FirstName = (payload["FirstName"]).ToString();
                    t.LastName = (payload["LastName"]).ToString();
                    t.EmployeePositionDetail = (payload["EmployeePositionDetail"]).ToString();
                    t.EmployeeTeamDetail = (payload["EmployeeTeamDetail"]).ToString();
                    t.BranchDetail = (payload["BranchDetail"]).ToString();
                    t.DepartmentDetail = (payload["DepartmentDetail"]).ToString();
                    t.EmployeeTeam_ID = Convert.ToInt32(payload["EmployeeTeam_ID"]);
                    t.Department_ID = Convert.ToInt32(payload["Department_ID"]);
                    t.Branch_ID = Convert.ToInt32(payload["Branch_ID"]);
                    t.EmployeeCode = payload["EmployeeCode"].ToString();
                    t.EmployeePosition_ID = Convert.ToInt32(payload["EmployeePosition_ID"]);
                    t.FullName = (payload["FullName"]).ToString();
                }

                //Token Expire
                catch (JWT.TokenExpiredException)
                {
                    //token has expire
                    t.Result = TokenResult.ERROR;
                    t.ErrorText = "Token has expired";
                }

                //Invalid Signature
                catch (JWT.SignatureVerificationException)
                {
                    //token has invalid signature
                    t.Result = TokenResult.ERROR;
                    t.ErrorText = "Token has invalid signature";
                }

                //other
                catch (Exception ex)
                {
                    t.Result = TokenResult.ERROR;
                    t.ErrorText = ex.ToString();
                }
            }
            else
            {
                t.Result = TokenResult.ERROR;
                t.ErrorText = "Login Required";
            }

            return t;
        }
    }
}