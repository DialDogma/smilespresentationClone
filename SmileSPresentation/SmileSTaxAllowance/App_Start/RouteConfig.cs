using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSTaxAllowance
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(); //Enables Attribute Routing

            routes.MapRoute(
               name: "TaxCustomer63",
               url: "TaxCustomer63",
               defaults: new { controller = "TaxCustomer63", action = "TaxCustomerSearch", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "TaxCustomer64",
               url: "TaxCustomer64",
               defaults: new { controller = "TaxCustomer64", action = "TaxCustomerSearch", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "TaxCustomer65",
               url: "TaxCustomer65",
               defaults: new { controller = "TaxCustomer65", action = "TaxCustomerSearch", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "TaxCustomer66",
               url: "TaxCustomer66",
               defaults: new { controller = "TaxCustomer66", action = "TaxCustomerSearch", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "TaxCustomer67",
              url: "TaxCustomer67",
              defaults: new { controller = "TaxCustomer67", action = "TaxCustomerSearch", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Tax", action = "TaxAllowance2567Monitor", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "PDPA",
                url: "confirm/{id}",
                defaults: new { controller = "PDPA", action = "PDPAConfirmData", id = UrlParameter.Optional }
                );

            routes.MapRoute(
               name: "PDPADetail",
               url: "Detail/{id}",
               defaults: new { controller = "PDPA", action = "PDPADetail", id = UrlParameter.Optional }
               );
        }
    }
}