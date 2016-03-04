using System;
using System.Web.Mvc.Html;
namespace System.Web.Mvc
{
	public static class HtmlHelperViewModeExtension
	{
		public static MvcHtmlString ViewModeHidden(this HtmlHelper helper)
		{
			return helper.Hidden(ViewModeSupportAttribute.ViewModeKey, helper.ViewMode());
		}
		public static ViewMode ViewMode(this HtmlHelper helper)
		{
			object mode = helper.ViewData[ViewModeSupportAttribute.ViewModeKey];
			ViewMode result;
			if (mode is ViewMode)
			{
				result = (ViewMode)mode;
			}
			else
			{
				result = System.Web.Mvc.ViewMode.Unknow;
			}
			return result;
		}
		public static ViewMode ViewMode(this Controller controller)
		{
			object mode = controller.ViewData[ViewModeSupportAttribute.ViewModeKey];
			ViewMode result;
			if (mode is ViewMode)
			{
				result = (ViewMode)mode;
			}
			else
			{
				result = System.Web.Mvc.ViewMode.Unknow;
			}
			return result;
		}
		public static ViewMode RequestViewMode(this Controller controller)
		{
			string mode = controller.Request[ViewModeSupportAttribute.ViewModeKey];
			ViewMode result;
			try
			{
				result = (ViewMode)System.Enum.Parse(typeof(ViewMode), mode);
				return result;
			}
			catch
			{
			}
			result = System.Web.Mvc.ViewMode.Unknow;
			return result;
		}
		public static void ViewMode(this Controller controller, ViewMode viewMode)
		{
			controller.ViewData[ViewModeSupportAttribute.ViewModeKey] = viewMode;
		}
	}
}
