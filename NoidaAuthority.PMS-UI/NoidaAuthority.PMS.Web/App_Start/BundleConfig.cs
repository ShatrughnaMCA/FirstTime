using System.Web.Optimization;

namespace NoidaAuthority.PMS.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            {
                // The jQuery bundle
                bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                                "~/Scripts/jquery-1.11.2.min.js",
                                //"~/Scripts/jquery-3.1.1.min.js",
                                 "~/Scripts/jquery.validate.min.js",
                                "~/Scripts/jquery.validate.unobtrusive.min.js"));


                // The others js bundle
                bundles.Add(new ScriptBundle("~/bundles/others").Include(
                                "~/Scripts/bootstrap.min.js",
                                 "~/Scripts/alertify.min.js"
                                 ));
                // The others js bundle
                bundles.Add(new ScriptBundle("~/bundles/HighChartScripts").Include(
                                "~/Scripts/HighChartScripts/highcharts.js",
                                //"~/Scripts/HighChartScripts/highcharts-3d.js",
                                "~/Scripts/HighChartScripts/exporting.js",
                                  "~/Scripts/HighChartScripts/drilldown.js"
                                 ));

                // The others CSS bundle
                bundles.Add(new StyleBundle("~/Content/others").Include(
                           "~/Content/CSS/bootstrap.css",
                           "~/Content/CSS/bootstrap-theme.min.css",
                            "~/Content/alertifycss/alertify.css",
                           "~/Content/CSS/font-awesome.css",
                           "~/Content/CSS/Main.css"));

                // Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
                bundles.IgnoreList.Clear();
                // Add back the default ignore list rules sans the ones which affect minified files and debug mode
                bundles.IgnoreList.Ignore("*.intellisense.js");
                bundles.IgnoreList.Ignore("*-vsdoc.js");
                bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            }
        }
    }
}