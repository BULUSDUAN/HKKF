using System;
using System.ComponentModel;
using System.Linq.Expressions;
namespace System.Web.Mvc.Html
{
	public static class DisplayValueExtension
	{
		public static MvcHtmlString DisplayValueFor<TModel, TProperty>(this HtmlHelper html, TModel model, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression)
		{
			ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForProperty(() => expression.Compile()(model), typeof(TModel), ExpressionHelper.GetExpressionText(expression));
			object property = metadata.Model;
			string format = metadata.DisplayFormatString;
			MvcHtmlString result;
			if (format.IsNullOrEmpty())
			{
				TypeConverter convert = metadata.TypeConverter();
				if (convert != null && convert.CanConvertTo(typeof(string)))
				{
					result = MvcHtmlString.Create(html.Encode(convert.ConvertToString(property)));
					return result;
				}
				format = "{0}";
			}
			result = MvcHtmlString.Create(html.Encode(string.Format(format, property)));
			return result;
		}
		public static MvcHtmlString DisplayValueFor<TModel, TProperty>(this HtmlHelper<TModel> html, TModel model, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);
			object property = metadata.Model;
			string format = metadata.DisplayFormatString;
			MvcHtmlString result;
			if (format.IsNullOrEmpty())
			{
				TypeConverter convert = metadata.TypeConverter();
				if (convert != null && convert.CanConvertTo(typeof(string)))
				{
					result = MvcHtmlString.Create(html.Encode(convert.ConvertToString(property)));
					return result;
				}
				format = "{0}";
			}
			result = MvcHtmlString.Create(html.Encode(string.Format(format, property)));
			return result;
		}
		public static MvcHtmlString DisplayValueFor<TModel, TProperty>(this HtmlHelper<TModel> html, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression) where TModel : class
		{
			return html.DisplayValueFor((TModel)html.ViewContext.ViewData.Model, expression);
		}
	}
}
