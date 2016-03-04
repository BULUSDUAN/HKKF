using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using hkkf.Models.Base;
using System.Linq.Expressions;
using hkkf.Models;
using hkkf.Common;

namespace System.Web.Mvc.Html
{
    public static class EnumerationItemSelectExtension
    {
        #region 未实现
        
        public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, IEnumerationItem>> expression, object htmlAttribtes = null)
        {
            throw new NotImplementedException();
            //var all = NHibernateHelper.GetCurrentSession().CreateCriteria<TProperty>().List<TProperty>();
            //return htmlHelper.DropDownList(
            //    ExpressionHelper.GetExpressionText(expression) + ".ID",
            //    all.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.Name, Selected = false })
            //    );
        } 
        #endregion

        #region Base
        private static IEnumerable<T> GetAllEnumerationItem<T>()
            where T : class, IEnumerationItem
        {
            return NHibernateHelper
                .GetCurrentSession()
                .CreateCriteria<T>()
                .List<T>();
        }

        public static MvcHtmlString DropDownListFor<T>(this HtmlHelper htmlHelper, string name, IEnumerable<T> enumerable)
            where T : IEnumerationItem
        {
            var list = enumerable.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.Name, Selected = false });
            return htmlHelper.DropDownList(name, list);
        }

        public static MvcHtmlString DropDownListFor<T>(this HtmlHelper htmlHelper, string name, IEnumerable<T> enumerable, string optionLabel)
            where T : IEnumerationItem
        {
            var list = enumerable.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.Name, Selected = (i.ID.ToString() == HttpContext.Current.Request[name]) });
            return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        }

        public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper htmlHelper, string name)
            where T : class, IEnumerationItem
        {
            var all = GetAllEnumerationItem<T>();
            Type tt = typeof(T);
            if (tt.FullName.Contains("EnmEngineerType"))
                all = all.Take(all.Count() - 2);
            return DropDownListFor<T>(htmlHelper, name, all);
        }

        public static MvcHtmlString ValueBoxForEdu<T>(this HtmlHelper htmlHelper, string name)
            where T : class, IEnumerationItem
        {
            var all = GetAllEnumerationItem<T>();
            return DropDownListFor<T>(htmlHelper, name, all);
        }

        public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper htmlHelper, string name, string optionLabel)
            where T : class, IEnumerationItem
        {
            var all = GetAllEnumerationItem<T>();
            Type tt = typeof(T);
            if (tt.FullName.Contains("EnmEngineerType"))
                all = all.Take(all.Count() - 2);
            var list = all.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.Name, Selected = (i.ID.ToString() == HttpContext.Current.Request[name]) });
            return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        }


        public static MvcHtmlString ValueBoxForEdu<T>(this HtmlHelper htmlHelper, string name, string optionLabel)
            where T : class, IEnumerationItem
        {
            var all = GetAllEnumerationItem<T>();
            var list = all.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.Name, Selected = (i.ID.ToString() == HttpContext.Current.Request[name]) });
            return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        }


        //分配权限对应用户类型
        //public static MvcHtmlString ValueBoxFor(this HtmlHelper htmlHelper, string name,string optionLabel)
        //{
        //    var all = GetAllEnumerationItem<UserType>()
        //        .Where(p =>p.ID!=1&&p.ID!=2)
        //        .ToList();
        //    var list = all.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.Name, Selected = (i.ID.ToString() == HttpContext.Current.Request[name]) });
        //    return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        //}
        #endregion

        public static MvcHtmlString ValueBoxForTemp<T>(this HtmlHelper htmlHelper, string name)
            where T : class, IEnumerationItem
        {
            var all = GetAllEnumerationItem<T>();
            return DropDownListFor<T>(htmlHelper, name, all);
        }

        public static MvcHtmlString ValueBoxForTemp<T>(this HtmlHelper htmlHelper, string name, string optionLabel)
            where T : class, IEnumerationItem
        {
            var all = GetAllEnumerationItem<T>();
            var list = all.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.Name, Selected = (i.ID.ToString() == HttpContext.Current.Request[name]) });
            return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        }

        #region 针对特定类型的扩展，目前已有：Area,ApplyState,ApplyType,EngineerType，FeeState，InvoiceType，SpecialityType
        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, UserType>> expression, object htmlAttribtes = null)
        //{
        //    return htmlHelper.DropDownListFor(
        //        ExpressionHelper.GetExpressionText(expression) + ".ID",
        //        GetAllEnumerationItem<UserType>()
        //        );
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, EnmArea>> expression, string optionlable, object htmlAttribtes = null)
        //{
        //    return htmlHelper.DropDownListFor(
        //        ExpressionHelper.GetExpressionText(expression) + ".ID",
        //        GetAllEnumerationItem<EnmArea>(), optionlable
        //        );
        //}

        //public static MvcHtmlString ValueBoxFor<T>(this HtmlHelper<T> htmlHelper, Expression<Func<T, ApplyType>> expression, object htmlAttribtes = null)
        //{
        //    return htmlHelper.DropDownListFor(
        //        ExpressionHelper.GetExpressionText(expression) + ".ID",
        //        GetAllEnumerationItem<ApplyType>()
        //        );
        //}

        #endregion

    }
}