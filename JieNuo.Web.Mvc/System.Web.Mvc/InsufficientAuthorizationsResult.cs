using System;
namespace System.Web.Mvc
{
	public class InsufficientAuthorizationsResult : ActionResult
	{
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new System.ArgumentNullException("context");
			}
			context.HttpContext.Response.StatusCode = 403;
			context.HttpContext.Response.AddHeader("Location", "/Error/InsufficientAuthorization");
		}
	}
}
