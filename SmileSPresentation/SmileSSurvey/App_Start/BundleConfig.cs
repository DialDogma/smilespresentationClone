using System.Web.Optimization;
using WebHelpers.Mvc5;

namespace SmileSSurvey.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/css")
                .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/bootstrap-select.css")
                .Include("~/Content/css/bootstrap-datepicker3.min.css")
                .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/icheck/blue.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/AdminLTE.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/sweetalert/sweetalert.css")
                .Include("~/Content/css/sweetalert/themes/google.css")
                .Include("~/Content/css/skins/skin-blue.css")
                .Include("~/Content/css/mySpinner.css")
                .Include("~/Content/css/myStyle.css")
                .Include("~/Content/css/myStyleSheetDatatable.css")
                .Include("~/Content/js/plugins/datatable/datatables.css"));

            bundles.Add(new ScriptBundle("~/Bundles/js")
                .Include("~/Content/js/plugins/jquery/jquery-3.3.1.js")
                .Include("~/Content/js/plugins/bootstrap/bootstrap.js")
                .Include("~/Content/js/plugins/fastclick/fastclick.js")
                .Include("~/Content/js/plugins/slimscroll/jquery.slimscroll.js")
                .Include("~/Content/js/plugins/bootstrap-select/bootstrap-select.js")
                .Include("~/Content/js/plugins/moment/moment.js")
                .Include("~/Content/js/plugins/datepicker/bootstrap-datepicker.js")
                .Include("~/Content/js/plugins/icheck/icheck.js")
                .Include("~/Content/js/plugins/validator/validator.js")
                .Include("~/Content/js/plugins/jquery-validation/dist/jquery.validate.js")
                .Include("~/Content/js/jQueryValidatorMessage.js")
                .Include("~/Content/js/jQueryValidator.js")
                .Include("~/Content/js/plugins/inputmask/jquery.inputmask.bundle.js")
                .Include("~/Content/js/plugins/sweetalert/dist/sweetalert.min.js")
                .Include("~/Content/js/plugins/datatable/datatables.js")
                .Include("~/Content/js/adminlte.js")
                .Include("~/Content/js/myScript.js")
                .Include("~/Content/js/init.js"));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}