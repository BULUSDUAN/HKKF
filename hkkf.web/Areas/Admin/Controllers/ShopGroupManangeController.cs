using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
using JieNuo.Data;
using JieNuo.Data.Exceptions;
using hkkf.web.Areas.Service.Common;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("店铺班组管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class ShopGroupManangeController : Controller
    {
        ShopGroupRepository ShopGroupRepo = new ShopGroupRepository();
        private ShopGroupDetailRepository shopGroupDetailRepo = new ShopGroupDetailRepository();//
        PinFenRepository pinFenRepo = new PinFenRepository();
        private ShopRepository shopRepo = new ShopRepository();//
        PersonShopGroupRepository PersonShopGrupRepo = new PersonShopGroupRepository();

        ShopTempletRepository ShopTempletRepo = new ShopTempletRepository();
        ShopTempletTypeRepository ShopTempletTypeRepo = new ShopTempletTypeRepository();
        ShopTempletDetailsRepository ShopTempletDetailsRepo = new ShopTempletDetailsRepository();

        #region "班组"

        public ActionResult ShopGroupIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction)
        {
            if (subAction == "delete")
            {
                foreach (int userid in ids)
                {
                    ShopGroupRepo.Delete(userid);
                }
                ViewBag.message = "删除成功！";
            }
           
            PagedData<ShopGroups> data = ShopGroupRepo.GetShopGroups(queryInfo, name,this.Users().DepartMent.ID);
            return View(data);
        }
        public ActionResult ShopGroupIndexForFenPei(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction)
        {
            //如果是更新，则更新能做班组的客服
            //首先删除PERSONSHOPGROUP里面的数据，然后添加shopGroupID,如果该班组里面的店铺都由同一个人能够担当，那么就插入该条shopGroupID。
            if (subAction == "update")
            {
                this.PersonShopGrupRepo.updatePersonShopGroup();
            }
            PagedData<ShopGroups> data = ShopGroupRepo.GetShopGroups(queryInfo, name, this.Users().DepartMent.ID);
            return View(data);
        }
        public ActionResult AddShopGroup(int? id)
        {
            Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
            IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
                .Select(p => new SelectListItem
                {
                    Text = p.DepartMentName,
                    Value = p.ID.ToString().Trim(),
                });
            ViewData["listDepartMent"] = listDepartMent;
            if (id != null)
            {
                ShopGroups shopGroup = this.ShopGroupRepo.GetByDatabaseID(id.Value); 
                return View(shopGroup);
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult AddShopGroup(FormCollection collection, string alertMessage, string IsEdit, int? id)
        //{
        //    ShopGroups shopGroup = new ShopGroups();
        //    if (id != null && id.Value > 0)
        //    {
        //        shopGroup = this.ShopGroupRepo.GetByDatabaseID(id.Value);
        //    }     
        //     try
        //    {
        //        if (IsEdit == "1")
        //        {

        //            var shopGroupEdit = this.ShopGroupRepo.GetByDatabaseID(id.Value);
        //            TryUpdateModel(shopGroupEdit, collection);
        //            this.ShopGroupRepo.Update(shopGroupEdit);
        //        }
        //        else
        //        {
        //            TryUpdateModel(shopGroup, collection);
        //            if (this.ShopGroupRepo.ExistShopGroupsName(shopGroup.ShopGroupName))
        //            {
        //                alertMessage = "添加失败 班组名称已存在！";
        //                return View(shopGroup);
        //            }
        //            if (shopGroup.ShopGroupName.IsNullOrEmpty())
        //            {
        //                alertMessage = "班组名不能为空!";
        //                return View(shopGroup);
        //            }
        //            //_shop._User = shopRepository.GetUserByuserName(_User);
        //            //_shop.TotalScore = _shop.DifficutyLevel.ID + _shop.ShopCountLevel.ID;

        //            //_shop._PayCircle = PayCircleRepo.GetByDatabaseID(Convert.ToInt32(collection["_PayCircle"]));
        //            //_shop._Kf_DepartMent = this.kf_DepartMentRepo.GetByDatabaseID(Convert.ToInt32(collection["_Kf_DepartMent"]));
        //            //_shop.DemandUser = this.UserRepository.GetByDatabaseID(Convert.ToInt32(collection["DemandUser"]));
        //            //_shop.SaleUser = UserRepository.GetByDatabaseID(Convert.ToInt32(collection["SaleUser"]));
        //            //_shop.MainKfUser = UserRepository.GetByDatabaseID(Convert.ToInt32(collection["MainKfUser"]));
        //            //_shop._Kf_DepartMent = this.kf_DepartMentRepo.GetByDatabaseID(Convert.ToInt32(collection["_Kf_DepartMent"]));
        //            shopGroup.UpdateTime = System.DateTime.Now;
        //            ShopGroupRepo.Save(shopGroup);
        //        }
        //        alertMessage = "操作成功！";
        //        ViewData["alertMessage"] = alertMessage;
        //        return RedirectToAction("ShopGroupIndex");             
        //    }
        //    catch (RuleException ex)
        //    {
        //        throw new RuleException(ex.Message, ex);
        //    }
        //}
        [HttpPost]
        public ActionResult AddShopGroup(string ShopGroupName, string WorkDayOrNight, string _Kf_DepartMent, int? id)
        {
            //Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
            //IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
            //    .Select(p => new SelectListItem
            //    {
            //        Text = p.DepartMentName,
            //        Value = p.ID.ToString().Trim(),
            //    });
            //ViewData["listDepartMent"] = listDepartMent;
            if (ShopGroupName.IsNullOrEmpty())
                return Json(new { state = false, message = "请填写分组名称！" });
            if (WorkDayOrNight.IsNullOrEmpty())
                return Json(new { state = false, message = "请选择值班类型！" });
            if (_Kf_DepartMent.IsNullOrEmpty())
                return Json(new { state = false, message = "请选择部门！" });

            ShopGroups shopGroup = new ShopGroups();
            if (id != null && id.Value > 0)
            {
                shopGroup = this.ShopGroupRepo.GetByDatabaseID(id.Value);
            }

            try
            {
                //赋值
                shopGroup.ShopGroupName = ShopGroupName;
                shopGroup.WorkDayOrNight = WorkDayOrNight.ToEnum<DayOrNight>();
                shopGroup._Kf_DepartMent = new Kf_DepartMent { ID = _Kf_DepartMent.ToInt() };

                //保存
                if (id != null && id.Value > 0)
                {
                    shopGroup.UpdateTime = System.DateTime.Today;
                    ShopGroupRepo.Update(shopGroup);
                }
                else
                {
                    if (ShopGroupRepo.ExistShopGroupsName(shopGroup.ShopGroupName))
                    {
                        return Json(new { state = false, message = "添加失败 店铺已存在！" });
                    }
                    shopGroup.ContentShops = "无";
                    shopGroup.UpdateTime = System.DateTime.Today;
                    ShopGroupRepo.Save(shopGroup);
                }

                //alertMessage = "操作成功！";
                //ViewData["alertMessage"] = alertMessage;
                //return RedirectToAction("ShopGroupIndex");
                return Json(new { state = true, message = "操作成功！" });
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }

        public ActionResult Add_ShopGroupDetail(string id)
        {
            ShopGroups shopGroup = this.ShopGroupRepo.GetByDatabaseID(Convert.ToInt32(id));
            //根据shopGroupID取出哪些店铺在shopGroupDetail里面，在前台显示的时候如果在里面的，那么默认为选中。
            List<ShopGroupDetails> listShopGroupDetails = this.shopGroupDetailRepo.GetAll()
                .Where(it => it._ShopGroup == shopGroup)
                .ToList();
            ViewData["listShopGroupDetails"] = listShopGroupDetails;
            ViewData["shopGroup"] = shopGroup;
            var list = shopRepo.GetData(this.Users().DepartMent.ID);          
            return View(list);
        }

        [HttpPost]
        public ActionResult Add_ShopGroupDetail(ShopGroups gp, string[] ids)
        { 

            if (ids!=null&&ids.Length > 0)
            {
                var listShopGroupDetail = this.shopGroupDetailRepo.GetAll()
                    .Where(it => it._ShopGroup == gp);
                foreach (var ShopGroupDetail in listShopGroupDetail)
                {
                    shopGroupDetailRepo.Delete(ShopGroupDetail);
                }
                string strContentShops = "";
                for (int i = 0; i < ids.Length; i++)
                {
                    ShopGroupDetails gd = new ShopGroupDetails()
                    {
                        //_Shop = new Shop { ID = ids[i].ToInt() },                      
                        _Shop=this.shopRepo.GetByDatabaseID(ids[i].ToInt()),
                        _ShopGroup = gp,
                        UpdateTime = DateTime.Now
                    };
                    shopGroupDetailRepo.Save(gd);
                    strContentShops = strContentShops + gd._Shop.Name.Trim() + " | ";
                }
                strContentShops = strContentShops.Substring(0, strContentShops.Trim().Length - 1);
                gp.UpdateTime = System.DateTime.Now;
                gp.ContentShops = strContentShops;
                this.ShopGroupRepo.Update(gp);
                return Json(new { state = true, message = "添加或修改成功, 请刷新显示" });
            }
            else
                return Json(new { state=false,message="请选择店铺"});
        }
        #endregion

        #region "模板"
        public ActionResult ShopTempletIndex(QueryInfo queryInfo,int[] ids, string name, string alertMessage, string subAction,FormCollection form)
        {
            if (subAction == "delete")
            {
                foreach (int userid in ids)
                {
                    //ShopGroupRepo.Delete(userid);
                }
                ViewBag.message = "删除成功！";
            }
            if (form["checkBtn"]!=null)
            {
                //检查班组是否配备齐全可排班，这个方法放在shopGroupRepo里面
                //检查班组是否配备齐全，把所有的店铺都囊括进去了

                //foreach(int shopTempletID in ids)
                //{
                string id = form["checkBtn"].ToString().Trim().Substring(2);
               string strResult= this.ShopTempletRepo.checkShopTempletValid(Convert.ToInt32(id));              
               // }        
               ViewBag.message = strResult;
            }
            PagedData<ShopTemplet> data = this.ShopTempletRepo.GetShopTemplet(queryInfo, name,this.Users().DepartMent);
            return View(data);
        }
        public ActionResult AddShopTemplet(int? id)
        {
            Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
            IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
                .Select(p => new SelectListItem
                {
                    Text = p.DepartMentName,
                    Value = p.ID.ToString().Trim(),
                });
            ViewData["listDepartMent"] = listDepartMent;
            if (id != null)
            {
                ShopTemplet shopTemplet = this.ShopTempletRepo.GetByDatabaseID(id.Value);
                ViewBag.Edit = "1";
                return View(shopTemplet);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddShopTemplet(string ShopTempletName, string _Kf_DepartMent, string ShopTempletTypeID, string SpecialDate, string isExpire, int? id)
        {
        
            if(ShopTempletName.IsNullOrEmpty())
            {
                   return Json(new { state = false, message = "请填写模板名称！" });
            }
            if(_Kf_DepartMent.IsNullOrEmpty())
            {
                   return Json(new { state = false, message = "请选择部门！" });
            }
            if(ShopTempletTypeID.IsNullOrEmpty())
            {
                   return Json(new { state = false, message = "请选择模板类型！" });
            }
            if (ShopTempletTypeID=="4" && SpecialDate.IsNullOrEmpty())
            {
                   return Json(new { state = false, message = "模板为特定日期时请选择日期！" });
            }
            if (isExpire.IsNullOrEmpty())
            {
                   return Json(new { state = false, message = "请选择是否过期！" });
            }
            ShopTemplet shopTemplet = new ShopTemplet();
            try
            {

                if (id!=null && id.Value>0)
                {
                    shopTemplet = this.ShopTempletRepo.GetByDatabaseID(id.Value);
                }
                shopTemplet.ShopTempletName = ShopTempletName;
                shopTemplet._Kf_DepartMent = new Kf_DepartMent { ID = _Kf_DepartMent.ToInt() };
                shopTemplet.ShopTempletTypeID = ShopTempletTypeID.ToEnum<_ShopTempletType>();
                shopTemplet.isExpire = isExpire.ToEnum<isExpire>();
                shopTemplet.UpdateTime = System.DateTime.Today;
                if (SpecialDate.IsNotNullAndEmpty())
                {
                    shopTemplet.SpecialDate = SpecialDate.ToDateTime();
                }
                else
                {
                    shopTemplet.SpecialDate = System.DateTime.Today;
                }
                 
                if(id!=null && id.Value>0)
                {
                     ShopTempletRepo.Update(shopTemplet);
                }  
                else
                {

                    shopTemplet.UpdateTime = System.DateTime.Today;
                    ShopTempletRepo.Save(shopTemplet);
                }
                return Json(new { state = true, message = "操作成功！" });
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }
        public ActionResult Add_ShopTempletDetail(string id)
        {
            //根据shopGroupID取出哪些店铺在shopGroupDetail里面，在前台显示的时候如果在里面的，那么默认为选中。
            List<ShopTempletDetails> listShopTempletDetails = this.ShopTempletDetailsRepo.GetAll()
                .Where(it => it._ShopTemplet == this.ShopTempletRepo.GetByDatabaseID(Convert.ToInt32(id)))
                .ToList();
            ViewData["listShopTempletDetails"] = listShopTempletDetails;
            ViewData["ShopTempletID"] = id;
            var list = this.ShopGroupRepo.GetData(this.Users().DepartMent.ID);
            return View(list);
        }
        [HttpPost]
        public ActionResult Add_ShopTempletDetail(string ShopTempletID, string[] ids)
        {

            if (ids != null && ids.Length > 0)
            {
                ShopTemplet shopTemplet=this.ShopTempletRepo.GetByDatabaseID(Convert.ToInt32(ShopTempletID));
                var listShopTempletDetail = this.ShopTempletDetailsRepo.GetAll()
                    .Where(it => it._ShopTemplet == shopTemplet);
                foreach (var ShopTempletDetail in listShopTempletDetail)
                {
                    this.ShopTempletDetailsRepo.Delete(ShopTempletDetail);
                }
                for (int i = 0; i < ids.Length; i++)
                {
                    ShopTempletDetails gd = new ShopTempletDetails()
                    {                        
                        _ShopTemplet=shopTemplet,
                        _ShopGroup=this.ShopGroupRepo.GetByDatabaseID(Convert.ToInt32(ids[i])),                        
                        UpdateTime = DateTime.Now
                    };
                    this.ShopTempletDetailsRepo.Save(gd);
                }
                return Json(new { state = true, message = "添加或修改成功, 请刷新显示" });
            }
            else
                return Json(new { state = false, message = "请选择班组" });
        }
        #endregion
    }
}