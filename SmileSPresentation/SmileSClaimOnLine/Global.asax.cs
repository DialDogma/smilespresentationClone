using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MassTransit;
using PayTransferAPI.Contract;
using Serilog;
using SmileSClaimOnLine.App_Start;
using SmileSClaimOnLine.Consumers;
using SmileSClaimOnLine.Controllers;

namespace SmileSClaimOnLine
{
    public class MvcApplication : HttpApplication
    {
        private static IBusControl _bus;
        private static BusHandle _busHandle;

        public static IBus Bus
        {
            get { return _bus; }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //06074 Add Serilog 20220801
            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);

            _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                // Login เข้ำ RabbitMQ
                configurator.Host(Properties.Settings.Default.RabbitMqHost, h =>
                {
                    h.Username(Properties.Settings.Default.RabbitMqUsername);
                    h.Password(Properties.Settings.Default.RabbitMqPassword);
                });

                configurator.ReceiveEndpoint(e =>
                {
                    e.Consumer<PayListResultConsumer>(config =>
                    {
                        config.UseConcurrentMessageLimit(1);
                        config.UseConcurrencyLimit(1);
                    });
                    e.Consumer<TempPayListHeaderCreatedConsumer>();
                });
            });
            _busHandle = MassTransit.Util.TaskUtil.Await<BusHandle>(() => _bus.StartAsync());

            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            Log.Information("SmileSClaimOnLine Start web host.");
        }

        protected void Application_End()
        {
            if (_busHandle != null)
                _busHandle.Stop(TimeSpan.FromSeconds(30));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();

            if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            {
                return;
            }

            Log.Error(exception, "SmileSClaimOnLine Application Error");

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