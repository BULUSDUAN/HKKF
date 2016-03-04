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

    public class PersonPBDataRepository : NHibernateRepository<PersonPBData, int>
    {
        public void updatePersonPBData(string Year,string Month,Kf_DepartMent kf_DepartMent)
        {
            PersonShopGroupPBsRepository PersonShopGroupPBRepo = new PersonShopGroupPBsRepository();
            List<PersonShopGroupPBs> listPersonShopGroupPBs = PersonShopGroupPBRepo.GetListPersonShopGroupPB(Year, Month,kf_DepartMent);

            UserRepository UserRepo = new UserRepository();
            List<User> ListUser = UserRepo.GetAll()
                                .Where(p => p.Type.ID.ToString().Trim() == "1")
                                .ToList();

            List<PersonPBData> listPersonPBData = this.GetData(Year,Month,kf_DepartMent);
            foreach (var PersonPBData in listPersonPBData)
            {
                this.Delete(PersonPBData);
            }

            var distinctWorkDate = from y in listPersonShopGroupPBs
                                   group y by new { y.UserWorkDate };
            foreach (var workDate in distinctWorkDate)
            {
                foreach (var user in ListUser)
                {
                  
                        PersonPBData PersonPBData = new PersonPBData();
                        PersonPBData.UserWorkDate = workDate.Key.UserWorkDate;
                        PersonPBData._user = user;
                        PersonPBData.Year = Year.ToInt();
                        PersonPBData.Month = Month.ToInt();
                        PersonPBData.UpdateTime = System.DateTime.Now;
                        //如果能查到是白班，那么就是白班，如果能查到是晚班，那么就是晚班，如果白班和白班都查到了，那么是全班，如果都没查到，那么是休班.
                        List<PersonShopGroupPBs> listLocalPersonShopGroupPBs = listPersonShopGroupPBs
                            .Where(it => it._User == user)
                            .Where(it => it.UserWorkDate == workDate.Key.UserWorkDate)
                            .ToList();
                        if (listLocalPersonShopGroupPBs.Count == 0)
                        {
                            PersonPBData.WorkDayOrNight = DayOrNight.休班;
                        }                       
                        else if (listLocalPersonShopGroupPBs.Count == 1)
                        {
                            if (listLocalPersonShopGroupPBs.FirstOrDefault()._ShopGroups.WorkDayOrNight == DayOrNight.白班)
                            {
                                PersonPBData.WorkDayOrNight =DayOrNight.白班;
                            }
                            if (listLocalPersonShopGroupPBs.FirstOrDefault()._ShopGroups.WorkDayOrNight == DayOrNight.晚班)
                            {
                                PersonPBData.WorkDayOrNight = DayOrNight.晚班;
                            }                           
                        }
                        else
                        {
                            PersonPBData.WorkDayOrNight = DayOrNight.全天;
                        }
                        
                        PersonPBData.DayNumMonth = 0;
                        PersonPBData.NightNumMonth = 0;
                        PersonPBData.RestNumMonth= 0;
                        this.Save(PersonPBData);                    
                }           
            }

            List<PersonPBData> listPersonPBDataNew = this.GetData(Year, Month,kf_DepartMent);
            foreach (var User in ListUser)
            {
                List<PersonPBData> listLocalPersonPBDataNew = listPersonPBDataNew.Where(it => it._user == User).ToList();
                int intUserDayNumMonth=0;
                int intUserNightNumMonth=0;
                int intUserRestNumMonth=0;
                foreach (var personPBData in listLocalPersonPBDataNew)
                {
                    if (personPBData.WorkDayOrNight == DayOrNight.休班)
                    {
                        intUserRestNumMonth = intUserRestNumMonth + 1;
                    }
                    if (personPBData.WorkDayOrNight == DayOrNight.全天)
                    {
                        intUserDayNumMonth = intUserDayNumMonth + 1;
                        intUserNightNumMonth = intUserNightNumMonth + 1;
                    }
                    if (personPBData.WorkDayOrNight == DayOrNight.白班)
                    {
                        intUserDayNumMonth = intUserDayNumMonth + 1;                   
                    }
                    if (personPBData.WorkDayOrNight == DayOrNight.晚班)
                    {
                        intUserNightNumMonth = intUserNightNumMonth + 1;
                    }
                }
                foreach (var personPBData in listLocalPersonPBDataNew)
                {
                    //PersonPBData pbData = this.GetByDatabaseID(personPBData.ID);
                    personPBData.DayNumMonth = intUserDayNumMonth;
                    personPBData.NightNumMonth = intUserNightNumMonth;
                    personPBData.RestNumMonth = intUserRestNumMonth;
                    this.Save(personPBData);
                }
            }          
        }
        public PagedData<PersonPBData> GetData(QueryInfo queryInfo, string Year, string Month, string UserName,Kf_DepartMent kf_DepartMent)
        {
            return GetSession()
                .Linq<PersonPBData>()           
                .Where(u => u.Year == Convert.ToInt64(Year))
                .Where(u => u.Month == Convert.ToInt64(Month))
                .WhereIf(u=>u._user.DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
                .WhereIf(u => u._user.userName.Contains(UserName.Trim()), UserName.IsNotNullAndEmpty())
                .OrderBy(u=>u.UserWorkDate)
                .Page(queryInfo);
        }
        public PagedData<PersonPBData> GetData(QueryInfo queryInfo, string Year, string Month, User user)
        {
            return GetSession()
                .Linq<PersonPBData>()
                .Where(u => u.Year == Convert.ToInt64(Year))
                .Where(u => u.Month == Convert.ToInt64(Month))
                .Where(u => u._user==user)
                .OrderBy(u => u.UserWorkDate)
                .Page(queryInfo);
        }
        public List<PersonPBData> GetData(string Year, string Month,Kf_DepartMent kf_DepartMent)
        {
            return GetSession()
                .Linq<PersonPBData>()
                .Where(u => u.Year == Convert.ToInt64(Year))
                .Where(u => u.Month == Convert.ToInt64(Month))       
                .WhereIf(u=>u._user.DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
                .ToList();
        }
    }
}
