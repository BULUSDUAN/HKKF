using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Common;
using hkkf.Repositories;
using JieNuo.Common;
using JieNuo.ComponentModel;
using JieNuo.Data;
using hkkf.Models;
namespace hkkf.web.Controllers
{
    [HandleError(Order = 100)]
    //快速查询设置1、在Users里 [TypeInfo("ID", "userName")]2、对应的ID添加 [QuickQuery(false, 5)]3、在对应的字段上添加  [QuickQuery(true, 5)]
    public class CommonController : BaseController
    {
        private static readonly Dictionary<Type, QuickQueryProperty[]> cache = new Dictionary<Type, QuickQueryProperty[]>();

        private CommonRepository repository = new CommonRepository();

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public ActionResult Query(
            QueryInfo queryInfo,
            string type, [DefaultValue(true)]bool gridOnly,
            IEnumerable<string> names, string value, string QueryString, int? ApplyTypeID)
        {
            //if (this.IsParamChanged(names, value)) queryInfo.Page = 1;

            if (string.IsNullOrEmpty(type)) return null;


            //Type _type = TypeHelper.GetTypeInCurrentDomainByName(type);
            Type _type = GetType(type);
            var properties = GetProperties(_type);

            if (properties.Count() > 0)
            {
                ViewData["checkedNames"] = names;

                var conditionProperties = properties.Where(p => p.AsCondition)
                    .Select(p => p.Text);
                ViewData["names"] = conditionProperties;

                foreach (var p in properties)
                {
                    if (names != null && p.Text.In(names)) p.Filter = value;
                    else p.Filter = null;
                }

                NameValueCollection nameValue = new NameValueCollection();
                if (QueryString != null)
                {
                    string[] querys = QueryString.Split('&');
                    nameValue = new NameValueCollection();
                    foreach (var qq in querys)
                    {
                        if (qq != null && qq != "")
                        {
                            nameValue.Add(qq.Split('=')[0], qq.Split('=')[1]);
                        }
                    }
                    ViewData["QueryString"] = QueryString;
                }
                else
                {
                    nameValue = Request.QueryString;
                    ViewData["QueryString"] = Request.QueryString.ToString();
                }
                PagedTableData data = repository.Query(_type, properties, queryInfo, nameValue, ApplyTypeID);

                if (gridOnly)
                    return View("Grid", data);
                return View(data);
            }
            else
            {
                string error = string.Format("类型 {0}  Attribute 不正确：\r\n至少一个属性应具有 {1}", _type.FullName, typeof(QuickQueryAttribute).Name);
                return View("Error", "", error);
            }
        }

        [HttpPost]
        public string GetText(string type, string id)
        {
            if (string.IsNullOrEmpty(type)) return null;

            Type _type = TypeHelper.GetTypeInCurrentDomainByName(type);
            return repository.GetText(_type, id);
        }


        private QuickQueryProperty[] GetProperties(Type type)
        {
            if (cache.ContainsKey(type)) return cache[type];

            var props = QuickQueryAttribute.GetQuickQueryProperties(type);
            cache.Add(type, props);
            return props;
        }

        [HttpPost]
        public string GetChineseSpell(string str)
        {
            return StringExtension.GetChineseSpell(str);
        }

        public ActionResult Exists(string type, string property, string value, int? excludeID)
        {
            Type _type = Type.GetType(type);
            bool b = CommonRepository.Exists(_type, property, value, excludeID);

            return Json(b, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getAuto(string str)
        {
            string aa = str;
            return Json(new CompanyRepository().GetAuto());
        }

        private Type GetType(string typeName)
        {
            Type type = typeof(Company).Assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            if (type == null)
                type = TypeHelper.GetTypeInCurrentDomainByName(typeName);
            return type;
        }
    }
}
