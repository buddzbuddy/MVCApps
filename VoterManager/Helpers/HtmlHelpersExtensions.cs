using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Helpers
{
    public static class HtmlHelpersExtensions
    {
        public static HtmlString Pager(this HtmlHelper helper, int itemsCount)
        {
            var model = new ListPager(itemsCount);
            return new HtmlString(RenderPartialViewToString("_Pager", model, helper.ViewContext));
        }

        public static HtmlString Pager(this HtmlHelper helper, int itemsCount, int pageSize)
        {
            var model = new ListPager(itemsCount, pageSize);
            return new HtmlString(RenderPartialViewToString("_Pager", model, helper.ViewContext));
        }

        public static HtmlString Pager(this HtmlHelper helper, int itemsCount, int pageSize, int maxDisplayedPages)
        {
            var model = new ListPager(itemsCount, pageSize, maxDisplayedPages);
            return new HtmlString(RenderPartialViewToString("_Pager", model, helper.ViewContext));
        }

        public static HtmlString Pager(this HtmlHelper helper, int itemsCount, int pageSize, int maxDisplayedPages,
                                       string queryParameterName)
        {
            var model = new ListPager(itemsCount, pageSize, maxDisplayedPages, queryParameterName);
            return new HtmlString(RenderPartialViewToString("_Pager", model, helper.ViewContext));
        }

        private static string RenderPartialViewToString(string viewName, object model, ViewContext viewContext)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("viewName");

            var context = new ControllerContext(viewContext.RequestContext, viewContext.Controller);
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var newViewContext = new ViewContext(context, viewResult.View, viewContext.ViewData,
                                                     viewContext.TempData, sw)
                                         {
                                             ViewData =
                                                 {
                                                     Model = model
                                                 }
                                         };
                viewResult.View.Render(newViewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}