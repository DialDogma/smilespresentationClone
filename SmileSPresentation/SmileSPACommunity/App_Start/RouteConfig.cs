﻿using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSPACommunity
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           routes.MapHttpRoute(
           name: "swagger_root",
           routeTemplate: "swagger",
           defaults: null,
           constraints: null,
           handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

            routes.MapMvcAttributeRoutes(); //Enables Attribute Routing

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
