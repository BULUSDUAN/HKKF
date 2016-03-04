using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 必填标识扩展
    /// </summary>
    public static class RequriedMarkForExtension
    {
        private static readonly MvcHtmlString Required = MvcHtmlString.Create("<span class='required'>*<span>");

        /// <summary>
        /// 必填标识
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString RequriedMarkFor<T, TProperty>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TProperty>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return metadata.IsRequired ? Required : MvcHtmlString.Empty;
        }
    }
}
