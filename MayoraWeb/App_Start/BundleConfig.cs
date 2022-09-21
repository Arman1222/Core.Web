using System.Web.Optimization;

namespace MyWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = false;
            ////bundles.IgnoreList.Clear();

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //"~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
            //           "~/Scripts/underscore.js"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
            //          "~/Scripts/respond.js"));
            
            ////bundle angular
            //bundles.Add(new ScriptBundle("~/js/all.js")
            //    //.Include("~/js/jquery.js")
            //    //.Include("~/js/bootstrap.js")
            //    .Include("~/Scripts/moment-with-locales.js")
            //    .Include("~/Scripts/tinymce/tinymce.min.js")
            //    .Include("~/js/angular.js")
            //    .Include("~/Scripts/angular-resource.js")
            //    .Include("~/Scripts/angular-route.js")
            //    .Include("~/js/angular-messages.js")
            //    .Include("~/js/angular-animate.js")
            //    .Include("~/js/angular-fcsa-number.js")
            //    .Include("~/js/angular-modal-service.min.js")
            //    .Include("~/js/angular-sanitize.min.js")
            //    .Include("~/Scripts/angular-block-ui.min.js")
            //    .Include("~/Scripts/alasql.min.js")                                 
            //    //.Include("~/js/ui-bootstrap.js")
            //    //.Include("~/Scripts/angular-ui/ui-bootstrap-tpls.min.js") //timepickercontroller dan timepicker directive depreacated                
            //    .Include("~/js/ui-bootstrap-tpls.js")
            //    .Include("~/js/ng-bs3-datepicker.js")
            //    .Include("~/js/timepickerpop.js")   
            //    .Include("~/js/ui-grid.js")
            //    .Include("~/js/number-input.min.js")
            //    .Include("~/Scripts/ng-file-upload-shim.min.js")
            //    .Include("~/Scripts/ng-file-upload.min.js")
            //    .Include("~/js/xlsx.full.min.js")
            //    .Include("~/js/xls.full.min.js")
            //    .Include("~/Scripts/tinymce/tinymce.js")
            //    .Include("~/Scripts/multiple-select.js")
            //    .Include("~/js/ods.js")
            //    .Include("~/js/app/app.js")
            //    .IncludeDirectory("~/js/app/", "*.js", true)
            //    );

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //              "~/Content/themes/base/core.css",
            //              "~/Content/themes/base/resizable.css",
            //              "~/Content/themes/base/selectable.css",
            //              "~/Content/themes/base/accordion.css",
            //              "~/Content/themes/base/autocomplete.css",
            //              "~/Content/themes/base/button.css",
            //              "~/Content/themes/base/dialog.css",
            //              "~/Content/themes/base/slider.css",
            //              "~/Content/themes/base/tabs.css",
            //              "~/Content/themes/base/datepicker.css",
            //              "~/Content/themes/base/progressbar.css",
            //              "~/Content/themes/base/theme.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(                      
            //          "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
            //          "~/bower_components/metisMenu/dist/metisMenu.min.css",
            //          "~/Content/timeline.css",
            //          "~/Content/angular-block-ui.min.css",
            //          "~/Content/sb-admin-2.css",
            //          "~/Content/site.css",
            //          "~/Content/codepen.min.css",
            //          "~/Content/multiple-select.css"));
            
            ////bundle angular
            //bundles.Add(new StyleBundle("~/css/all.css")
            //    .Include("~/font-awesome/css/font-awesome.css")
            //    //.Include("~/css/bootstrap.css")
            //    //.Include("~/css/sb-admin.css")
            //    .Include("~/css/ng-bs3-datepicker.css")    
            //    .Include("~/css/layout.css")
            //    .Include("~/css/ui-grid.css")
            //    );

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/js/all")
                .IncludeDirectory("~/js/app/", "*.js", true)
                );
        }
    }
}
