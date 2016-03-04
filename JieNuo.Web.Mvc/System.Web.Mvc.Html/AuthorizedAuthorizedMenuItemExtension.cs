using System;
using System.Collections.Generic;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
	public static class AuthorizedAuthorizedMenuItemExtension
	{
		public static MvcHtmlString AuthorizedMenuItemIf(this HtmlHelper helper, bool condition, string linkText, string action, object routeValues = null, object htmlAttributes = null)
		{
			MvcHtmlString result;
			if (condition)
			{
				result = helper.AuthorizedMenuItem(linkText, action, null, routeValues, htmlAttributes);
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
		public static MvcHtmlString AuthorizedMenuItem(this HtmlHelper helper, string linkText, string action, string controller, System.Web.Routing.RouteValueDictionary routeValues, System.Collections.Generic.IDictionary<string, object> htmlAttributes = null)
		{
			MvcHtmlString link = helper.AuthorizedActionLink(linkText, action, controller, routeValues, htmlAttributes);
			MvcHtmlString result;
			if (link == MvcHtmlString.Empty)
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				TagBuilder builer = new TagBuilder("li")
				{
					InnerHtml = link.ToHtmlString()
				};
				result = MvcHtmlString.Create(builer.ToString());
			}
			return result;
		}
		public static MvcHtmlString AuthorizedMenuItem(this HtmlHelper helper, string linkText, string action, string controller = null, object routeValues = null, object htmlAttributes = null)
		{
			return helper.AuthorizedMenuItem(linkText, action, controller, new System.Web.Routing.RouteValueDictionary(routeValues), new System.Web.Routing.RouteValueDictionary(htmlAttributes));
		}
		public static MvcHtmlString AuthorizedMenuItem<TController>(this HtmlHelper helper, string linkText, string action, object routeValues = null) where TController : IController
		{
			return helper.AuthorizedMenuItem(linkText, action, typeof(TController).Name.Replace("Controller", ""), routeValues, null);
		}
	}
}
