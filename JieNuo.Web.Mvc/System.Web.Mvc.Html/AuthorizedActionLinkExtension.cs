using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
	public static class AuthorizedActionLinkExtension
	{
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
		{
			return htmlHelper.AuthorizedActionLink(linkText, actionName, null, new System.Web.Routing.RouteValueDictionary(), new System.Web.Routing.RouteValueDictionary());
		}
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
		{
			return htmlHelper.AuthorizedActionLink(linkText, actionName, null, new System.Web.Routing.RouteValueDictionary(routeValues), new System.Web.Routing.RouteValueDictionary());
		}
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
		{
			return htmlHelper.AuthorizedActionLink(linkText, actionName, controllerName, new System.Web.Routing.RouteValueDictionary(), new System.Web.Routing.RouteValueDictionary());
		}
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, System.Web.Routing.RouteValueDictionary routeValues)
		{
			return htmlHelper.AuthorizedActionLink(linkText, actionName, null, routeValues, new System.Web.Routing.RouteValueDictionary());
		}
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
		{
			return htmlHelper.AuthorizedActionLink(linkText, actionName, null, new System.Web.Routing.RouteValueDictionary(routeValues), new System.Web.Routing.RouteValueDictionary(htmlAttributes));
		}
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, System.Web.Routing.RouteValueDictionary routeValues, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.AuthorizedActionLink(linkText, actionName, null, routeValues, htmlAttributes);
		}
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
		{
			return htmlHelper.AuthorizedActionLink(linkText, actionName, controllerName, new System.Web.Routing.RouteValueDictionary(routeValues), new System.Web.Routing.RouteValueDictionary(htmlAttributes));
		}
		public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			if (routeValues == null)
			{
				routeValues = new System.Web.Routing.RouteValueDictionary();
			}
			if (!routeValues.ContainsKey("area"))
			{
				routeValues.Add("area", "");
			}
			if (string.IsNullOrEmpty(controllerName))
			{
				controllerName = (routeValues.TryGetValue("controller") as string);
			}
			if (string.IsNullOrEmpty(controllerName))
			{
				controllerName = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
			}
			string url = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, System.Web.Routing.RouteTable.Routes, htmlHelper.ViewContext.RequestContext, false);
			MvcHtmlString result;
			if (!Authorization.Instance.IsAuthrized(htmlHelper.ViewContext.HttpContext, url))
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				TagBuilder builder = new TagBuilder("a")
				{
					InnerHtml = (!string.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : string.Empty
				};
				builder.MergeAttributes<string, object>(htmlAttributes);
				builder.MergeAttribute("href", url);
				result = MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
			}
			return result;
		}
		public static MvcHtmlString AuthorizedActionLink<TController>(this HtmlHelper helper, string linkText, string action, object routeValues = null, object htmlAttributes = null) where TController : IController
		{
			return helper.AuthorizedActionLink(linkText, action, typeof(TController).Name.Replace("Controller", ""), routeValues, htmlAttributes);
		}
		public static MvcHtmlString AuthorizedActionLink<TController>(this HtmlHelper helper, string linkText, string action) where TController : IController
		{
			return helper.AuthorizedActionLink(linkText, action, null, null);
		}
		public static MvcHtmlString AuthorizedActionLink<TModel, TMember>(this HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TMember>> expresion, string action, string controller = null, object routeValues = null, object htmlAttributes = null)
		{
			MvcHtmlString text = htmlHelper.DisplayValueFor(htmlHelper.ViewData.Model, expresion);
			MvcHtmlString result;
			if (MvcHtmlString.IsNullOrEmpty(text))
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				result = htmlHelper.AuthorizedActionLink(text.ToHtmlString(), action, controller, routeValues, htmlAttributes);
			}
			return result;
		}
		public static MvcHtmlString AuthorizedActionLinkIf(this HtmlHelper htmlHelper, bool condition, string linkText, string actionName, object routeValues = null, object htmlAttributes = null)
		{
			MvcHtmlString result;
			if (condition)
			{
				result = htmlHelper.AuthorizedActionLink(linkText, actionName, routeValues, htmlAttributes);
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
	}
}
