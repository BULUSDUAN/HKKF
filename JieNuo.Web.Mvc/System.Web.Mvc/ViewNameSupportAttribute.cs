using System;
namespace System.Web.Mvc
{
	public class ViewNameSupportAttribute : ActionFilterAttribute
	{
		public static readonly string ViewNameKey;
		static ViewNameSupportAttribute()
		{
			ViewNameSupportAttribute.ViewNameKey = typeof(ViewNameSupportAttribute).GUID.ToString() + "_ViewName";
		}
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (!filterContext.IsChildAction)
			{
				ViewResult viewResult = filterContext.Result as ViewResult;
				if (viewResult != null)
				{
					viewResult.ViewData.Add(ViewNameSupportAttribute.ViewNameKey, viewResult.ViewName);
				}
			}
		}
	}
}
