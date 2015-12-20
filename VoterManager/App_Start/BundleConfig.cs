using System.Web;
using System.Web.Optimization;

namespace VoterManager
{
    public class BundleConfig
    {
        // Дополнительные сведения о Bundling см. по адресу http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/leaflet/base/js", "http://cdn.leafletjs.com/leaflet/v0.7.7/leaflet.js").Include(
                "~/Scripts/Leaflet/leaflet.js"));

            bundles.Add(new ScriptBundle("~/bundles/leaflet/label/js").Include(
                "~/Scripts/Leaflet/Label/Label.js",
                "~/Scripts/Leaflet/Label/BaseMarkerMethods.js",
                "~/Scripts/Leaflet/Label/Marker.Label.js",
                "~/Scripts/Leaflet/Label/CircleMarker.Label.js",
                "~/Scripts/Leaflet/Label/Path.Label.js",
                "~/Scripts/Leaflet/Label/Map.Label.js",
                "~/Scripts/Leaflet/Label/FeatureGroup.Label.js"));

            bundles.Add(new ScriptBundle("~/bundles/leaflet/awesome-markers/js").Include(
                "~/Scripts/Leaflet/leaflet.awesome-markers.js"));

            bundles.Add(new ScriptBundle("~/bundles/leaflet/draw/js").Include(
                "~/Scripts/Leaflet/Leaflet.draw.js",
                "~/Scripts/Leaflet/Toolbar.js",
                "~/Scripts/Leaflet/Tooltip.js",
                "~/Scripts/Leaflet/ext/GeometryUtil.js",
                "~/Scripts/Leaflet/ext/LatLngUtil.js",
                "~/Scripts/Leaflet/ext/LineUtil.Intersect.js",
                "~/Scripts/Leaflet/ext/Polygon.Intersect.js",
                "~/Scripts/Leaflet/ext/Polyline.Intersect.js",
                "~/Scripts/Leaflet/draw/DrawToolbar.js",
                "~/Scripts/Leaflet/draw/handler/Draw.Feature.js",
                "~/Scripts/Leaflet/draw/handler/Draw.SimpleShape.js",
                "~/Scripts/Leaflet/draw/handler/Draw.Polyline.js",
                "~/Scripts/Leaflet/draw/handler/Draw.Circle.js",
                "~/Scripts/Leaflet/draw/handler/Draw.Marker.js",
                "~/Scripts/Leaflet/draw/handler/Draw.Polygon.js",
                "~/Scripts/Leaflet/draw/handler/Draw.Rectangle.js",
                "~/Scripts/Leaflet/edit/EditToolbar.js",
                "~/Scripts/Leaflet/edit/handler/EditToolbar.Edit.js",
                "~/Scripts/Leaflet/edit/handler/EditToolbar.Delete.js",
                "~/Scripts/Leaflet/Control.Draw.js",
                "~/Scripts/Leaflet/edit/handler/Edit.Poly.js",
                "~/Scripts/Leaflet/edit/handler/Edit.SimpleShape.js",
                "~/Scripts/Leaflet/edit/handler/Edit.Circle.js",
                "~/Scripts/Leaflet/edit/handler/Edit.Rectangle.js",
                "~/Scripts/Leaflet/edit/handler/Edit.Marker.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/core.css",
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/datepicker.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/theme.css"));

            //Chosen plugin
            bundles.Add(new StyleBundle("~/bundles/chosen/css").Include("~/Content/Chosen/chosen.min.css"));

            //leafletjs styles
            bundles.Add(new StyleBundle("~/bundles/leaflet/base/css", "http://cdn.leafletjs.com/leaflet/v0.7.7/leaflet.css").Include(
                "~/Content/Leaflet/leaflet.css"));

            bundles.Add(new StyleBundle("~/bundles/leaflet/draw/css").Include("~/Content/Leaflet/leaflet.draw.css"));

            bundles.Add(new StyleBundle("~/bundles/leaflet/label/css").Include("~/Content/Leaflet/leaflet.label.css"));
            bundles.Add(new StyleBundle("~/bundles/leaflet/awesome-markers/css").Include(
                "~/Content/Leaflet/font-awesome.css",
                "~/Content/Leaflet/leaflet.awesome-markers.css"));


            BundleTable.EnableOptimizations = true;
        }
    }
}