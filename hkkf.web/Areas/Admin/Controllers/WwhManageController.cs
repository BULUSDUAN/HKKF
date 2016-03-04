using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
using JieNuo.Data;
using JieNuo.Data.Exceptions;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("旺旺号管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class WwhManageController : Controller
    {
        //
        // GET: /Admin/WwhManage/
        private wwhRepository wwhRepository = new wwhRepository();
        private ShopRepository shopRepository = new ShopRepository();
        public ActionResult wwhIndex(QueryInfo queryInfo, int[] ids, string wwhName, int? shopid, string subAction)
        {
             if (subAction == "delete")
            {
                Delete(ids,shopid);

            }
            ViewBag.shopid = shopid.ToString();
            ViewBag.wwhName = wwhName;
            var data = wwhRepository.GetData(queryInfo, wwhName, shopid.Value);
            return View(data);
        }

        public ActionResult wwhAdd(string shopid, string IsEdit, int? id)
        {
            ViewBag.shopid = shopid;
            if (shopid!=null)
            {
                ViewBag.shopName = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid)).Name;
            }
            
            if (IsEdit == "1" && id != null)
            {
                ViewBag.Edit = "1";
                ViewBag.id = id.Value;
                var data = wwhRepository.GetByDatabaseID(id.Value);
                return View(data);
            }
            
            

            return View();
        }
        [HttpPost]
        public ActionResult wwhAdd(FormCollection collection, string alertMessage,string shopid, string IsEdit, int? id)
        {
            ViewBag.shopid = shopid;
            if (shopid != null)
            {
                ViewBag.shopName = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid)).Name;
            }
            if (IsEdit == "1")
            {
                var wwh = wwhRepository.GetByDatabaseID(id.Value);
                //var shop = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid));
                try
                {
                    TryUpdateModel(wwh, collection);
                    //wwh._Shop = shop;
                    wwhRepository.Update(wwh);
                    alertMessage = "操作成功！";
                    ViewData["alertMessage"] = alertMessage;
                    return View("wwhAdd");
                }
                catch (RuleException ex)
                {
                    throw new RuleException(ex.Message, ex);
                }
            }
            else
            {
                wwh wwh = new wwh();
                var shop = shopRepository.GetByDatabaseID(Convert.ToInt32(shopid));
                try
                {
                    TryUpdateModel(wwh, collection);
                    wwh._Shop = shop;
                    wwhRepository.Save(wwh);
                    alertMessage = "操作成功！";
                    ViewData["alertMessage"] = alertMessage;
                    return View("wwhAdd");
                }
                catch (RuleException ex)
                {
                    throw new RuleException(ex.Message, ex);
                }
            }

        }


        public ActionResult Delete(int[] ids,int? shopId)
        {
            if (ids == null || ids.Count() == 0)
                return RedirectToAction("wwhIndex", "WwhManage", new { shopid = shopId, alertMessage = "请选择要删除的数据！" });
            else
            {
                for (int i = 0; i < ids.Count(); i++)
                {
                    var user = wwhRepository.GetByDatabaseID(ids[i]);
                    wwhRepository.Delete(user);
                }
                return RedirectToAction("wwhIndex", "WwhManage", new { shopid = shopId,alertMessage = "删除成功！" });

            }
        }

    }
}
