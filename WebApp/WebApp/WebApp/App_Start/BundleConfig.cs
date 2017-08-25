using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/bower_components/jquery/dist/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/bootstrap-theme.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/themejs").Include(
                      "~/bower_components/jquery-ui/jquery-ui.min.js",
                      "~/bower_components/tether/dist/js/tether.min.js",
                      "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                      "~/bower_components/Waves/dist/waves.min.js",
                      "~/bower_components/jquery-slimscroll/jquery.slimscroll.js",
                      "~/bower_components/jquery.nicescroll/dist/jquery.nicescroll.min.js",
                      "~/bower_components/classie/classie.js",
                      "~/assets/plugins/notification/js/bootstrap-growl.min.js",
                      "~/bower_components/syntaxhighlighter/scripts/shCore.js",
                      "~/bower_components/syntaxhighlighter/scripts/shBrushJScript.js",
                      "~/bower_components/syntaxhighlighter/scripts/shBrushXml.js",
                      "~/assets/js/main.js",
                      "~/assets/pages/elements.js",
                      "~/assets/js/menu.min.js"));

            bundles.Add(new StyleBundle("~/Content/themecss").Include(
                      "~/assets/icon/icofont/css/icofont.css",
                      "~/assets/icon/simple-line-icons/css/simple-line-icons.css",
                      "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/bower_components/syntaxhighlighter/styles/shCoreDjango.css",
                      "~/assets/css/main.css",
                      "~/assets/css/responsive.css",
                      "~/assets/css/color/color-1.min.css"));
            
        }
    }
}
