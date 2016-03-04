using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class ActionSubmitButtonExtension
    {
        public static MvcHtmlString ActionSubmitButton(this HtmlHelper htmlHelper, string name, string text, object htmlAttributes = null)
        {
            var htmlAttributeDict = new RouteValueDictionary(htmlAttributes ?? new{});

            TagBuilder builer = new TagBuilder("input");
            builer.MergeAttributes(htmlAttributeDict);
            builer.MergeAttribute("name", name);
            builer.MergeAttribute("type", "submit");
            builer.MergeAttribute("value", text);
            builer.AddCssClass("actionSubmitButton");

            return MvcHtmlString.Create(builer.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString AjaxActionSubmitButton(this HtmlHelper htmlHelper, string name, string text, string updateTarget, object htmlAttributes = null)
        {
            var htmlAttributeDict = new RouteValueDictionary(htmlAttributes ?? new { });

            TagBuilder builer = new TagBuilder("input");
            builer.MergeAttributes(htmlAttributeDict);
            builer.MergeAttribute("name", name);
            builer.MergeAttribute("type", "submit");
            builer.MergeAttribute("value", text);
            builer.AddCssClass("ajaxActionSubmitButton");
            builer.MergeAttribute("updateTarget", updateTarget);

            return MvcHtmlString.Create(builer.ToString(TagRenderMode.SelfClosing));
        }
    }
}