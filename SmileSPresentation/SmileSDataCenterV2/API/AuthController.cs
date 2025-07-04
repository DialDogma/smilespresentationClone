
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using System.Web.Mvc;
using SmileSDataCenterV2.Models;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace SmileSDataCenterV2.API
{
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
       
        private readonly DataCenterV1Entities _context;

        public AuthController() => _context = new DataCenterV1Entities();

        
        [HttpGet]
        [Route("GetUserInformation/{username}")]
        public IHttpActionResult GetUserInformation(string username)
        {
            var result = _context.usp_AspNetUsersDetailByUsername_Select(username).FirstOrDefault(); ;
            
             return Json(result);
            
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword([FromBody] Auth_ChangePassword_DTO password)
        {
            bool result;
            var user = _context.usp_AspNetUsersDetailByUsername_Select(password.Username).FirstOrDefault();
            

            UserManager manager = new UserManager();
            IdentityResult RemoveResult = manager.RemovePassword(user.Id);
            if (RemoveResult.Succeeded)
            {
                //when success remove old password,then add new password
                IdentityResult SetPasswordResult = manager.AddPassword(user.Id, password.PasswordNew);
                if (SetPasswordResult.Succeeded)
                {
                    var sssService = new SSSLoginService.SSSLoginServiceSoapClient();
                    bool resultSSS = sssService.ChangePasswordSSS(password.Username, password.PasswordNew);
                    return Json(resultSSS);
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

            return Json(result);
        }

        [HttpGet]
        [Route("GetUserRoles/{username}")]
        public IHttpActionResult GetUserRoles(string username)
        {
            var result = _context.usp_GetUserRoles(username).ToList(); ;

            return Json(result);

        }
    }
}