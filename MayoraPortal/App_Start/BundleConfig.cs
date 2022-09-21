using System.Web.Optimization;

namespace Mayora
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/js/all.js")
                .IncludeDirectory("~/js/app/", "*.js", true)
                );
        }
    }
}
