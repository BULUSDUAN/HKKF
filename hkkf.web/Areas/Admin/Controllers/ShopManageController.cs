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

namespace hkkf.web.Areas.Admin.Controllers
{

    [NavigationRoot("店铺管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class ShopManageController : Controller
    {
        //
        // GET: /Admin/ShopManage/
        ShopRepository shopRepository=new ShopRepository();
        ShopTypeRepositories shopTypeRepositories=new ShopTypeRepositories();
        ShopCountLevelRepository ShopCountLevelRepository = new ShopCountLevelRepository();
        ShopDifficultyLevelRepository ShopDifficultyLevelRepository = new ShopDifficultyLevelRepository();
        UserRepository UserRepository = new UserRepository();
        Kf_Role_TypeRepository kfRoleTypeRopo = new Kf_Role_TypeRepository();
        PinFenRepository pinFenRepo = new PinFenRepository();
        Kf_DepartMentRepository kf_DepartMentRepo = new Kf_DepartMentRepository();
        public ActionResult ShopIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction)
        {
            if (subAction == "delete")
            {
                foreach (int userid in ids)
                {
                    shopRepository.Delete(userid);
                }
                ViewBag.message = "删除成功！";
            }
            ViewBag.message = alertMessage;
            User user = this.Users();
            Kf_DepartMent departMent = user.DepartMent;      
            PagedData<Shop> data = shopRepository.GetData(queryInfo, null, name,departMent);
            return View(data);           
        }
        public ActionResult ShopIndexForFenPei(QueryInfo queryInfo, string name, string alertMessage, string subAction,FormCollection form)
        {
            User user = this.Users();
            Kf_DepartMent departMent = user.DepartMent;      
            PagedData<Shop> data = shopRepository.GetData(queryInfo, null, name,departMent);
            return View(data);
        }
        public ActionResult ShopIndexForFenPeiQuery(QueryInfo queryInfo, string name, string alertMessage, string subAction, FormCollection form)
        {
            User user = this.Users();
            Kf_DepartMent departMent = user.DepartMent;      
            PagedData<Shop> data = shopRepository.GetData(queryInfo, null, name,departMent);
            return View(data);
        }
        public ActionResult AddShop(int? id, string IsEdit)
        {
            if (IsEdit=="1")
            {
                var shop = shopRepository.GetByDatabaseID(id.Value);
                ViewBag.IsEdit = "1";
               
            }
            ViewBag.TypeList = shopTypeRepositories.GetAll().ToList().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
            });
            ViewBag.ShopCountLevelList = this.ShopCountLevelRepository.GetAll().ToList().Select(p => new SelectListItem
            {
                Text = p.CountLevel,
                Value = p.ID.ToString(),
            });
            ViewBag.ShopDifficultLevelList = this.ShopDifficultyLevelRepository.GetAll().ToList().Select(p => new SelectListItem
            {
                Text = p.ShopDifficultyLevelName,
                Value = p.ID.ToString(),
            });
            UserRepository UserRepo = new UserRepository();
            IEnumerable<SelectListItem> ListSaleUser = UserRepo.GetData(this.Users().DepartMent.ID,Convert.ToInt32(UserEnmType.Sale))
                 .Select(p => new SelectListItem
            {
                Text = p.userName,
                Value = p.ID.ToString().Trim(),
            });
            //IEnumerable<SelectListItem> ListSaleUser = UserRepo.GetAll()
            //    .Where(p=>p.Type.ID==2) //销售
            //    .Select(p => new SelectListItem
            //{
            //    Text = p.userName,
            //    Value = p.ID.ToString().Trim(),
            //});
            ViewData["ListSaleUser"] = ListSaleUser;

            IEnumerable<SelectListItem> listKfUser = UserRepo.GetData(this.Users().DepartMent.ID, Convert.ToInt32(UserEnmType.Person))
                .Select(p => new SelectListItem
            {
                Text = p.userName,
                Value = p.ID.ToString().Trim(),
            });
            ViewData["listKfUser"] = listKfUser;


            Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
            IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
                .Select(p => new SelectListItem
                {
                    Text = p.DepartMentName,
                    Value = p.ID.ToString().Trim(),
                });
            ViewData["listDepartMent"] = listDepartMent;

            PayCircleRepository PayCircleRepo = new PayCircleRepository();
            IEnumerable<SelectListItem> listPayCircle = PayCircleRepo.GetAll().Select(p => new SelectListItem
            {
                Text = p.PayCircleName,
                Value = p.ID.ToString().Trim(),
            });
            ViewData["listPayCircle"] = listPayCircle;


            IEnumerable<SelectListItem> listDepart = this.kf_DepartMentRepo.GetAll().Select(p => new SelectListItem
            {
                Text = p.DepartMentName,
                Value = p.ID.ToString().Trim(),
            });
            ViewData["listDepart"] = listDepart;

            if (id!=null)
            {
                var shop= shopRepository.GetByDatabaseID(id.Value);
                ViewBag.Edit = "1";
                shop.TotalScore = 0;
                return View(shop);

            }
            return View();
        }

        [HttpPost]
        public ActionResult AddShop(FormCollection collection, string alertMessage, string IsEdit, int? id, string _User)
        {
            Shop _shop = new Shop();
           
            ViewBag.TypeList = shopTypeRepositories.GetAll().ToList().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
            });
            //ViewBag.ShopCountLevelList = this.ShopCountLevelRepository.GetAll().ToList().Select(p => new SelectListItem
            //{
            //    Text = p.CountLevel,
            //    Value = p.ID.ToString(),
            //});
            //ViewBag.ShopDifficultLevelList = this.ShopDifficultyLevelRepository.GetAll().ToList().Select(p => new SelectListItem
            //{
            //    Text = p.ShopDifficultyLevelName,
            //    Value =p.ID.ToString(),
            //});
            UserRepository UserRepo = new UserRepository();
            IEnumerable<SelectListItem> ListUser = UserRepo.GetAll().Select(p => new SelectListItem
            {
                Text = p.userName,
                Value = p.ID.ToString().Trim(),
            });
            ViewData["listUser"] = ListUser;

            PayCircleRepository PayCircleRepo = new PayCircleRepository();
            IEnumerable<SelectListItem> listPayCircle = PayCircleRepo.GetAll().Select(p => new SelectListItem
            {
                Text = p.PayCircleName,
                Value = p.ID.ToString().Trim(),
            });
            ViewData["listPayCircle"] = listPayCircle;

            Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
            IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
                .Select(p => new SelectListItem
                {
                    Text = p.DepartMentName,
                    Value = p.ID.ToString().Trim(),
                });
            ViewData["listDepartMent"] = listDepartMent;

            try
            {
                
                    if (IsEdit == "1")
                    {
                        
                        var shopEdit = shopRepository.GetByDatabaseID(id.Value);
                        
                        TryUpdateModel(shopEdit, collection);
                        shopRepository.Update(shopEdit);
                    }
                    else
                    {
                        TryUpdateModel(_shop, collection);
                        if (shopRepository.ExistShopName(_shop.Name))
                        {
                            alertMessage = "添加失败 店铺已存在！";
                            return View(_shop);
                        }
                        if (_shop.Name.IsNullOrEmpty())
                        {
                            alertMessage = "店铺名不能为空!";
                            return View(_shop);
                        }
                        //_shop._User = shopRepository.GetUserByuserName(_User);
                        //_shop.TotalScore = _shop.DifficutyLevel.ID + _shop.ShopCountLevel.ID;

                        //_shop._PayCircle = PayCircleRepo.GetByDatabaseID(Convert.ToInt32(collection["_PayCircle"]));
                        //_shop._Kf_DepartMent = this.kf_DepartMentRepo.GetByDatabaseID(Convert.ToInt32(collection["_Kf_DepartMent"]));
                        //_shop.DemandUser = this.UserRepository.GetByDatabaseID(Convert.ToInt32(collection["DemandUser"]));
                        //_shop.SaleUser = UserRepository.GetByDatabaseID(Convert.ToInt32(collection["SaleUser"]));
                        //_shop.MainKfUser = UserRepository.GetByDatabaseID(Convert.ToInt32(collection["MainKfUser"]));
                        //_shop._Kf_DepartMent = this.kf_DepartMentRepo.GetByDatabaseID(Convert.ToInt32(collection["_Kf_DepartMent"]));
                        shopRepository.Save(_shop);
                    }

                    alertMessage = "操作成功！";
                    ViewData["alertMessage"] = alertMessage;
                    return RedirectToAction("ShopIndex");    

            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }

        //店铺管理中给某一个店铺添加能做这个店铺的客服的人员。
        public ActionResult Add_ShopKefu(string id)
        {
            ViewData["DepartMentID"] = this.Users().DepartMent.ID;            
            //根据shopID取出哪些客服在pinfen里面，在前台显示的时候如果在里面的，那么默认为选中。
            List<PinFen> listPinFen = this.pinFenRepo.GetAll()
                .Where(it => it._shop == this.shopRepository.GetByDatabaseID(Convert.ToInt32(id)))
                .ToList();
            ViewData["listPinFen"] = listPinFen;
            ViewData["_Shop"]= this.shopRepository.GetByDatabaseID(Convert.ToInt32(id));
            //var list = this.UserRepository.GetAll()
            //    .Where(it => it.UserStateID != UserEnmState.离职)
            //    .Where(it => it.Type.ID ==1 ) //客服
            //    .WhereIf(it=>it.DepartMent==this.Users().DepartMent,this.Users().DepartMent.ID!=1);
            var list = this.UserRepository.GetData(this.Users().DepartMent.ID, Convert.ToInt32(UserEnmType.Person));
            return View(list);
        }
        //MyShopPaiBanList
        //店铺管理中给某一个店铺添加能做这个店铺的客服的人员。
        //取出店铺的排班记录,PersonPBs
        public ActionResult MyShopPaiBanList(QueryInfo queryInfo, string id, DateTime? startDate, DateTime? endDate)
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
            ViewData["id"] = id;

            PersonPBRepository PersonPBRepo = new PersonPBRepository();
            PagedData<PersonPB> data = PersonPBRepo.GetPagedData(queryInfo, id, localStartDate, localEndDate);
            return View(data);
        }
        [HttpPost]
        public ActionResult Add_ShopKefu(Shop _Shop, string[] ids)
        {

            if (ids != null && ids.Length > 0)
            {
                var listPinFen = this.pinFenRepo.GetAll()
                    .Where(it => it._shop == _Shop);
                foreach (var PinFen in listPinFen)
                {
                    //if (PinFen._user != _Shop.MainKfUser)
                    //{
                        this.pinFenRepo.Delete(PinFen);
                    //}
                }
                for (int i = 0; i < ids.Length; i++)
                {
                    PinFen gd = new PinFen()
                    {
                        _user = new hkkf.Models.User { ID = ids[i].ToInt() },
                        _shop = _Shop,
                        UpdateTime = DateTime.Now
                    };
                    this.pinFenRepo.Save(gd);
                }
                //更新下能做店铺班组
                return Json(new { state = true, message = "添加或修改成功, 请刷新显示" });
            }
            else
                return Json(new { state = false, message = "请选择店铺" });
        }

        public ActionResult DeleteShop(int[] ids)
        {


            if (ids == null || ids.Count() == 0)
                return RedirectToAction("ShopIndex", "ShopManage", new { alertMessage = "请选择要删除的数据11！" });
            else
            {
                for (int i = 0; i < ids.Count(); i++)
                {
                    var _shop = shopRepository.GetByDatabaseID(ids[i]);
                    shopRepository.Delete(_shop);
                }
                return RedirectToAction("ShopIndex", "ShopManage", new { alertMessage = "删除成功！" });

            }

        }

        //给店铺分配哪个客服可以做这个店铺

    }
}
