using System.Web.Optimization;
using WebHelpers.Mvc5;

namespace SmileUnderwriteBranchV2.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/css")
                .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/bootstrap-select.css")
                .Include("~/Content/css/bootstrap-datepicker3.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/js/plugins/datepicker/css/datepicker.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/icheck/blue.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/AdminLTE.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/js/plugins/datatable/datatables.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/js/plugins/apexChart/apexcharts.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/StyleSheetDatatable.css")
                .Include("~/Content/css/skins/skin-blue.css")
                .Include("~/Content/css/select2/select2.min.css")
                .Include("~/Content/css/sweetalert/sweetalert.css")
                .Include("~/Content/js/plugins/dropzone/dropzone.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/sweetalert/themes/google.css")
                .Include("~/Content/css/mySpinner.css")
                .Include("~/Content/css/mySpinnerCancer.css")
                .Include("~/Content/css/toastr/toastr.min.css")
                .Include("~/Content/css/myCardStyle.css")
                .Include("~/Content/css/MaterialIconsStyle.css")
                .Include("~/Content/css/myStyle.css")
                .Include("~/Content/css/myStyleQuestions.css")
                .Include("~/Content/css/magnify/jquery.magnify.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/magnify/jquery.magnify.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/myStyleSheetDatatable.css"));

            bundles.Add(new ScriptBundle("~/Bundles/js")
               .Include("~/Content/js/plugins/jquery/jquery-2.2.4.js")
                .Include("~/Content/js/plugins/bootstrap/bootstrap.js")
                .Include("~/Content/js/plugins/fastclick/fastclick.js")
                .Include("~/Content/js/plugins/slimscroll/jquery.slimscroll.js")
                .Include("~/Content/js/plugins/bootstrap-select/bootstrap-select.js")
                .Include("~/Content/js/plugins/moment/moment.js")
                .Include("~/Content/js/plugins/datepicker/js/bootstrap-datepicker.js")
                .Include("~/Content/js/plugins/datepicker/js/bootstrap-datepicker-thai.js")
                .Include("~/Content/js/plugins/datepicker/js/locales/bootstrap-datepicker.th.js")
                .Include("~/Content/js/plugins/icheck/icheck.js")
                .Include("~/Content/js/plugins/apexChart/apexcharts.min.js")
                .Include("~/Content/js/plugins/chart/Chart.min.js")
                .Include("~/Content/js/plugins/chart/chartjs-plugin-labels.min.js")
                .Include("~/Content/js/plugins/validator/validator.js")
                .Include("~/Content/js/plugins/jquery-validation/dist/jquery.validate.js")
                .Include("~/Content/js/plugins/inputmask/jquery.inputmask.bundle.js")
                .Include("~/Content/js/plugins/datatable/datatables.js")
                .Include("~/Content/js/plugins/select2/js/select2.min.js")
                .Include("~/Content/js/plugins/dropzone/dropzone.js")
                .Include("~/Content/js/plugins/sweetalert/dist/sweetalert.min.js")
                .Include("~/Content/js/plugins/toastr/toastr.min.js")
                .Include("~/Content/js/adminlte.js")
                .Include("~/Content/js/jQueryValidatorMessage.js")
                .Include("~/Content/js/myScript.js", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/js/init.js", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/js/plugins/magnify/jquery.magnify.js")
                .Include("~/Content/js/plugins/magnify/jquery.magnify.min.js")
                .Include("~/Content/js/plugins/pdfobject/pdfobject.js")
                .Include("~/Content/js/plugins/pdfobject/pdfobject.min.js")
                .Include("~/Scripts/jquery.signalR-2.4.1.min.js"));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}