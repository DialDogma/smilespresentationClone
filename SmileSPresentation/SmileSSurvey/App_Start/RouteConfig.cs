using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSSurvey
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
            name: "swagger_root",
            routeTemplate: "",
            defaults: null,
            constraints: null,
            handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

            routes.MapMvcAttributeRoutes(); //Enables Attribute Routing

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "SurveyResearch", action = "SurveyMonitor", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Satisfaction",
               url: "st/{surveyId}",
               defaults: new { controller = "Satisfaction", action = "Satisfaction", surveyId = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "SurveyResearch",
               url: "sr/{surveyId}",
               defaults: new { controller = "SurveyResearch", action = "SurveyResearch", surveyId = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Success",
                url: "success",
                defaults: new { controller = "Home", action = "Success", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "SurveyDuplicate",
            url: "surveyduplicate",
            defaults: new { controller = "Home", action = "SurveyDuplicate", id = UrlParameter.Optional }
        );

            routes.MapRoute(
              name: "SurveySuccess",
              url: "surveysuccess",
              defaults: new { controller = "Home", action = "SurveySuccess", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "Fail",
               url: "fail",
               defaults: new { controller = "Home", action = "Fail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "SendSurvey",
              url: "sendsurvey",
              defaults: new { controller = "SendSurvey", action = "SendSurvey", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "ReportSurvey",
              url: "reportsurvey",
              defaults: new { controller = "ReportSurvey", action = "GetSmileCRMReport", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "FeedbackSuccess",
              url: "fb-success",
              defaults: new { controller = "AgentFeedback", action = "SurveySuccess", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "FeedbackFail",
               url: "fb-fail",
               defaults: new { controller = "AgentFeedback", action = "Fail", id = UrlParameter.Optional }
           );
        }
    }
}