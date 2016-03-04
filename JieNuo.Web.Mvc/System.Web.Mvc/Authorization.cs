using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web.Routing;
namespace System.Web.Mvc
{
	public class Authorization
	{
		private class ActionInvoker : ControllerActionInvoker
		{
			public bool IsAuthrized(ControllerContext controllerContext, string actionName)
			{
				if (controllerContext == null)
				{
					throw new System.ArgumentNullException("controllerContext");
				}
				if (string.IsNullOrEmpty(actionName))
				{
					throw new System.ArgumentNullException("actionName");
				}
				ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(controllerContext);
				ActionDescriptor actionDescriptor = this.FindAction(controllerContext, controllerDescriptor, actionName);
				bool result;
				if (actionDescriptor != null)
				{
					FilterInfo filterInfo = this.GetFilters(controllerContext, actionDescriptor);
					try
					{
						AuthorizationContext authContext = this.InvokeAuthorizationFilters(controllerContext, filterInfo.AuthorizationFilters, actionDescriptor);
						if (authContext.Result == null)
						{
							result = true;
							return result;
						}
					}
					catch
					{
					}
				}
				result = false;
				return result;
			}
		}
		public class SimpleHttpWorkerRequest : HttpWorkerRequest
		{
			private string uriPath;
			public SimpleHttpWorkerRequest(string url)
			{
				this.uriPath = Regex.Replace(url, "[?].*$", "", RegexOptions.Compiled);
			}
			public override string GetUriPath()
			{
				return this.uriPath;
			}
			public override string GetHttpVerbName()
			{
				return "GET";
			}
			public override string GetHttpVersion()
			{
				return this.GetServerVariable("SERVER_PROTOCOL");
			}
			public override string GetLocalAddress()
			{
				return this.GetServerVariable("LOCAL_ADDR");
			}
			public override int GetLocalPort()
			{
				return 80;
			}
			public override string GetQueryString()
			{
				return string.Empty;
			}
			public override string GetRawUrl()
			{
				return this.uriPath;
			}
			public override string GetRemoteAddress()
			{
				return this.GetServerVariable("REMOTE_ADDR");
			}
			public override int GetRemotePort()
			{
				return 0;
			}
			public override void EndOfRequest()
			{
			}
			public override void FlushResponse(bool finalFlush)
			{
			}
			public override void SendKnownResponseHeader(int index, string value)
			{
			}
			public override void SendResponseFromFile(System.IntPtr handle, long offset, long length)
			{
			}
			public override void SendResponseFromFile(string filename, long offset, long length)
			{
			}
			public override void SendResponseFromMemory(byte[] data, int length)
			{
			}
			public override void SendStatus(int statusCode, string statusDescription)
			{
			}
			public override void SendUnknownResponseHeader(string name, string value)
			{
			}
		}
		private static readonly string SessionKey;
		private static readonly string SessionKey_User;
		private Authorization.ActionInvoker invoker = new Authorization.ActionInvoker();
		public static Authorization Instance;
		private Authorization()
		{
		}
		public bool IsAuthrized(System.Web.HttpContextBase httpContextBase, string url)
		{
			System.Security.Principal.IPrincipal user = httpContextBase.Session[Authorization.SessionKey_User] as System.Security.Principal.IPrincipal;
			if (user == null || httpContextBase.User == null || user.Identity.Name != httpContextBase.User.Identity.Name)
			{
				httpContextBase.Session.Remove(Authorization.SessionKey);
				httpContextBase.Session[Authorization.SessionKey_User] = httpContextBase.User;
			}
			System.Collections.Generic.Dictionary<string, bool> dict = httpContextBase.Session[Authorization.SessionKey] as System.Collections.Generic.Dictionary<string, bool>;
			if (dict == null)
			{
				dict = new System.Collections.Generic.Dictionary<string, bool>();
				httpContextBase.Session[Authorization.SessionKey] = dict;
			}
			if (!dict.ContainsKey(url))
			{
				bool b = this._IsAuthrized(httpContextBase, url);
				dict.Add(url, b);
			}
			return dict[url];
		}
		private bool _IsAuthrized(System.Web.HttpContextBase httpContextBase, string url)
		{
			System.Security.Principal.IPrincipal user = httpContextBase.User;
			bool result;
			try
			{
				HttpWorkerRequest request = new Authorization.SimpleHttpWorkerRequest(url);
				HttpContext httpContext = new HttpContext(request);
				System.Web.HttpContextWrapper httpContextWrapper = new System.Web.HttpContextWrapper(httpContext);
				System.Web.Routing.RouteData routeData = System.Web.Routing.RouteTable.Routes.GetRouteData(httpContextWrapper);
				httpContext.User = user;
				System.Web.HttpContextBase _httpContextBase = new System.Web.HttpContextWrapper(httpContext);
				System.Web.Routing.RequestContext requestContext = new System.Web.Routing.RequestContext(_httpContextBase, routeData);
				string controllerName = requestContext.RouteData.GetRequiredString("controller");
				string actionName = requestContext.RouteData.GetRequiredString("action");
				IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
				try
				{
					IController controller = factory.CreateController(requestContext, controllerName);
					if (controller is ControllerBase)
					{
						ControllerContext controllerContext = new ControllerContext(requestContext, controller as ControllerBase);
						result = this.invoker.IsAuthrized(controllerContext, actionName);
					}
					else
					{
						result = true;
					}
				}
				catch (System.Exception)
				{
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		static Authorization()
		{
			// Note: this type is marked as 'beforefieldinit'.
			System.Guid gUID = typeof(Authorization).GUID;
			Authorization.SessionKey = gUID.ToString() + "_SessionKey";
			gUID = typeof(Authorization).GUID;
			Authorization.SessionKey_User = gUID.ToString() + "_SessionKey_User";
			Authorization.Instance = new Authorization();
		}
	}
}
