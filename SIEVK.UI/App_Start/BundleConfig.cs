using System.Web;
using System.Web.Optimization;

namespace SIEVK.Service
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Content/template/js").Include(
                      "~/Content/template/js/jquery-1.7.2.min.js",
                      "~/Content/template/js/bootstrap.js",
                      "~/Content/template/js/base.js",
                      "~/Content/template/js/chart.min.js",
                      "~/Content/template/js/excanvas.min.js",
                      "~/Content/template/js/faq.js",
                      "~/Content/template/js/signin.js",
                      "~/Content/template/js/angular.min.js",
                      "~/Content/datatables/datatables.min.js",
                      "~/Content/datatables/dataTables.fixedColumns.min.js",
                      "~/Content/jquery-flexdatalist-2.2.4/jquery.flexdatalist.min.js",
                      "~/Content/daterangepicker/moment.min.js",
                      "~/Content/daterangepicker/daterangepicker.js"   ,
                      "~/Scripts/autoNumeric.js" ,
                      "~/Content/chartjs/Chart.bundle.min.js",
                      "~/Content/chartjs/Chart.min.js",
                      "~/Content/chartjs/chartjs-plugin-labels.min.js",
                      "~/Content/chartjs/jquery.number.min.js",
                      "~/Content/chartjs/chartjs-plugin-datalabels.min.js"
                      ));
            
            bundles.Add(new StyleBundle("~/Content/template/css").Include(
                      "~/Content/template/css/base-admin-responsive.css",
                      "~/Content/template/css/bootstrap-responsive.min.css",
                      "~/Content/template/css/bootstrap.min.css",
                      "~/Content/template/css/font-awesome-ie7.min.css",
                      "~/Content/template/css/font-awesome.min.css",
                      "~/Content/template/css/pages/dashboard.css",
                      "~/Content/template/css/pages/faq.css",
                      "~/Content/template/css/pages/plans.css",
                      "~/Content/template/css/pages/reports.css",
                      "~/Content/template/css/pages/signin.css",
                      "~/Content/template/css/style.css",
                      "~/Content/template/css/Custom_MultiLevelMenu.css",
                      "~/Content/jquery-flexdatalist-2.2.4/jquery.flexdatalist.min.css",
                      "~/Content/jquery-flexdatalist-2.2.4/custom.flexdatalist.css",
                      "~/Content/daterangepicker/daterangepicker.css",
                      "~/Content/datatables/datatables.css",
                      "~/Content/chartjs/Chart.min.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
