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
    public class ShopTempletRepository : NHibernateRepository<ShopTemplet, int>
    {
        public PagedData<ShopTemplet> GetShopTemplet(QueryInfo queyInfo,string name,Kf_DepartMent kf_DepartMent)
        {
            return GetSession().Linq<ShopTemplet>()
                .WhereIf(p => p.ShopTempletName.Contains(name), name.IsNotNullAndEmpty())
                  .WhereIf(u => u._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .Page(queyInfo);
        }
        public List<ShopTemplet> GetShopTemplet(Kf_DepartMent kf_DepartMent)
        {
            return GetSession().Linq<ShopTemplet>()
               .Where(it=>it.isExpire==isExpire.未过期)
               .Where(it=>it.isValid==isValid.有效)
                .WhereIf(u => u._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .ToList();
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
        //检查有无模板，如果有，是不是可用。
        public string checkShopTempletValid(Kf_DepartMent kf_DepartMentID)
        {
            string strResult = "";
            List<ShopTemplet> listShopTemplet=this.GetAll()
                .Where(it => it.isExpire == isExpire.未过期)
                .Where(it => it.isValid == isValid.有效)
                .WhereIf(u => u._Kf_DepartMent == kf_DepartMentID, kf_DepartMentID.ID != 1)
                .ToList();
            if (listShopTemplet.Count() == 0)
            {
                strResult = "没有未过期的并且有效的模板，请检查后再排班";
                return strResult;
            }          
            foreach(var shopTemplet in listShopTemplet)
            {
                strResult = this.checkShopTempletValid(shopTemplet.ID);
                if (strResult != "模板有效")
                {
                    return strResult;
                }
            }
            return strResult;
        }

        //检查某个模板是否可用，同时更新模板的可用状态。
        //(1)过期的无所谓了(2)店铺是否都包含在了模板的班组里面(3)班组里面是否设置了客服。
        public string checkShopTempletValid(int shopTempletID)
        {
            //历史的模板就不更新了
            //检查模板里面的班组，全部班组加起来是否满足了模板当地的店铺的需求。          
        
            ShopGroupRepository shopGroupRepo = new ShopGroupRepository();
            ShopGroupDetailRepository shopGroupDetailsRepo = new ShopGroupDetailRepository();
            ShopRepository shopRepo = new ShopRepository();
                     

            ShopTemplet shopTemplet = this.GetByDatabaseID(shopTempletID);
            //如果是历史的模板，就不检查了。
            if (shopTemplet.isExpire == isExpire.过期)
            {
                return "过期的模板不需要检查";
            }
            //该模板有哪些班组
            ShopTempletDetailsRepository shopTempletDetailsRepo = new ShopTempletDetailsRepository();
            List<ShopTempletDetails> listShopTempletDetails = shopTempletDetailsRepo.GetAll()
                .Where(it=>it._ShopTemplet==shopTemplet)
                .ToList();

            //排进去的班组
            List<ShopGroups> listShopGroups = new List<ShopGroups>();
            foreach(var shopTempletDetail in listShopTempletDetails)
            {
               listShopGroups.Add(shopTempletDetail._ShopGroup);
            }

            List<ShopGroupDetails> listShopGroupDetails = shopGroupDetailsRepo.GetAll().ToList();
            List<ShopGroupDetails> listLocalShopGroupDetails = new List<ShopGroupDetails>();
            
            foreach(var shopGroup in listShopGroups)
            {
                List<ShopGroupDetails> listLocalSmallShopGroupDetails=listShopGroupDetails.Where(it=>it._ShopGroup==shopGroup).ToList();
                foreach(var shopGroupDetail in listLocalSmallShopGroupDetails)
                {
                    listLocalShopGroupDetails.Add(shopGroupDetail);
                }                
            }
            
            //实际的店铺
            List<Shop> listExistShop = shopRepo.GetAll()
                .Where(it => it.ShopStateID == ShopStates.正常服务)
                .Where(it=>it._Kf_DepartMent==shopTemplet._Kf_DepartMent)
                .ToList();
            string strResult = "";

            //(1)首先检查所有的店铺是否在班组中，如果有不在的，那么就退出，提示哪个店铺没有在班组中。
            foreach (var shop in listExistShop)
            {
                List<ShopGroupDetails> localShopGroupDetails = listLocalShopGroupDetails.Where(it => it._Shop == shop).ToList();
                //有这些店铺的班组列表，检查这些列表，看里面的东西。
                List<ShopGroups> localShopGroupList = localShopGroupDetails.Select(it => it._ShopGroup).ToList();

                if (localShopGroupDetails.Count() == 0)
                {
                    strResult = "店铺" + shop.Name + "没有分配班组，请分配后再检查";
                    shopTemplet.isValid = isValid.无效;
                    this.Update(shopTemplet);
                    return strResult;
                }
                //这个店铺分配了几个班组，如果班组数量小于店铺中设置的组数数量，那么是不对的，退出,白班晚班要分开统计.
                if (localShopGroupDetails.Count < shop.GroupCount)
                {
                    strResult = "店铺" + shop.Name + "的组数为" + shop.GroupCount.ToString().Trim() + ",班组数量为" + localShopGroupDetails.Count.ToString().Trim() + "，班组数量少于店铺要求的组数，请分配后再检查";
                    shopTemplet.isValid = isValid.无效;
                    this.Update(shopTemplet);
                    return strResult;
                }
                //

                switch (shop.ZhiBanTypeID)
                {
                    case ZhiBanType.全托:
                    //如果是全托，那么要么分配全天，要么一个白班一个晚班。    
                    //检查是否有白班和夜班的班组，如果有一个没有的，那么就退出
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.全天).Count() == 0 && 
                            (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() == 0 &&
                            localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0))
                        {
                            strResult = "店铺" + shop.Name + "是全托，但是没有分配白晚班班组，也没有分配全天班组。请分配后再检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }                      
                        break;
                    case ZhiBanType.仅白班:
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅白班，但是白班没有分配班组，请分配后再检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() != 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅白班，但是晚班分配了班组" + localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).FirstOrDefault().ToString() + "，请检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        break;
                    case ZhiBanType.仅夜班:
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() != 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班，但是分配了班组" + localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).FirstOrDefault().ToString() + "，请检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班，但是晚班没有分配班组，请分配后再检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        break;
                    case ZhiBanType.周末:
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是周末，但是白班没有分配班组，请分配后再检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是周末，但是晚班没有分配班组，请分配后再检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        break;
                    case ZhiBanType.夜班加周末:
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).Count() != 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班加周末，但是分配了班组" + localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.白班).FirstOrDefault().ToString() + "，请检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        if (localShopGroupList.Where(it => it.WorkDayOrNight == DayOrNight.晚班).Count() == 0)
                        {
                            strResult = "店铺" + shop.Name + "是仅夜班加周末，但是晚班没有分配班组，请分配后再检查";
                            shopTemplet.isValid = isValid.无效;
                            this.Update(shopTemplet);
                            return strResult;
                        }
                        break;
                    default: break;
                }

            }
            //再(判断模板中的班组中的客服是否齐全，如果客服设置不全，那么也进行提示。
              //用于检查班组是否安排客服了
            //List<ShopGroups> listShopGroups = new List<ShopGroups>();
            PersonShopGroupRepository personShopGroupRepo = new PersonShopGroupRepository();
            foreach(var shopGroup in listShopGroups)
            {
                strResult = personShopGroupRepo.checkPersonShopGroupValid(shopGroup);
                if (strResult.Trim() != "班组客服有效")
                {
                    shopTemplet.isValid = isValid.无效;
                    this.Update(shopTemplet);
                    return strResult;
                }
            }  

            strResult = "模板有效";
            shopTemplet.isValid = isValid.有效;
            this.Update(shopTemplet);
            return strResult;
        }
    }
}
