using System;
namespace System.Web.Mvc
{
	public class DisableClientCacheAttribute : ActionFilterAttribute
	{
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			filterContext.HttpContext.Response.Buffer = true;
			System.Web.HttpResponseBase arg_34_0 = filterContext.HttpContext.Response;
			System.DateTime now = System.DateTime.Now;
			arg_34_0.ExpiresAbsolute = now.AddSeconds(-1.0);
			filterContext.HttpContext.Response.Expires = -1;
			System.Web.HttpCachePolicyBase arg_72_0 = filterContext.HttpContext.Response.Cache;
			now = System.DateTime.Now;
			arg_72_0.SetExpires(now.AddSeconds(-1.0));
			filterContext.HttpContext.Response.Cache.SetNoServerCaching();
			filterContext.HttpContext.Response.Cache.SetNoStore();
			filterContext.HttpContext.Response.CacheControl = "no-cache";
			filterContext.HttpContext.Response.AddHeader("Pragma", "no-cache");
			filterContext.HttpContext.Response.AddHeader("cache-ctrol", "no-cache");
		}
	}
}
