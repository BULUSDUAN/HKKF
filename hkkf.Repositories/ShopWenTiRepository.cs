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
    public class ShopWenTiRepository : NHibernateRepository<ShopWenTi, int>
    {

        public PagedData<ShopWenTi> GetData(QueryInfo queryInfo, string shopID, string Ncontent, int? wtType,string UserID)
        {
            if (UserID.IsNullOrEmpty())
            {
                return GetSession().Linq<ShopWenTi>()
               .WhereIf(p => p._Shop.Name.Contains(shopID), shopID.IsNotNullAndEmpty())
               .WhereIf(p => p.NContent.Contains(Ncontent), Ncontent.IsNotNullAndEmpty())
               .WhereIf(p => p.wtType == wtType.ToString().ToEnum<WenTiType>(), wtType != null)
               .Page(queryInfo);
            }
            else
            {
                List<PinFen> ListPinFen = this.GetSession().Linq<PinFen>().Where(u=>u._user.ID==Convert.ToInt64(UserID)).ToList();
                List<ShopWenTi>ListShopWenTi=new List<ShopWenTi>();
                //ListShopWenTi=this.GetAll().ToList();
                foreach(var PinFen in ListPinFen)
                {
                    IEnumerable<ShopWenTi> ShopWenTi = this.GetSession().Linq<ShopWenTi>().Where(u => u._Shop.ID == PinFen._shop.ID);
                    foreach(var WenTi in ShopWenTi)
                    {
                        ListShopWenTi.Add(WenTi);
                    }
                }
                ListShopWenTi=ListShopWenTi.WhereIf(p => p._Shop.Name.Contains(shopID), shopID.IsNotNullAndEmpty()).ToList();
                ListShopWenTi=ListShopWenTi.WhereIf(p => p.NContent.Contains(Ncontent), Ncontent.IsNotNullAndEmpty()).ToList();
                ListShopWenTi = ListShopWenTi.WhereIf(p => p.wtType == wtType.ToString().ToEnum<WenTiType>(), wtType != null).ToList();
                return  ListShopWenTi.Page(queryInfo);
            }
        }
    }
}
