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
    public class wwhRepository : NHibernateRepository<wwh, int>
    {
        public PagedData<wwh> GetData(QueryInfo queryInfo,string name ,int shopid)
        {
            return GetSession().Linq<wwh>()
                .Where(p=>p._Shop.ID==shopid)
                .WhereIf(p => p.Name.Contains(name), name.IsNotNullAndEmpty())
                .Page(queryInfo);
        }
    }
}
