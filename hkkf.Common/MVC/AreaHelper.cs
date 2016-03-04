using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class AreaHelper
    {
        // Methods
        public static string GetAreaName(this RouteBase route)
        {
            IRouteWithArea area = route as IRouteWithArea;
            if (area != null)
            {
                return area.Area;
            }
            Route route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string);
            }
            return null;
        }

        public static string GetAreaName(this RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            return GetAreaName(routeData.Route);
        }
    }


}
