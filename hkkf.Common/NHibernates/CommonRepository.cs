using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Data;
using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;
using NHibernate.Criterion;
using NHibernate.Mapping;
using System.IO;
using JieNuo.Data.Exceptions;
using System.ComponentModel;
using JieNuo.Data;
using System.Collections.Specialized;

namespace hkkf.Common
{
    public class CommonRepository
    {
        public PagedTableData Query(Type type, IEnumerable<QuickQueryProperty> properties, QueryInfo queryInfo, NameValueCollection addtionalCondtions, int? ApplyTypeID)
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            ICriterion criterion = GetCriterion(properties);
            string searchName = null;//所搜索的单位名称
            string[] allKeys = addtionalCondtions.AllKeys;   //所有的参数     
            if (criterion != null)
            {

                string keyValue = null; //只获取单位更名或者单位更名加延续的参数  ApplyTypeID
                if (System.Array.IndexOf(allKeys, "ApplyTypeID") == -1)
                {
                    if (System.Array.IndexOf(allKeys, "applytypeid") > -1)
                        keyValue = addtionalCondtions["applytypeid"];
                }
                else
                    keyValue = addtionalCondtions["ApplyTypeID"];

                if (keyValue.IsNotNullAndEmpty() && (keyValue == "13" || keyValue == "16"))
                {
                    searchName = criterion.ToString().Substring(10);
                    criterion = null; //单位更名或者单位更名加延续 清除所搜索的单位名称
                }
            }

            var projections = properties
                .Select(p => Projections.Property(p.Name))
                .ToArray();

            ICriteria criteria = session.CreateCriteria(type).AddIf(criterion, criterion != null);
            //criteria.Add(Restrictions.Gt("ID", 0));            
            string sql = "{0}";
            foreach (var key in allKeys)
            {
                if (key == "ApplyTypeID" || key == "applytypeid")
                {
                    if (searchName.IsNotNullAndEmpty())
                    {
                        sql = (sql == "{0}")
                            ? string.Format(sql, "CompanyID in (select CompanyID from Engineers where ApplyTypeID='" + addtionalCondtions[key] + "' and Engineerid in (select EngineerId from Reg_changes where NewCompanyName like '" + searchName + "' or OldCompanyName like '" + searchName + "')){0}")
                            : string.Format(sql, "and CompanyID in (select CompanyID from Engineers where ApplyTypeID='" + addtionalCondtions[key] + "' and Engineerid in (select EngineerId from Reg_changes where NewCompanyName like '" + searchName + "' or OldCompanyName like '" + searchName + "')){0}");
                    }
                    else
                    {
                        string keyValue = addtionalCondtions["ApplyTypeID"];
                        if (keyValue.IsNotNullAndEmpty() && (keyValue == "4" || keyValue == "17"))
                        {
                            sql = (sql == "{0}")
                                ? string.Format(sql, "CompanyID in (select NewCompanyID from reg_changes where ApplyTypeID='" + addtionalCondtions[key] + "'){0}")
                                : string.Format(sql, "and CompanyID in (select NewCompanyID from reg_changes where ApplyTypeID='" + addtionalCondtions[key] + "'){0}");
                        }
                        else
                        {
                            sql = (sql == "{0}")
                                ? string.Format(sql, "CompanyID in (select CompanyID from Engineers where ApplyTypeID='" + addtionalCondtions[key] + "'){0}")
                                : string.Format(sql, "and CompanyID in (select CompanyID from Engineers where ApplyTypeID='" + addtionalCondtions[key] + "'){0}");
                        }
                    }
                }
                if (key == "CheckStateID" || key == "checkstateid")
                {
                    sql = (sql == "{0}")
                        ? string.Format(sql, "CheckStateID='" + addtionalCondtions[key] + "'{0}")
                        : string.Format(sql, "and CheckStateID='" + addtionalCondtions[key] + "'{0}");
                }
                if (key == "CheckStateIDE2" || key == "checkstateide2")
                {
                    sql = (sql == "{0}")
                        ? string.Format(sql, "CheckStateIDE2='" + addtionalCondtions[key] + "'{0}")
                        : string.Format(sql, "and CheckStateIDE2='" + addtionalCondtions[key] + "'{0}");
                }
                if (key == "CheckStateID2" || key == "checkstateid2")
                {
                    sql = (sql == "{0}")
                        ? string.Format(sql, "CheckStateIDE2='" + addtionalCondtions[key] + "'{0}")
                        : string.Format(sql, "and CheckStateIDE2='" + addtionalCondtions[key] + "'{0}");
                }
                if (key == "CheckStateID3" || key == "checkstateid3")
                {
                    sql = (sql == "{0}")
                        ? string.Format(sql, "CheckStateIDE2='" + addtionalCondtions[key] + "'{0}")
                        : string.Format(sql, "and CheckStateIDE2='" + addtionalCondtions[key] + "'{0}");
                }
                if (key == "areaid" || key == "AreaID")
                {
                    sql = (sql == "{0}")
                        ? string.Format(sql, " AreaID='" + addtionalCondtions[key] + "'{0}")
                        : string.Format(sql, " and AreaID='" + addtionalCondtions[key] + "'{0}");
                }
                if (key == "pointid" || key == "PointId")
                {
                    sql = (sql == "{0}")
                        ? string.Format(sql, " TrainPointID='" + addtionalCondtions[key] + "'{0}")
                        : string.Format(sql, " and TrainPointID='" + addtionalCondtions[key] + "'{0}");
                }
            }
            //object checkState = null;
            //var checkStateType = type.GetProperty("CheckStateIDE2").PropertyType;
            //checkState = Enum.Parse(checkStateType, addtionalCondtions["CheckStateIDE2"]);
            //criteria.AddIf(Restrictions.Eq("CheckStateIDE2", GetStates(type, "CheckStateIDE2", addtionalCondtions["CheckStateIDE2"])), allKeys.Contains("CheckStateIDE2"));
            sql = string.Format(sql, "");
            if (sql != "")
                criteria.Add(new SQLCriterion(new NHibernate.SqlCommand.SqlString(sql), new object[] { }, new NHibernate.Type.IType[] { }));

            ICriteria countCriteria = criteria.SetProjection(Projections.Count(properties.Select(p => p.Name).FirstOrDefault()));

            int count = (int)countCriteria.UniqueResult();
            Pager pager = new Pager(queryInfo.Page, queryInfo.PageSize, count, queryInfo.OrderBy, queryInfo.IsDesc);

            ICriteria dataCriteria = criteria
                .SetProjection(projections)
                .SetFirstResult(pager.FristDataPos - 1)
                .SetMaxResults(pager.TakeCount);

            IList<object[]> list = criteria.List<object[]>();

            DataTable table = new DataTable(type.Name);
            foreach (QuickQueryProperty property in properties)
                table.Columns.Add(property.Text, property.Type);
            foreach (var row in list)
                table.Rows.Add(row);

            return new PagedTableData(
                pager,
                table,
                properties.Select(p => p.Format),
                properties.Select(p => p.IsBusinessIdProperty),
                properties.Select(p => p.IsTextProperty)
                );
        }


        private ICriterion GetCriterion(IEnumerable<QuickQueryProperty> properties)
        {
            // TypeDescriptor.GetConverter(

            var criterions = properties
                            .Where(p => p.IsFilter)
                            .Select(p => new { Name = p.Name, FilterValue = p.Filter.ConvertTo(p.Type) })
                            .Where(p => p.FilterValue != null)
                            .Select(p => p.FilterValue is string ? Restrictions.Like(p.Name, p.FilterValue as string, MatchMode.Anywhere) : Restrictions.Eq(p.Name, p.FilterValue))
                            .ToArray();

            ICriterion criterion = null;
            if (criterions.Length > 1)
                criterion = criterions
                    .Cast<ICriterion>()
                    .Aggregate((c1, c2) => Restrictions.Or(c1, c2));
            else if (criterions.Length == 1)
                criterion = criterions.First();
            return criterion;
        }


        public string GetText(Type type, string id)
        {
            TypeInfoAttribute attr = TypeInfoAttribute.GetTypInfo(type);
            //
            string textColumn = AttributeHelper
                .GetNoInherit<PropertyAttribute>(type, attr.TextProperty)
                .Select(p => p.Name)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(textColumn)) textColumn = attr.TextProperty;

            //
            string businessId = AttributeHelper
                .GetNoInherit<PropertyAttribute>(type, attr.BusinessIdProperty)
                .Select(p => p.Name)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(businessId)) businessId = attr.BusinessIdProperty;

            ISession session = NHibernateHelper.GetCurrentSession(); //SessionHelper.OpenSession(type);
            ICriteria criteria = session.CreateCriteria(type)
                .SetProjection(Projections.Property(textColumn))
                .Add(Restrictions.Eq(businessId, id));

            object result = criteria.UniqueResult();

            if (result == null) return null;
            return result.ToString();
        }


        #region GetByXXXX

        public object GetByCode(Type type, string code)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            object result = session.CreateCriteria(type)
                 .Add(Restrictions.Eq("Code", code))
                 .AddOrder(new Order("ID", false))
                 .SetMaxResults(1)
                 .UniqueResult();
            return result;
        }

        public object GetEntityByBusinessID(Type type, object id)
        {
            TypeInfoAttribute attr = TypeInfoAttribute.GetTypInfo(type);

            ISession session = NHibernateHelper.GetCurrentSession();
            return session.CreateCriteria(type)
                .Add(Restrictions.Eq(attr.BusinessIdProperty, id))
                .UniqueResult();
        }

        public object GetByDatabaseID(Type type, object id)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            return session.Get(type, id);
        }
        #endregion

        #region Exists
        public static bool Exists(string table, string column, string value)
        {
            return Exists(table, column, value, null, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <param name="idColumn"></param>
        /// <param name="excludeID"></param>
        /// <returns></returns>
        public static bool Exists(string table, string column, string value, string idColumn, int? excludeID)
        {
            string sql = "Select Count(*) from {0} where {1} = :value";
            if (excludeID.HasValue) sql += " and {2} != :excludeID";

            ISession session = NHibernateHelper.GetCurrentSession();
            sql = string.Format(sql, table, column, idColumn);

            IQuery query = session.CreateSQLQuery(sql)
                 .SetString("value", value);

            if (excludeID.HasValue)
                query = query.SetInt32("excludeID", excludeID.Value);

            int count = query.UniqueResult<int>();
            return count > 0;
        }

        public static bool Exists(Type type, string propertyName, string value, int? excludeID)
        {
            //PersistentClass pc = NHibernateHelper.GetPersistentClass(type);
            string idColumn = "ID";//pc.Identifier.ColumnIterator.First().Text;

            ISession session = NHibernateHelper.OpenSession();
            ICriteria criteria = session.CreateCriteria(type)
                .Add(Restrictions.Eq(propertyName, value))
                .AddIf(Restrictions.Not(Restrictions.Eq(idColumn, excludeID.Value)), excludeID.HasValue)
                .SetProjection(Projections.Count(idColumn));
            return criteria.UniqueResult<int>() > 0;
        }

        public static bool Exists<T>(string propertyName, string value, int? excludeID)
        {
            return Exists(typeof(T), propertyName, value, excludeID);
        }

        public static bool Exists<T>(string propertyName, string value)
        {
            return Exists<T>(propertyName, value);
        }
        #endregion

        public static void BackUpDb(string fileName, string password)
        {
            if (File.Exists(fileName))
                throw new RuleException("fileName", "已存在名为 {0} 的备份文件", Path.GetFileName(fileName));

            ISession session = NHibernateHelper.GetCurrentSession();
            IQuery query = session.CreateSQLQuery("exec backUpDB :fileName, :password")
                .SetString("fileName", fileName)
                .SetString("password", password);
            try
            {
                int i = query.ExecuteUpdate();
            }//
            catch (HibernateException ex)
            {
                throw new RuleException("fileName", ex.InnerException.Message);
            }
        }

        private static object GetStates(Type type, string property, string value)
        {
            object checkState = null;
            var checkStateType = type.GetProperty(property).PropertyType;
            try
            {
                checkState = Enum.Parse(checkStateType, value);
            }
            catch { }
            return checkState;
        }
    }
}
