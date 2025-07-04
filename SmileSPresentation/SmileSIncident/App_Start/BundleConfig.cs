using System.Web.Optimization;
using WebHelpers.Mvc5;

namespace SmileSIncident.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/css")
                 .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/css/bootstrap-select.css")
                 .Include("~/Content/css/bootstrap-datepicker3.min.css")
                 .Include("~/Content/js/plugins/datepicker/css/datepicker.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/bootstrap-datetimepicker-master/build/css/bootstrap-datetimepicker.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/css/all.min.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/css/icheck/blue.min.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/datatable/datatables.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/select2/dist/css/select2.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/sweetalert/dist/sweetalert.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/toastr/toastr.min.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/hightlight/styles/default.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/quill/quill.snow.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/dropzone/dropzone.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/js/plugins/star-rating/css/star-rating-svg.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/css/AdminLTE.css", new CssRewriteUrlTransformAbsolute())
                 .Include("~/Content/css/myStyle.css")
                 .Include("~/Content/css/mySpinner.css")
                 .Include("~/Content/css/skins/skin-blue.css")
                 .Include("~/Content/js/plugins/timepicker/jquery.timepicker.min.css", new CssRewriteUrlTransformAbsolute())

                 .Include("~/Content/css/ladda-bootstrap/ladda-themeless.min.css"));

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
                .Include("~/Content/js/plugins/timepicker/jquery.timepicker.min.js")
                .Include("~/Content/js/plugins/icheck/icheck.js")
                .Include("~/Content/js/plugins/validator.js")
                .Include("~/Content/js/plugins/inputmask/jquery.inputmask.bundle.js")
                .Include("~/Content/js/plugins/datatable/datatables.js")
                .Include("~/Content/js/plugins/datatables-mark-js/dist/datatables.mark.js")
                .Include("~/Content/js/plugins/datatables-mark-js/dist/jquery.mark.js")
                .Include("~/Content/js/plugins/datatables-mark-js/dist/mark.js")
                .Include("~/Content/js/plugins/chart/chart.min.js")
                .Include("~/Content/js/plugins/chart/chartjs-plugin-labels.min.js")
                .Include("~/Content/js/plugins/select2/dist/js/select2.js")
                .Include("~/Content/js/plugins/sweetalert/dist/sweetalert.min.js")
                .Include("~/Content/js/plugins/hightlight/hihlight.pack.js")
                .Include("~/Content/js/plugins/quill/quill.min.js")
                .Include("~/Content/js/plugins/dropzone/dropzone.js")
                .Include("~/Content/js/plugins/star-rating/jquery.star-rating-svg.js")
                .Include("~/Content/js/plugins/jquery-validation/dist/jquery.validate.js")
                .Include("~/Content/js/adminlte.js")
                .Include("~/Content/js/all.min.js")
                .Include("~/Content/js/init.js")
                .Include("~/Content/js/myScript.js")
                .Include("~/scripts/jquery.signalR-2.3.0.min.js")
                .Include("~/Content/js/plugins/toastr/toastr.min.js")
                .Include("~/Content/js/plugins/jquery-maskmoney-master/dist/jquery.maskMoney.min.js")

                .Include("~/Content/js/plugins/ladda-bootstrap/spin.min.js")
                .Include("~/Content/js/plugins/ladda-bootstrap/ladda.min.js"));
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}