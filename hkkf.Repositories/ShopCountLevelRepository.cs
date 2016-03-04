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
    public class ShopCountLevelRepository : NHibernateRepository<ShopCountLevel, int>
    {
        public PagedData<ShopCountLevel> GetShopCountLevel(QueryInfo queyInfo, string name)
        {
            return GetSession().Linq<ShopCountLevel>()
                .WhereIf(p => p.CountLevel.Contains(name), name.IsNotNullAndEmpty())
                .Page(queyInfo);
        }



        public bool ExistShopCountLevelName(string Name)
        {
            return GetSession().Linq<ShopCountLevel>()
               .Where(u => u.CountLevel == Name).Any(); 
        }

        public bool ExistShopCountLevelID(int id)
        {
            return GetSession().Linq<ShopCountLevel>()
               .Where(u => u.ID ==id).Any(); 
        }
    }
}
