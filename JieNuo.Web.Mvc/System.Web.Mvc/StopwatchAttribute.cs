using System;
using System.Diagnostics;
namespace System.Web.Mvc
{
	public class StopwatchAttribute : ActionFilterAttribute
	{
		private Stopwatch stopwatch = new Stopwatch();
		private string actionName;
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			this.stopwatch.Reset();
			this.stopwatch.Start();
			this.actionName = filterContext.ActionDescriptor.ActionName;
		}
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			Debug.WriteLine("====StopwatchAttribute==== {0}.{1} Action: {2} ms", new object[]
			{
				filterContext.Controller.GetType().Name, 
				this.actionName, 
				this.stopwatch.ElapsedMilliseconds
			});
		}
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			this.stopwatch.Reset();
			this.stopwatch.Start();
		}
		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			Debug.WriteLine("====StopwatchAttribute==== {0}.{1} Result: {2} ms", new object[]
			{
				filterContext.Controller.GetType().Name, 
				this.actionName, 
				this.stopwatch.ElapsedMilliseconds
			});
		}
	}
}
