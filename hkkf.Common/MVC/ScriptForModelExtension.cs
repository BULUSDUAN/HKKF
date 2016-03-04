using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Common.MVC;

namespace System.Web.Mvc
{
    public static class ScriptForModelExtension
    {
        /// <summary>
        /// 生成相关脚本
        /// </summary>
        public static MvcHtmlString ScriptForModel<T>(this HtmlHelper<T> htmlHelper)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var propertyMetadata in htmlHelper.ViewData.ModelMetadata.Properties)
            {
                var scriptables = propertyMetadata.AllAttribute().OfType<IClientScriptable>();
                foreach (var scriptable in scriptables)
                {
                    builder.Append("\t\t");
                    builder.AppendLine(scriptable.GenerateScriptForProperty(propertyMetadata.PropertyName).ToHtmlString());
                }
            }
            if (builder.Length == 0) return MvcHtmlString.Empty;

            builder.Insert(0, "\t$(function(){\r\n");
            builder.Insert(0, "<script type=\"text/javascript\">\r\n");
            builder.AppendLine("\t});");
            builder.AppendLine("</script>");

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
