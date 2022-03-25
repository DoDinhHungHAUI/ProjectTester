using System.Web;
using System.Web.Optimization;

namespace OnlineShop.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/Assets/client/js/jquery.min.js"));

               bundles.Add(new ScriptBundle("~/js/plugins").Include(

                    "~/Assets/Client2/js/jquery.js",
                    "~/Assets/Client2/js/bootstrap.min.js",
                    "~/Assets/Client2/js/jquery.easing-1.3.min.js",
                    "~/Assets/Client2/js/shop.js",
                    "~/Assets/libs/jquery-ui.min.js",
                    "~/Assets/client/js/Mustache/node_modules/mustache/mustache.js",
                    "~/Assets/client/js/ShoppingCart.js",
                    "~/Assets/libs/NumeralJs/min/numeral.min.js",
                    "~/Assets/client/js/common.js",
                    "~/Assets/client/js/jqzoom.pack.1.0.1.js",
                    "~/Assets/client/js/imagezoom.js",
                    "~/Assets/client/js/jquery.flexslider.js"

                ));
           
            //BundleTable.EnableOptimizations = bool.Parse(ConfigHelper.GetByKey("EnableBundles"));


            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            /*bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));*/

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            /* bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                         "~/Scripts/modernizr-*"));

             bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/bootstrap.js"));

             bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.css",
                       "~/Content/site.css"));*/
        }
    }
}
