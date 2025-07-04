using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SmileSSurvey.App_Start;
using SmileSSurvey.Controllers;
using NLog;
using System.Web.Http;
using System.Web.Routing;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Serilog;

namespace SmileSSurvey
{
    public class MvcApplication : HttpApplication
    {
        private static readonly Logger _Log = LogManager.GetCurrentClassLogger();
        static IBusControl _bus;
        static BusHandle _busHandle;
        public static IBus Bus
        {
            get { return _bus; }
        }

        protected void Application_Start()
        {
            _Log.Info("Starting up...");
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Add Serilog 20220801
            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);
            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            Log.Information("SmileSSurvey Start web host.");

            _Log.Info("Routes and bundles registered");
            _Log.Info("Started");

            _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                // Login เข้ำ RabbitMQ
                configurator.Host(Properties.Settings.Default.RabbitMqHost, "/", h =>
                 {
                     h.Username(Properties.Settings.Default.RabbitMqUsername);
                     h.Password(Properties.Settings.Default.RabbitMqPassword);
                 });
            });
            _busHandle = MassTransit.Util.TaskUtil.Await<BusHandle>(() => _bus.StartAsync());
        }

        protected void Application_End()
        {
            if (_busHandle != null)
                _busHandle.Stop(TimeSpan.FromSeconds(30));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Log.Error(exception, "Unhandled application exception");

            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();

            if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            {
                return;
            }

            ExecuteErrorController(httpContext, exception as HttpException);
        }

        private void ExecuteErrorController(HttpContext httpContext, HttpException exception)
        {
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";

            if (exception != null && exception.GetHttpCode() == (int)HttpStatusCode.NotFound)
            {
                routeData.Values["action"] = "NotFound";
            }
            else
            {
                routeData.Values["action"] = "InternalServerError";
            }

            using (Controller controller = new ErrorController())
            {
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
        }
    }
}