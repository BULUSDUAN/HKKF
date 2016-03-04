using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NavigationRootAttribute: ActionFilterAttribute
    {
        public static readonly string ViewDataKey = typeof(NavigationRootAttribute).GUID.ToString();

        public string Name { get; private set; }

        public NavigationRootAttribute(string name)
        {
            this.Name = name;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewData[ViewDataKey] = Name;
        }
    }
}
