using System;
using System.Configuration;
using System.Web.Routing;
namespace System.Web.Mvc
{
	public class VersionAuthorizeAttribute : AuthorizeAttribute
	{
		public double Version
		{
			get;
			private set;
		}
		public Comparisons Comparison
		{
			get;
			set;
		}
		public VersionAuthorizeAttribute(double version)
		{
			this.Version = version;
			this.Comparison = Comparisons.GreatOrEquals;
		}
		protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
			string versionStr = ConfigurationManager.AppSettings["version"];
			double version;
			bool result;
			if (!double.TryParse(versionStr, out version))
			{
				result = false;
			}
			else
			{
				switch (this.Comparison)
				{
					case Comparisons.Greate:
					{
						result = (version > this.Version);
						break;
					}
					case Comparisons.GreatOrEquals:
					{
						result = (version >= this.Version);
						break;
					}
					case Comparisons.Equals:
					{
						result = (version == this.Version);
						break;
					}
					case Comparisons.LessOrEquals:
					{
						result = (version <= this.Version);
						break;
					}
					case Comparisons.Less:
					{
						result = (version < this.Version);
						break;
					}
					default:
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary(new
			{
				controller = "Error", 
				action = "NeedBuyHighVersion", 
				area = ""
			});
			filterContext.Result = new RedirectToRouteResult(dict);
		}
	}
}
