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
using JieNuo.Data.Exceptions;


namespace hkkf.web.Areas.Service.Controllers
{
    [NavigationRoot("店铺常见问题管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class MyWentiController : Controller
    {
        //
        // GET: /Admin/Wenti/
       private  ShopWenTiRepository shopWenTiRepository=new ShopWenTiRepository();
       private ShopRepository shopRepository = new ShopRepository();
       public ActionResult MyWenTiIndex(QueryInfo queryInfo, int[] ids,string typeid, string shopid,string shopname, string NContent, int? wtTypeid, string subAction)
        {
            if (typeid!=null)
            {
                ViewBag.isShow = "Yes";
            }
            ViewBag.shopid = shopid;
           ViewBag.ShopName = shopname;
            if (subAction == "delete")
            {
                DeleteShop(ids);

            }
            var data = shopWenTiRepository.GetData(queryInfo, shopname, NContent, wtTypeid,this.Users().ID.ToString().Trim());
            return View(data);
        }


       public ActionResult AddWenTi(string isedit, int? id, string shopid)
        {
            ViewBag.shopid = shopid;
            if (shopid != null)
            {
                ViewBag.shopName = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid)).Name;
            }
            if (isedit == "1"&&id!=null)
            {
                ViewBag.Edit = "1";
                ViewBag.id = id.Value;
                var data = shopWenTiRepository.GetByDatabaseID(id.Value);
                return View(data);
            }
            return View();
        }

        [HttpPost]
       public ActionResult AddWenTi(FormCollection collection, string alertMessage, string shopid, string IsEdit, int? id)
       {
           ViewBag.shopid = shopid;
           if (shopid != null)
           {
               ViewBag.shopName = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid)).Name;
           }
            if (IsEdit == "1")
            {
                var shopWenTi = shopWenTiRepository.GetByDatabaseID(id.Value);
                var shop = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid));
                try
                {
                    TryUpdateModel(shopWenTi, collection);
                    shopWenTi._Shop = shop;
                    shopWenTi.UpdateTime = System.DateTime.Now;
                    shopWenTi._User = this.Users();
                    shopWenTiRepository.Update(shopWenTi);
                    alertMessage = "操作成功！";
                    ViewData["alertMessage"] = alertMessage;
                   // return View("AddWenTi");
                    return RedirectToAction("MyWenTiIndex");
                }
                catch (RuleException ex)
                {
                    throw new RuleException(ex.Message, ex);
                }
            }
            else
            {
                ShopWenTi shopWenTi = new ShopWenTi();
                if (shopid.IsNotNullAndEmpty())
                {
                    var shop = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid));
                    try
                    {
                        TryUpdateModel(shopWenTi, collection);
                        shopWenTi._Shop = shop;
                        shopWenTi.UpdateTime = System.DateTime.Now;
                        shopWenTi._User = this.Users();
                        shopWenTiRepository.Save(shopWenTi);
                        alertMessage = "操作成功！";
                        ViewData["alertMessage"] = alertMessage;
                        //return View("AddWenTi");
                        return RedirectToAction("MyWenTiIndex");
                    }
                    catch (RuleException ex)
                    {
                        throw new RuleException(ex.Message, ex);
                    }
                }
            }
            
            return View();

        }

        public ActionResult DeleteShop(int[] ids)
        {


            if (ids == null || ids.Count() == 0)
                return RedirectToAction("WenTiIndex", "Wenti", new { alertMessage = "请选择要删除的数据！" });
            else
            {
                for (int i = 0; i < ids.Count(); i++)
                {
                    var user = shopWenTiRepository.GetByDatabaseID(ids[i]);
                    shopWenTiRepository.Delete(user);
                }
                return RedirectToAction("WenTiIndex", "Wenti", new { alertMessage = "删除成功！" });

            }

        }

    }
}
