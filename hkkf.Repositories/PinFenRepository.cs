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
    public class PinFenRepository : NHibernateRepository<PinFen, int>
    {
        public PagedData<PinFen> GetPagedData(QueryInfo queryInfo, string userName, string ShopName)
        {
            return GetSession().Linq<PinFen>()
                .WhereIf(p => p._user.userName.Contains(userName), userName.IsNotNullAndEmpty())
                .WhereIf(p => p._shop.Name == ShopName,ShopName.IsNotNullAndEmpty())
                .Page(queryInfo);
        }
        //是否存在相同的店铺和姓名
        public bool IsExitByNameShopId(int userid,int shopId)
        {
            return GetSession().Linq<PinFen>()
                .Where(p => p._user.ID == userid && p._shop.ID == shopId)
                .Any();
        }

        //是否分配过此店铺
        public bool IsFPShop(int shopid)
        {
            return GetSession().Linq<PinFen>()
                .Where(p => p._shop.ID == shopid)
                .Any();
        }

        //分配过此店铺信息
        public List<PinFen> FPShopList(int shopid)
        {
            return GetSession().Linq<PinFen>()
                .Where(p => p._shop.ID == shopid)
                .ToList();
        }
        //根据店铺ID查询出所有的分配给这个店铺的人员
        public List<User> GetUserListByShopID(int shopID)
        {
            List<User> temp = new List<User>();
            List<PinFen> PinFen = this.GetSession().Linq<PinFen>()
                .Where(p => p._shop.ID == shopID)
               // .OrderBy(p=>p.Sort)
                .ToList();
            foreach (var o in PinFen)
            {
                temp.Add(o._user);
            }
            return temp;
        }
        //根据客服ID查询出所有的分配给这个人员的店铺
        public List<Shop> GetShopListByUserID(int UserID)
        {
            List<Shop> temp = new List<Shop>();
            List<PinFen> PinFen = this.GetSession().Linq<PinFen>().Where(p => p._user.ID == UserID).ToList();
            foreach (var o in PinFen)
            {
                temp.Add(o._shop);
            }
            return temp;
        }
        //根据客服ID查询出所有的分配给这个人员的分配信息
        public List<PinFen> GetPinFenListByUserID(int UserID)
        {
            List<PinFen> PinFen = this.GetSession().Linq<PinFen>()
                .Where(p => p._user.ID == UserID)
               // .OrderBy(p=>p.Sort)
                .ToList();
            return PinFen;
        }

        //查询该店铺的排序为1或者2的人员
        //public User GetUserBySort(int shopid, int sort)
        //{
        //    var pifen = GetSession().Linq<PinFen>()
        //        .WhereIf(p => p._shop.ID == shopid, shopid != null)
        //        //.WhereIf(p => p.Sort == sort, sort != null)                
        //        .FirstOrDefault();
        //    if (pifen==null)
        //    {
        //        return null;
        //    }
        //    return pifen._user;
                
        //}


    }
}
