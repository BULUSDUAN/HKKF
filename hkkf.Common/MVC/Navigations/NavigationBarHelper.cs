using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
    public static class NavigationBarHelper
    {
        public static MvcHtmlString NavigationBar(this HtmlHelper htmlHelper, string root = null, string item = null, string viewName = "_NavigationBar")
        {
            if (item.IsNotNullAndEmpty())
                htmlHelper.ViewData[NavigationItemAttribute.ViewDataKey] = item;
            if (root.IsNotNullAndEmpty()) 
                htmlHelper.ViewData[NavigationRootAttribute.ViewDataKey] = root;
            return htmlHelper.Partial(viewName);
        }
    }
}
