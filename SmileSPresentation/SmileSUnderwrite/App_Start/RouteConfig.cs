using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSUnderwrite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "QueueCall", action = "QueueCallIndex", id = UrlParameter.Optional }
            );
        }
    }
}