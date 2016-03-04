using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
//using JieNuo.ComponentModel;
using JieNuo.Data;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("分配管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class PersonShopManageController : Controller
    {
        //
        // GET: /Admin/PersonShopManage/
        PersonShopRepository personShopRepository=new PersonShopRepository();
        public ActionResult PersonShopIndex(QueryInfo queryInfo, string Name,string ShopName, int[] ids, DateTime? startDate, DateTime? endDate, string alertmessage,string subAction)
        {

            if (subAction == "delete")
            {
                foreach (int personshopid in ids)
                {
                    personShopRepository.Delete(personshopid);
                }
                ViewBag.message = "删除成功！";
            }
            if (alertmessage!=null)
            {
                ViewBag.message = alertmessage;
            }
            TempData["startDate"] = startDate;
            TempData["endDate"] = endDate;
            if (ShopName.IsNotNullAndEmpty())
            {
                int shop = personShopRepository.GetIdByShopName(ShopName);
                    var list1 = personShopRepository.GetPersonShopList(queryInfo, Name,shop.ToString(), startDate, endDate);
                   return View(list1);
            }
            var list = personShopRepository.GetPersonShopList(queryInfo, Name, ShopName, startDate, endDate);
           return View(list);
        }

        public ActionResult EditPersonShop(int id)
        {
            var personShop = personShopRepository.GetByDatabaseID(id);
            ViewBag.sID = id;
            return View(personShop);
        }
        [HttpPost]
        public ActionResult EditPersonShop(string id,int DayOrNight,string name)
        {
            var personShop = personShopRepository.GetByDatabaseID(Convert.ToInt32(id));
            personShop.DayOrNight = DayOrNight.ToString().ToEnum<DayOrNight>();
            personShopRepository.Update(personShop);

            return RedirectToAction("PersonShopIndex", new { alertmessage ="修改成功！"});
        }
    }
}
