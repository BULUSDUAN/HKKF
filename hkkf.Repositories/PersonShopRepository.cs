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

    public class PersonShopRepository : NHibernateRepository<PersonShop, int>
    {
        private ShopRepository shopRepository=new ShopRepository();
        public PagedData<PersonShop> GetPersonShopList(QueryInfo queryInfo, string name,string ShopName,DateTime? starTime,DateTime? endTime)
        {
            var list=  GetSession()
                .Linq<PersonShop>()
               // .Where(p=>p._user.Type==UserEnmType.Person)
                .WhereIf(u => u._user.userName.Contains(name.Trim()), name.IsNotNullAndEmpty())
                //.WhereIf(u => u.shopID.Split(','), ShopName.IsNotNullAndEmpty())
                .WhereIf(u=>u._DateTime>=starTime,starTime!=null)
                .WhereIf(u => u._DateTime <= endTime, endTime != null)
                .ToList();

                //.Page(queryInfo);
            List<PersonShop> newList=new List<PersonShop>();

            if (ShopName.IsNotNullAndEmpty())
            {
                foreach (var personShop in list)
                {
                    var shop= personShop.shopID.Split(',');
                    foreach (var s in shop)
                    {
                        if (ShopName==s)
                        {
                            newList.Add(personShop);
                        }
                    }
                }
            }
            if (newList.Count!=0)
            {
                return newList.Page(queryInfo);
            }
            else
            {
                return list.Page(queryInfo);
            }

        }


        public PersonShop GetPSByUserID(int userid)
        {
            return GetSession().Linq<PersonShop>()
                .Where(p=>p._user.ID==userid)
                .FirstOrDefault();
        }

        public List<PersonShop> GetPSListByUserID(int userid)
        {
            return GetSession().Linq<PersonShop>()
                .Where(p => p._user.ID == userid)
                .ToList();
        }

        public PersonShop GetPSBydate(int userid,DateTime? date)
        {
            return GetSession().Linq<PersonShop>()
                .Where(p => p._user.ID == userid)
                .WhereIf(p => p._DateTime == date, date != null)
                .FirstOrDefault();
        }

        public PagedData<PersonShop> GetListByUserID(QueryInfo queryInfo, int userid, DateTime? startDateTime, DateTime? endDateTime)
        {
            return GetSession().Linq<PersonShop>()
                .Where(p => p._user.ID == userid)
                .WhereIf(p=>p._DateTime>=startDateTime,startDateTime!=null)
                .WhereIf(p => p._DateTime <= endDateTime, endDateTime != null)
                .Page(queryInfo);
        }

        public PersonShop GetListBydatetime( int userid,DateTime? dayTime)
        {
            return GetSession().Linq<PersonShop>()
                .Where(p => p._user.ID == userid)
                .WhereIf(p => p._DateTime ==dayTime.Value.Date, dayTime != null)
                .FirstOrDefault();
        }

        public bool IsFenpei(int userid,DateTime? time)
        {
            return GetSession().Linq<PersonShop>()
                .Where(p => p._user.ID == userid && p._DateTime == time)
                .Any();
        }


        public int GetIdByShopName(string shopname)
        {
            return GetSession().Linq<Shop>()
                .WhereIf(p => p.Name == shopname, shopname.IsNotNullAndEmpty())
                .FirstOrDefault()
                .ID;
        }


        public string GetShopNameByShopID(string Shopid)
        {
            if (Shopid.IsNullOrEmpty())
            {
                return "无";
            }
            string endShopName = null;
            string shopString = Shopid.Substring(1,Shopid.Length-1);
            string[] ids = shopString.Split(',');
            int[] newids = System.Array.ConvertAll<string, int>(ids, delegate(string s) { return int.Parse(s); });

            for (int i = 0; i < newids.Length; i++)
            {
                var shop1 = shopRepository.GetByDatabaseID(newids[i]);
                endShopName += shop1.Name + "、  ";
            }

            return endShopName;
        }

    }
}
