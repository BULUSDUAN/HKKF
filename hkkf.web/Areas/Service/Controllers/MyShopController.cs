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
    public class MyShopController : Controller
    {
        //
        // GET: /Service/Home/
        PersonPBRepository personPbRepository = new PersonPBRepository();
        ShopRepository shopRepository=new ShopRepository();
        DaysDataRepository daysDataRepository=new DaysDataRepository();
        [HttpGet]
        public ActionResult test(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction, string isKf)
        {

            int UserID = this.Users().ID;
            ViewBag.UserName = this.Users().Name;

            PagedData<PinFen> data = shopRepository.GetUserShopByUserID(queryInfo, null, name, this.Users().ID.ToString());
            return View(data);
        }
        public ActionResult MyShopList(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction, string isKf)
        {

            int UserID = this.Users().ID;
            ViewBag.UserName = this.Users().Name;

            PagedData<Shop> data = shopRepository.GetUserMainShopByUserID(queryInfo, null, name, this.Users().ID.ToString());
            return View(data);
        }
        //取出某一个店铺的排班记录来显示,PersonPBs
        public ActionResult MyShopPaiBanDetail(QueryInfo queryInfo, string id, DateTime startDate, DateTime endDate)
        {
            //根据shopGroupID取出哪些店铺在shopGroupDetail里面，在前台显示的时候如果在里面的，那么默认为选中。
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

            return View();
            PagedData<PersonPB> data = this.personPbRepository.GetPagedData(queryInfo, id, localStartDate, localEndDate);
            return View(data);
        }
     
        //根据当前用户取出该用户所拥有的店铺
        public ActionResult aaa(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction, string isKf)
        {
            
            int UserID = this.Users().ID;
            ViewBag.UserName = this.Users().Name;

            PagedData<PinFen> data = shopRepository.GetUserShopByUserID(queryInfo, null, name,this.Users().ID.ToString());
            return View(data);
        }
    }
}
