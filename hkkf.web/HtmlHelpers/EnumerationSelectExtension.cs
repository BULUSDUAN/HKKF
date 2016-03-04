using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using hkkf.Models;
using System.Web.Mvc.Html;
using hkkf.Repositories;
using hkkf.Common.Attributes;
using System.Web.Caching;
using hkkf.Common;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 枚举选择扩展
    /// </summary>
    public static class EnumerationSelectExtension
    {
        private static readonly string keySuffix = typeof(EnumerationSelectExtension).GUID.ToString();

        //private static IEnumerable<SelectListItem> GetWithCache(Cache cache, string name,string enumerationEnglishName)
        //{
        //    string key = keySuffix + enumerationEnglishName;
        //    var result = cache.Get(key) as IEnumerable<SelectListItem>;
        //    if (result == null)
        //    {
        //        using (var session = NHibernateHelper.OpenSession())
        //        {
        //            EnumerationRepository repository = new EnumerationRepository();
        //            var enumeration = repository.GetByEnglishName(session, enumerationEnglishName);
        //            result = enumeration.Values.Select(v => new SelectListItem { Value = v.ID.ToString(), Text = v.EnmTextValue, Selected = (v.ID.ToString() == HttpContext.Current.Request[name]) });
        //        }
        //        cache.Remove(key);
        //        cache.Add(key, result, null, DateTime.MaxValue, new TimeSpan(0, 20, 0), CacheItemPriority.High, null);               
        //    }
        //    return result;
        //}

        //private static IEnumerable<SelectListItem> GetWithEnumCache(Cache cache, string name,string enumerationEnglishName)
        //{
        //    string key = keySuffix + enumerationEnglishName;
        //    var result = cache.Get(key) as IEnumerable<SelectListItem>;
        //    if (result == null)
        //    {
        //        using (var session = NHibernateHelper.OpenSession())
        //        {
        //            EnumerationRepository repository = new EnumerationRepository();
        //            var enumeration = repository.GetByEnglishName(session, enumerationEnglishName);
        //            result = enumeration.Values.Select(v => new SelectListItem { Value = v.EnmValue.ToString(), Text = v.EnmTextValue, Selected = (v.ID.ToString() == HttpContext.Current.Request[name]) });
        //        }                   
        //        cache.Add(key, result, null, DateTime.MaxValue, new TimeSpan(0, 20, 0), CacheItemPriority.High, null);
        //        cache.Remove(key);
        //    }
        //    return result;
        //}

        //private static IEnumerable<SelectListItem> GetWithEnumCache(Cache cache, string name, string enumerationEnglishName,string defaultValue)
        //{
        //    string key = keySuffix + enumerationEnglishName;
        //    var result = cache.Get(key) as IEnumerable<SelectListItem>;
        //    if (result == null)
        //    {
        //        using (var session = NHibernateHelper.OpenSession())
        //        {
        //            EnumerationRepository repository = new EnumerationRepository();
        //            var enumeration = repository.GetByEnglishName(session, enumerationEnglishName);
        //            result = enumeration.Values.Select(v => new SelectListItem { Value = v.EnmValue.ToString(), Text = v.EnmTextValue, Selected = (v.EnmTextValue==defaultValue)?true:(v.ID.ToString() == HttpContext.Current.Request[name]) });
        //        }
        //        cache.Remove(key);
        //        cache.Add(key, result, null, DateTime.MaxValue, new TimeSpan(0, 20, 0), CacheItemPriority.High, null);                
        //    }
        //    return result;
        //}

        //private static IEnumerable<SelectListItem> GetWithCacheText(Cache cache, string enumerationEnglishName)
        //{
        //    string key = keySuffix + enumerationEnglishName;
        //    var result = cache.Get(key) as IEnumerable<SelectListItem>;
        //    if (result == null)
        //    {
        //        using (var session = NHibernateHelper.OpenSession())
        //        {
        //            EnumerationRepository repository = new EnumerationRepository();
        //            var enumeration = repository.GetByEnglishName(session, enumerationEnglishName);
        //            result = enumeration.Values.Select(v => new SelectListItem { Value = v.EnmTextValue, Text = v.EnmTextValue });
        //        }
        //        cache.Remove(key);
        //        cache.Add(key, result, null, DateTime.MaxValue, new TimeSpan(0, 20, 0), CacheItemPriority.High, null);                
        //    }
        //    return result;
        //}



        //private static IEnumerable<SelectListItem> GetWithCacheEnum(Cache cache, string enumerationEnglishName, string optionLabel)
        //{
        //    string key = keySuffix + enumerationEnglishName;
        //    var result = cache.Get(key) as IEnumerable<SelectListItem>;
        //    if (result == null)
        //    {
        //        using (var session = NHibernateHelper.OpenSession())
        //        {
        //            EnumerationRepository repository = new EnumerationRepository();
        //            EnmCommonValue enmCommonValue = new EnmCommonValue();
        //            var enumeration = repository.GetByEnglishName(session, enumerationEnglishName);
        //            enumeration.Values.Add(enmCommonValue);
        //            for (int i = enumeration.Values.Count-1; i >0 ;i-- )
        //            {
        //                enumeration.Values.ElementAt(i).EnmValue = enumeration.Values.ElementAt(i-1).EnmValue;
        //                enumeration.Values.ElementAt(i).EnmTextValue = enumeration.Values.ElementAt(i - 1).EnmTextValue;
        //            }
        //            enumeration.Values.ElementAt(0).EnmValue = -1;
        //            enumeration.Values.ElementAt(0).EnmTextValue = optionLabel;
        //            result = enumeration.Values.Select(v => new SelectListItem { Value = v.EnmValue.ToString(), Text = v.EnmTextValue });
        //        }
        //        cache.Remove(key);
        //        cache.Add(key, result, null, DateTime.MaxValue, new TimeSpan(0, 20, 0), CacheItemPriority.High, null);
        //    }
        //    return result;
        //}
        ///// <summary>
        ///// 枚举类型下拉列表
        ///// </summary>
        ///// <param name="htmlHelper">htmlHelper</param>
        ///// <param name="name">控件名称</param>
        ///// <param name="attr">EnumerationAttribute</param>
        ///// <param name="htmlAttributes">htmlAttributes</param>
        ///// <returns></returns>
        //public static MvcHtmlString DropDownListForEnumeration(this HtmlHelper htmlHelper, string name, string enumerationEnglishName, string optionLabel, object htmlAttributes = null)
        //{
        //    var list = GetWithCache(htmlHelper.ViewContext.HttpContext.Cache, name, enumerationEnglishName);
        //    return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        //}


        //public static MvcHtmlString ValueBoxFor(this HtmlHelper htmlHelper, string name, string enumerationEnglishName, string optionLabel, object htmlAttributes = null)
        //{
        //    var list = GetWithCache(htmlHelper.ViewContext.HttpContext.Cache, name, enumerationEnglishName);
        //    return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        //}

        //public static MvcHtmlString ValueBoxForEnum(this HtmlHelper htmlHelper, string name, string enumerationEnglishName, string optionLabel, object htmlAttributes = null)
        //{
        //    var list = GetWithEnumCache(htmlHelper.ViewContext.HttpContext.Cache, name, enumerationEnglishName);
        //    return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        //}

        //public static MvcHtmlString ValueBoxForEnum(this HtmlHelper htmlHelper, string name, string enumerationEnglishName, string optionLabel, string defaultValue, object htmlAttributes = null)
        //{
        //    var list = GetWithEnumCache(htmlHelper.ViewContext.HttpContext.Cache, name, enumerationEnglishName, defaultValue);
        //    return htmlHelper.DropDownList(name, list, optionLabel, htmlHelper);
        //}


    }
}
