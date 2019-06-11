using System.Web;
using System.Web.Optimization;

namespace MobileWebsitePCHI
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

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-3.2.0.js",
                        "~/Scripts/KnockoutHelper.js",
                        "~/Scripts/knockout.mapping-latest.debug.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/pchiScripts").Include(
                        "~/Scripts/jquery.globalize.min.js",
                        "~/Scripts/ej/ej.web.all.min.js",
                        "~/Scripts/ej/ej.unobtrusive.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/ejThemes/Default-theme/ej.widgets.all.min.css",
                      "~/Content/CSS/PCHI-Framing.css",
                      "~/Content/CSS/boilerplate.css",
                      "~/Content/CSS/PCHI-PopUp.css"));
        }
    }
}
