using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public class NavigationItemAttribute : ActionFilterAttribute
    {
        public static readonly string ViewDataKey = typeof(NavigationItemAttribute).GUID.ToString();

        public string Name { get; private set; }

        public NavigationItemAttribute(string name)
        {
            this.Name = name;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewData[ViewDataKey] = Name;
        }


    }
}
