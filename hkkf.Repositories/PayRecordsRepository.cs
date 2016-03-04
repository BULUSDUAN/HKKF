using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using hkkf.Common;
using hkkf.Models;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Mapping;
using JieNuo.Data;


namespace hkkf.Repositories
{
    public class PayRecordsRepository : NHibernateRepository<PayRecords, int>
    {
        public PagedData<PayRecords> GetPayRecords(QueryInfo queyInfo, string name)
        {
            return GetSession().Linq<PayRecords>()
                .WhereIf(p => p._Shop.Name.Contains(name), name.IsNotNullAndEmpty())
                .Page(queyInfo);
        }
        public PagedData<PayRecords> GetPayRecords(QueryInfo queyInfo, Kf_DepartMent kf_DepartMent,string name,string Year,string Month)
        {
            return GetSession().Linq<PayRecords>()
                .WhereIf(p=>p._Shop._Kf_DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
                .WhereIf(p => p._Shop.Name.Contains(name), name.IsNotNullAndEmpty())
                .Where(p=>p.Year==Convert.ToInt32(Year))
                .Where(p=>p.Month==Convert.ToInt32(Month))
                .OrderByDescending(p=>p.PayDate)
                .Page(queyInfo);
        }
        //取出最近4次的付款记录，因为服务费和提成是分开的，所以也就是本次和上一次的记录。
        // 此处还没写完，不应取出所以的，应该取出前几条就可以了。
        public List<PayRecords> GetPayRecords(string shopID,_PayType payType)
        {
            return this.GetAll()
                 .Where(it=>it._Shop.ID==Convert.ToInt32(shopID))  
                 .Where(it=>it._PayType==payType)
                 .OrderByDescending(it=>it.UpdateTime)  
                 .Take(12)
                 .ToList();
        }
        public int GetPayRecordsSum(_PayType PayType, string Year, string Month)
        {
            return GetSession().Linq<PayRecords>()
                .Where(p => p._PayType==PayType)
                .Where(p => p.Year == Convert.ToInt32(Year))
                .Where(p => p.Month == Convert.ToInt32(Month))
                .Sum(p=>p.PayNum);
        }
    }
}
