using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Common;
using hkkf.Models;
using JieNuo.Data;
using NHibernate.Linq;

namespace hkkf.Repositories
{
    public class WrongRepository : NHibernateRepository<Wrong, int>
    {
        public Wrong GetWrongByAnswer(int wrong, DateTime time, int userid)
        {
            return GetSession().Linq<Wrong>()
                .Where(p => p.wrong.ID == wrong)
                .Where(P => P.time<time && time.AddDays(-1)<P.time)
                .Where(p => p.userid.ID== userid)
                .FirstOrDefault();


        }
        public int GetWrongByAnswer( DateTime time, int userid)
        {
            return GetSession().Linq<Wrong>()
               .Where(P => P.time < time && time.AddDays(-1) < P.time)
                .Where(p => p.userid.ID == userid)
                .Count();
        }
        public PagedData<Wrong> Wrong(QueryInfo queryInfo, DateTime? time, string name)
        {
            return GetSession()
                .Linq<Wrong>()
                .WhereIf(e => e.userid.Name == name, name.IsNotNullAndEmpty())
                .WhereIf(e => e.time > time.Value.Date && time.Value.Date.AddDays(1) > e.time, time != null)
                .Page(queryInfo);
        } 
    }
}
