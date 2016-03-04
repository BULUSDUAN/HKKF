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

    public class PersonShopGroupPBsRepository : NHibernateRepository<PersonShopGroupPBs, int>
    {
        #region "查询"
        public PagedData<PBDateTemplet> GetData(QueryInfo queyInfo, DateTime localStartDate, DateTime localEndDate, Kf_DepartMent kf_DepartMent)
        {
            return GetSession().Linq<PBDateTemplet>()
                .Where(it => it._UserWorkDate.WorkDate >= localStartDate)
                 .Where(it => it._UserWorkDate.WorkDate <= localEndDate)
                .WhereIf(u => u._ShopTemplet._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .OrderBy("_UserWorkDate", false)
                .Page(queyInfo);
        }
        //
        public PagedData<PersonShopGroupPBs> GetData(QueryInfo queyInfo,string UserName ,DateTime localStartDate, DateTime localEndDate, Kf_DepartMent kf_DepartMent)
        {
            return GetSession().Linq<PersonShopGroupPBs>()
                .Where(it => it.UserWorkDate.WorkDate >= localStartDate)
                 .Where(it => it.UserWorkDate.WorkDate <= localEndDate)
                .WhereIf(u => u._User.userName.Contains(UserName.Trim()), UserName.IsNotNullAndEmpty())
                .WhereIf(u => u._User.DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .OrderBy(u=>u.UserWorkDate)
                .Page(queyInfo);
        }
        //查询出某一部门的某一个时间段的排班信息
        public List<PersonShopGroupPBs> GetData(Kf_DepartMent kf_DepartMent, DateTime localStartDate, DateTime localEndDate)
        {
            return GetSession().Linq<PersonShopGroupPBs>()
                .Where(it => it.UserWorkDate.WorkDate >= localStartDate)
                .Where(it => it.UserWorkDate.WorkDate <= localEndDate)
                .WhereIf(it=>it._ShopGroups._Kf_DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
                .OrderBy(it => it.UserWorkDate)
                .ToList();
        }
        //查询出某一个人的某一个时间段的排班信息
        public PagedData<PersonShopGroupPBs> GetData(QueryInfo queyInfo, User User, DateTime localStartDate, DateTime localEndDate)
        {
            return GetSession().Linq<PersonShopGroupPBs>()
                .Where(it => it.UserWorkDate.WorkDate >= localStartDate)
                .Where(it => it.UserWorkDate.WorkDate <= localEndDate)
                .Where(u => u._User==User)             
                .OrderBy(u => u.UserWorkDate)
                .Page(queyInfo);
        }

        //根据客服人员取出该客服的班次信息List<PersonShopGroupPBs>
        public List<PersonShopGroupPBs> GetListPersonShopGroupPBByUser(User User, string Year, string Month)
        {
            var list = GetSession()
                .Linq<PersonShopGroupPBs>()
                .Where(u => u._User == User)
                .Where(u => u.UserWorkDate.Year==Year.ToInt())
                 .Where(u => u.UserWorkDate.Month == Month.ToInt())
                //   .OrderBy("UserWorkDateID", false)
                .ToList();
            return list;
        }
        //根据客服人员取出该客服的班次信息List<PersonShopGroupPBs>
        public List<PersonShopGroupPBs> GetListPersonShopGroupPB(string Year, string Month,Kf_DepartMent kf_DepartMent)
        {
            var list = GetSession()
                .Linq<PersonShopGroupPBs>()
                .Where(u => u.UserWorkDate.Year == Year.ToInt())
                 .Where(u => u.UserWorkDate.Month == Month.ToInt())
                 .WhereIf(u=>u._User.DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
                //.OrderBy("UserWorkDateID", false)
                .ToList();
            return list;
        }
        //按理说应该根据模板，如果部门确定了，日期确定了，那么模板也就确定了，所以根据这两个也可以
        public List<PersonShopGroupPBs> GetListPersonShopGroupPB(Kf_DepartMent _kf_DepartMent,UserWorkDate workDate)
        {
            var list = GetSession()
                .Linq<PersonShopGroupPBs>()
                .Where(u => u.UserWorkDate == workDate)                
                .Where(u=>u._ShopGroups._Kf_DepartMent==_kf_DepartMent)
                .OrderBy(u => u.WorkDayOrNight)
                .ToList();
            return list;
        }
        #endregion
        #region "排班"
        //根据PBDateTemplet，模板排班里面的内容，更新班组排班表PersonShopGroupPBs
        public void PaiBan(DateTime localStartDate, DateTime localEndDate,Kf_DepartMent kf_DepartMent)
        {
            ShopTempletDetailsRepository shopTempletDetailsRepo = new ShopTempletDetailsRepository();
            PBDateTempletRepository pbDateTempletRepo = new PBDateTempletRepository();
                      
            List<PersonShopGroupPBs> listPersonShopGroupPBs = this.GetAll()
                .Where(it => it.UserWorkDate.WorkDate <= localEndDate)
                .Where(it => it.UserWorkDate.WorkDate >= localStartDate)              
                .WhereIf(it => it._ShopGroups._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .ToList();
            //删除原来的
            foreach (var PersonShopGroupPB in listPersonShopGroupPBs)
            {
                this.Delete(PersonShopGroupPB);
            }         
            //取出来哪些shopGroup，插入进去            
            List<ShopTempletDetails> listShopTempletDetails = shopTempletDetailsRepo.GetAll()
                .Where(it => it._ShopTemplet._Kf_DepartMent == kf_DepartMent)
                .Where(it => it._ShopTemplet.isExpire == isExpire.未过期)
                .Where(it => it._ShopTemplet.isValid == isValid.有效)
                .ToList();

            List<PBDateTemplet> listPBDateTemplet=pbDateTempletRepo.GetAll()
                .Where(it => it._UserWorkDate.WorkDate <= localEndDate)
                .Where(it => it._UserWorkDate.WorkDate >= localStartDate)
                .ToList();
       

            foreach (var PBDateTemplet in listPBDateTemplet)
            {
                List<ShopTempletDetails> localListShopTempletDetails = listShopTempletDetails
                    .Where(it => it._ShopTemplet == PBDateTemplet._ShopTemplet).ToList();

                foreach (var ShopTempletDetails in localListShopTempletDetails)
                {
                    //根据ShopGroup，进行PersonShopGroupPBs的排班插入
                    this.ShopGroupPB(ShopTempletDetails._ShopGroup, PBDateTemplet._UserWorkDate, kf_DepartMent);
                }              
            }
        }     
        //如果是手工排班需要不需要检查里面有无排班，如果有排班的数据，那么就不重新全部删除，而是保留一部分原来的排班的数据
        public void PaiBanHandChange(PBDateTemplet PbDateTempletOld,ShopTemplet shopTempletNew,Kf_DepartMent kf_DepartMent)
        {
            ShopTempletDetailsRepository shopTempletDetailsRepo = new ShopTempletDetailsRepository();
            PBDateTempletRepository pbDateTempletRepo = new PBDateTempletRepository();

               
            List<ShopTempletDetails> listShopTempletDetailsOld = shopTempletDetailsRepo.GetAll()
                .Where(it => it._ShopTemplet == PbDateTempletOld._ShopTemplet)
                .WhereIf(it=>it._ShopGroup._Kf_DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
                .ToList();

            List<ShopTempletDetails> listShopTempletDetailsNew = shopTempletDetailsRepo.GetAll()
               .Where(it => it._ShopTemplet == shopTempletNew)
               .WhereIf(it => it._ShopGroup._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
               .ToList();

            //在老的里面有，新的里面也有的，不动。
            List<PersonShopGroupPBs> listPersonShopGroupPBs = this.GetAll()
               .Where(it => it.UserWorkDate==PbDateTempletOld._UserWorkDate)
               .WhereIf(it=>it._ShopGroups._Kf_DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
               .ToList();
            //在老的里面有，新的里面没有的，删除掉。
            foreach(var PersonShopGroupPB in listPersonShopGroupPBs)
            {
                if (listShopTempletDetailsNew.Where(it => it._ShopGroup == PersonShopGroupPB._ShopGroups).Count() == 0)
                {
                    this.Delete(PersonShopGroupPB);
                }             
            }
            //在老的里面没有，新的里面有的，插入进去。
            //都是哪些人可以做这个班组的客服
            PersonShopGroupRepository PersonShopGroupRepo = new PersonShopGroupRepository();
            List<PersonShopGroup> listPersonShopGroup = PersonShopGroupRepo.GetAll().ToList();
            foreach (var shopGroupDetail in listShopTempletDetailsNew)
            {
                if (listPersonShopGroupPBs.Where(it => it._ShopGroups == shopGroupDetail._ShopGroup).Count() == 0)
                {
                    //根据排班来插入店铺
                    this.ShopGroupPB(shopGroupDetail._ShopGroup, PbDateTempletOld._UserWorkDate,kf_DepartMent);                                            
                }
            }
        }
        //根据ShopGroup，进行PersonShopGroupPBs的排班插入
        public void ShopGroupPB(ShopGroups ShopGroup,UserWorkDate WorkDate,Kf_DepartMent kf_DepartMent)
        {                 
           if (ShopGroup.WorkDayOrNight != DayOrNight.全天)
            {
                PersonShopGroupPBs personShopGroupPBs = new PersonShopGroupPBs();
                personShopGroupPBs.WorkDayOrNight = ShopGroup.WorkDayOrNight;
                personShopGroupPBs.UserWorkDate = WorkDate;
                personShopGroupPBs.UpdateTime = System.DateTime.Now.Date;
                personShopGroupPBs._ShopGroups = ShopGroup;    
                personShopGroupPBs._User = this.getAvailableUser(ShopGroup,personShopGroupPBs.WorkDayOrNight,WorkDate);
                this.Save(personShopGroupPBs);
            }
            else
            {
                for (int t = 0; t < 2; t++)
                {
                    PersonShopGroupPBs personShopGroupPBs = new PersonShopGroupPBs();
                    if (t == 0)
                    {
                        personShopGroupPBs.WorkDayOrNight = DayOrNight.白班;
                    }
                    else
                    {
                        personShopGroupPBs.WorkDayOrNight = DayOrNight.晚班;
                    }
                    personShopGroupPBs.UserWorkDate = WorkDate;
                    personShopGroupPBs.UpdateTime = System.DateTime.Now.Date;
                    personShopGroupPBs._ShopGroups = ShopGroup;
                    personShopGroupPBs._User = this.getAvailableUser(ShopGroup,personShopGroupPBs.WorkDayOrNight, WorkDate);              
                    this.Save(personShopGroupPBs);
                }
            }
        }
        //根据排班的内容返回可以排班的人员
        private User getAvailableUser(ShopGroups ShopGroup,DayOrNight dayOrNight, UserWorkDate WorkDate)
        {
            //取出都是哪些人能做这个班组。
            //目前只安排了第一个值班的人员，接下来要考虑的问题：
            //(1)不安排连班，如果该人员已经值该天的白班了，那么该人员不安排晚班了。
            //（2）不安排重复的值班类型的班组，也就是如果值一个白班班组了，那么就不能再排其他班组的白班。如果已经值一个晚班的班组，那么也不能排其他晚班的班组了。
            //（3）休班的问题怎么解决？

            PersonShopGroupRepository PersonShopGroupRepo = new PersonShopGroupRepository();
            List<PersonShopGroup> localListPersonShopGroup = PersonShopGroupRepo.GetAll()
                .Where(it => it._ShopGroups == ShopGroup)
                .OrderBy(it=>it._User.UserStateID)
                .ToList();         

            User userTemp = new User();
            //找该日期的值班类型有无该人，如果有，那么就不排，排下一个人。
            Boolean find = false;
            for (int i = 0; i <= localListPersonShopGroup.Count - 1; i++)
            {
            
                //找该日期的值班有无该人，如果有，那么就不排，排下一个人。
                if (this.checkRepeatDayOrNight(localListPersonShopGroup.ElementAt(i)._User,WorkDate) == false)
                {
                    //  if (this.checkAllToday(localListPersonShopGroup.ElementAt(i)._User, personShopGroupPBs.UserWorkDate) == false)
                    //  {
                    userTemp = localListPersonShopGroup.ElementAt(i)._User;                   
                    find = true;
                    return userTemp;                    
                    //  }                          
                }
            }
            if (find == false)//没有找到这样的,就用第一个
            {
                //localListPersonShopGroup.ElementAt(i)._User;
                userTemp = localListPersonShopGroup.FirstOrDefault()._User;
            }
            return userTemp;
        }
        //检查该用户是否有重复的白班或者晚班。当天只要是值班了，就不再安排值班了
        private Boolean checkRepeatDayOrNight(User userTemp, UserWorkDate workDate)
        {
            return GetSession().Linq<PersonShopGroupPBs>()
               .Where(it => it._User == userTemp)
               //.Where(it => it.WorkDayOrNight == DayOrNight)
               .Where(it => it.UserWorkDate == workDate)
               .Any();
        }
        //坚持该用户是否值了全天的班
        private Boolean checkAllToday(User userTemp, UserWorkDate workDate)
        {
            return GetSession().Linq<PersonShopGroupPBs>()
               .Where(it => it._User == userTemp)
               .Where(it => it.UserWorkDate == workDate)
               .ToList().Count == 2;
        }
        #endregion
    }
}
