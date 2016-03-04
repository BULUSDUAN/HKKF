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
    public class ShopTempletTypeRepository : NHibernateRepository<ShopTempletType, int>
    {
        public PagedData<ShopTemplet> GetShopTemplet(QueryInfo queyInfo,string name)
        {
            return GetSession().Linq<ShopTemplet>()
                .WhereIf(p => p.ShopTempletName.Contains(name), name.IsNotNullAndEmpty())
                .Page(queyInfo);
        }



        public bool ExistShopTempletName(string Name)
        {
            return GetSession().Linq<ShopTemplet>()
               .Where(u => u.ShopTempletName == Name).Any(); 
        }

        public bool ExistShopTempletID(int id)
        {
            return GetSession().Linq<ShopTemplet>()
               .Where(u => u.ID ==id).Any(); 
        }
    }
}
