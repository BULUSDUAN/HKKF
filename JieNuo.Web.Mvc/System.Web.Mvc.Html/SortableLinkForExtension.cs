using JieNuo.Data;
using System;
using System.Linq.Expressions;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
	public static class SortableLinkForExtension
	{
		public static MvcHtmlString SortableLinkFor<TModelItem, TValue>(this HtmlHelper<PagedData<TModelItem>> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModelItem, TValue>> expression, string linkText = null)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModelItem, TValue>(expression, new ViewDataDictionary<TModelItem>());
			string propertyName = ExpressionHelper.GetExpressionText(expression);
			if (string.IsNullOrEmpty(linkText))
			{
				linkText = metadata.DisplayName;
			}
			if (string.IsNullOrEmpty(linkText))
			{
				linkText = propertyName;
			}
			PagedData<TModelItem> pagedData = htmlHelper.ViewData.Model;
			System.Web.Routing.RouteValueDictionary htmlAttributes = new System.Web.Routing.RouteValueDictionary();
			string sort = string.IsNullOrEmpty(metadata.RealSort()) ? propertyName : metadata.RealSort();
			htmlAttributes.Add("orderBy", sort);
			if (sort == pagedData.Pager.OrderBy)
			{
				htmlAttributes.Add("isDesc", !pagedData.Pager.IsDesc);
				htmlAttributes.Add("class", (pagedData.Pager.IsDesc ? "desc" : "asc") + " sortableLink");
			}
			else
			{
				htmlAttributes.Add("isDesc", false);
				htmlAttributes.Add("class", "sortableLink");
			}
			return htmlHelper.ActionLink(linkText, "", null, htmlAttributes);
		}
	}
}
