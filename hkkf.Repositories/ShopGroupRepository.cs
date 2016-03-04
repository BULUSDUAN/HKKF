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
    public class ShopGroupRepository : NHibernateRepository<ShopGroups, int>
    {
       
        public PagedData<ShopGroups> GetShopGroups(QueryInfo queyInfo, string ShopGroupName,int kf_DepartMentID)
        {
            return GetSession().Linq<ShopGroups>()
                .WhereIf(p => p.ShopGroupName.Contains(ShopGroupName.Trim()), ShopGroupName.IsNotNullAndEmpty())
                .WhereIf(u => u._Kf_DepartMent.ID == kf_DepartMentID, kf_DepartMentID != 1)
                .Page(queyInfo);
        }
        //用于选模板中包含哪些班组，班组的内容
        public IEnumerable<ShopGroups> GetData(int kf_DepartMentID)
        {
            return GetSession()
                .Linq<ShopGroups>()
                //.Where(u => u.s == ShopStates.正常服务)
                .WhereIf(u => u._Kf_DepartMent.ID == kf_DepartMentID, kf_DepartMentID != 1);
        }
        public bool ExistShopGroupsName(string Name)
        {
            return GetSession().Linq<ShopGroups>()
               .Where(u => u.ShopGroupName == Name).Any();
        }


        public string checkShopGroupValid()
        {
            ShopGroupDetailRepository shopGroupDetailsRepo = new ShopGroupDetailRepository();
            ShopRepository shopRepo = new ShopRepository();
            //检查班组是否配备齐全，把所有的店铺都囊括进去了，并且安排的班组有效。
            List<ShopGroups> listShopGroup = this.GetAll().ToList();
            List<ShopGroupDetails> listShopGroupDetails = shopGroupDetailsRepo.GetAll().ToList();
            List<Shop> listShop = shopRepo.GetAll().Where(it => it.ShopStateID == ShopStates.正常服务).ToList();

            string strResult = "";

            //(1)首先检查所有的店铺是否在班组中，如果有不在的，那么就退出，提示哪个店铺没有在班组中。
            foreach (var shop in listShop)
            {
                List<ShopGroupDetails> localShopGroupDetails = listShopGroupDetails.Where(it => it._Shop == shop).ToList();
                List<ShopGroups> localShopGroupList = localShopGroupDetails.Select(it => it._ShopGroup).ToList();

                if (localShopGroupDetails.Count() == 0)
                {
                    strResult = "店铺" + shop.Name + "没有分配班组，请分配后再检查";
                    return strResult;
                }
                //这个店铺分配了几个班组，如果班组数量小于店铺中设置的组数数量，那么是不对的，退出,白班晚班要分开统计.
                if (localShopGroupDetails.Count < shop.GroupCount)
                {
                    strResult = "店铺" + shop.Name + "的组数为" + shop.GroupCount.ToString().Trim() + ",班组数量为" + localShopGroupDetails.Count.ToString().Trim() + "，班组数量少于店铺要求的组数，请分配后再检查";
                    return strResult;
                }
                //

                switch (shop.ZhiBanTypeID)
                {
                    case ZhiBanType.全托:
                        //检查是否有白班和夜班的班组，如果有一个没有的，那么就退出
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是全托，但是白班没有分配班组，请分配后再检查";
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是全托，但是晚班没有分配班组，请分配后再检查";
                            return strResult;
                        }
                        break;
                    case ZhiBanType.仅白班:
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅白班，但是白班没有分配班组，请分配后再检查";
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() != 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅白班，但是晚班分配了班组" + localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).FirstOrDefault().ToString() + "，请检查";
                            return strResult;
                        }
                        break;
                    case ZhiBanType.仅夜班:
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() != 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班，但是分配了班组" + localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).FirstOrDefault().ToString() + "，请检查";
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班，但是晚班没有分配班组，请分配后再检查";
                            return strResult;
                        }
                        break;
                    case ZhiBanType.周末:
                         if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是周末，但是白班没有分配班组，请分配后再检查";
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是周末，但是晚班没有分配班组，请分配后再检查";
                            return strResult;
                        }
                        break;
                    case ZhiBanType.夜班加周末:
                          if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() != 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班加周末，但是分配了班组" + localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).FirstOrDefault().ToString() + "，请检查";
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班加周末，但是晚班没有分配班组，请分配后再检查";
                            return strResult;
                        }
                        break;
                    default: break;
                }

            }
            strResult = "班组有效";
            return strResult;
        }
        //public bool ExistShopDifficultyLevelID(int id)
        //{
        //    return GetSession().Linq<ShopGroups>()
        //       .Where(u => u.ID ==id).Any(); 
        //}
    }
}
