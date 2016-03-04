using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2010.Excel;
using hkkf.Models;
using hkkf.Repositories;
using hkkf.web.Areas.Service.Common;
using JieNuo.Data;

namespace hkkf.web.Areas.Service.Controllers
{
    [NavigationRoot("客服所拥有的店铺管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class MyPaiBanController : Controller
    {
        //
        // GET: /Service/Home/
        PersonPBRepository personPbRepository = new PersonPBRepository();
        DaysDataRepository daysDataRepository=new DaysDataRepository();
        PersonPBRepository personPBRepository = new PersonPBRepository();
        PinFenRepository pinfenRepository = new PinFenRepository();
        ShopRepository shopRepository = new ShopRepository();
        UserRepository userRepository = new UserRepository();
       // DaysDataRepository daysDataRepository = new DaysDataRepository();
        WorkDateRepository WorkDateRepository = new WorkDateRepository();
       // UserRepository userRepository = new UserRepository();
       // D//aysDataRepository daysDataRepository = new DaysDataRepository();
        UserSalaryRepository UserSalaryRepository = new UserSalaryRepository();

        PersonShopGroupPBsRepository personShopGroupPBsRepo = new PersonShopGroupPBsRepository();
        public ActionResult Add_ShopTempletDetail(string id)
        {
            ShopTempletRepository shopTempletRepo = new ShopTempletRepository();
            ShopTempletDetailsRepository shopTempletDetailsRepo = new ShopTempletDetailsRepository();
            ShopGroupRepository shopGroupRepo = new ShopGroupRepository();
            //根据shopGroupID取出哪些店铺在shopGroupDetail里面，在前台显示的时候如果在里面的，那么默认为选中。
            List<ShopTempletDetails> listShopTempletDetails = shopTempletDetailsRepo.GetAll()
                .Where(it => it._ShopTemplet == shopTempletRepo.GetByDatabaseID(Convert.ToInt32(id)))
                .ToList();
            ViewData["listShopTempletDetails"] = listShopTempletDetails;
            ViewData["ShopTempletID"] = id;
            var list = shopGroupRepo.GetData(this.Users().DepartMent.ID);
            return View(list);
        }
        public ActionResult MyShopPaiBanDetail(string id)
        {
            ShopTempletRepository shopTempletRepo = new ShopTempletRepository();
            ShopTempletDetailsRepository shopTempletDetailsRepo = new ShopTempletDetailsRepository();
            ShopGroupRepository shopGroupRepo = new ShopGroupRepository();
            //根据shopGroupID取出哪些店铺在shopGroupDetail里面，在前台显示的时候如果在里面的，那么默认为选中。
            List<ShopTempletDetails> listShopTempletDetails = shopTempletDetailsRepo.GetAll()
                .Where(it => it._ShopTemplet == shopTempletRepo.GetByDatabaseID(Convert.ToInt32(id)))
                .ToList();
            ViewData["listShopTempletDetails"] = listShopTempletDetails;
            ViewData["ShopTempletID"] = id;
            var list = shopGroupRepo.GetData(this.Users().DepartMent.ID);
            return View(list);
        }
        [HttpGet]       
        public ActionResult MyShopList(QueryInfo queryInfo, int[] ids, string Name, string alertMessage, string subAction, string isKf)
        {

            int UserID = this.Users().ID;
            ViewBag.UserName = this.Users().Name;

            ViewData["ShopName"] = Name;

            PagedData<PinFen> data = shopRepository.GetUserShopByUserID(queryInfo, null, Name, this.Users().ID.ToString());
            return View(data);
        }
        public ActionResult MyPaiBanRecords(QueryInfo queryInfo, DateTime? startDate, DateTime? endDate, string subAction, FormCollection form)
        {   //取出排班客服表；
            //统计表；            
            DateTime localStartDate;
            DateTime localEndDate;
            if (startDate == null)
            {
                localStartDate = System.DateTime.Now.Date;
            }
            else
            {
                localStartDate = Convert.ToDateTime(startDate);
            }
            if (endDate == null)
            {
                localEndDate = localStartDate.AddDays(6);
            }
            else
            {
                localEndDate = Convert.ToDateTime(endDate);
            }
            ViewData["startDate"] = localStartDate;
            ViewData["endDate"] = localEndDate;
            ViewData["UserID"] = this.Users().ID.ToString().Trim();
            PagedData<PersonShopGroupPBs> data = this.personShopGroupPBsRepo.GetData(queryInfo, this.Users(), localStartDate, localEndDate);
            return View(data);
        }

        public ActionResult MySalary(QueryInfo queryInfo, string Year, string Month, string subAction)
        {   //取出排班客服表；
            //统计表；            
            string localYear;
            string localMonth;
            if (Year.IsNullOrEmpty())
            {
                localYear = System.DateTime.Now.Year.ToString().Trim();
            }
            else
            {
                localYear = Year;
            }
            if (Month.IsNullOrEmpty())
            {
                localMonth = System.DateTime.Now.Month.ToString().Trim();
            }
            else
            {
                localMonth = Month;
            }
            ViewData["Year"] = localYear;
            ViewData["Month"] = localMonth;

           
            if (subAction == "queryTongji")
            {
                //重新统计
                List<UserSalary> ListUserSalary = UserSalaryRepository.GetData(localYear, localMonth,this.Users().ID.ToString().Trim());
                //先删除表中的记录
                //List<UserWorkDayOrNight> User
                foreach (UserSalary s in ListUserSalary)
                {
                    UserSalaryRepository.Delete(s);
                }
                //再统计客服的排班情况，最后更新值班奖金
                //先插入客服的数据，再更新值班数量
                List<User> ListUser = userRepository.GetAll()
                                    .Where(p => p.Type.ID.ToString().Trim() == "1")
                                    .Where(p=>p.ID==this.Users().ID)
                                    .ToList();
                //List<UserWorkDate> localListUserWorkDate = this.WorkDateRepository.GetAll()
                //     .Where(p => p.WorkDate.Year == Convert.ToInt32(localYear))
                //     .Where(p => p.WorkDate.Month == Convert.ToInt32(localMonth))
                //     .ToList();

                foreach (User u in ListUser)
                {
                    UserSalary LocalUserSalary = new UserSalary();
                    LocalUserSalary.User = u;

                    LocalUserSalary.Year = Convert.ToInt32(localYear);
                    LocalUserSalary.Month = Convert.ToInt32(localMonth);


                    ////统计值班数量
                    //取出排班表的信息，然后统计白班晚班的信息
                    List<PersonPB> listPersonPB = this.personPBRepository.GetListPersonPBByUser(u, DateTime.Parse(localYear + "/" + localMonth + "/" + "01"), DateTime.Parse(localYear + "/" + localMonth + "/" + "31"));

                    var distinctZhiBan = from p in listPersonPB
                                         group p by new { p.UserWorkDate, p.WorkDayOrNight };

                    int DayNum = 0;
                    int NightNum = 0;
                    foreach (var d in distinctZhiBan)
                    {
                        if (d.Key.WorkDayOrNight == DayOrNight.白班)
                        {
                            DayNum = DayNum + 1;
                        }
                        if (d.Key.WorkDayOrNight == DayOrNight.晚班)
                        {
                            NightNum = NightNum + 1;
                        }
                    }

                    //DateTime WorkDate = DateTime.Parse(localYear+"/"+localMonth+"/"+"01");
                    //for (int i = 0; i <= 30; i++)
                    //{
                    //    WorkDate = WorkDate.AddDays(i);
                    //    if (WorkDate.Month != Convert.ToInt32(localMonth))
                    //        goto Last;
                    //    if (this.personPBRepository.GetAll()
                    //        .Where(p => p._user == u)
                    //        .Where(p => p.UserWorkDate == this.WorkDateRepository.getUserWorkDate(WorkDate))
                    //        .Where(p => p.WorkDayOrNight == this.WorkDayOrNightRepository.GetAll().Where(it=>it.ID==Convert.ToInt32("1")).First())                           
                    //        .Any())
                    //    {
                    //        DayNum = DayNum + 1;
                    //    }
                    //    if (this.personPBRepository.GetAll()
                    //       .Where(p => p._user == u)
                    //        .Where(p => p.UserWorkDate == this.WorkDateRepository.getUserWorkDate(WorkDate))
                    //       .Where(p => p.WorkDayOrNight == this.WorkDayOrNightRepository.GetAll().Where(it=>it.ID==Convert.ToInt32("2")).First()) 
                    //       .Any())
                    //    {
                    //        NightNum = NightNum + 1;
                    //    }
                    //}

                    LocalUserSalary.DayNum = DayNum;
                    LocalUserSalary.NightNum = NightNum;
                    LocalUserSalary.TotalNum = LocalUserSalary.DayNum + LocalUserSalary.NightNum;
                    LocalUserSalary.zhiBanSalary = LocalUserSalary.TotalNum * u.UserEnmLevel.UserLevelSalary;
                    LocalUserSalary.TotalSalary = LocalUserSalary.zhiBanSalary;
                    this.UserSalaryRepository.Save(LocalUserSalary);
                }
                ViewBag.message = "重新统计成功";
            }

            PagedData<UserSalary> data = this.UserSalaryRepository.GetData(queryInfo, localYear, localMonth, this.Users().userName);
            return View(data);
        }

        public ActionResult MyPaiBanRecordsTongji(QueryInfo queryInfo, string Year, string Month, string Name, string subAction)
        {   //取出排班客服表；
            //统计表；            
            string localYear;
            string localMonth;
            if (Year.IsNullOrEmpty())
            {
                localYear = System.DateTime.Now.Year.ToString().Trim();
            }
            else
            {
                localYear = Year;
            }
            if (Month.IsNullOrEmpty())
            {
                localMonth = System.DateTime.Now.Month.ToString().Trim();
            }
            else
            {
                localMonth = Month;
            }
            ViewData["Year"] = localYear;
            ViewData["Month"] = localMonth;

            string localName = "";
            if (Name != null)
            {
                localName = Name.ToString();
                ViewData["Name"] = localName;
            }
            PersonPBDataRepository PersonPBDataRepo = new PersonPBDataRepository();
            //if (subAction == "queryTongji")
            //{


            //    //根据年、月统计该月的值班数量以及班次
            //    //重新统计用户工资表
            //    this.UserSalaryRepository.updateUserSalary(localYear, localMonth);

            //    PersonPBDataRepo.updatePersonPBData(localYear, localMonth, this.Users().DepartMent);

            //    ViewBag.message = "重新统计成功";
            //}
            PagedData<PersonPBData> data = PersonPBDataRepo.GetData(queryInfo, localYear, localMonth,  this.Users());
            return View(data);

        }

        ////根据当前用户取出该用户所拥有的店铺
        //public ActionResult MyShopIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction, string isKf)
        //{
            
        //    int UserID = this.Users().ID;
        //    ViewBag.UserName = this.Users().Name;

        //    PagedData<Shop> data = shopRepository.GetShopByUserID(queryInfo, null, name, this.Users().ID.ToString());
        //    return View(data);
        //}
    }
}
