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
    public class ShopTypeRepositories : NHibernateRepository<ShopType, int>
    {
        public PagedData<ShopType> GetShopTypes(QueryInfo queyInfo,string name)
        {
            return GetSession().Linq<ShopType>()
                .WhereIf(p => p.Name.Contains(name), name.IsNotNullAndEmpty())
                .Page(queyInfo);
        }



        public bool ExistShopName(string Name)
        {
            return GetSession().Linq<ShopType>()
               .Where(u => u.Name == Name).Any(); 
        }

        public bool ExistShopID(int id)
        {
            return GetSession().Linq<ShopType>()
               .Where(u => u.ID ==id).Any(); 
        }
    }
}
