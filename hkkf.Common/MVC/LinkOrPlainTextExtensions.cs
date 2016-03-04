using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class LinkOrPlainTextExtensions
    {
        /// <summary>
        /// RouteLink 或文本 PlainText
        /// </summary>
        public static MvcHtmlString RouteLinkOrPlainText(this HtmlHelper htmlHelper, bool isLink, string linkText, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (isLink == false) return MvcHtmlString.Create(htmlHelper.Encode(linkText));
            return htmlHelper.RouteLink(linkText, routeValues, htmlAttributes);
        }

        /// <summary>
        /// 自动使用链接或文本，如果请求数据中包含参数中指定的 key value 则为文本
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">文本，不能为空</param>
        /// <param name="key">键，不能为空</param>
        /// <param name="value">值，可为空</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AutoLinkOrText(this HtmlHelper htmlHelper, string linkText, string key, object value, IDictionary<string, object> htmlAttributes)
        {
            if (linkText == null) throw new ArgumentNullException("linkText");
            if (key == null) throw new ArgumentNullException("key");

            string valueString = value != null ? value.ToString() : "";

            RouteValueDictionary urlData = UrlDataExtensions.UrlData(htmlHelper, true);
            string value2 = urlData.TryGetValue(key, "") as string;

            if (valueString == value2) return MvcHtmlString.Create(linkText.ToString());

            urlData.SelfAdd(key, value, true);
            return LinkExtensions.RouteLink(htmlHelper, linkText, urlData, htmlAttributes);
        }

        /// <summary>
        /// 自动使用链接或文本，如果请求数据中包含参数中指定的 key value 则为文本，value 作为显示的文本
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key">键，不能为空</param>
        /// <param name="value">值，也作为显示的文本，不能为空</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AutoLinkOrText(this HtmlHelper htmlHelper, string key, object value, IDictionary<string, object> htmlAttributes)
        {
            if (value == null) throw new ArgumentNullException("value");
            return AutoLinkOrText(htmlHelper, value.ToString(), key, value, htmlAttributes);
        }
    }
}
