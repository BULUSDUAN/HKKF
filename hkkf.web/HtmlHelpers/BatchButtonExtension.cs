using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class BatchButtonExtension
    {
        public static MvcHtmlString AllCheckButton(this HtmlHelper htmlHelper, object htmlAttributes = null)
        {
            var htmlAttributeDict = new RouteValueDictionary(htmlAttributes ?? new { });
            TagBuilder builer = new TagBuilder("input");
            builer.MergeAttributes(htmlAttributeDict);
            builer.MergeAttribute("id", "btnAllCheck");
            builer.MergeAttribute("type", "button");
            builer.MergeAttribute("value", "全选");
            builer.AddCssClass("button");
            return MvcHtmlString.Create(builer.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString AllCancelButton(this HtmlHelper htmlHelper, object htmlAttributes = null)
        {
            var htmlAttributeDict = new RouteValueDictionary(htmlAttributes ?? new { });
            TagBuilder builer = new TagBuilder("input");
            builer.MergeAttributes(htmlAttributeDict);
            builer.MergeAttribute("id", "btnAllCancel");
            builer.MergeAttribute("type", "button");
            builer.MergeAttribute("value", "取消");
            builer.AddCssClass("button");
            return MvcHtmlString.Create(builer.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString AllOtherButton(this HtmlHelper htmlHelper, object htmlAttributes = null)
        {
            var htmlAttributeDict = new RouteValueDictionary(htmlAttributes ?? new { });
            TagBuilder builer = new TagBuilder("input");
            builer.MergeAttributes(htmlAttributeDict);
            builer.MergeAttribute("id", "btnAllOther");
            builer.MergeAttribute("type", "button");
            builer.MergeAttribute("value", "反选");
            builer.AddCssClass("button");
            return MvcHtmlString.Create(builer.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString CheckBoxButton(this HtmlHelper htmlHelper, int value, object htmlAttributes = null)
        {
            var htmlAttributeDict = new RouteValueDictionary(htmlAttributes ?? new { });
            TagBuilder builer = new TagBuilder("input");
            builer.MergeAttributes(htmlAttributeDict);
            builer.MergeAttribute("name", "ids");
            builer.MergeAttribute("type", "checkbox");
            builer.MergeAttribute("value", value.ToString());
            //builer.AddCssClass("button");
            return MvcHtmlString.Create(builer.ToString(TagRenderMode.SelfClosing));
        }

    }
}