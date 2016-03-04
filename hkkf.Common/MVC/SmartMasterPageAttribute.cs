using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// 智能选择 母版页
    /// </summary>
    public class SmartMasterPageAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Ajax请求时母版页名称 
        /// </summary>
        public string AjaxMasterPageName { get; set; }
        /// <summary>
        /// 正常请求时母版页名称 
        /// </summary>
        public string NormalRequestMasterPageName { get; set; }

        public SmartMasterPageAttribute()
        {
            AjaxMasterPageName = "Ajax";
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewResult result = filterContext.Result as ViewResult;
            if (result == null) return;
            if (string.IsNullOrEmpty(result.MasterName))
            {
                 result.MasterName = filterContext.HttpContext.Request.IsAjaxRequest() ? AjaxMasterPageName : NormalRequestMasterPageName;
            }
        }
    }
}
