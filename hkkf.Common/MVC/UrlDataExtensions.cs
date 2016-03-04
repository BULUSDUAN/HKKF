//ldp 2010年12月10日
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{

    public static class UrlDataExtensions
    {
        public static readonly string UrlDataKey = typeof(UrlDataExtensions).GUID.ToString() + "UrlDataKey";

        /// <summary>
        /// 汇集  Request.QueryString 及 RouteData，Request.QueryString 优先
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="getCopy">获取副本</param>
        /// <returns></returns>
        public static RouteValueDictionary UrlData(this HtmlHelper htmlHelper, bool getCopy = false)
        {
            var data = htmlHelper.ViewData[UrlDataKey] as RouteValueDictionary;
            if (data == null)
            {
                data = CreateUrlData(htmlHelper);
                htmlHelper.ViewData.Add(UrlDataKey, data);
            }
            if (getCopy) data = new RouteValueDictionary(data);
            return data;
        }
        /// <summary>
        /// 从 Request.QueryString 和 RouteData 中获取值，Request.QueryString 优先
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string UrlData(this HtmlHelper htmlHelper, string key)
        {
            var result = htmlHelper.ViewContext.HttpContext.Request.QueryString[key];
            if (string.IsNullOrWhiteSpace(result) == false) return result;

            var routeData = htmlHelper.ViewContext.RouteData.Values;
            if (routeData.ContainsKey(key)) return routeData[key] as string;
            return null;
        }

        private static RouteValueDictionary CreateUrlData(HtmlHelper htmlHelper)
        {
            RouteValueDictionary dict = new RouteValueDictionary(htmlHelper.ViewContext.RouteData.Values);

            var queryString = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            var keys = queryString.Keys
                .Cast<string>()
                .Where(k=>k.NotIn(null, "", "_", "X-Requested-With"))
                .ToArray();

            foreach (string key in keys)
            {
                var value = queryString.Get(key);
                if (value.IsNullOrEmpty()) continue;
                if (dict.ContainsKey(key)) dict[key] = value;
                else dict.Add(key, value);
            }
            if (dict.ContainsKey("area") == false)
                dict.Add("area", htmlHelper.ViewContext.RouteData.GetAreaName());

            return dict;
        }

    }
}
