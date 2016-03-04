using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace hkkf.Common.MVC
{
    public class DenyIE56: ActionFilterAttribute
    {
        public string ViewName { get; private set; }

        public DenyIE56(string viewName)
        {
            ViewName = viewName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var browser = filterContext.RequestContext.HttpContext.Request.Browser;

            if (browser.Browser == "IE" && browser.Version.In("5.0", "6.0"))
                filterContext.Result = new ViewResult { ViewName = ViewName };
        }
    }
}
