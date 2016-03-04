using System;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
	public static class ReturnToListLinkExtension
	{
		public static MvcHtmlString AuthorizedReturnToListLink(this HtmlHelper htmlHelper, string linkText, string actionName, object htmlAttributes)
		{
			string area = htmlHelper.ViewContext.RouteData.Values["area"] as string;
			string controller = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
			string url = UrlHelper.GenerateUrl(null, actionName, controller, new System.Web.Routing.RouteValueDictionary(new
			{
				area = area
			}), System.Web.Routing.RouteTable.Routes, htmlHelper.ViewContext.RequestContext, false);
			System.Web.HttpRequestBase request = htmlHelper.ViewContext.HttpContext.Request;
			MvcHtmlString result;
			if (Authorization.Instance.IsAuthrized(htmlHelper.ViewContext.HttpContext, url))
			{
				TagBuilder builder = new TagBuilder("a");
				if (htmlAttributes != null)
				{
					builder.MergeAttributes<string, object>(new System.Web.Routing.RouteValueDictionary(htmlAttributes));
				}
				builder.MergeAttribute("href", url);
				builder.InnerHtml = htmlHelper.Encode(linkText);
				result = MvcHtmlString.Create(builder.ToString());
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
		public static MvcHtmlString AuthorizedReturnToListLink(this HtmlHelper htmlHelper, string linkText, string actionName)
		{
			return htmlHelper.AuthorizedReturnToListLink(linkText, actionName, null);
		}
	}
}
