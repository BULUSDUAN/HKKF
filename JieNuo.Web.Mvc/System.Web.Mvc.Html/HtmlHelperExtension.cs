using JieNuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
	public static class HtmlHelperExtension
	{
		public static MvcHtmlString Partial(this HtmlHelper htmlHelper, string partialViewName, object model, object viewData)
		{
			ViewDataDictionary viewDataDict = new ViewDataDictionary();
			viewDataDict.AddRange(viewData, false);
			return htmlHelper.Partial(partialViewName, model, viewDataDict);
		}
		public static MvcHtmlString PartialIf(this HtmlHelper htmlHelper, string partialViewName, int colSpan, bool condition)
		{
			MvcHtmlString result;
			if (condition)
			{
				result = htmlHelper.Partial(partialViewName, colSpan);
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
		public static MvcHtmlString PartialIf(this HtmlHelper htmlHelper, string partialViewName, object model, bool condition)
		{
			MvcHtmlString result;
			if (condition)
			{
				result = htmlHelper.Partial(partialViewName, model);
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
		public static MvcHtmlString PartialIf(this HtmlHelper htmlHelper, string partialViewName, bool condition)
		{
			return htmlHelper.PartialIf(partialViewName, null, condition);
		}
		public static MvcHtmlString ActionIf(this HtmlHelper htmlHelper, string actionName, object routeValues, bool condition)
		{
			MvcHtmlString result;
			if (condition)
			{
				result = htmlHelper.Action(actionName, routeValues);
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
		public static string IfControllerIs(this HtmlHelper helper, string controllerName, string s)
		{
			string result;
			if (helper.ViewContext.RouteData.Values["controller"] as string == controllerName)
			{
				result = helper.Encode(s);
			}
			else
			{
				result = "";
			}
			return result;
		}
		public static string IfControllerIn(this HtmlHelper helper, System.Collections.Generic.IEnumerable<string> controllerNames, string s)
		{
			string controllerName = helper.ViewContext.RouteData.Values["controller"] as string;
			string result;
			if (controllerNames.Contains(controllerName))
			{
				result = helper.Encode(s);
			}
			else
			{
				result = "";
			}
			return result;
		}
		public static string If(this HtmlHelper helper, bool b, string s)
		{
			return b ? helper.Encode(s) : string.Empty;
		}
		public static MvcHtmlString If(this HtmlHelper helper, bool b, MvcHtmlString s)
		{
			return b ? s : MvcHtmlString.Empty;
		}
		public static MvcHtmlString If(this HtmlHelper helper, bool b, MvcHtmlString ifTrue, MvcHtmlString @else)
		{
			return b ? ifTrue : @else;
		}
		public static string If(this HtmlHelper helper, bool b1, string s1, bool b2, string s2)
		{
			string result;
			if (b1)
			{
				result = helper.Encode(s1);
			}
			else
			{
				if (b2)
				{
					result = helper.Encode(s2);
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}
		public static string Label(this HtmlHelper helper, string value, string forName, object htmlAttributes)
		{
			TagBuilder builder = new TagBuilder("label");
			builder.Attributes.Add("for", HtmlHelperExtension.GetId(forName));
			builder.MergeAttributes<string, object>(new System.Web.Routing.RouteValueDictionary(htmlAttributes));
			builder.SetInnerText(value);
			return builder.ToString();
		}
		private static string GetId(string name)
		{
			TagBuilder builder = new TagBuilder("div");
			builder.GenerateId(name);
			return builder.Attributes["id"];
		}
		public static MvcHtmlString ActionLinkIf(this HtmlHelper htmlHelper, bool b, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
		{
			MvcHtmlString result;
			if (b)
			{
				result = htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public static MvcHtmlString ActionLinkIf(this HtmlHelper htmlHelper, bool b, string linkText, string actionName, object routeValues)
		{
			return htmlHelper.ActionLinkIf(b, linkText, actionName, null, routeValues, null);
		}
		public static string Format(this object s, string format)
		{
			string result;
			if (s == null)
			{
				result = string.Empty;
			}
			else
			{
				if (string.IsNullOrEmpty(format))
				{
					result = s.ToString();
				}
				else
				{
					result = string.Format("{0:" + format + "}", s);
				}
			}
			return result;
		}
		public static bool IsCreateNew(this HtmlHelper helper)
		{
			return helper.ViewMode() == ViewMode.Create;
		}
		public static string DisplayNameForMode(this HtmlHelper helper)
		{
			return KeyValueInfoAttribute.GetValue(helper.ViewMode(), "Text") as string;
		}
		public static string Format(this HtmlHelper helper, string format, params object[] args)
		{
			return string.Format(format, args);
		}
		public static string DisplayNameForSerialNum(this HtmlHelper helper)
		{
			return "序号";
		}
		public static string DisplayNameForOperation(this HtmlHelper helper)
		{
			return "操作";
		}
		public static string DisplayNameForCreateNew(this HtmlHelper helper)
		{
			return "新增";
		}
		public static string DisplayNameForModify(this HtmlHelper helper)
		{
			return "修改";
		}
	}
}
