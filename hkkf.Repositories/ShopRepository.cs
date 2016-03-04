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
   
    public class ShopRepository: NHibernateRepository<Shop, int>
    {
        PinFenRepository pinFenRepository = new PinFenRepository();
        public PagedData<Shop> GetData(QueryInfo queryInfo, int? typeId, string name,Kf_DepartMent kf_DepartMent)
        {
            return GetSession()
                .Linq<Shop>()
                .WhereIf(u => u.Name.Contains(name.Trim()), name.IsNotNullAndEmpty())
                .WhereIf(u=>u._Kf_DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
                .Page(queryInfo);
        }
        //用于选择班组内容
        public IEnumerable<Shop> GetData(int kf_DepartMentID)
        {
            return GetSession()
                .Linq<Shop>()
                .Where(u => u.ShopStateID==ShopStates.正常服务)
                .WhereIf(u => u._Kf_DepartMent.ID == kf_DepartMentID, kf_DepartMentID != 1);
        }

        //根据当前用户ID取出这个用户ID所拥有的主店铺信息。。。。
        public PagedData<Shop> GetUserMainShopByUserID(QueryInfo queryInfo, int? typeId, string name, string UserID)
        {
               List<Shop> queryShop = this.GetSession().Linq<Shop>()
                  .Where(p => p.MainKfUser.ID == Convert.ToInt64(UserID))
                  .WhereIf(p => p.Name.Contains(name),name.IsNotNullAndEmpty())
                  .ToList();
                return queryShop.Page(queryInfo);     
            // .ToList();                           
        }
        //根据当前用户ID取出这个用户ID所拥有的店铺信息。。。。
        public PagedData<PinFen> GetUserShopByUserID(QueryInfo queryInfo, int? typeId, string name,string UserID)
        {
            if (name.IsNullOrEmpty())
            {
                List<PinFen> queryPinFen = this.GetSession().Linq<PinFen>()
                .Where(p => p._user.ID == Convert.ToInt64(UserID))
                .ToList();
                return queryPinFen.Page(queryInfo);
            }
            else
            { 
              List<PinFen> queryPinFen = this.GetSession().Linq<PinFen>()
                .Where(p => p._user.ID == Convert.ToInt64(UserID))
                .Where(p => p._shop.Name.Contains(name))
                .ToList();
                 return queryPinFen.Page(queryInfo);
            }          
               // .ToList();                           
       }

        //检查一个店铺能不能排班，也就是有没有主客服、第二客服、第三客服。
        public Boolean ifShopCanPaiBan(Shop shop)
        {
            if (this.pinFenRepository.GetUserListByShopID(shop.ID).Count < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

      

        public Boolean ExistShopName(string Name)
        {
            return GetSession().Linq<Shop>()
                .Where(u => u.Name == Name).Any();            
        }


        public User GetUserByuserName(string username)
        {
            return GetSession().Linq<User>()
                .Where(u => u.userName == username)
                .FirstOrDefault();
        }

        public Shop GetShopByShopName(string shopName)
        {
            return GetSession().Linq<Shop>()
                .Where(u => u.Name.Contains(shopName))
                .FirstOrDefault();
        }
        public Shop GetShopsByShopName(string shopName)
        {
            return GetSession().Linq<Shop>()
                .Where(u => u.Name == shopName)
                .FirstOrDefault();
        }
       
    }
}
