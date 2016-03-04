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
   public class UserSalaryRepository:NHibernateRepository<UserSalary,int>
   {
       #region "查询"
       public PagedData<UserSalary> GetData(QueryInfo queryInfo, string Year, string Month,string UserName)
       {
           return GetSession()
               .Linq<UserSalary>()
               // .Where(u => u.Type == "1".ToEnum<UserEnmType>())
               //.WhereIf(u => u.Type == typeId.ToString().ToEnum<UserEnmType>(), typeId != null)
               //  .WhereIf(u=>u.Type.ID==Convert.ToInt64(typeID),typeID!=null)
               .Where(u=>u.Year==Convert.ToInt64(Year))
               .Where(u => u.Month == Convert.ToInt64(Month))
               .WhereIf(u => u.User.userName.Contains(UserName.Trim()), UserName.IsNotNullAndEmpty())
               .Page(queryInfo);
       }
       public List<UserSalary> GetData(string Year, string Month)
       {
           return GetSession()
               .Linq<UserSalary>()
               .Where(u => u.Year == Convert.ToInt64(Year))
               .Where(u => u.Month == Convert.ToInt64(Month))
               .ToList();              
       }
       public List<UserSalary> GetData(string Year, string Month,string UserID)
       {
           UserRepository UserRepo = new UserRepository();
           return GetSession()
               .Linq<UserSalary>()
               .Where(u => u.Year == Convert.ToInt64(Year))
               .Where(u => u.Month == Convert.ToInt64(Month))
               .Where(u=>u.User==UserRepo.GetByDatabaseID(Convert.ToInt32(UserID)))
               .ToList();
       }
       #endregion
       #region "更新"
       public void updateUserSalary(string localYear, string localMonth)
       {
           List<UserSalary> ListUserSalary = this.GetData(localYear, localMonth);
           //先删除表中的记录
           //List<UserWorkDayOrNight> User
           foreach (UserSalary s in ListUserSalary)
           {
               this.Delete(s);
           }
           //再统计客服的排班情况，最后更新值班奖金
           //先插入客服的数据，再更新值班数量
           UserRepository UserRepo = new UserRepository();
           List<User> ListUser = UserRepo.GetAll()
                               .Where(p => p.Type.ID.ToString().Trim() == "1")
                               .ToList();

           PersonShopGroupPBsRepository PersonShopGroupPBRepo = new PersonShopGroupPBsRepository();
           foreach (User u in ListUser)
           {
               UserSalary LocalUserSalary = new UserSalary();
               LocalUserSalary.User = u;

               LocalUserSalary.Year = Convert.ToInt32(localYear);
               LocalUserSalary.Month = Convert.ToInt32(localMonth);

               ////统计值班数量
               //取出排班表的信息，然后统计白班晚班的信息
               List<PersonShopGroupPBs> listPersonShopGroupPBs = PersonShopGroupPBRepo.GetListPersonShopGroupPBByUser(u, localYear, localMonth);

               int DayNum = 0;
               int NightNum = 0;
               foreach (var d in listPersonShopGroupPBs)
               {
                   if (d._ShopGroups.WorkDayOrNight == DayOrNight.白班)
                   {
                       DayNum = DayNum + 1;
                   }
                   if (d._ShopGroups.WorkDayOrNight == DayOrNight.晚班)
                   {
                       NightNum = NightNum + 1;
                   }
               }

               LocalUserSalary.DayNum = DayNum;
               LocalUserSalary.NightNum = NightNum;
               LocalUserSalary.TotalNum = LocalUserSalary.DayNum + LocalUserSalary.NightNum;
               LocalUserSalary.zhiBanSalary = LocalUserSalary.TotalNum * u.UserEnmLevel.UserLevelSalary;
               LocalUserSalary.TotalSalary = LocalUserSalary.zhiBanSalary;
               this.Save(LocalUserSalary);
           }
       }
       #endregion
   }
}
