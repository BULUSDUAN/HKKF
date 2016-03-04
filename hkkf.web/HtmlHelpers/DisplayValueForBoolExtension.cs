using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using JieNuo.Data;

namespace System.Web.Mvc.Html
{
    public static class DisplayValueForBoolExtension
    {
        public static MvcHtmlString DisplayValueFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, bool>> expression)
            where T: class
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var boolAttr = metadata.BoolAttribute();

            if (boolAttr == null)
                return htmlHelper.DisplayValueFor(htmlHelper.ViewData.Model, expression);

            var displayValue = (bool)metadata.Model ? boolAttr.TextForTrue : boolAttr.TextForFalse;
            return MvcHtmlString.Create(HttpUtility.HtmlEncode(displayValue));
        }

        public static MvcHtmlString DisplayValueFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, bool?>> expression)
            where T : class
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var boolAttr = metadata.BoolAttribute();

            if (boolAttr == null)
                return htmlHelper.DisplayValueFor(htmlHelper.ViewData.Model, expression);
            
            string displayValue;
            if (metadata.Model == null) displayValue = boolAttr.TextForNull;
            else displayValue = (bool)metadata.Model ? boolAttr.TextForTrue : boolAttr.TextForFalse;
            return MvcHtmlString.Create(HttpUtility.HtmlEncode(displayValue));
        }

        public static string DisplayValueForBool<TModel>(this HtmlHelper htmlHelper, TModel model, Expression<Func<TModel, bool>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(model));
            var boolAttr = metadata.BoolAttribute();

            string displayValue;
            if (metadata.Model == null) displayValue = boolAttr.TextForNull;
            else displayValue = (bool)metadata.Model ? boolAttr.TextForTrue : boolAttr.TextForFalse;
            return htmlHelper.Encode(displayValue);
        }

        public static string DisplayValueForBool<TModel>(this HtmlHelper htmlHelper, TModel model, Expression<Func<TModel, bool?>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(model));
            var boolAttr = metadata.BoolAttribute();

            string displayValue;
            if (metadata.Model == null) displayValue = boolAttr.TextForNull;
            else displayValue = (bool)metadata.Model ? boolAttr.TextForTrue : boolAttr.TextForFalse;
            return htmlHelper.Encode(displayValue);
        }
    }
}