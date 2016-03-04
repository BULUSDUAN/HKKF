using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Web.Routing;
namespace System.Web.Mvc.Ajax
{
	public static class JQueryExtension
	{
		private const string _globalizationScript = "<script type=\"text/javascript\" src=\"{0}\"></script>";
		private const string FormOnClickValue = "Sys.Mvc.AsyncForm.handleClick(this, new Sys.UI.DomEvent(event));";
		private const string FormOnSubmitFormat = "Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), {0});";
		private const string LinkOnClickFormat = "Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), {0});";
		private static System.Reflection.PropertyInfo ViewContext_FormIdGenerator = typeof(ViewContext).GetProperty("FormIdGenerator", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, (object)null, jQueryOptions);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, object routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, null, routeValues, jQueryOptions);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, controllerName, null, jQueryOptions, null);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, null, routeValues, jQueryOptions);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, null, routeValues, jQueryOptions, htmlAttributes);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, controllerName, routeValues, jQueryOptions, null);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, controllerName, routeValues, jQueryOptions, null);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			return ajaxHelper.JQueryActionLink(linkText, actionName, null, routeValues, jQueryOptions, htmlAttributes);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			System.Web.Routing.RouteValueDictionary dictionary = new System.Web.Routing.RouteValueDictionary(routeValues);
			System.Collections.Generic.Dictionary<string, object> dictionary2 = JQueryExtension.ObjectToCaseSensitiveDictionary(htmlAttributes);
			return ajaxHelper.JQueryActionLink(linkText, actionName, controllerName, dictionary, jQueryOptions, dictionary2);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			if (string.IsNullOrEmpty(linkText))
			{
				throw new System.ArgumentException("MvcResources.Common_NullOrEmpty", "linkText");
			}
			string targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, true);
			return MvcHtmlString.Create(JQueryExtension.GenerateLink(linkText, targetUrl, JQueryExtension.GetJQueryOptions(jQueryOptions), htmlAttributes));
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			System.Web.Routing.RouteValueDictionary dictionary = new System.Web.Routing.RouteValueDictionary(routeValues);
			System.Collections.Generic.Dictionary<string, object> dictionary2 = JQueryExtension.ObjectToCaseSensitiveDictionary(htmlAttributes);
			return ajaxHelper.JQueryActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, dictionary, jQueryOptions, dictionary2);
		}
		public static MvcHtmlString JQueryActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			if (string.IsNullOrEmpty(linkText))
			{
				throw new System.ArgumentException("MvcResources.Common_NullOrEmpty", "linkText");
			}
			string targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, protocol, hostName, fragment, routeValues, ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, true);
			return MvcHtmlString.Create(JQueryExtension.GenerateLink(linkText, targetUrl, jQueryOptions, htmlAttributes));
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, JQueryOptions jQueryOptions)
		{
			string rawUrl = ajaxHelper.ViewContext.HttpContext.Request.RawUrl;
			return ajaxHelper.FormHelper(rawUrl, jQueryOptions, new System.Web.Routing.RouteValueDictionary());
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginForm(actionName, (object)null, jQueryOptions);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, object routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginForm(actionName, null, routeValues, jQueryOptions);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, string controllerName, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginForm(actionName, controllerName, null, jQueryOptions, null);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginForm(actionName, null, routeValues, jQueryOptions);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			return ajaxHelper.JQueryBeginForm(actionName, null, routeValues, jQueryOptions, htmlAttributes);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, string controllerName, object routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginForm(actionName, controllerName, routeValues, jQueryOptions, null);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginForm(actionName, controllerName, routeValues, jQueryOptions, null);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			return ajaxHelper.JQueryBeginForm(actionName, null, routeValues, jQueryOptions, htmlAttributes);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, string controllerName, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			System.Web.Routing.RouteValueDictionary dictionary = new System.Web.Routing.RouteValueDictionary(routeValues);
			System.Collections.Generic.Dictionary<string, object> dictionary2 = JQueryExtension.ObjectToCaseSensitiveDictionary(htmlAttributes);
			return ajaxHelper.JQueryBeginForm(actionName, controllerName, dictionary, jQueryOptions, dictionary2);
		}
		public static JQueryForm JQueryBeginForm(this AjaxHelper ajaxHelper, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			string formAction = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues ?? new System.Web.Routing.RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, true);
			return ajaxHelper.FormHelper(formAction, jQueryOptions, htmlAttributes);
		}
		public static JQueryForm JQueryBeginRouteForm(this AjaxHelper ajaxHelper, string routeName, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginRouteForm(routeName, null, jQueryOptions, null);
		}
		public static JQueryForm JQueryBeginRouteForm(this AjaxHelper ajaxHelper, string routeName, object routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginRouteForm(routeName, routeValues, jQueryOptions, null);
		}
		public static JQueryForm JQueryBeginRouteForm(this AjaxHelper ajaxHelper, string routeName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryBeginRouteForm(routeName, routeValues, jQueryOptions, null);
		}
		public static JQueryForm JQueryBeginRouteForm(this AjaxHelper ajaxHelper, string routeName, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			System.Collections.Generic.Dictionary<string, object> dictionary = JQueryExtension.ObjectToCaseSensitiveDictionary(htmlAttributes);
			return ajaxHelper.JQueryBeginRouteForm(routeName, new System.Web.Routing.RouteValueDictionary(routeValues), jQueryOptions, dictionary);
		}
		public static JQueryForm JQueryBeginRouteForm(this AjaxHelper ajaxHelper, string routeName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			string formAction = UrlHelper.GenerateUrl(routeName, null, null, routeValues ?? new System.Web.Routing.RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, false);
			return ajaxHelper.FormHelper(formAction, jQueryOptions, htmlAttributes);
		}
		private static JQueryForm FormHelper(this AjaxHelper ajaxHelper, string formAction, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			TagBuilder builder = new TagBuilder("form");
			builder.MergeAttributes<string, object>(htmlAttributes);
			builder.MergeAttribute("action", formAction);
			builder.MergeAttribute("method", "post");
			builder.GenerateId(string.Format("form_{0:n}", System.Guid.NewGuid()));
			ajaxHelper.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag));
			JQueryForm form = new JQueryForm(ajaxHelper.ViewContext, builder.Attributes["id"], jQueryOptions);
			ajaxHelper.ViewContext.FormContext.FormId = form.Id;
			return form;
		}
		private static string GenerateAjaxScript(JQueryOptions jQueryOptions, string scriptFormat)
		{
			string str = jQueryOptions.ToJavascriptString();
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, scriptFormat, new object[]
			{
				str
			});
		}
		private static string GenerateLink(string linkText, string targetUrl, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			return string.Format("<a href=\"{0}\" onclick=\"{1}\">{2}</a>", targetUrl, JQueryExtension.GenerateAjaxScript(jQueryOptions, "$(this).ajaxLink({0});return false;"), HttpUtility.HtmlEncode(linkText));
		}
		private static JQueryOptions GetJQueryOptions(JQueryOptions jQueryOptions)
		{
			JQueryOptions result;
			if (jQueryOptions == null)
			{
				result = new JQueryOptions(null, null);
			}
			else
			{
				result = jQueryOptions;
			}
			return result;
		}
		public static MvcHtmlString GlobalizationScript(this AjaxHelper ajaxHelper, System.Globalization.CultureInfo cultureInfo)
		{
			return JQueryExtension.GlobalizationScriptHelper(AjaxHelper.GlobalizationScriptPath, cultureInfo);
		}
		private static MvcHtmlString GlobalizationScriptHelper(string scriptPath, System.Globalization.CultureInfo cultureInfo)
		{
			if (cultureInfo == null)
			{
				throw new System.ArgumentNullException("cultureInfo");
			}
			string str = VirtualPathUtility.AppendTrailingSlash(scriptPath) + cultureInfo.Name + ".js";
			string format = "<script type=\"text/javascript\" src=\"{0}\"></script>".Replace("\r\n", System.Environment.NewLine);
			return MvcHtmlString.Create(string.Format(System.Globalization.CultureInfo.InvariantCulture, format, new object[]
			{
				str
			}));
		}
		private static System.Collections.Generic.Dictionary<string, object> ObjectToCaseSensitiveDictionary(object values)
		{
			System.Collections.Generic.Dictionary<string, object> dictionary = new System.Collections.Generic.Dictionary<string, object>(System.StringComparer.Ordinal);
			if (values != null)
			{
				foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
				{
					object obj2 = descriptor.GetValue(values);
					dictionary[descriptor.Name] = obj2;
				}
			}
			return dictionary;
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, object routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryRouteLink(linkText, null, new System.Web.Routing.RouteValueDictionary(routeValues), jQueryOptions, new System.Collections.Generic.Dictionary<string, object>());
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryRouteLink(linkText, routeName, new System.Web.Routing.RouteValueDictionary(), jQueryOptions, new System.Collections.Generic.Dictionary<string, object>());
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryRouteLink(linkText, null, routeValues, jQueryOptions, new System.Collections.Generic.Dictionary<string, object>());
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			return ajaxHelper.JQueryRouteLink(linkText, null, new System.Web.Routing.RouteValueDictionary(routeValues), jQueryOptions, JQueryExtension.ObjectToCaseSensitiveDictionary(htmlAttributes));
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, object routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryRouteLink(linkText, routeName, new System.Web.Routing.RouteValueDictionary(routeValues), jQueryOptions, new System.Collections.Generic.Dictionary<string, object>());
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			return ajaxHelper.JQueryRouteLink(linkText, routeName, new System.Web.Routing.RouteValueDictionary(), jQueryOptions, htmlAttributes);
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			return ajaxHelper.JQueryRouteLink(linkText, routeName, new System.Web.Routing.RouteValueDictionary(), jQueryOptions, JQueryExtension.ObjectToCaseSensitiveDictionary(htmlAttributes));
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions)
		{
			return ajaxHelper.JQueryRouteLink(linkText, routeName, routeValues, jQueryOptions, new System.Collections.Generic.Dictionary<string, object>());
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			return ajaxHelper.JQueryRouteLink(linkText, null, routeValues, jQueryOptions, htmlAttributes);
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, object routeValues, JQueryOptions jQueryOptions, object htmlAttributes)
		{
			return ajaxHelper.JQueryRouteLink(linkText, routeName, new System.Web.Routing.RouteValueDictionary(routeValues), jQueryOptions, JQueryExtension.ObjectToCaseSensitiveDictionary(htmlAttributes));
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			if (string.IsNullOrEmpty(linkText))
			{
				throw new System.ArgumentException("MvcResources.Common_NullOrEmpty", "linkText");
			}
			string targetUrl = UrlHelper.GenerateUrl(routeName, null, null, routeValues ?? new System.Web.Routing.RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, false);
			return MvcHtmlString.Create(JQueryExtension.GenerateLink(linkText, targetUrl, JQueryExtension.GetJQueryOptions(jQueryOptions), htmlAttributes));
		}
		public static MvcHtmlString JQueryRouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, string protocol, string hostName, string fragment, System.Web.Routing.RouteValueDictionary routeValues, JQueryOptions jQueryOptions, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			if (string.IsNullOrEmpty(linkText))
			{
				throw new System.ArgumentException("MvcResources.Common_NullOrEmpty", "linkText");
			}
			string targetUrl = UrlHelper.GenerateUrl(routeName, null, null, protocol, hostName, fragment, routeValues ?? new System.Web.Routing.RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, false);
			return MvcHtmlString.Create(JQueryExtension.GenerateLink(linkText, targetUrl, JQueryExtension.GetJQueryOptions(jQueryOptions), htmlAttributes));
		}
		private static System.Func<string> GetFormIdGenerator(ViewContext viewContext)
		{
			return JQueryExtension.ViewContext_FormIdGenerator.GetValue(viewContext, null) as System.Func<string>;
		}
	}
}
