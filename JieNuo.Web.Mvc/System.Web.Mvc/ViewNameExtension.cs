using System;
using System.Web.Mvc.Html;
namespace System.Web.Mvc
{
	public static class ViewNameExtension
	{
		public static MvcHtmlString ViewNameHidden(this HtmlHelper htmlHelper)
		{
			return htmlHelper.Hidden(ViewNameSupportAttribute.ViewNameKey, htmlHelper.ViewData[ViewNameSupportAttribute.ViewNameKey]);
		}
		public static string RequestViewName(this Controller controller)
		{
			return controller.Request[ViewNameSupportAttribute.ViewNameKey];
		}
	}
}
