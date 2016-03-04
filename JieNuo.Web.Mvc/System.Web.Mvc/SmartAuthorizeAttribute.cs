using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Routing;
namespace System.Web.Mvc
{
	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class SmartAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
	{
		protected enum AuthorizeResult
		{
			Success,
			NotAuthorize,
			NotInRoles,
			NotInUsers,
			NeedBuyHighVersion,
			SessionOut
		}
		private readonly object _typeId = new object();
		private string _roles;
		private string[] _rolesSplit = new string[0];
		private string _users;
		private string[] _usersSplit = new string[0];
		public string Roles
		{
			get
			{
				return this._roles ?? string.Empty;
			}
			set
			{
				this._roles = value;
				this._rolesSplit = SmartAuthorizeAttribute.SplitString(value);
			}
		}
		public override object TypeId
		{
			get
			{
				return this._typeId;
			}
		}
		public string Users
		{
			get
			{
				return this._users ?? string.Empty;
			}
			set
			{
				this._users = value;
				this._usersSplit = SmartAuthorizeAttribute.SplitString(value);
			}
		}
		protected virtual SmartAuthorizeAttribute.AuthorizeResult AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
			if (httpContext == null)
			{
				throw new System.ArgumentNullException("httpContext");
			}
			System.Security.Principal.IPrincipal user = httpContext.User;
			SmartAuthorizeAttribute.AuthorizeResult result;
			if (user == null || !user.Identity.IsAuthenticated)
			{
				result = SmartAuthorizeAttribute.AuthorizeResult.NotAuthorize;
			}
			else
			{
				if (this._usersSplit.Length > 0 && !this._usersSplit.Contains(user.Identity.Name, System.StringComparer.OrdinalIgnoreCase))
				{
					result = SmartAuthorizeAttribute.AuthorizeResult.NotInUsers;
				}
				else
				{
					if (this._rolesSplit.Length > 0 && !this._rolesSplit.Any(new System.Func<string, bool>(user.IsInRole)))
					{
						result = SmartAuthorizeAttribute.AuthorizeResult.NotInRoles;
					}
					else
					{
						result = SmartAuthorizeAttribute.AuthorizeResult.Success;
					}
				}
			}
			return result;
		}
		public virtual void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new System.ArgumentNullException("filterContext");
			}
			System.Console.WriteLine("验证 {0}.{1}".FormatWith(new object[]
			{
				filterContext.Controller.GetType().Name, 
				filterContext.ActionDescriptor.ActionName
			}));
			SmartAuthorizeAttribute.AuthorizeResult result = this.AuthorizeCore(filterContext.HttpContext);
			if (result == SmartAuthorizeAttribute.AuthorizeResult.Success)
			{
				System.Web.HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
				cachePolicy.SetProxyMaxAge(new System.TimeSpan(0L));
				cachePolicy.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), null);
			}
			else
			{
				this.HandleUnauthorizedRequest(filterContext, result);
			}
		}
		protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext, SmartAuthorizeAttribute.AuthorizeResult result)
		{
			bool isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
			if (isAjaxRequest)
			{
				filterContext.Result = new InsufficientAuthorizationsResult();
			}
			else
			{
				System.Web.Routing.RouteData routeData = filterContext.RouteData;
				string virtualPath = routeData.Route.GetVirtualPath(filterContext.RequestContext, routeData.Values).VirtualPath;
				if (result == SmartAuthorizeAttribute.AuthorizeResult.SessionOut)
				{
					System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary(new
					{
						controller = "Error", 
						action = "SessionOut", 
						area = "", 
						returnUrl = virtualPath
					});
					filterContext.Result = new RedirectToRouteResult(dict);
				}
				else
				{
					if (result == SmartAuthorizeAttribute.AuthorizeResult.NotInRoles)
					{
						filterContext.Controller.TempData["Error_InsufficientAuthorization_Roles"] = this._rolesSplit;
						System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary(new
						{
							controller = "Error", 
							action = "InsufficientAuthorization", 
							area = ""
						});
						filterContext.Result = new RedirectToRouteResult(dict);
					}
					else
					{
						if (result == SmartAuthorizeAttribute.AuthorizeResult.NotInUsers)
						{
							filterContext.Controller.TempData["Error_InsufficientAuthorization_Users"] = this._usersSplit;
							System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary(new
							{
								controller = "Error", 
								action = "InsufficientAuthorization", 
								area = ""
							});
							filterContext.Result = new RedirectToRouteResult(dict);
						}
						else
						{
							System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary(new
							{
								controller = "Account", 
								action = "Login", 
								area = "", 
								returnUrl = virtualPath
							});
							filterContext.Result = new RedirectToRouteResult(dict);
						}
					}
				}
			}
		}
		protected virtual void HandlePartialAuthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new HttpUnauthorizedResult();
		}
		private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
		{
			validationStatus = this.OnCacheAuthorization(new System.Web.HttpContextWrapper(context));
		}
		protected virtual HttpValidationStatus OnCacheAuthorization(System.Web.HttpContextBase httpContext)
		{
			if (httpContext == null)
			{
				throw new System.ArgumentNullException("httpContext");
			}
			SmartAuthorizeAttribute.AuthorizeResult resut = this.AuthorizeCore(httpContext);
			return (resut == SmartAuthorizeAttribute.AuthorizeResult.Success) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
		}
		internal static string[] SplitString(string original)
		{
			string[] result;
			if (string.IsNullOrEmpty(original))
			{
				result = new string[0];
			}
			else
			{
				System.Collections.Generic.IEnumerable<string> split = 
					from piece in original.Split(new char[]
					{
						','
					})
					let trimmed = piece.Trim()
					where !string.IsNullOrEmpty(trimmed)
					select trimmed;
				result = split.ToArray<string>();
			}
			return result;
		}
	}
}
