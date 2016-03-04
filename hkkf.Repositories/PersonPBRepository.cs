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
    
    public class PersonPBRepository : NHibernateRepository<PersonPB, int>
    {

        #region "根据班组和日期，自动更新店铺的排班数据"
       //根据班组排班表中的内容，更新PersonPB里的数据
        public void PaiBan(DateTime localStartDate, DateTime localEndDate,Kf_DepartMent kf_DepartMent)
        {     
            List<PersonPB> listPersonPB = this.GetAll()
              .Where(it => it.UserWorkDate.WorkDate.Date <= localEndDate.Date)
              .Where(it => it.UserWorkDate.WorkDate.Date >= localStartDate.Date)
              .WhereIf(it=>it._Shop._Kf_DepartMent==kf_DepartMent,kf_DepartMent.ID!=1)
              .ToList();
            foreach (var PB in listPersonPB)
            {
                this.Delete(PB);
            }

            ShopGroupDetailRepository shopGroupDetailRepo = new ShopGroupDetailRepository();//
            PersonShopGroupPBsRepository personShopGroupPBsRepo = new PersonShopGroupPBsRepository();

            List<PersonShopGroupPBs> listPersonShopGroupPBs = personShopGroupPBsRepo.GetAll()
                 .Where(it => it.UserWorkDate.WorkDate.Date <= localEndDate.Date)
                .Where(it => it.UserWorkDate.WorkDate.Date >= localStartDate.Date)
                 .WhereIf(it => it._ShopGroups._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .ToList();

            List<ShopGroupDetails> listShopGroupDetails = shopGroupDetailRepo.GetAll()
                 .WhereIf(it => it._Shop._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .ToList();
            foreach (var personShopGroupPBs in listPersonShopGroupPBs)
            {
                   List<Shop> listShop = listShopGroupDetails
                       .Where(it => it._ShopGroup == personShopGroupPBs._ShopGroups)
                       .Select(it => it._Shop)
                       .ToList();
                    foreach (var shop in listShop)
                    {
                        PersonPB pb = new PersonPB();
                        pb._Shop = shop;
                        pb._user = personShopGroupPBs._User;
                        pb.UserWorkDate = personShopGroupPBs.UserWorkDate;
                        pb.WorkDayOrNight = personShopGroupPBs._ShopGroups.WorkDayOrNight;
                        this.Save(pb);
                    }                   
            }        
        }
        #endregion

        //根据日期取出LIST<PERSONPBS>
        public List<PersonPB> GetListPersonPBByDate(UserWorkDate workDate)
        {
            var list = GetSession()
                .Linq<PersonPB>()
                .Where(u => u.UserWorkDate==workDate)
                //.WhereIf(u => u.shopID.Split(','), ShopName.IsNotNullAndEmpty())
                // .WhereIf(u => u._DateTime >= startDate, startDate != null)
                // .WhereIf(u => u._DateTime <= endDate, endDate != null)
                .ToList();

            //.Page(queryInfo);
            //List<PersonPB> newList = new List<PersonPB>();
            return list;
        }
        public List<PersonPB> GetListPersonPBByDateAndUser(UserWorkDate workDate,String UserID)
        {
            UserRepository UserRepo = new UserRepository();
            User user = UserRepo.GetByDatabaseID(Convert.ToInt32(UserID));
            var list = GetSession()
                .Linq<PersonPB>()
                .Where(u => u.UserWorkDate == workDate)
                .Where(u=>u._user==user)
                //.WhereIf(u => u.shopID.Split(','), ShopName.IsNotNullAndEmpty())
                // .WhereIf(u => u._DateTime >= startDate, startDate != null)
                // .WhereIf(u => u._DateTime <= endDate, endDate != null)
                .ToList();
            //.Page(queryInfo);
            //List<PersonPB> newList = new List<PersonPB>();
            return list;
        }
        //根据店铺取出该店铺的排班信息List<PersonPBs>
        public List<PersonPB> GetListPersonPBByShop(Shop shop,DateTime startDate,DateTime endDate)
        {
            var list = GetSession()
                .Linq<PersonPB>()
                .Where(u => u._Shop == shop)
                .WhereIf(u => u.UserWorkDate.WorkDate >= startDate, startDate != null)
                .WhereIf(u => u.UserWorkDate.WorkDate <= endDate, endDate != null)
                .ToList();

            //.Page(queryInfo);
            //List<PersonPB> newList = new List<PersonPB>();
            return list;
        }
        //根据客服人员取出该人员的班组的的排班信息List<PersonPBs>
        public List<PersonPB> GetListPersonPBByUser(User User, DateTime startDate, DateTime endDate)
        {
            var list = GetSession()
                .Linq<PersonPB>()
                .Where(u => u._user == User)
                //.WhereIf(u => u.shopID.Split(','), ShopName.IsNotNullAndEmpty())
                .WhereIf(u => u.UserWorkDate.WorkDate >= startDate, startDate != null)
                .WhereIf(u => u.UserWorkDate.WorkDate <= endDate, endDate != null)
                .ToList();

            //.Page(queryInfo);
            //List<PersonPB> newList = new List<PersonPB>();
            return list;
        }
        //根据客服人员取出该店铺的排班信息List<PersonPBs>
        public List<PersonPB> GetListPersonPBByUserTongJiPaiBan(User User, DateTime startDate, DateTime endDate)
        {
            var list = GetSession()
                .Linq<PersonPB>()
                .Where(u => u._user == User)
                //.WhereIf(u => u.shopID.Split(','), ShopName.IsNotNullAndEmpty())
                .WhereIf(u => u.UserWorkDate.WorkDate >= startDate, startDate != null)
                .WhereIf(u => u.UserWorkDate.WorkDate <= endDate, endDate != null)
                .ToList();

            //.Page(queryInfo);
            //List<PersonPB> newList = new List<PersonPB>();
            return list;
        }

        List<tongjipb> tongjilist = new List<tongjipb>();
        public PagedData<PersonPB> GetPagedData(QueryInfo queryInfo, string ShopName, DateTime startDate, DateTime endDate,Kf_DepartMent kf_DepartMent)
        {
            return GetSession()
                .Linq<PersonPB>()
                .WhereIf(u => u.UserWorkDate.WorkDate >= startDate, startDate != null)
                .WhereIf(u => u.UserWorkDate.WorkDate <= endDate, endDate != null)
                .WhereIf(u => u._Shop.Name.Contains(ShopName.Trim()), ShopName.IsNotNullAndEmpty())
                .WhereIf(u => u._Shop._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
               .Page(queryInfo);         
        }
        public PagedData<PersonPB> GetPagedData(QueryInfo queryInfo, string ShopID, DateTime startDate, DateTime endDate)
        {
            return GetSession()
                .Linq<PersonPB>()
                .WhereIf(u => u.UserWorkDate.WorkDate >= startDate, startDate != null)
                .WhereIf(u => u.UserWorkDate.WorkDate <= endDate, endDate != null)
                .Where(u => u._Shop.ID==Convert.ToInt32(ShopID))
                .Page(queryInfo);
        }


        //查询不是在同一家店铺的是否超过6次
        public bool IsAnyBetweenTime(DateTime start, DateTime end, int userid)
        {
            var countList = GetSession().Linq<PersonPB>()
               // .Where(p => p._DateTime >= start && p._DateTime <= end)
                .Where(p => p._user.ID == userid)
                .ToList();
            if (countList.Count() != 0)
            {
                var expr = from p in countList
                          // group p by new { p._DateTime, p.DayOrNight } into g
                           group p by new { p.UserWorkDate,p.WorkDayOrNight } into g
                           select new
                           {
                               key = g.Key,
                               count = g.Count()
                           };
                
              //  if (expr.Count() > 5)
                {
                    int countlin = 0;
                    int index = 0;
                    foreach (var item in expr)
                    {
                        index++;
                        if (index==5)
                        {
                    //        countlin = item.count;
                        }
                    }
                    int index1 = 0;
                    foreach (var item in expr)
                    {
                        index1++;
                        if (index1==6)
                        {
                            if (item.count == countlin)
                            {
                                return true;
                            }
                        }
                       

                    }
                   
                }
                

            }
            return false;

        }

        //查询此人在这段时间内是否值过白班/晚班
        public bool IsBai(DateTime start, DateTime end, int userid)
        {
            var list = GetSession().Linq<PersonPB>()
               // .Where(p => p._user.ID == userid && p._DateTime >= start && p._DateTime <= end && p.DayOrNight == Ban.白班)
                .ToList();
            if (list.Count>0)
            {
                return true;
            }
            return false;

        }
        public bool IsWan(DateTime start, DateTime end, int userid)
        {
            var list = GetSession().Linq<PersonPB>()
               // .Where(p => p._user.ID == userid && p._DateTime >= start && p._DateTime <= end && p.DayOrNight == Ban.晚班)
                .ToList();
            if (list.Count > 0)
            {
                return true;
            }
            return false;

        }

        //去除重复
        public static List<int> RemoveRepeat(int[] strRepeat)
        {
            List<int> list = new List<int>();//定义一个泛型用来装数组的元素
            foreach (int str in strRepeat)//foreach循环出 数组的元素
            {
                if (list.Contains(str) == false && !string.IsNullOrEmpty(str.ToString()))//list.Contains(str)判断list中是否有相同的元素，list.Contains(str) == false当不同时为true
                    list.Add(str);//把数组中的不相同元素添加到list中
            }
            return list;//返回list，list中的值就是所要得到的结果
        }

        //查询最大的ID
        public int MaxID(int userid)
        {
            return GetSession().Linq<PersonPB>()
                .Where(p => p._user.ID == userid)
                .OrderByDescending(p => p.ID)
                .FirstOrDefault()
                .ID;
        }

        public bool IsAnyCreateTime(DateTime starTime)
        {
            return GetSession().Linq<PersonPB>()
               // .Where(p => p._DateTime == starTime)
                .Any();
        }


        public List<tongjipb> GetlistByTongji(DateTime? starTime, DateTime? EndTime)
        {

            DateTime date = starTime.Value;
            for (int i = 1; i <= 7; i++)
            {
                #region 累加一天

                if (i == 1)
                {
                    date = starTime.Value;
                }
                else if (i == 2)
                {
                    date = starTime.Value.AddDays(1);
                }
                else
                {
                    date = starTime.Value.AddDays(i - 1);
                }

                #endregion
                tongjipb tj = new tongjipb();
                tj.id = i;
                tj._date = date;
                tj.Week = CaculateWeekDay(tj._date.Year, tj._date.Month, tj._date.Day);
                List<string> Bperson = new List<string>();
                var plist = GetSession().Linq<PersonPB>()
                   // .Where(p => p._DateTime == date)
                   // .Where(p => p.DayOrNight == Ban.白班)
                    .ToList()
                    .Select(p => p._user);
                foreach (var user in plist)
                {
                    if (!Bperson.Contains(user.userName))
                    {
                        Bperson.Add(user.userName);
                    }
                }
                tj.DayPersons = Bperson;

                List<string> Wperson = new List<string>();
                var wplist = GetSession().Linq<PersonPB>()
                   // .Where(p => p._DateTime == date)
                   // .Where(p => p.DayOrNight == 1)
                    .ToList()
                    .Select(p => p._user);
                foreach (var user in wplist)
                {
                    if (!Wperson.Contains(user.userName))
                    {
                        Wperson.Add(user.userName);
                    }
                }
                tj.NightPersons = Wperson;

                tongjilist.Add(tj);

            }
            return tongjilist;
        }

        public static string CaculateWeekDay(int y, int m, int d)
        {
            if (m == 1 || m == 2)
            {
                m += 12;
                y--;         //把一月和二月看成是上一年的十三月和十四月，例：如果是2004-1-10则换算成：2003-13-10来代入公式计算。
            }
            int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
            string weekstr = "";
            switch (week)
            {
                case 0: weekstr = "星期一"; break;
                case 1: weekstr = "星期二"; break;
                case 2: weekstr = "星期三"; break;
                case 3: weekstr = "星期四"; break;
                case 4: weekstr = "星期五"; break;
                case 5: weekstr = "星期六"; break;
                case 6: weekstr = "星期日"; break;
            }
            return weekstr;
        }
        //.DayOfWeek.根据英文的周几返回中文的周几
        public string GetWeekDayChina(System.DayOfWeek dayOfWeek)
        {
            
            string weekstr = "";           
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: weekstr = "星期一"; break;
                case DayOfWeek.Tuesday: weekstr = "星期二"; break;
                case DayOfWeek.Wednesday: weekstr = "星期三"; break;
                case DayOfWeek.Thursday: weekstr = "星期四"; break;
                case DayOfWeek.Friday: weekstr = "星期五"; break;
                case DayOfWeek.Saturday: weekstr = "星期六"; break;
                case DayOfWeek.Sunday: weekstr = "星期日"; break;
            }
            return weekstr;
        }

        //根据日期、白班、次序查询姓名
        public List<string> PNameByddc(List<tongjipb> tongjilist, DateTime date, int id)
        {
            var str = tongjilist
                .Where(p => p.id == id)
                .Where(p => p._date == date)
                .ToList()
                .Select(p => p.DayPersons)
                .FirstOrDefault();


            return str;
        }
        //根据日期、晚班、次序查询姓名
        public List<string> PNameBywdc(List<tongjipb> tongjilist, DateTime date, int id)
        {
            return tongjilist
                .Where(p => p.id == id)
                .Where(p => p._date == date)
                .ToList()
                .Select(p => p.NightPersons)
                .FirstOrDefault();
        }

        //根据日期、姓名、白班查出所负责的店铺
        public string PShop(DateTime date, string Name, int bw)
        {
            string shop = null;
            var list = GetSession().Linq<PersonPB>()
                .Where(p => p._user.userName == Name)
               // .Where(p => p._DateTime == date)
               // .WhereIf(p => p.DayOrNight == Ban.白班, bw == 1)
               // .WhereIf(p => p.DayOrNight == Ban.晚班, bw == 2)
                .ToList();
            foreach (var personPb in list)
            {

                shop += (personPb._Shop.Name + ",");

            }
            return shop;
        }
        //查询当天此人是否排过班
        public bool IsPBByUserID(DateTime dayTime, int userid, DayOrNight ban)
        {
            return GetSession().Linq<PersonPB>()
               // .Where(p => p._user.ID == userid && p._DateTime == dayTime && p.DayOrNight == ban)
                .Any();

        }

        //根据日期和用户查询排班记录
        public List<PersonPB> GetDayPbByuserAndDate(int userid, DateTime date)
        {
            return GetSession().Linq<PersonPB>()
                //.Where(p => p._user.ID == userid && p._DateTime == date)
                .ToList();
        }

        //根据日期和用户查询排班记录
        public List<PersonPB> GetBightPbByuserAndDate(int userid, DateTime date)
        {
            return GetSession().Linq<PersonPB>()
               // .Where(p => p._user.ID == userid && p._DateTime == date && p.DayOrNight == Ban.晚班)
                .ToList();
        }

        //查询根据用户集合
        public List<PersonPB> GetPSListByUserID(int userid)
        {
            return GetSession().Linq<PersonPB>()
                .Where(p => p._user.ID == userid)
                .ToList();
        }

        //根据日期查询此人为一条的记录
        public int GetMaxData(DateTime start, DateTime end, int userid)
        {
            return GetSession().Linq<PersonPB>()
                 .Where(p => p._user.ID == userid)
                // .Where(p => p._DateTime >= start && p._DateTime <= end)
                 .Max(p => p.ID);
        }
    }
}
