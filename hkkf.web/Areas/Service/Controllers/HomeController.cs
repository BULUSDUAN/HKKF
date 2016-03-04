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
    [NavigationRoot("客服管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class HomeController : Controller
    {
        //
        // GET: /Service/Home/
        PersonPBRepository personPbRepository = new PersonPBRepository();
        ShopRepository shopRepository=new ShopRepository();
        DaysDataRepository daysDataRepository=new DaysDataRepository();
        [HttpGet]
        public ActionResult Index()
        {
          
            return View();
        }
        public ActionResult Index1(string date)
        {
            string dayshopName = null;
            string NightshopName = null;
            int Userid = this.Users().ID;
            if (date != null)
            {
                var date1 = Convert.ToDateTime(date);
                var ps = personPbRepository.GetDayPbByuserAndDate(Userid, date1);
                
                if (ps!=null)
                {
                    foreach (var perpb in ps)
                    {
                        
                        //if (perpb.WorkDayOrNight.ID==Convert.ToInt64(Ban.白班))
                        //{
                        //    dayshopName += perpb._Shop.Name + ",";
                        //}
                        //else if (perpb.WorkDayOrNight.ID==Convert.ToInt64(Ban.晚班))
                        //{
                        //    NightshopName += perpb._Shop.Name + ",";
                        //}
                       
                    }

                    if (dayshopName.IsNotNullAndEmpty() || NightshopName.IsNotNullAndEmpty())
                    {
                        return Content("<span style='color:blue;'>" + "白班负责店铺：</span>" + dayshopName + "。" +
                                       "<br/>" + "<span style='color:blue;'>"+"晚班负责店铺：" + NightshopName);
                    }

                    else
                    {
                        return Content("该天休息哦！");
                    }
                    
                }
                else
                {
                    return Content("该天没有排班哦！");
                }
                
                
            }
            var pesonShop = personPbRepository.GetPSListByUserID(Userid);
            string message = null;
            string message1 = null;
            string dayshop = null;
            string nightshop = null;
            foreach (var shop in pesonShop)
            {
                if (DateTime.Now.ToShortDateString() == shop.UserWorkDate.WorkDate.ToShortDateString()&&shop.WorkDayOrNight!=DayOrNight.白班)
                {
                    //if (shop.WorkDayOrNight.ID == Convert.ToInt64(Ban.白班))
                    //{
                    //    dayshop += shop._Shop.Name + ",";
                        
                    //}
                    //else if (shop.WorkDayOrNight.ID == Convert.ToInt64(Ban.晚班))
                    //{
                    //    nightshop += shop._Shop.Name + ",";
                    //}
                    message = dayshop;
                    message1 = nightshop;

                }

            }
            ViewBag.tx = message ?? "无！";
            ViewBag.tx1 = message1 ?? "无！";
         

            //最近一周排班情况
            int wd = (int)DateTime.Now.DayOfWeek;
            List<PersonPB> list = new List<PersonPB>();
            for (int i = 1 - wd; i < 8 - wd; i++)
            {
                DateTime currentDay = DateTime.Now.AddDays(i).Date;
                var personDayShopList = personPbRepository.GetDayPbByuserAndDate(Userid, currentDay);
                PersonPB pbs=new PersonPB();
                foreach (var pb in personDayShopList)
                {
                    //pbs.UserWorkDate.WorkDate = pb.UserWorkDate.WorkDate;
                  //  pbs.weeks = pb.weeks;
                    
                    //if (pb.WorkDayOrNight.ID==Convert.ToInt64(Ban.白班))
                    //{
                    //    pbs.DayShopName += pb._Shop.Name + "，";
                    //}
                    //else if (pb.WorkDayOrNight.ID == Convert.ToInt64(Ban.晚班))
                    //{

                    //    pbs.NightShopName += pb._Shop.Name + "，";
                    //}
                   
                }
                list.Add(pbs);
            }

            //最近一月排班情况
            int wd1 = (int)DateTime.Now.DayOfWeek;
            List<PersonPB> list1 = new List<PersonPB>();
            DateTime datelinshi;
            for (int i = 0; i <= 30; i++)
            {
                datelinshi=  daysDataRepository.Getdata().BeginDateTime;
                DateTime currentDay = datelinshi.AddDays(i).Date;
                var personDayShopList = personPbRepository.GetDayPbByuserAndDate(Userid, currentDay);
                PersonPB pbs = new PersonPB();
                foreach (var pb in personDayShopList)
                {
                    pbs.UserWorkDate.WorkDate = pb.UserWorkDate.WorkDate;
                   // pbs.weeks = pb.weeks;

                    if (pb.WorkDayOrNight ==DayOrNight.白班)
                    {
                        pbs.DayShopName += pb._Shop.Name + "，";
                    }
                    else if (pb.WorkDayOrNight == DayOrNight.晚班)
                    {

                        pbs.NightShopName += pb._Shop.Name + "，";
                    }

                }
                list1.Add(pbs);
            }
            ViewBag.list = list;
            ViewBag.list1 = list1;
            return View();
        }
        
        public ActionResult ShopRecord(QueryInfo queryInfo,DateTime? startDateTime,DateTime? endDateTime,string ShopName)
        {
            TempData["startDate"] = startDateTime;
            TempData["EndDate"] = endDateTime;
            var User = this.Users();
           // var list = personPbRepository.GetPagedData(queryInfo, User.userName,ShopName, startDateTime, endDateTime);
            return View();
        }

        public string shopName(string shopidstring)
        {
            string shopid = shopidstring;
            string shopString = null;
            if (shopid != null)
            {
                string shop = shopid.Substring(1, shopid.Length - 1);


                string[] ids = shop.Split(',');
                int[] newids = Array.ConvertAll<string, int>(ids, delegate(string s) { return int.Parse(s); });

                for (int i = 0; i < newids.Length; i++)
                {
                    var shop1 = shopRepository.GetByDatabaseID(newids[i]);
                    shopString += shop1.Name + ", ";
                }
                return shopString;
            }
            else
            {
                return "";
            }

        }

    }
}
