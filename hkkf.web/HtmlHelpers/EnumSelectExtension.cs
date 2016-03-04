using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using hkkf.Models;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 枚举下拉列表，用于在代码中编写的枚举
    /// </summary>
    public static class EnumSelectExtension
    {
        #region Base



        public static MvcHtmlString ValueBoxForEnum<TEnum>(this HtmlHelper htmlHelper, string name, 
string optionLabel, object htmlAttributes = null)
            where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var list = values.Select(e => new SelectListItem { Value = (e as Enum).ToString("d"), Text = e.ToString() });
            return SelectExtensions.DropDownList(htmlHelper, name, list,optionLabel, htmlAttributes);
        }

        public static MvcHtmlString ValueBoxForEnum<TEnum>(this HtmlHelper htmlHelper, string name, IEnumerable<TEnum> values,  string optionLabel, object htmlAttributes = null)
            where TEnum : struct
        { 
            var list = values.Select(e => new SelectListItem { Value = (e as Enum).ToString("d"), Text = e.ToString() }); 
            return SelectExtensions.DropDownList(htmlHelper, name, list, optionLabel, htmlAttributes);
        }
        public static MvcHtmlString ValueBoxForEnum<TEnum>(this HtmlHelper htmlHelper, string name, IEnumerable<TEnum> values, string obj, string optionLabel, object htmlAttributes = null)
        where TEnum : struct
        {
            //List<SelectListItem> list = new List<SelectListItem>();
            //foreach (var item in values)
            //{
            //    SelectListItem ss = new SelectListItem();
            //    ss.Text = item.ToString();
            //    ss.Value = (item as Enum).ToString("d");
            //    if (ss.Text == obj)
            //        ss.Selected = true;
            //    else
            //        ss.Selected = false;
            //    list.Add(ss);
            //}
            //return SelectExtensions.DropDownList(htmlHelper, name, list, optionLabel); 
            string option = "<option value=\"\">" + optionLabel + "</option>";
            foreach (var item in values)
            {
                if (string.IsNullOrEmpty(obj) == false && item.ToString() == obj)
                    option += "<option selected=\"selected\" value=\"" + (item as Enum).ToString("d") + "\">" + item.ToString() + "</option>";
                else
                    option += "<option value=\"" + (item as Enum).ToString("d") + "\">" + item.ToString() + "</option>";
            }
            string select = "<select id=\"" + name + "\" name=\"" + name + "\">"
                + option
                + "</select>";

            return MvcHtmlString.Create(select);
        }

        //  public static MvcHtmlString ValueBoxForEnum<TEnum>(this HtmlHelper htmlHelper, string name, IEnumerable<TEnum> values,string obj,  string optionLabel, object htmlAttributes = null)
        //    where TEnum : struct
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    foreach (var item in values)
        //    {
        //        SelectListItem select = new SelectListItem();
        //        select.Text = item.ToString();
        //        select.Value = (item as Enum).ToString("d");
        //        if (obj.IsNotNullAndEmpty() && select.Text == obj)
        //        {
        //            select.Selected = true;
        //        }
        //        list.Add(select);
        //    }
        //    return SelectExtensions.DropDownList(htmlHelper, name, list, optionLabel);
        //}

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string obj, string optionLabel)
        {
            var tt = expression.Body as MemberExpression;
            string option = "<option value=\"\">" + optionLabel + "</option>";
            foreach (var item in selectList)
            {
                if (string.IsNullOrEmpty(obj) == false && item.Value == obj)
                    option += "<option selected=\"selected\" value=\"" + item.Value + "\">" + item.Text + "</option>";
                else
                    option += "<option value=\"" + item.Value + "\">" + item.Text + "</option>";
            }
            string select = "<select id=\"" + tt.Member.Name + "\" name=\"" + tt.Member.Name + "\">"
                + option
                + "</select>";
            return MvcHtmlString.Create(select);
        }


          //public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,string obj, string optionLabel)
          //{
          //    return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, optionLabel);
          //}

        /// <summary>
        /// 适用于Value和Text是一个字符串的下拉选单（value值和Text值一致）
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ValueBoxForEnum3<TEnum>(this HtmlHelper htmlHelper, string name, string optionLabel,
            object htmlAttributes = null)
            where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var list = values.Select(e => new SelectListItem { Value = e.ToString(), Text = e.ToString() });
            return SelectExtensions.DropDownList(htmlHelper, name, list, optionLabel, htmlAttributes);
        }

        /// <summary>
        /// 适用于Value和Text是一个数字值的下拉选单（Text值和value值一致）
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ValueBoxForEnumYearsAdd<TEnum>(this HtmlHelper htmlHelper, 
            string name, string optionLabel, object htmlAttributes = null)
            where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var list = values.Select(e => new SelectListItem { Value = e.ToString(), Text = (e as Enum).ToString("d") });
            return SelectExtensions.DropDownList(htmlHelper, name, list, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString ValueBoxForEnum2<TEnum>(this HtmlHelper htmlHelper, string name, 
            string optionLabel, object htmlAttributes = null)
           where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var list = values.Select(e => new SelectListItem { Value = (e as Enum).ToString("d"), 
                Text = (e as Enum).ToString("d") });
            return SelectExtensions.DropDownList(htmlHelper, name, list, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString ValueBoxForEnum<T, TEnum>(this HtmlHelper<T> htmlHelper,
            Expression<Func<T, TEnum>> expression, TEnum? defaultValue, object htmlAttributes = null)
            where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var list = values.Select(e => new SelectListItem { Value = (e as Enum).ToString("d"), 
                Text = e.ToString(), Selected = defaultValue.HasValue && e.Equals(defaultValue.Value) });
            return SelectExtensions.DropDownListFor(htmlHelper, expression, list, htmlAttributes);
        }

        public static MvcHtmlString ValueBoxForEnum<T, TEnum>(this HtmlHelper<T> htmlHelper,
            Expression<Func<T, TEnum>> expression, TEnum? defaultValue,string optionlable, object htmlAttributes = null)
            where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var list = values.Select(e => new SelectListItem { Value = e.ToString(), Text = (e as Enum).ToString("d"),
                Selected = defaultValue.HasValue && e.Equals(defaultValue.Value) });
            return SelectExtensions.DropDownListFor(htmlHelper, expression, list, optionlable, htmlAttributes);
        } 
        #endregion


        //#region 枚举下拉列表：ApplyStates

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, ApplyStates>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, ApplyStates>> expression, ApplyStates defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：CertificateClasses

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, CertificateClass>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, CertificateClasses>> expression, CertificateClasses defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：CompanyTypes

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, CompanyTypes>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, CompanyTypes>> expression, CompanyTypes defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：RegStates

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, RegStates>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, RegStates>> expression, RegStates defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：Specialty

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, Specialty>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, Specialty>> expression, Specialty defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：Levels

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, Levels>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, Levels>> expression, Levels defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：EduCheckStates

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, EduCheckStates>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, EduCheckStates>> expression, EduCheckStates defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion


        

        

        
        //#region 枚举下拉列表：ClassStage

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, ClassStage>> expression, string optionlable, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, optionlable, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, ClassStage>> expression, ClassStage defaultValue, string optionlable, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, optionlable, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：Years

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, Years>> expression, string optionlable, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, optionlable, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, Years>> expression, Years defaultValue, string optionlable, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, optionlable, htmlAttributes);
        //}

        //#endregion

        //#region 枚举下拉列表：CheckStates

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, CheckStates>> expression, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, null, htmlAttributes);
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, CheckStates>> expression, CheckStates defaultValue, object htmlAttributes = null)
        //{
        //    return ValueBoxForEnum(htmlHelper, expression, defaultValue, htmlAttributes);
        //}

        //#endregion

    }
}
