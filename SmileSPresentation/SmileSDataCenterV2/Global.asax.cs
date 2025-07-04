using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Metadata;
using NLog;
using SmileSDataCenterV2.App_Start;
using SmileSDataCenterV2.Consumer;
using SmileSDataCenterV2.Contracts;
using SmileSDataCenterV2.Controllers;
using SmileSDataCenterV2.Helper;

namespace SmileSDataCenterV2
{
    public class MvcApplication : HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        //MassTransit
        private static IBusControl _bus;

        private static BusHandle _busHandle;

        public static IBus Bus => _bus;

        protected void Application_Start()
        {
            Log.Info("Starting up...");
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            routeConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Log.Info("Routes and bundles registered");
            Log.Info("Started");

            //Start RabbitMQ
            StartBus();
        }

        protected void Application_End()
        {
            Log.Info("Stopped");

            //Stop RabbitMQ
            StopBus();
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

        #region RabbitMQ

        public void StartBus()
        {
            try
            {
                _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(Properties.Settings.Default.RabbitMQ_URL, "/", h =>
                    {
                        h.Username(Properties.Settings.Default.RabbitMQ_Username);
                        h.Password(Properties.Settings.Default.RabbitMQ_Password);
                    });

                    // ===============================
                    // 🔹 กลุ่ม: Consumer ทั่วไปภายในระบบ
                    // ===============================
                    // ตั้งชื่อ formatter
                    var formatter = new KebabCaseEndpointNameFormatter("datacenter-api", false);
                    cfg.ReceiveEndpoint(formatter.Consumer<TestConsumer>(), e =>
                    {
                        e.Consumer<TestConsumer>();
                    });

                    // ===============================
                    // 🔹 กลุ่ม: เชื่อมกับระบบอื่นๆ เช่น .Net Core / .NET 6 (API Template)
                    // ===============================
                    // ตั้งชื่อ formatter แบบเฉพาะ
                    var formatter2 = new KebabCaseEndpointNameFormatter("policy-api", false);
                    cfg.ReceiveEndpoint(formatter2.Consumer<PolicyPremiumDebtCreateConsumer>(), e =>
                    {
                        e.Consumer<PolicyPremiumDebtCreateConsumer>();
                    });
                });

                _busHandle = _bus.StartAsync().GetAwaiter().GetResult();
                Console.WriteLine("RabbitMQ Bus Started Successfully!");

                // ตรวจสอบสถานะการเชื่อมต่อ
                CheckConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ Connection Failed: {ex.Message}");
            }
        }

        private void CheckConnection()
        {
            try
            {
                var healthStatus = _bus.CheckHealth();
                if (healthStatus.Status == BusHealthStatus.Healthy)
                {
                    Console.WriteLine("RabbitMQ is Connected and Healthy.");
                }
                else
                {
                    Console.WriteLine("RabbitMQ is Not Healthy.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Checking RabbitMQ Health: {ex.Message}");
            }
        }

        public void StopBus()
        {
            _busHandle?.StopAsync().GetAwaiter().GetResult();
            Console.WriteLine("RabbitMQ Bus Stopped.");
        }

        #endregion RabbitMQ
    }
}