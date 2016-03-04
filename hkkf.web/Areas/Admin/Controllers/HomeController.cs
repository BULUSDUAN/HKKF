using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("首页")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/
        UserRepository userRepository=new UserRepository();
        private ShopRepository shopRepository = new ShopRepository();
        public ActionResult Index()
        {
            return View();
        }


        //public ActionResult FenPeiDetails(int ID)
        //{
            
        //    var user = userRepository.GetByDatabaseID(ID);

        //    string shopid = user.ShopID;
        //    string shopString = null;
        //    if (shopid !=null)
        //    {
        //    string shop = shopid.Substring(1, shopid.Length - 1);
            
            
        //        string[] ids = shop.Split(',');
        //        int[] newids = Array.ConvertAll<string, int>(ids, delegate(string s) { return int.Parse(s); });
                
        //        for (int i = 0; i < newids.Length; i++)
        //        {
        //            var shop1 = shopRepository.GetByDatabaseID(newids[i]);
        //            shopString += shop1.Name + ",";
        //        }
        //    }
            
        //    ViewBag.Name = user.userName;
        //    ViewBag.TypeList = shopString??"暂没有分配的店铺！";
        //    return View(user);

        //}

    }
}
