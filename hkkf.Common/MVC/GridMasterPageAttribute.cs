using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace hkkf.Common.MVC
{
    public class GridMasterPageAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// Ajax请求时母版页名称 
        /// </summary>
        public string AjaxMasterPageName { get; set; }
        /// <summary>
        /// 正常请求时母版页名称 
        /// </summary>
        public string NormalRequestMasterPageName { get; set; }

        public GridMasterPageAttribute()
        {
            AjaxMasterPageName = "Ajax";
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewResult result = filterContext.Result as ViewResult;
            if (result == null) return;
            if (string.IsNullOrEmpty(result.MasterName))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest() && ("GET"==filterContext.HttpContext.Request.RequestType))
                    result.MasterName = filterContext.HttpContext.Request.IsAjaxRequest() ? AjaxMasterPageName : NormalRequestMasterPageName;
            }
        }
    }
}