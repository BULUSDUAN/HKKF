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
    public class ShopGroupDetailRepository : NHibernateRepository<ShopGroupDetails, int>
    {
        public PagedData<ShopGroupDetails> GetShopGroupDetails(QueryInfo queyInfo)
        {
            return GetSession().Linq<ShopGroupDetails>()
               // .WhereIf(p => p.ShopGroupName.Contains(ShopGroupName), ShopGroupName.IsNotNullAndEmpty())
                .Page(queyInfo);
        }

        public bool ExistShopDifficultyLevelName(string Name)
        {
            return GetSession().Linq<ShopDifficultyLevel>()
               .Where(u => u.ShopDifficultyLevelName == Name).Any(); 
        }

        public bool ExistShopDifficultyLevelID(int id)
        {
            return GetSession().Linq<ShopDifficultyLevel>()
               .Where(u => u.ID ==id).Any(); 
        }
    }
}
