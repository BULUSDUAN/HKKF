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
    public class PayRequireRecordsRepository : NHibernateRepository<PayRequireRecords, int>
    {
        #region "查询"     
        public PagedData<PayRequireRecords> GetPayRequireRecords(QueryInfo queyInfo, string name)
        {
            return GetSession().Linq<PayRequireRecords>()
                .WhereIf(p => p._Shop.Name.Contains(name), name.IsNotNullAndEmpty())
                .Page(queyInfo);
        }
        public PagedData<PayRequireRecords> GetPayRequireRecords(QueryInfo queyInfo,Kf_DepartMent kf_DepartMent, string name, string Year, string Month)
        {
            return GetSession().Linq<PayRequireRecords>()
                .WhereIf(p => p._Shop._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .WhereIf(p => p._Shop.Name.Contains(name), name.IsNotNullAndEmpty())
                .Where(p => p.Year == Convert.ToInt32(Year))
                .WhereIf(p => p.Month == Convert.ToInt32(Month),Month.Trim()!="全部")
                .OrderBy(p=>p.PayRequireDate)
                .Page(queyInfo);
        }
        public int GetPayRequireRecordsSum(_PayType PayType, string Year, string Month)
        {
            return GetSession().Linq<PayRequireRecords>()
                .Where(p => p._PayType == PayType)
                .Where(p => p.Year == Convert.ToInt32(Year))
                .WhereIf(p => p.Month == Convert.ToInt32(Month), Month.Trim() != "全部")
                .Sum(p => p.PayRequireNum);
        }
        #endregion
        //根据添加或者修改的payRecords更新PayRequireRecords。
        public void AddPayRequireRecords(PayRecords payRecords)
        {
            //PayRecordsRepository payRecordsRepo=new PayRecordsRepository();
            List<PayRequireRecords> listPayRequireRecords = this.GetAll()
                .Where(it => it._PayType == payRecords._PayType)
                .Where(it => it._Shop == payRecords._Shop)
                .ToList();
            foreach (var PayRequireRecord in listPayRequireRecords)
            {
                this.Delete(PayRequireRecord);
            }
            PayRequireRecords payRequireRecord = new PayRequireRecords();
            payRequireRecord._Shop = payRecords._Shop;
            payRequireRecord._PayType = payRecords._PayType;
            payRequireRecord.PayRequireDate = payRecords.NextPayDate;
            payRequireRecord.PayRequireNum = payRecords.NextPayNum;
            payRequireRecord.Year = payRecords.NextPayDate.Year;
            payRequireRecord.Month = payRecords.NextPayDate.Month;
            payRequireRecord.UpdateTime = System.DateTime.Now;
            this.Save(payRequireRecord);
        }  
    }
}
