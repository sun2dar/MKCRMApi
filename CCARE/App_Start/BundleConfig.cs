using System.Web;
using System.Web.Optimization;

namespace CCARE
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Js Bundle CRM
            bundles.Add(new ScriptBundle("~/bundles/jquerynBootstrap").Include(
                        "~/assets/js/jquery.js",
                        //"~/Scripts/jquery-ui-1.9.2.custom.js",
                        "~/dist/js/bootstrap.js",
                        "~/Scripts/jquery-migrate-1.2.1.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/assets/bootstrap-datepicker-1.4.0-dist/js/bootstrap-datepicker.js"));

            //for ie 8-9 support
            bundles.Add(new ScriptBundle("~/bundles/respondjs").Include(
                        "~/assets/js/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                        "~/assets/jquery/jqGrid-master/js/jquery.jqGrid.js",
                        "~/assets/jquery/jqGrid-master/js/i18n/grid.locale-en.js",
                        "~/assets/bootstrap-typeahead/js/typeahead.bundle.js",
                        "~/assets/bootstrap-treeview/js/bootstrap-treeview.js",
                        "~/assets/js/customJs.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery.unobtrusive*",
                       "~/Scripts/jquery.validate*"));

            //CSS CRM
            bundles.Add(new StyleBundle("~/maincss").Include(
                        "~/dist/css/bootstrap.css",
                        //"~/assets/css/jquery-ui-1.9.2.custom.css",
                        "~/assets/font-awesome-4.3.0/css/font-awesome.css",
                        "~/assets/css/General.css"));

            bundles.Add(new StyleBundle("~/layoutcss").Include(
                        "~/assets/css/_Layout.css",
                        "~/assets/bootstrap-datepicker-1.4.0-dist/css/bootstrap-datepicker3.css"));

            bundles.Add(new StyleBundle("~/layoutEntitycss").Include(
                        "~/assets/css/_LayoutEntityStyle.css",
                        "~/assets/bootstrap-datepicker-1.4.0-dist/css/bootstrap-datepicker3.css"));

            bundles.Add(new StyleBundle("~/addOncss").Include(
                        "~/assets/jquery/themes-1.10.3/cupertino/jquery-ui.css",
                        "~/assets/jquery/jqGrid-master/css/ui.jqgrid.css",
                        "~/assets/bootstrap-typeahead/css/typeahead.js-bootstrap.css",
                        "~/assets/bootstrap-treeview/css/bootstrap-treeview.css"));

            bundles.Add(new StyleBundle("~/typeahead").Include(
                        "~/assets/jquery/themes-1.10.3/cupertino/jquery-ui.css",
                        "~/assets/jquery/jqGrid-master/css/ui.jqgrid.css"));


            //Bawaan
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}