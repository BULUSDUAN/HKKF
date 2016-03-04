using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Bibliography;
using hkkf.Repositories;
using hkkf.Models;
using hkkf.web.Common;
using JieNuo.Data;
using hkkf.web.Areas.Service.Common;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("排班管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class PaiBanController : Controller
    {
        PersonPBRepository personPBRepository = new PersonPBRepository();
        PinFenRepository pinfenRepository = new PinFenRepository();
        DaysDataRepository DaysDataRepository = new DaysDataRepository();

        ShopGroupRepository ShopGroupRepo = new ShopGroupRepository();
        private ShopGroupDetailRepository shopGroupDetailRepo = new ShopGroupDetailRepository();//
        PersonShopGroupRepository personShopGroupRepo = new PersonShopGroupRepository();
        PersonShopGroupPBsRepository personShopGroupPBsRepo = new PersonShopGroupPBsRepository();

        ShopRepository shopRepository = new ShopRepository();
        UserRepository userRepository = new UserRepository();
        DaysDataRepository daysDataRepository = new DaysDataRepository();
        UserSalaryRepository UserSalaryRepository = new UserSalaryRepository();
        WorkDateRepository WorkDateRepository = new WorkDateRepository();
        UserWorkDayOrNightRepository WorkDayOrNightRepository = new UserWorkDayOrNightRepository();

        ShopTempletRepository shopTempletRepo = new ShopTempletRepository();
        PBDateTempletRepository pbDateTempletRepo = new PBDateTempletRepository();

        #region "排班"
        //根据店铺、值班日期、白班、晚班找出能够带这个店铺的客服人员做为DROPDOWNLIST的成员
        public ActionResult GetAvailableUsers(Shop shop, UserWorkDate workDate, UserWorkDayOrNight DayOrNight)
        {
            //白班晚班值班日期还没有做限定。
            return View(pinfenRepository.GetUserListByShopID(shop.ID));
        }
        //系统排班。。。。。。
        public ActionResult PaiBanIndex(QueryInfo queryInfo, DateTime? startDate, DateTime? endDate, string subAction, FormCollection form)
        {
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
                localEndDate = localStartDate.AddDays(10);
            }
            else
            {
                localEndDate = Convert.ToDateTime(endDate);
            }
            ViewData["startDate"] = localStartDate;
            ViewData["endDate"] = localEndDate;


            if (subAction == "排班")
            //点排班按钮//
            //(1)先检查开始时间和结束时间，如果开始时间在今天之前，那么提示不能排班。如果结束时间小于等于开始时间，那么也提示不能排班

            //(2)判断是否有有效，并且不是历史的模板可以用于排班，如果没有，则提示没有模板；

            //(3)判断模板中的班组中的客服是否齐全，如果客服设置不全，那么也进行提示，则有效里面提示

            //(4)进行系统排班,清空PBDateTemplet,PersonShopPBs，PersonPBs之前的记录。插入PBDateTemplet,PersonShopPBs，PersonPBs自动插入。

            //（5）手工调整PBDateTemplet和值班人员，调整时PersonShopPBs，PersonPBs自动更新。
            //
            {
                if (localStartDate < System.DateTime.Now.Date)
                {
                    ViewBag.message = "系统排班不能排当天之前的班次，请选择当天以及之后的开始日期！";
                    goto Last;
                }
                if (localStartDate >= localEndDate)
                {
                    ViewBag.message = "结束时间不能小于开始时间，请重新选择结束日期！";
                    goto Last;
                }
                //

                string strResult = this.shopTempletRepo.checkShopTempletValid(this.Users().DepartMent);
                if (strResult.Trim() != "模板有效")
                {
                    ViewBag.message = strResult;
                    goto Last;
                }

                //(4)进行系统排班,清空PBDateTemplet,PersonShopPBs，PersonPBs之前的记录。插入PBDateTemplet,PersonShopPBs，PersonPBs自动插入。
                //（5）手工调整PBDateTemplet和值班人员，调整时PersonShopPBs，PersonPBs自动更新。
                PBDateTempletRepository pbDateTempletRepo = new PBDateTempletRepository();
                pbDateTempletRepo.PaiBan(localStartDate, localEndDate,this.Users().DepartMent);

                PersonShopGroupPBsRepository PersonShopGroupPBsRepo = new PersonShopGroupPBsRepository();
                PersonShopGroupPBsRepo.PaiBan(localStartDate, localEndDate,this.Users().DepartMent);

                PersonPBRepository personPBRepo = new PersonPBRepository();
                personPBRepo.PaiBan(localStartDate, localEndDate, this.Users().DepartMent);

                ViewBag.message = "排班成功";
            }
           
        Last:PagedData<UserWorkDate> userWorkDate = this.WorkDateRepository.GetUserWorkDate(queryInfo, localStartDate, localEndDate);
            return View(userWorkDate);
        }

        public ActionResult ToPaiBanByShopTempletIndex(QueryInfo queryInfo, DateTime? startDate, DateTime? endDate, FormCollection form, string subAction)
        {
            //取出排班店铺表
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
                localEndDate = localStartDate.AddDays(10);
            }
            else
            {
                localEndDate = Convert.ToDateTime(endDate);
            }
            ViewData["startDate"] = localStartDate;
            ViewData["endDate"] = localEndDate;

            //List<User> listAllUser = this.userRepository.GetData(this.Users().DepartMent.ID, Convert.ToInt32(UserEnmType.Person)).ToList();
            //this.ViewData["listAllUser"] = listAllUser;

            if (localStartDate < System.DateTime.Now.Date)
            {
                ViewBag.message = "手工排班不能排当天之前的班次，请选择当天以及之后的开始日期！";
                goto Last;
            }

        Last: PagedData<PBDateTemplet> data = this.pbDateTempletRepo.GetData(queryInfo,localStartDate,localEndDate,this.Users().DepartMent);
            return View(data);
        }

        public ActionResult ToPaiBanByShopGroupIndex(QueryInfo queryInfo, DateTime? startDate, DateTime? endDate, string ShopName, FormCollection form, string subAction)
        {
            //取出排班店铺表
            // ViewBag.TypeList = ShopTypeRepository.GetAll().ToList().Select(p => new SelectListItem { Text = p.Name, Value = p.ID.ToString() });
            ////if (subAction == "delete")
            ////{
            //  //  foreach (int StudentID in ids)
            //    {
            //        StudentRepository.Delete(StudentID);
            //    }
            //    alertMessage = "删除成功！";

            //}FormCollection

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
                localEndDate = localStartDate.AddDays(0);
            }
            else
            {
                localEndDate = Convert.ToDateTime(endDate);
            }
            ViewData["startDate"] = localStartDate;
            ViewData["endDate"] = localEndDate;


            string localShopName = "";
            if (ShopName != null)
            {
                localShopName = ShopName.ToString();
                ViewData["ShopName"] = localShopName;
            }

            if (localStartDate < System.DateTime.Now.Date)
            {
                ViewBag.message = "手工排班不能排当天之前的班次，请选择当天以及之后的开始日期！";
                goto Last;
            }

            //string PersonPBIDs = "";
            //string Users = "";

            //if (subAction == "save")
            //{
                //PersonPBIDs = form["hiddenPersonPBID"];
                //Users = form["NewUser"];

                //string[] PersonPBID = PersonPBIDs.Split(new char[] { ',' });
                //string[] User = Users.Split(new char[] { ',' });
                //PersonPB pb = new PersonPB();
                //for (int i = 0; i < PersonPBID.Count(); i++)
                //{
                //    if (User.GetValue(i) == null || User.GetValue(i).ToString().Trim() == "")
                //    {
                //        ;
                //    }
                //    else
                //    {
                //        int id = Convert.ToInt32(PersonPBID.GetValue(i));
                //        pb = personPBRepository.GetByDatabaseID(id);

                //        int UserID = Convert.ToInt32(User.GetValue(i).ToString().Trim());
                //        pb._user = userRepository.GetByDatabaseID(UserID);

                //        personPBRepository.Update(pb);
                //        ViewBag.message = "保存成功";

                //    }
                //}
           // }
        Last: PagedData<ShopGroups> data = this.ShopGroupRepo.GetShopGroups(queryInfo, localShopName, this.Users().DepartMent.ID);
            return View(data);
        }

        
        //手工排班......

         //店铺班组排班管理中给某一个店铺班组排班添加能做这个店铺的客服的人员。
        //传进来PersonShopGroupPBs的ID，更新的也是PersonShopGroupPBs的USERID，选择范围在PersonShopGroup中的USERID
        public ActionResult Add_ShopGroupKefu(string id)
        {
            //根据PersonShopGroupPBs的ID取出那些客服在排班，那么默认为选中。    Add_ShopGroupKefu

            PersonShopGroupPBs _PersonShopGroupPB = personShopGroupPBsRepo.GetByDatabaseID(Convert.ToInt32(id));
            List<PersonShopGroup> listPersonShopGroup = this.personShopGroupRepo.GetAll()
                .Where(it => it._ShopGroups == _PersonShopGroupPB._ShopGroups)
                .ToList();
            ViewData["_PersonShopGroupPB"] = _PersonShopGroupPB;
            return View(listPersonShopGroup);
        }
        [HttpPost]
        public ActionResult Add_ShopGroupKefu(string PersonShopGroupPBID ,string id, FormCollection form)
        {
            //
            PersonShopGroupPBs PersonShopGroupPB = this.personShopGroupPBsRepo.GetByDatabaseID(Convert.ToInt32(PersonShopGroupPBID));
            PersonShopGroupPB._User = this.userRepository.GetByDatabaseID(Convert.ToInt32(id));
            this.personShopGroupPBsRepo.Update(PersonShopGroupPB);
            //修改店铺值班信息表PersonPBs中的内容
            List<PersonPB> listPersonPB = this.personPBRepository.GetAll()
                .Where(it => it.UserWorkDate == PersonShopGroupPB.UserWorkDate)
                .Where(it => it.WorkDayOrNight == PersonShopGroupPB._ShopGroups.WorkDayOrNight)
                .ToList();
            List<ShopGroupDetails> listShopGroupDetails = this.shopGroupDetailRepo.GetAll()
                .Where(it => it._ShopGroup == PersonShopGroupPB._ShopGroups)
                .ToList();

            foreach (var ShopGroupDetails in listShopGroupDetails)
            {
                foreach (var PersonPB in listPersonPB)
                {
                    //如果在DETAILS里面找到该店铺，则就修改该店铺的值班人员
                    if (PersonPB._Shop == ShopGroupDetails._Shop)
                    {
                        PersonPB._user = PersonShopGroupPB._User;
                        this.personPBRepository.Update(PersonPB);
                    }
                }   
            }           
            return Json(new { state = true, message = "添加或修改成功, 请刷新显示" });         
        }

        public ActionResult Add_ShopTemplet(string id)
        {
            //根据PersonShopGroupPBs的ID取出那些客服在排班，那么默认为选中。Add_ShopGroupKefu
            PBDateTemplet _PBDateTemplet = this.pbDateTempletRepo.GetByDatabaseID(Convert.ToInt32(id));
            List<ShopTemplet> listShopTemplet = this.shopTempletRepo.GetShopTemplet(this.Users().DepartMent);
            ViewData["_PBDateTemplet"] = _PBDateTemplet;
            return View(listShopTemplet);
        }
        [HttpPost]
        public ActionResult Add_ShopTemplet(string PBDateTempletID, string id, FormCollection form)
        {
            //
            PBDateTemplet _PBDateTemplet = this.pbDateTempletRepo.GetByDatabaseID(Convert.ToInt32(PBDateTempletID));
            PBDateTemplet _PBDateTempletOld=_PBDateTemplet;
            if (_PBDateTemplet._ShopTemplet.ID.ToString().Trim() != id.Trim())
            {
                _PBDateTemplet._ShopTemplet = this.shopTempletRepo.GetByDatabaseID(Convert.ToInt32(id));
                this.pbDateTempletRepo.Update(_PBDateTemplet);

                PersonShopGroupPBsRepository PersonShopGroupPBsRepo = new PersonShopGroupPBsRepository();
                PersonShopGroupPBsRepo.PaiBanHandChange(_PBDateTempletOld,_PBDateTemplet._ShopTemplet,this.Users().DepartMent);

                PersonPBRepository personPBRepo = new PersonPBRepository();
                personPBRepo.PaiBan(_PBDateTemplet._UserWorkDate.WorkDate, _PBDateTemplet._UserWorkDate.WorkDate, this.Users().DepartMent);

                return Json(new { state = true, message = "添加或修改成功, 请刷新显示" });
            }
            else
            {
                return Json(new { state = true, message = "更新成功" });
            }
        }
        #endregion
        #region “查询”
        public ActionResult PaiBanByDateIndex(QueryInfo queryInfo, DateTime? startDate, DateTime? endDate, string subAction)
        {   //取出排班日期表
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
                localEndDate = localStartDate.AddDays(30);
            }
            else
            {
                localEndDate = Convert.ToDateTime(endDate);
            }
            ViewData["startDate"] = localStartDate;
            ViewData["endDate"] = localEndDate;

            List<User> listAllUser = this.userRepository.GetData(this.Users().DepartMent.ID, Convert.ToInt32(UserEnmType.Person)).ToList();
            this.ViewData["listAllUser"] = listAllUser;         

            PagedData<PBDateTemplet> data = this.pbDateTempletRepo.GetData(queryInfo, localStartDate, localEndDate, this.Users().DepartMent);
            return View(data);
        }
    
        //根据店铺查询排班的情况，也就是取出PersonPBs表中的内容，客服和店铺的排班对应表。 
        public ActionResult PaiBanByShopIndex(QueryInfo queryInfo, DateTime? startDate, DateTime? endDate, string ShopName)
        {   
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
            string localShopName = "";
            if (ShopName != null)
            {
                localShopName = ShopName.ToString();
                ViewData["ShopName"] = localShopName;
            }
            PagedData<PersonPB> data = this.personPBRepository.GetPagedData(queryInfo,localShopName,localStartDate,localEndDate,this.Users().DepartMent);
            return View(data);
        }
        //根据客服查询排班的情况，也就是取出PersonShopGroupPBs表中的内容，客服和班组的排班对应表。 
        public ActionResult PaiBanByUsersIndex(QueryInfo queryInfo, DateTime? startDate, DateTime? endDate, string Name,Kf_DepartMent kf_DepartMent)
        {   //取出排班客服表
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
            //根据

            //this.personShopGroupPBsRepo.GetAll()

            string localName = "";
            if (Name != null)
            {
                localName = Name.ToString();
                ViewData["Name"] = localName;
            }
            PagedData<PersonShopGroupPBs> data = this.personShopGroupPBsRepo.GetData(queryInfo,localName,localStartDate,localEndDate,this.Users().DepartMent);
            return View(data);
        }
        public ActionResult PaiBanByUsersTongjiIndex(QueryInfo queryInfo, string Year, string Month, string Name, string subAction)
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
            if (subAction == "queryTongji")
            {


                //根据年、月统计该月的值班数量以及班次
                //重新统计用户工资表
                this.UserSalaryRepository.updateUserSalary(localYear, localMonth);
             
                PersonPBDataRepo.updatePersonPBData(localYear, localMonth,this.Users().DepartMent);
             
                ViewBag.message = "重新统计成功";
            }
            PagedData<PersonPBData> data = PersonPBDataRepo.GetData(queryInfo, localYear, localMonth, localName,this.Users().DepartMent);
            return View(data);

        }
        #endregion

        #region "原代码"
        //public ActionResult Index(DateTime? startDate, DateTime? endDate, string subAction)
        //{
        //    if (subAction == "排班")
        //    {
        //        #region 日期基本判断
        //        //清空daysdata表
        //        var alldays = daysDataRepository.GetAll();
        //        if (alldays != null)
        //        {
        //            foreach (var day in alldays)
        //            {
        //                daysDataRepository.Delete(day);
        //            }
        //        }
        //        int daycount = 0;
        //        if (startDate != null && endDate.Value != null)
        //        {

        //            System.TimeSpan ts = (endDate.Value - startDate.Value);
        //            int iday = ts.Days;
        //            #region 这个月分为几段
        //            //余数
        //            int yucount = iday / 7 + 1;
        //            //初始化所有区间段的日期
        //            DateTime startdaydate = startDate.Value;
        //            DateTime enddaydate = startdaydate.AddDays(6);

        //            for (int i = 0; i < yucount; i++)
        //            {
        //                DaysData daysData = new DaysData();
        //                daysData.BeginDateTime = startdaydate;
        //                daysData.EndDateTime = enddaydate;
        //                daysDataRepository.Save(daysData);
        //                //给予值
        //                startdaydate = daysData.BeginDateTime.AddDays(7);
        //                enddaydate = daysData.EndDateTime.AddDays(7);
        //            }
        //            #endregion
        //            daycount = iday;
        //            if (iday != 30 && iday != 31 && iday != 28 && iday != 29)
        //            {
        //                ViewBag.message = "选择周期不能少于30天！";
        //                return View();
        //            }
        //            if (personPBRepository.IsAnyCreateTime(startDate.Value) || personPBRepository.IsAnyCreateTime(endDate.Value))
        //            {
        //                ViewBag.message = "已经分配过此日期！如果想重新分配，请到”排班管理记录“删除后继续分配！";
        //                return View();
        //            }
        //        } 
        //        #endregion

        //        //查询所有店铺
        //        var allShops =this.shopRepository.GetAll();
        //        DateTime date;
        //        DateTime datelinshi;
        //        for (int j = 0; j <= daycount; j++)
        //        {
        //            datelinshi = startDate.Value;
        //            date = datelinshi.AddDays(j).Date;

        //            #region 是否分配过排序1、2、3、4、.......
        //            foreach (var shop in allShops)
        //            {
        //                var userlist = userRepository.GetAll().Where(p=>p.Type.ID==1);
        //                for (int i = 1; i <= userlist.Count(); i++)
        //                {
        //                    var User = pinfenRepository.GetUserBySort(shop.ID, i);
        //                    if (User==null)
        //                    {
        //                        ViewBag.message = shop.Name + "该店铺没有分配过排序"+i+"，请到“客服分配店铺管理”分配！";
        //                        return View();
        //                    }
        //                }
        //            } 
        //            #endregion
        //            foreach (var shop in allShops)
        //            {
        //                #region User1,User2
        //                //找到该店铺排序为1、2的进行插入数据
        //                var User1 = pinfenRepository.GetUserBySort(shop.ID, 1);
        //                var User2 = pinfenRepository.GetUserBySort(shop.ID, 2);


        //                #endregion
        //                //查询该店铺是什么值班类型
        //                if (shop.ZhiBanTypeID == ZhiBanType.全托)
        //                {
        //                    //一天插入两条数据 白班/晚班
        //                    for (int i = 1; i <= 2; i++)
        //                    {
        //                        if (i==1)
        //                        {

        //                                UpdateAndIsAny6(shop, User1, Ban.白班, date);


        //                        }
        //                        else
        //                        {
        //                            UpdateAndIsAny6(shop, User2, Ban.晚班, date);
        //                        }
        //                    }
        //                }
        //                else if (shop.ZhiBanTypeID == ZhiBanType.白班)
        //                {
        //                    UpdateAndIsAny6(shop, User1, Ban.白班, date);
        //                }
        //                else if (shop.ZhiBanTypeID == ZhiBanType.夜班)
        //                {
        //                    UpdateAndIsAny6(shop, User2, Ban.晚班, date);

        //                }
        //                else if (shop.ZhiBanTypeID == ZhiBanType.周末)
        //                {
        //                    string week = GetWeeks.CaculateWeekDay(date.Year, date.Month, date.Day);
        //                    if (week == "星期六" || week == "星期日")
        //                    {
        //                        for (int i = 1; i <= 2; i++)
        //                        {

        //                            if (i == 1)
        //                            {
        //                                UpdateAndIsAny6(shop, User1, Ban.白班, date);
        //                            }
        //                            else
        //                            {
        //                                UpdateAndIsAny6(shop, User2, Ban.晚班, date);
        //                            }
        //                        }
        //                    }

        //                }
        //                else if (shop.ZhiBanTypeID == ZhiBanType.夜班加周末)
        //                {
        //                    string week = GetWeeks.CaculateWeekDay(date.Year, date.Month, date.Day);
        //                    if (week == "星期六" || week == "星期日")
        //                    {
        //                        UpdateAndIsAny6(shop, User1, Ban.晚班, date);
        //                    }

        //                }
        //            }
        //        }
        //        TempData["startDate"] = startDate;
        //        TempData["endDate"] = endDate;
        //        //foreach (var daysdata in daysDataRepository.GetAll())
        //        //{
        //        //    foreach (var user in userRepository.GetAll().Where(p=>p.Type==UserEnmType.Person))
        //        //    {
        //        //        int maxid =personPBRepository.GetMaxData(daysdata.BeginDateTime, daysdata.EndDateTime,
        //        //            user.ID);
        //        //        var personpb = personPBRepository.GetByDatabaseID(maxid);
        //        //        if (personpb==null)
        //        //        {
        //        //            continue;

        //        //        }
        //        //        personPBRepository.Delete(personpb);
        //        //    }

        //        //}

        //        ViewBag.message = "排班成功！";
        //    }

        //    return View();
        //}
            
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection, string _user)
        {
            PersonPB personPb = new PersonPB();
            TryUpdateModel(personPb, collection);
            var _userls = shopRepository.GetUserByuserName(_user);
            if (_userls != null)
            {
                personPb._user = _userls;
            }
            else
            {
                ViewData["alertMessage"] = "此人不存在！";
                return View();
            }
            personPBRepository.Save(personPb);
            return RedirectToAction("ManageIndex", new { alertmessage = "添加成功！" });
        }

        public ActionResult EditPb(int id)
        {
            var data = personPBRepository.GetByDatabaseID(id);
            return View(data);
        }
        [HttpPost]
        public ActionResult EditPb(FormCollection collection, string _user, int? id)
        {
            var data = personPBRepository.GetByDatabaseID(id.Value);
            TryUpdateModel(data, collection);
            var _userls = shopRepository.GetUserByuserName(_user);
            if (_userls != null)
            {
                data._user = _userls;
            }
            else
            {
                ViewData["alertMessage"] = "此人不存在！";
                return View();
            }
            personPBRepository.Update(data);
            return RedirectToAction("ManageIndex", new { alertmessage = "修改成功！" });
        }

        //查询出来User1的用户的排班次数，然后+1,
        //+1的前提是，查询出当天是否排过班，如果排过，不+1，
        //public void UpdateUserPPcount(DateTime date,User _user,Ban ban)
        //{
        //    var user = userRepository.GetByDatabaseID(_user.ID);
        //    if (!personPBRepository.IsPBByUserID(date, user.ID,ban))
        //    {
        //        user.PPCount += 1;
        //        userRepository.Update(user);
        //    }
        //}


        //值够六个班，清空排班次数
        //public void EmptyUserPPcount(User _user)
        //{
        //    var user = userRepository.GetByDatabaseID(_user.ID);

        //        user.PPCount=0;
        //        userRepository.Update(user);

        //}
        //更新数据并且判断是否值班够6天
        //public void UpdateAndIsAny6(Shop shop,User user,Ban ban,DateTime date)
        //{
        //    //根据日期查询开始日期和结束日期
        //    DateTime starTime = daysDataRepository.GetDataByDate(date).BeginDateTime;
        //    DateTime endTime = daysDataRepository.GetDataByDate(date).EndDateTime;
        //    PersonPB personPb=new PersonPB();
        //    //是否值班够6天
        //    //Todo：根据日期查询区间段内此人是否值班够6天(周一来处理)
        //    #region 递归算法
        //    //if (IsAnyBy6(starTime, endTime, user))
        //    //{
        //    //    //int id = personPBRepository.MaxID(user.ID);
        //    //    //personPBRepository.Delete(id);
        //    //    var User2 = pinfenRepository.GetUserBySort(shop.ID, 2);
        //    //    var User3 = pinfenRepository.GetUserBySort(shop.ID, 3);
        //    //    var User4 = pinfenRepository.GetUserBySort(shop.ID, 4);
        //    //    var User5 = pinfenRepository.GetUserBySort(shop.ID, 5);
        //    //    var User6 = pinfenRepository.GetUserBySort(shop.ID, 6);
        //    //    var User7 = pinfenRepository.GetUserBySort(shop.ID, 7);
        //    //    var User8 = pinfenRepository.GetUserBySort(shop.ID, 8);
        //    //    if (IsAnyBy6(starTime, endTime, User2))
        //    //    {
        //    //        //int id1 = personPBRepository.MaxID(User2.ID);
        //    //        //personPBRepository.Delete(id1);
        //    //        if (IsAnyBy6(starTime, endTime, User3))
        //    //        {
        //    //            //int id2 = personPBRepository.MaxID(User3.ID);
        //    //            //personPBRepository.Delete(id2);
        //    //            if (IsAnyBy6(starTime, endTime, User4))
        //    //            {
        //    //                //int id3 = personPBRepository.MaxID(User4.ID);
        //    //                //personPBRepository.Delete(id3);
        //    //                if (IsAnyBy6(starTime, endTime, User5))
        //    //                {
        //    //                    if (IsAnyBy6(starTime, endTime, User6))
        //    //                    {
        //    //                        if (IsAnyBy6(starTime, endTime, User7))
        //    //                        {
        //    //                            if (IsAnyBy6(starTime, endTime, User8))
        //    //                            {

        //    //                            }
        //    //                            else
        //    //                            {
        //    //                                personPb._user = User8;
        //    //                            }
        //    //                        }
        //    //                        else
        //    //                        {
        //    //                            personPb._user = User7;
        //    //                        }
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        personPb._user = User6;
        //    //                    }
        //    //                }
        //    //                else
        //    //                {
        //    //                    personPb._user = User5;
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                personPb._user = User4;
        //    //            }

        //    //        }
        //    //        else
        //    //        {
        //    //            personPb._user = User3;
        //    //        }

        //    //    }
        //    //    else
        //    //    {
        //    //        personPb._user = User2;
        //    //    }

        //    //    personPb.DayOrNight = ban;
        //    //    personPb._DateTime = date;
        //    //    personPb._Shop = shop;
        //    //    personPb.weeks = GetWeeks.CaculateWeekDay(date.Year, date.Month, date.Day);
        //    //    personPBRepository.Save(personPb);
        //    //}
        //    //else
        //    //{
        //    //    personPb._user = user;
        //    //} 
        //    #endregion

        //    personPb._user = GetForUser(shop,starTime,endTime,user,ban);
        //   // personPb.DayOrNight = ban;
        //   // personPb._DateTime = date;
        //    personPb._Shop = shop;
        //   // personPb.weeks = GetWeeks.CaculateWeekDay(date.Year, date.Month, date.Day);
        //    personPBRepository.Save(personPb);
        //}

        //  private int id;   
        //public  User GetForUser(Shop shop, DateTime starTime, DateTime endTime, User user,Ban ban)
        //{
        //    User currentuser=new User();
        //    if (IsAnyBy6(starTime, endTime, user))
        //    {
        //        //查询是否在这段时间内，值过白班/晚班，值过的话只能值相同的班
        //       // if (ban==Ban.白班&& personPBRepository.IsBai(starTime,endTime,user.ID))
        //            id++;
        //            currentuser = pinfenRepository.GetUserBySort(shop.ID, id);
        //            return GetForUser(shop, starTime, endTime, currentuser,ban);


        //    }
        //    currentuser = user;
        //    id = 1;
        //    return currentuser;

        //}
    }
        #endregion
      
}
