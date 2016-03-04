using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Bibliography;
using hkkf.Models;
using hkkf.Repositories;
using JieNuo.Data;
using JieNuo.Data.Exceptions;
using hkkf.web.Areas.Service.Common;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("用户管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class UserManageController : Controller
    {
        //
        // GET: /Admin/UserManage/
        UserRepository userRepository = new UserRepository();
        PersonShopRepository personShopRepository=new PersonShopRepository();
        ShopRepository shopRepository = new ShopRepository();
        PinFenRepository pinFenRepository=new PinFenRepository();
        UserEnmTypeRepository UserEnmTypeRepository = new UserEnmTypeRepository();
        UserEnmLevelRepository UserEnmLevelRepository = new UserEnmLevelRepository();
        Kf_Role_TypeRepository Kf_Role_TypeRepository = new Kf_Role_TypeRepository();
        public ActionResult UserIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction, string isKf)
        {

            if (subAction == "delete")
            {
                foreach (int userid in ids)
                {
                    userRepository.Delete(userid);
                }
                ViewBag.message = "删除成功！";
            }
            if (alertMessage!=null)
            {
                ViewBag.message = alertMessage;
            }
            if (isKf=="1")
            {
                PagedData<User> data = userRepository.GetUserData(queryInfo, name, null,this.Users().DepartMent);
                ViewBag.iskf = "1";
                return View(data);
            }
            else if (isKf == "2")
                {
                    PagedData<User> data = userRepository.GetUserData(queryInfo, name, null,this.Users().DepartMent);
                    ViewBag.iskf = "2";
                    return View(data);
                }
            else
            {
                PagedData<User> data = userRepository.GetUserData(queryInfo, name, null,this.Users().DepartMent);
                ViewBag.iskf = "0";
                return View(data);
            }
            
           
        }
        
        public ActionResult AddUser(int? id, string isedit)
        {
            ViewBag.TypeList = this.UserEnmTypeRepository.GetAll().ToList().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
            });
            ViewBag.UserLevelList = this.UserEnmLevelRepository.GetAll().ToList().Select(p => new SelectListItem
            {
                Text=p.UserLevelName,
                Value=p.ID.ToString(),
            });
            Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
            IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
                .Select(p => new SelectListItem
                {
                    Text = p.DepartMentName,
                    Value = p.ID.ToString().Trim(),
                });
            ViewData["listDepartMent"] = listDepartMent;
            if (isedit == "1" && id!=null)
            {
                var user = userRepository.GetByDatabaseID(id.Value);
                ViewBag.isEdit = "1";
              
             return View(user);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection collection,int? id, string alertMessage, string isedit)
        {
            ViewBag.TypeList = this.UserEnmTypeRepository.GetAll().ToList().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
            });
            ViewBag.UserLevelList = this.UserEnmLevelRepository.GetAll().ToList().Select(p => new SelectListItem
            {
                Text = p.UserLevelName,
                Value = p.ID.ToString(),
            });
            Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
            IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
                .Select(p => new SelectListItem
                {
                    Text = p.DepartMentName,
                    Value = p.ID.ToString().Trim(),
                });
            ViewData["listDepartMent"] = listDepartMent;
            User user = new User();
            try
            {
                user.RegistrationTime = DateTime.Now;
                if (isedit == "1")
                {
                    var user1 = userRepository.GetByDatabaseID(id.Value);
                    TryUpdateModel(user1, collection);
                    userRepository.Update(user1);
                }
                else
                {
                    TryUpdateModel(user, collection);
                    userRepository.Save(user);
                }
               
               
                return RedirectToAction("UserIndex", new { alertMessage = "操作成功！" });
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }


        public ActionResult DeleteUser(int[] ids)
        {


            if (ids == null || ids.Count() == 0)
                return RedirectToAction("UserIndex", "UserManage", new { alertMessage = "请选择要删除的数据！" });
            else
            {
                for (int i = 0; i < ids.Count(); i++)
                {
                    User user = userRepository.GetByDatabaseID(ids[i]);
                    userRepository.Delete(user);
                }
                return RedirectToAction("UserIndex", "UserManage", new { alertMessage = "删除成功！" });

            }

        }

        //public ActionResult FenPei2(int id)
        //{
            
        //    var user = userRepository.GetByDatabaseID(id);

        //    ViewBag.userid = user.ID;
        //    ViewBag.UserName = user.userName;
        //    //ViewBag.TypeList = shopRepository.GetAll().ToList();
        //    string shopid = user.ShopID;
        //    if (shopid!=null)
        //    {
        //        string shop = shopid.Substring(1, shopid.Length - 1);
        //        string[] ids = shop.Split(',');
        //        int[] newids = Array.ConvertAll<string, int>(ids, delegate(string s) { return int.Parse(s); });
        //        List<Shop> shopList = new List<Shop>();
        //        for (int i = 0; i < newids.Length; i++)
        //        {
        //            shopList.Add(shopRepository.GetByDatabaseID(newids[i]));
        //        }
        //        ViewBag.TypeList = shopList;
        //    }
            
        //   return View();

        //}
        [HttpPost]
        public ActionResult FenPei2(FormCollection collection, string alertMessage,string userid,
            int hideDay1, int hideDay2, int hideDay3, int hideDay4, int hideDay5, int hideDay6, int hideDay7,
            string hideweek1, string hideweek2, string hideweek3, string hideweek4, string hideweek5, string hideweek6, string hideweek7,
            DateTime? hidetime1, DateTime? hidetime2, DateTime? hidetime3, DateTime? hidetime4, DateTime? hidetime5, DateTime? hidetime6, DateTime? hidetime7,
            string hidetypeid1,string hidetypeid2,string hidetypeid3,string hidetypeid4,string hidetypeid5,string hidetypeid6,string hidetypeid7)
        {
            //string shop = Shoptypeid.Substring(1, Shoptypeid.Length-1);
            //string[] ids = shop.Split(',');
            //int[] newids = Array.ConvertAll<string, int>(ids, delegate(string s) { return int.Parse(s); });
            var user = userRepository.GetByDatabaseID(Convert.ToInt32(userid));
            if (personShopRepository.IsFenpei(user.ID, hidetime1) || personShopRepository.IsFenpei(user.ID, hidetime1.Value.AddDays(1)) || personShopRepository.IsFenpei(user.ID, hidetime1.Value.AddDays(2))
                || personShopRepository.IsFenpei(user.ID, hidetime1.Value.AddDays(3)) || personShopRepository.IsFenpei(user.ID, hidetime1.Value.AddDays(4)) || personShopRepository.IsFenpei(user.ID, hidetime1.Value.AddDays(5)) || personShopRepository.IsFenpei(user.ID, hidetime1.Value.AddDays(6)))
            {
                return RedirectToAction("UserIndex", new { alertMessage = "此人已分配过此日期！" });
            }
            else
            {
                PersonShop personShop1 = new PersonShop();
                personShop1._user = user;
                personShop1.shopID = hidetypeid1 ?? "无";
                personShop1._DateTime = hidetime1 ?? null;
                personShop1.DayOrNight = hideDay1.ToString().ToEnum<DayOrNight>();
                personShop1.weeks = hideweek1 == "1" ? "周一" : "无";
                personShopRepository.Save(personShop1);
                PersonShop personShop2 = new PersonShop();
                personShop2._user = user;
                personShop2.shopID = hidetypeid2 ?? "无";
                personShop2._DateTime = hidetime1.Value.AddDays(1);
                personShop2.DayOrNight = hideDay2.ToString().ToEnum<DayOrNight>();
                personShop2.weeks = hideweek2 == "2" ? "周二" : "无";
                personShopRepository.Save(personShop2);
                PersonShop personShop3 = new PersonShop();
                personShop3._user = user;
                personShop3.shopID = hidetypeid3 ?? "无";
                personShop3._DateTime = hidetime1.Value.AddDays(2);
                personShop3.DayOrNight = hideDay3.ToString().ToEnum<DayOrNight>();
                personShop3.weeks = hideweek3 == "3" ? "周三" : "无";
                personShopRepository.Save(personShop3);
                PersonShop personShop4 = new PersonShop();
                personShop4._user = user;
                personShop4.shopID = hidetypeid4 ?? "无";
                personShop4._DateTime = hidetime1.Value.AddDays(3);
                personShop4.DayOrNight = hideDay4.ToString().ToEnum<DayOrNight>();
                personShop4.weeks = hideweek4 == "4" ? "周四" : "无"; ;
                personShopRepository.Save(personShop4);
                PersonShop personShop5 = new PersonShop();
                personShop5._user = user;
                personShop5.shopID = hidetypeid5 ?? "无";
                personShop5._DateTime = hidetime1.Value.AddDays(4);
                personShop5.DayOrNight = hideDay5.ToString().ToEnum<DayOrNight>();
                personShop5.weeks = hideweek5 == "5" ? "周五" : "无";
                personShopRepository.Save(personShop5);
                PersonShop personShop6 = new PersonShop();
                personShop6._user = user;
                personShop6.shopID = hidetypeid6 ?? "无";
                personShop6._DateTime = hidetime1.Value.AddDays(5);
                personShop6.DayOrNight = hideDay6.ToString().ToEnum<DayOrNight>();
                personShop6.weeks = hideweek6 == "6" ? "周六" : "无";
                personShopRepository.Save(personShop6);
                PersonShop personShop7 = new PersonShop();
                personShop7._user = user;
                personShop7.shopID = hidetypeid7 ?? "无";
                personShop7._DateTime = hidetime1.Value.AddDays(6);
                personShop7.DayOrNight = hideDay7.ToString().ToEnum<DayOrNight>();
                personShop7.weeks = hideweek7 == "7" ? "周日" : "无";
                personShopRepository.Save(personShop7);
                return RedirectToAction("UserIndex", new { alertMessage = "分配成功！" });
            
            }          
        }


        public ActionResult FenPei(int shopid,string message)
        {
            //查询此店铺是否之前分配过客服
            if (pinFenRepository.IsFPShop(shopid))
            {
                ViewBag.IsFenPei = "1";
                ViewBag.FenPeiList = pinFenRepository.FPShopList(shopid);
            }
            ViewBag.UserList = userRepository.GetAll().Where(p=>p.Type.ID==1).ToList();
            //ViewBag.UserList = userRepository.GetAll().ToList();
            //var user = userRepository.GetByDatabaseID(id);
            var shop = shopRepository.GetByDatabaseID(shopid);
            ViewBag.shopeName = shop.Name;
            ViewBag.shopeId = shop.ID;
         
            List<Kf_Role_Type> listKfRoleType = this.Kf_Role_TypeRepository.GetAll().ToList();
            IEnumerable<SelectListItem> listRoleType = listKfRoleType.Select(
                p => new SelectListItem {
                    Text=p.RoleName,
                    Value=p.ID.ToString().Trim(),
                });
            ViewData["kfRoleList"] = listRoleType;
           
          
            if (message.IsNotNullAndEmpty())
            {
                ViewData["alertMessage"] = message;
            }            
            return View();
        }
        [HttpPost]
        public ActionResult FenPei(string shopId, FormCollection  form,string subAction)
        {
            Shop shop = shopRepository.GetByDatabaseID(Convert.ToInt32(shopId));           
           

            //查询此店铺是否之前分配过客服
            if (pinFenRepository.IsFPShop(Convert.ToInt32(shopId)))
            {
                ViewBag.FenPeiList = pinFenRepository.FPShopList(Convert.ToInt32(shopId));
            }
            ViewBag.UserList = userRepository.GetAll().Where(p => p.Type.ID == 1).ToList();
            ViewBag.shopeName = shop.Name;
            ViewBag.shopeId = shop.ID;

            List<Kf_Role_Type> listKfRoleType = this.Kf_Role_TypeRepository.GetAll().ToList();
            IEnumerable<SelectListItem> listRoleType = listKfRoleType.Select(
                p => new SelectListItem
                {
                    Text = p.RoleName,
                    Value = p.ID.ToString().Trim(),
                });
            ViewData["kfRoleList"] = listRoleType;

            string mess = "";
            string UserIDs = "";
            string KfRoleIDs = "";
            
            if (subAction == "save")
            {
                //清空该店铺所有的分配人员
                var fplist = pinFenRepository.FPShopList(Convert.ToInt32(shopId));
                if (fplist != null)
                {
                    foreach (var fp in fplist)
                    {
                        pinFenRepository.Delete(fp);
                    }
                }

                UserIDs = form["UserIDs"];
                KfRoleIDs = form["KfRoleIDs"];
                string[] UserID = UserIDs.Split(new char[] {','});
                string[] KfRoleID = KfRoleIDs.Split(new char[] {','});
                //如果USERID里面全是FALSE，则跳出，如果有一个是数字，则判断对应的角色是不是0，如果是0，则提示必须选择对应的熟悉分数或者排序。
                for (int i = 0; i < UserID.Count();i++ )
                {
                    if (UserID[i].ToString().Trim() == "false")
                    {
                        goto Last;
                    }
                    //如果判断
                    if(KfRoleID[i].ToString().Trim()=="0")
                    {
                        mess = "请选择人员的客服角色!";
                        return RedirectToAction("FenPei", new { shopid=shopId,message=mess});
                    }

                        PinFen pinFen = new PinFen();
                        pinFen._shop = shop;
                        pinFen._user = this.userRepository.GetByDatabaseID(Convert.ToInt32(UserID[i]));
                        pinFen.UpdateTime = System.DateTime.Today;
                        this.pinFenRepository.SaveOrUpdate(pinFen);                    
                Last: ;
                 }
            }
            //string mess = "如果选择了人员，必须选择对应的熟悉分数或者排序!";
            //return RedirectToAction("FenPei", new { shopid = shopId, message = mess });

            return RedirectToAction("FenPei", new { shopid = shopId, message = "sdfsdf" });
            //try
            //{
            //    if (Sortypeid.IsNotNullAndEmpty() && Usertypeid.IsNotNullAndEmpty() && ScoreTypeid.IsNotNullAndEmpty())
            //    {
            //        var stringUser = Usertypeid.Substring(1, Usertypeid.Length - 1);
            //        var arrayUser = stringUser.Split(',');
            //        int[] userids = Array.ConvertAll<string, int>(arrayUser, delegate(string s) { return int.Parse(s); });

            //        var stringSort = Sortypeid.Substring(1, Sortypeid.Length - 1);
            //        var arraysort = stringSort.Split(',');
            //        int[] sortids = Array.ConvertAll<string, int>(arraysort, delegate(string s) { return int.Parse(s); });

            //        var stringScore = ScoreTypeid.Substring(1, ScoreTypeid.Length - 1);
            //        var arrayscore = stringScore.Split(',');
            //        int[] scoreids = Array.ConvertAll<string, int>(arrayscore, delegate(string s) { return int.Parse(s); });


            //        if (userids.Count() == sortids.Count() && userids.Count() == scoreids.Count())
            //        {
            //            for (int i = 0; i < userids.Length; i++)
            //            {
            //                if (pinFenRepository.IsExitByNameShopId(userids[i], shop.ID))
            //                {
            //                    ViewData["alertMessage"] = "选择的人员不能分配相同的店铺";
            //                    return View("FenPei");
            //                }
            //                PinFen _pinFen = new PinFen();
            //                _pinFen._shop = shop;
            //                //_pinFen.Sort = sortids[i];
            //                //_pinFen.Score = scoreids[i];
            //                _pinFen._user = userRepository.GetByDatabaseID(userids[i]);
            //                pinFenRepository.Save(_pinFen);
            //            }
            //        }
            //        else
            //        {
            //            string mess = "如果选择了人员，必须选择对应的熟悉分数或者排序!";
            //            return RedirectToAction("FenPei", new { shopid = shopId, message = mess });
            //        }
            //    }
            //    else
            //    {
            //        string mess = "如果选择了人员，必须选择对应的熟悉分数或者排序!";
            //        return RedirectToAction("FenPei", new { shopid = shopId, message=mess });
            //    }
            //    return RedirectToAction("ShopIndexForFenPei", "ShopManage", new { alertMessage = "分配成功！" });
            //}
            //catch (RuleException ex)
            //{
            //    throw new RuleException(ex.Message, ex);
                
            //}

        }


        //public ActionResult UsershopIndex(QueryInfo queryInfo, string Name, string shopName)
        //{
        //    PagedData<User> data = userRepository.GetUserData(queryInfo, Name,"1");
        //    if (Name.IsNotNullAndEmpty())
        //    {
               

        //        return View(data);
        //    }
        //    else if (shopName.IsNotNullAndEmpty())
        //    {
        //        int shop = personShopRepository.GetIdByShopName(shopName);
        //        var list1 = userRepository.GetPersonShopList(queryInfo, shop);
        //        return View(list1);

                
        //    }
        //    return View(data);


        //}
        
    }
}
