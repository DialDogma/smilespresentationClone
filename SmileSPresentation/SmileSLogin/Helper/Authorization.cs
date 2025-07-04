using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSLogin.Helper
{
    public class Authorization : ActionFilterAttribute, IActionFilter
    {
        public string Roles;

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Get Cookie
            var tokenName = Properties.Settings.Default.TokenName;
            var c = filterContext.HttpContext.Request.Cookies[tokenName];

            //If Has Cookie
            if (c != null)
            {
                //Validate Cookie
                var validateCookie = new Services.SSOService().ValidateKey(c.Value);

                //Cookie Validate
                if (validateCookie)
                {
                    //Check Authorize
                    var token = new Helper.TokenManager().GetToken(filterContext.HttpContext);
                    var userRole = new Services.SSOService().GetRoleByUserName(token.UserName);
                    if ((Roles == null) || IsAuthen(Roles, userRole))
                    {
                        //Execute
                        OnActionExecuting(filterContext);
                    }
                    else
                    {
                        //Un authorize
                        filterContext.Result = new RedirectToRouteResult(
                          new RouteValueDictionary
                          {
                                 {"Controller", "Auth"},
                                 {"Action", "Unauthorize"}
                          });
                    }
                }
                //If Cookie Not Validate
                else
                {
                    //Delete cookie
                    HttpCookie myCookie = new HttpCookie("tokenName");
                    myCookie.Value = "";
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    filterContext.HttpContext.Response.Cookies.Add(myCookie);

                    //Go To Login
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                                    {"Controller", "Auth"},
                                    {"Action", "Login"}
                        });
                }
            }
            //No Cookie Go to login page
            else
            {
                {
                    filterContext.Result = new RedirectToRouteResult(
                         new RouteValueDictionary
                         {
                                    {"Controller", "Auth"},
                                    {"Action", "Login"}
                         });
                }
            }
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
    }
}