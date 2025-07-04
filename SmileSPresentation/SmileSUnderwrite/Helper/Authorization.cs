using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSUnderwrite.Helper
{
    public class Authorization : ActionFilterAttribute, IActionFilter
    {
        public string Roles;

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session["Username"] != null & session["Role"] != null)
            {
                if ((Roles == null) || IsAuthen(Roles, session["Role"].ToString()))
                {
                    OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                      new RouteValueDictionary
                      {
                        {"Controller", "Auth"},
                        {"Action", "Unauthorize"}
                      });
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                  new RouteValueDictionary
                  {
                        {"Controller", "Auth"},
                        {"Action", "LogIn"}
                  });
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