using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MassTransit;
using SmileSLogin.App_Start;
using SmileSLogin.Consumers;
using SmileSLogin.Controllers;

namespace SmileSLogin
{
    public class MvcApplication : HttpApplication
    {
        //static IBusControl _bus;
        //static BusHandle _busHandle;
        //public static IBus Bus
        //{
        //    get { return _bus; }
        //}

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            // Manually installed WebAPI 2.2 after making an MVC project.
            GlobalConfiguration.Configure(WebApiConfig.Register); // NEW way

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //_bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(configurator => {
            //    // Login เข้ำ RabbitMQ
            //    configurator.Host(Properties.Settings.Default.RabbitMqHost, h => {
            //        h.Username(Properties.Settings.Default.RabbitMqUsername);
            //        h.Password(Properties.Settings.Default.RabbitMqPassword);
            //    });

            //    configurator.ReceiveEndpoint(Properties.Settings.Default.SmileSLoginQueue, e =>
            //    {
            //        e.Consumer<ChangePasswordConsumer>();
            //    });
            //});

            //_busHandle = MassTransit.Util.TaskUtil.Await<BusHandle>(() => _bus.StartAsync());

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
        
        protected void Application_End()
        {
            //if (_busHandle != null)
            //    _busHandle.Stop(TimeSpan.FromSeconds(30));
        }
    }
}