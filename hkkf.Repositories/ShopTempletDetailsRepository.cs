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
    public class ShopTempletDetailsRepository : NHibernateRepository<ShopTempletDetails, int>
    {
        public PagedData<ShopTempletDetails> GetShopTempletDetails(QueryInfo queyInfo,string name)
        {
            return GetSession().Linq<ShopTempletDetails>()
                .WhereIf(p => p._ShopTemplet.ShopTempletName.Contains(name), name.IsNotNullAndEmpty())
                .Page(queyInfo);
        }

    }
}
