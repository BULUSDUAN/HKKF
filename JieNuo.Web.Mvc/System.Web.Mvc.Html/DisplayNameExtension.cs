using JieNuo.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
namespace System.Web.Mvc.Html
{
	public static class DisplayNameExtension
	{
		public static string DisplayName<TModel>(this HtmlHelper<TModel> htmlHelper, string expression)
		{
			ModelMetadata metadata = ModelMetadata.FromStringExpression(expression, htmlHelper.ViewData);
			return metadata.DisplayName;
		}
		public static string DisplayName<TModel>(this HtmlHelper<TModel> htmlHelper)
		{
			return DisplayNameExtension.DisplayName<TModel>();
		}
		public static string DisplayName<TModel>(this HtmlHelper<System.Collections.Generic.IEnumerable<TModel>> htmlHelper)
		{
			return DisplayNameExtension.DisplayName<TModel>();
		}
		public static string DisplayName<TModel>(this HtmlHelper<PagedData<TModel>> htmlHelper)
		{
			return DisplayNameExtension.DisplayName<TModel>();
		}
		public static string DisplayName<TModel>(this HtmlHelper htmlHelper)
		{
			return DisplayNameExtension.DisplayName<TModel>();
		}
		private static string DisplayName<T>()
		{
			DisplayNameAttribute attr = AttributeHelper.GetNoInherit<DisplayNameAttribute>(typeof(T)).FirstOrDefault<DisplayNameAttribute>();
			return (attr != null) ? attr.DisplayName : typeof(T).Name;
		}
		public static string DisplayNameFor<TModel, TValue>(this HtmlHelper htmlHelper, Expression<System.Func<TModel, TValue>> expression)
		{
			string propertyName = ExpressionHelper2.GetExpressionText(expression);
			ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, expression.Parameters[0].Type, propertyName);
			return (metadata != null) ? metadata.DisplayName : propertyName;
		}
		public static string DisplayNameFor<TModel>(this HtmlHelper htmlHelper, Expression<System.Func<TModel, object>> expression)
		{
			return htmlHelper.DisplayNameFor<TModel, object>(expression);
		}
		public static string DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<System.Func<TModel, TValue>> expression)
		{
			return (htmlHelper as HtmlHelper).DisplayNameFor(expression);
		}
		public static string RealDisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<System.Func<TModel, TValue>> expression)
		{
			string result;
			if (htmlHelper.ViewData.Model == null)
			{
				result = "";
			}
			else
			{
				string property = ExpressionHelper.GetExpressionText(expression);
				if (string.IsNullOrEmpty(property))
				{
					result = "";
				}
				else
				{
					ModelMetadata metadata = null;
					try
					{
						ModelMetadataProvider arg_94_0 = ModelMetadataProviders.Current;
						System.Func<object> arg_94_1 = () => htmlHelper.ViewData.Model;
						TModel model = htmlHelper.ViewData.Model;
						metadata = arg_94_0.GetMetadataForProperty(arg_94_1, model.GetType(), property);
					}
					catch
					{
					}
					result = ((metadata != null) ? metadata.DisplayName : property);
				}
			}
			return result;
		}
		public static string DisplayNameFor<TModelItem, TValue>(this HtmlHelper<System.Collections.Generic.IEnumerable<TModelItem>> htmlHelper, Expression<System.Func<TModelItem, TValue>> expression)
		{
			return (htmlHelper as HtmlHelper).DisplayNameFor(expression);
		}
		public static string DisplayNameFor<TModelItem, TValue>(this HtmlHelper<PagedData<TModelItem>> htmlHelper, Expression<System.Func<TModelItem, TValue>> expression)
		{
            return (htmlHelper as HtmlHelper).DisplayNameFor(expression);
		}
		public static string DisplayNameFor<TModel, TValue>(this HtmlHelper htmlHelper, TModel model, Expression<System.Func<TModel, TValue>> expression)
		{
			return htmlHelper.DisplayNameFor(expression);
		}
		public static string DisplayNameForItem<TModelItem, TValue>(this HtmlHelper<System.Collections.Generic.IEnumerable<TModelItem>> htmlHelper, Expression<System.Func<TModelItem, TValue>> expression)
		{
			return htmlHelper.DisplayNameFor(expression);
		}
		public static string DisplayNameForItem<TModelItem, TValue>(this HtmlHelper<PagedData<TModelItem>> htmlHelper, Expression<System.Func<TModelItem, TValue>> expression)
		{
			return htmlHelper.DisplayNameFor(expression);
		}
	}
}
