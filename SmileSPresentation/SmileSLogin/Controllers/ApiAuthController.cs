using SmileSLogin.Helper;
using SmileSLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SmileSLogin.Controllers
{
    [RoutePrefix("api")]
    public class ApiAuthController : ApiController
    {
        /// <summary>
        /// สำหรับ Get Token ไปใช้ในการทดสอบ API
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetLoginToken(string username, string password)
        {
            //Login via login service
            var result = AuthMethod.Login(username, password);
            var tokenString = "";

            if (result != null)
            {
                tokenString = new TokenManager().GenerateToken(result);
            }
            return Json(tokenString);
        }

        /// <summary>
        /// สำหรับ Login Get Token (V2)
        /// </summary>
        /// <param name="login">from body username,password</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public IHttpActionResult LoginToken([FromBody] Login login)
        {
            //Login via login service
            var result = AuthMethod.Login(login.Username.Trim(), login.Password.Trim());

            if (result == null) return Content(HttpStatusCode.Unauthorized, "Unauthorized");
            if (result.EmployeeStatus_ID == 6) return Content(HttpStatusCode.OK, "Resign");
            return Json(new TokenManager().GenerateToken(result));
        }
    }
}