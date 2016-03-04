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
    public class ShopDifficultyLevelRepository : NHibernateRepository<ShopDifficultyLevel, int>
    {
        public PagedData<ShopDifficultyLevel> GetShopDifficultyLevel(QueryInfo queyInfo, string name)
        {
            return GetSession().Linq<ShopDifficultyLevel>()
                .WhereIf(p => p.ShopDifficultyLevelName.Contains(name), name.IsNotNullAndEmpty())
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
