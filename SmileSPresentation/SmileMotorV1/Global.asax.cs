using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Optimization;
using System.Data.Entity;
using System.Web.Security;
using System.Web.SessionState;
using SmileMotorV1.Models;
using SmileMotorV1.Logic;
using SmileMotorV1.App_Start;

namespace SmileMotorV1
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RoleAction roleActions = new RoleAction();
            roleActions.AddUserAndRole();
        }

    }
}