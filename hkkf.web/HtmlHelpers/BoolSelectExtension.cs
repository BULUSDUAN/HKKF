using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 性别选择扩展
    /// </summary>
    public static class BoolSelectExtension
    {
        //private static SelectListItem[] sexItems = new[]{ 
        // new SelectListItem{ Value = true.ToString(), Text = "男" },
        // new SelectListItem{ Value = false.ToString(), Text = "女" }
        //};
        
        ///// <summary>
        ///// 性别下拉列表
        ///// </summary>
        ///// <param name="htmlHelper">htmlHelper</param>
        ///// <param name="name">控件名称</param>
        ///// <param name="optionLabel">显示的文本，如“请选择XXX”</param>
        ///// <param name="htmlAttributes">htmlAttributes</param>
        ///// <returns></returns>
        //public static MvcHtmlString DropDownListForSex(this HtmlHelper htmlHelper, string name, string optionLabel = "请选择", object htmlAttributes = null)
        //{
        //    return htmlHelper.DropDownList(name, sexItems, optionLabel, htmlAttributes);
        //}

        public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, bool?>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var boolAttr = metadata.BoolAttribute();

            if(boolAttr == null) return htmlHelper.ValueBoxFor<T, bool?>(expression, htmlAttributes);

            var list = new SelectListItem[]{ 
                    new SelectListItem{ Value = bool.TrueString, Text = boolAttr.TextForTrue, Selected = boolAttr.Default == true},
                    new SelectListItem{ Value = bool.FalseString, Text = boolAttr.TextForFalse, Selected = boolAttr.Default == false}
            };

            return htmlHelper.DropDownListFor(expression, list, boolAttr.Default.HasValue ? null : boolAttr.Message, htmlAttributes);
        }

        
        public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, bool>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var boolAttr = metadata.BoolAttribute();

            if (boolAttr == null) return htmlHelper.ValueBoxFor<T, bool>(expression, htmlAttributes);

            var list = new SelectListItem[]{ 
                    new SelectListItem{ Value = bool.TrueString, Text = boolAttr.TextForTrue, Selected = boolAttr.Default == true},
                    new SelectListItem{ Value = bool.FalseString, Text = boolAttr.TextForFalse, Selected = boolAttr.Default == false}
            };

            return htmlHelper.DropDownListFor(expression, list, boolAttr.Default.HasValue ? null : boolAttr.Message, htmlAttributes);
        }

       
    }
}