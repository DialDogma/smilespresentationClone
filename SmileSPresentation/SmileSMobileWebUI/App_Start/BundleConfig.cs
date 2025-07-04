using System.Web;
using System.Web.Optimization;

namespace SmileSMobileWebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/JsBarcode").Include(
                        "~/Scripts/JsBarcode-master/dist/JsBarcode.all.js"));

            bundles.Add(new ScriptBundle("~/bundles/qrcode").Include(
                        "~/Scripts/qrcodejs-master/qrcode.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //My Script
            bundles.Add(new ScriptBundle("~/bundles/myScript").Include(
                      "~/Scripts/myScript.js",
                      "~/Scripts/stickyfill.min.js",
                      "~/Scripts/moment/moment-with-locales.min.js",
                      "~/Scripts/datatable/datatables.js",
                      "~/Scripts/datatables-mark-js/dist/datatables.mark.js"));
            //My Style
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/site.css",
                      "~/Content/Style.css",
                      "~/Scripts/datatable/datatables.css"));
        }
    }
}