namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Runtime.CompilerServices;
    using System.Web.Routing;
    /// <summary>
    /// 将 Request 中的 Action 值作为路由的 Action （如果有且不空）
    /// </summary>
    public class RequestActionRoute : Route
    {
        public RequestActionRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler) { }


        public RequestActionRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)

            : base(url, defaults, routeHandler) { }


        public RequestActionRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)

            : base(url, defaults, constraints, routeHandler) { }


        public RequestActionRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler) { }


        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var data = base.GetRouteData(httpContext);
            if (data == null) return null;

            var requestAction = httpContext.Request["action"];

            if (string.IsNullOrWhiteSpace(requestAction) == false)
                data.Values["action"] = requestAction;

            return data;
        }
    }

    public static class RequestActionRouteExtension
    {
        #region RouteCollection MapRequestActionRoute
        public static Route MapRequestActionRoute(this RouteCollection routes, string name, string url)
        {
            return routes.MapRequestActionRoute(name, url, null, null);
        }

        public static Route MapRequestActionRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapRequestActionRoute(name, url, defaults, null);
        }

        public static Route MapRequestActionRoute(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return routes.MapRequestActionRoute(name, url, null, null, namespaces);
        }

        public static Route MapRequestActionRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.MapRequestActionRoute(name, url, defaults, constraints, null);
        }

        public static Route MapRequestActionRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return routes.MapRequestActionRoute(name, url, defaults, null, namespaces);
        }

        public static Route MapRequestActionRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            var route = new RequestActionRoute(url, new MvcRouteHandler());
            route.Defaults = new RouteValueDictionary(defaults);
            route.Constraints = new RouteValueDictionary(constraints);
            route.DataTokens = new RouteValueDictionary();
            Route item = route;
            if ((namespaces != null) && (namespaces.Length > 0))
            {
                item.DataTokens["Namespaces"] = namespaces;
            }
            routes.Add(name, item);
            return item;
        }
        #endregion

        #region AreaRegistrationContext MapRequestActionRoute
        public static Route MapRequestActionRoute(this AreaRegistrationContext areaRegistrationContext, string name, string url)
        {
            return areaRegistrationContext.MapRequestActionRoute(name, url, null);
        }

        public static Route MapRequestActionRoute(this AreaRegistrationContext areaRegistrationContext, string name, string url, object defaults)
        {
            return areaRegistrationContext.MapRequestActionRoute(name, url, defaults, null);
        }

        public static Route MapRequestActionRoute(this AreaRegistrationContext areaRegistrationContext, string name, string url, string[] namespaces)
        {
            return areaRegistrationContext.MapRequestActionRoute(name, url, null, namespaces);
        }

        public static Route MapRequestActionRoute(this AreaRegistrationContext areaRegistrationContext, string name, string url, object defaults, object constraints)
        {
            return areaRegistrationContext.MapRequestActionRoute(name, url, defaults, constraints, null);
        }

        public static Route MapRequestActionRoute(this AreaRegistrationContext areaRegistrationContext, string name, string url, object defaults, string[] namespaces)
        {
            return areaRegistrationContext.MapRequestActionRoute(name, url, defaults, null, namespaces);
        }

        public static Route MapRequestActionRoute(this AreaRegistrationContext areaRegistrationContext, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if ((namespaces == null) && (areaRegistrationContext.Namespaces != null))
            {
                namespaces = areaRegistrationContext.Namespaces.ToArray<string>();
            }
            Route route = areaRegistrationContext.Routes.MapRequestActionRoute(name, url, defaults, constraints, namespaces);
            route.DataTokens["area"] = areaRegistrationContext.AreaName;
            bool flag = (namespaces == null) || (namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = flag;
            return route;
        }
        #endregion
    }
}
