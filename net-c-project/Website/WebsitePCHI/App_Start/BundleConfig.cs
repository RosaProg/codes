using System.Web;
using System.Web.Optimization;

namespace Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-3.2.0.js",
                        "~/Scripts/KnockoutHelper.js",
                        "~/Scripts/knockout.mapping-latest.debug.js"
                        ));


            bundles.Add(new ScriptBundle("~/bundles/pchiScripts").Include(
                        "~/Scripts/jsrender.min.js",
                         "~/Scripts/jquery.easing-1.3.min.js",
                        "~/Scripts/jquery.globalize.min.js",
                        "~/Scripts/ej/ej.web.all.min.js",
                        "~/Scripts/ej/ej.unobtrusive.min.js",
                        "~/Scripts/jquery-ui-1.9.2.tabs.custom.min.js",
                        "~/Scripts/jquery.imagemapster.min.js",
                        "~/Scripts/jquery.capslockstate.js",
                        "~/Scripts/PCHI-GadgetsSupport.js",
                        "~/Scripts/jquery-ui-1.10.4.js",
                        "~/Scripts/GeneralFunctionality.js"                        
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/Questionnaire").Include(
                        "~/Scripts/QuestionnaireSupport.js",
                        "~/Scripts/QuestionnaireChatSupport.js"));

            bundles.Add(new StyleBundle("~/Content/CSS/mainstyles").Include(
                      "~/Content/CSS/PCHI-Framing.css",
                      "~/Content/CSS/PCHI-Menu.css",
                      "~/Content/CSS/jquery-ui-1-10-4-themes-smoothness.css",
                      "~/ejThemes/Default-theme/ej.widgets.all.min.css",
                      "~/Content/CSS/PCHI-Gadgets.css",
                      "~/Content/CSS/PCHI-PopUp.css"
            ));
        }
    }
}
