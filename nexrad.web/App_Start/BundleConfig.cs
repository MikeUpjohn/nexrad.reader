using System.Web;
using System.Web.Optimization;

namespace nexrad.web {
    public class BundleConfig {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/lib/jquery/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/lib/jquery/js/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/lib/modernizr/js/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Content/lib/bootstrap/js/bootstrap.js"));

            bundles.Add(new Bundle("~/bundles/threejs").Include(
                "~/Content/lib/three/js/three.js"));

            bundles.Add(new Bundle("~/bundles/d3js").Include(
                "~/Content/lib/d3js/js/d3.v7.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/lib/bootstrap/css/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
