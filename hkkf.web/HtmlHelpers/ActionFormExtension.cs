using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    [Flags]
    public enum ActionButtonOptions
    {
        NoAction = 0,
        Refresh = 1,
        DisableButton = 2
    }

    public class  JQueryActionButtonOption
    {
        public string SuccessUrl { get; set; }
        public string SuccessTarget { get; set; }

        public string GenerateSuccessScript()
        {
            if (SuccessUrl.IsNotNullAndEmpty() && SuccessTarget.IsNotNullAndEmpty())
                return string.Format("$.get('{0}', function(data){{ $('{1}').html(data); }})", SuccessUrl, SuccessTarget);
            else return null;
        }
    }

    public static class ActionFormExtension
    {
        //private static readonly string actionButton = @"<form action=''><input type='submit'></form>";

        public static MvcHtmlString ActionForm(this HtmlHelper htmlHelper, string text, string action, string controller = null, object routeValues = null, object htmlAttributes = null)
        {
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString JQueryActionButtonForm(this HtmlHelper htmlHelper, string text, string action, string controller = null, object routeValues = null, JQueryActionButtonOption option = null, object htmlAttributes = null)
        {
            var routeValueDict = new RouteValueDictionary(routeValues);
            var url = UrlHelper.GenerateUrl(null, action, controller, routeValueDict, RouteTable.Routes, htmlHelper.ViewContext.RequestContext, false);
            var htmlAttributesDict = new RouteValueDictionary(htmlAttributes);

            TagBuilder buttonBuilder = new TagBuilder("input");
            buttonBuilder.MergeAttributes(htmlAttributesDict);
            buttonBuilder.MergeAttribute("type", "submit", true);
            buttonBuilder.MergeAttribute("value", text, true);

            TagBuilder formBuilder = new TagBuilder("form");
            formBuilder.MergeAttribute("action", url);
            formBuilder.MergeAttribute("method", "post");
            formBuilder.AddCssClass("actionButtonForm");
            if (option != null)
                formBuilder.MergeAttribute("success", option.GenerateSuccessScript());
            formBuilder.InnerHtml = buttonBuilder.ToString(TagRenderMode.SelfClosing);

            
            

            return MvcHtmlString.Create(formBuilder.ToString());
        }

    }
}