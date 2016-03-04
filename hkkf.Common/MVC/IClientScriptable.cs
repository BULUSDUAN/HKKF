using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace hkkf.Common.MVC
{
    /// <summary>
    /// 可生成脚本接口
    /// </summary>
    public interface IClientScriptable
    {
        /// <summary>
        /// 生成脚本
        /// </summary>
        /// <returns></returns>
        MvcHtmlString GenerateScriptForProperty(string propertyName);
    }
}
