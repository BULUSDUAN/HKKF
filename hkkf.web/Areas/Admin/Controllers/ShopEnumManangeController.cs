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
    [NavigationRoot("店铺类型管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class ShopEnumManangeController : Controller
    {
        //
        // GET: /Admin/ShopEnumManange/
        
        ShopTypeRepositories shopTypeRepositories = new ShopTypeRepositories();
        public ActionResult ShopTypeIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction)
        {
            if (subAction == "delete")
            {
                foreach (int userid in ids)
                {
                    shopTypeRepositories.Delete(userid);
                }
                ViewBag.message = "删除成功！";
            }
            PagedData<ShopType> data = shopTypeRepositories.GetShopTypes(queryInfo,name);
            return View(data);
        }


        public ActionResult AddShopEnumType(int? id)
        {
            if (id != null)
            {
                var shop = shopTypeRepositories.GetByDatabaseID(id.Value);
                ViewBag.Edit = "1";
                return View(shop);

            }
            return View();
        }
        [HttpPost]
        public ActionResult AddShopEnumType(FormCollection collection, string alertMessage, string IsEdit, int? id)
        {

            ShopType _shop = new ShopType();
            try
            {

                if (IsEdit == "1")
                {

                    var shopEdit = shopTypeRepositories.GetByDatabaseID(id.Value);
                    TryUpdateModel(shopEdit, collection);
                    shopTypeRepositories.Update(shopEdit);
                }
                else
                {
                    TryUpdateModel(_shop, collection);
                    if (shopTypeRepositories.ExistShopName(_shop.Name))
                    {
                        alertMessage = "添加失败 店铺已存在！";
                        return View(_shop);
                    }
                    shopTypeRepositories.Save(_shop);
                }

                alertMessage = "操作成功！";
                ViewData["alertMessage"] = alertMessage;
                return RedirectToAction("ShopTypeIndex");
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }

    }
}
