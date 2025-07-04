using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using SmileSPoint.Helper;

namespace SmileSPoint.Helper
{
    public class APIAuthorization : AuthorizationFilterAttribute
    {
        /// <summary>
        /// read requested header and validated
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //Get Key from actionContext
            var actionContextKey = actionContext.Request.Headers.GetValues("Authorization").First();

            //Get User from key
            var user = GlobalFunction.GetAPIUserFromJWTKey(actionContextKey);

            if (string.IsNullOrEmpty(user))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            //Get valid user
            var validUser = new List<String>();
            //TODO: Add valid user here
            validUser.Add("14"); // for mockup

            var IsAuthorize = validUser.Contains(user);

            //IF unauthorize
            if (!IsAuthorize)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            base.OnAuthorization(actionContext);
        }
    }
}