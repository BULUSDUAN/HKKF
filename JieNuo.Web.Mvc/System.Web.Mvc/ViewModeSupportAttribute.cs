using JieNuo.ComponentModel;
using System;
using System.Linq;
using System.Text.RegularExpressions;
namespace System.Web.Mvc
{
	public class ViewModeSupportAttribute : ActionFilterAttribute
	{
		internal static readonly string ViewModeKey;
		private ViewMode? viewMode;
		static ViewModeSupportAttribute()
		{
			ViewModeSupportAttribute.ViewModeKey = typeof(ViewModeSupportAttribute).GUID.ToString() + "_ViewMode";
		}
		public ViewModeSupportAttribute()
		{
		}
		public ViewModeSupportAttribute(ViewMode viewMode)
		{
			this.viewMode = new ViewMode?(viewMode);
		}
		private ViewMode GuessViewMode(string action)
		{
			if (action == null)
			{
				action = "";
			}
			ViewMode result;
			foreach (ViewMode mode in 
				from ViewMode m in System.Enum.GetValues(typeof(ViewMode))
				where m != ViewMode.Unknow
				select m)
			{
				string pattern = (KeyValueInfoAttribute.GetValue(mode, "MatchPattern") as string) ?? "^$";
				if (Regex.Match(action, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled).Success)
				{
					result = mode;
					return result;
				}
			}
			result = ViewMode.Unknow;
			return result;
		}
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!this.viewMode.HasValue)
			{
				this.viewMode = new ViewMode?(this.GuessViewMode(filterContext.RouteData.Values["action"] as string));
			}
			filterContext.Controller.ViewData[ViewModeSupportAttribute.ViewModeKey] = this.viewMode;
		}
	}
}
