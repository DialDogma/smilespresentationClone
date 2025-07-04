using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Serilog;
using SmileSClaimPayBack.App_Start;
using SmileSClaimPayBack.Controllers;
using MassTransit;
using static MassTransit.MessageHeaders;
using SmileSClaimPayBack.Consumers;

namespace SmileSClaimPayBack
{
    public class MvcApplication : HttpApplication
    {
        static IBusControl _bus;
        static BusHandle _busHandle;

        public static IBus bus
        {
            get { return _bus; }
        }

        protected void Application_Start()
        {
            var RabbitIp = Properties.Settings.Default.RabbitMqIP;
            var RabbitUser = Properties.Settings.Default.RabbitMqUser;
            var RabbitPass = Properties.Settings.Default.RabbitMqPass;


            var setting = Properties.Settings.Default;

            Log.Information("Starting up...");
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                configurator.Host(RabbitIp, h =>
                {
                    h.Username(RabbitUser);
                    h.Password(RabbitPass);
                });

                // สำหรับ update status การสร้างรายการ
                configurator.ReceiveEndpoint(setting.ClaimPayBackQueueCreated, e =>
                {
                    e.Consumer<TempPayListHeaderCreatedConsumer>();
                });

                // สำหรับ update status การโอนเงิน
                configurator.ReceiveEndpoint(setting.ClaimPayBackQueueResult, e =>
                {
                    e.Consumer<PayListResultConsumer>();
                });

                // update status reconcile result
                configurator.ReceiveEndpoint(setting.ClaimPayBackReconcileResult, e =>
                {
                    e.Consumer<ReconcileResultConsumer>();  
                });
            });


            _busHandle = MassTransit.Util.TaskUtil.Await<BusHandle>(() => _bus.StartAsync());


            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Log.Information("Routes and bundles registered");
            Log.Information("Started");

            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);
            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            Log.Information("SmileSClaimPayBack V1.0 Start web host.");

        }

        protected void Application_End()
        {
            if (_busHandle != null)
            {
                _busHandle.Stop(TimeSpan.FromSeconds(30));
            }

            Log.Information("Stopped");
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
