using System.Web;
using System.Web.Optimization;

namespace CASServer
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/Plus/jquery.validate.unobtrusive.extends.js"));

            //那个焦点轮图
            bundles.Add(new ScriptBundle("~/bundles/sochange").Include(
                       "~/Scripts/Plus/jquery.soChange.js"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/common.css"));
            bundles.Add(new StyleBundle("~/Content/dialog").Include("~/Content/dialg_style.css"));
            bundles.Add(new StyleBundle("~/Content/indexcss").Include("~/Content/index.css"));

            bundles.Add(new StyleBundle("~/bundles/themes/base/css").Include(
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

            //jquery-loadmask
            bundles.Add(new StyleBundle("~/Content/jqueryloadmask").Include(
                       "~/Scripts/Plus/jquery-loadmask/jquery.loadmask.css"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryloadmask").Include(
                     "~/Scripts/Plus/base/jquery-loadmask/jquery.loadmask.js"));


            //toastmessage
            bundles.Add(new StyleBundle("~/Content/toastmessage").Include(
                        "~/Scripts/Plus/toastmessage/css/jquery.toastmessage.css"));
            bundles.Add(new ScriptBundle("~/bundles/toastmessage").Include(
                     "~/Scripts/Plus/toastmessage/jquery.toastmessage.js"));


            bundles.Add(new ScriptBundle("~/bundles/json").Include(
                    "~/Scripts/Plus/json/jquery.json-2.3.js"));

        }




        public static class Css
        {
            public static void Loadmask()
            {
                Styles.Render("~/Content/jqueryloadmask");
            }

            public static void Toastmessage()
            {
                Styles.Render("~/Content/toastmessage");
                Scripts.Render("~/bundles/toastmessage");


            }
        }

        public static class Js
        {
            public static void Loadmask()
            {
                Scripts.Render("~/bundles/jqueryloadmask");
            }

            public static void Toastmessage()
            {
                Scripts.Render("~/bundles/toastmessage");
            }
        }
    }
}