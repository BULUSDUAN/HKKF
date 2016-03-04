using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Bibliography;
using hkkf.Repositories;
using hkkf.Models;
using hkkf.web.Common;
using JieNuo.Data;
using JieNuo.Data.Exceptions;
using hkkf.web.Areas.Service.Common;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("财务管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class PayController : Controller
    {
        PayRecordsRepository PayRecordsRepo = new PayRecordsRepository();
        PayRequireRecordsRepository PayRequireRecordRepo = new PayRequireRecordsRepository();
        //PayRecordManageIndex
        //实收款管理
        public ActionResult PayRecordManageIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction)
        {
            ShopRepository shopRepo = new ShopRepository();
            PagedData<Shop> data = shopRepo.GetData(queryInfo,null,name,this.Users().DepartMent);
            return View(data);
        }

        public ActionResult PayRecordIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string Year,string Month)
        {
            string localYear;
            string localMonth;
            if (Year.IsNullOrEmpty())
            {
                localYear = System.DateTime.Now.Year.ToString().Trim();
            }
            else
            {
                localYear = Year;
            }
            if (Month.IsNullOrEmpty())
            {
                localMonth = System.DateTime.Now.Month.ToString().Trim();
            }
            else
            {
                localMonth = Month;
            }
            ViewData["Year"] = localYear;
            ViewData["Month"] = localMonth;
            ViewData["baseSum"] = this.PayRecordsRepo.GetPayRecordsSum(_PayType.基础服务费, localYear, localMonth);
            ViewData["tiChengSum"] = this.PayRecordsRepo.GetPayRecordsSum(_PayType.提成, localYear, localMonth);
            ViewData["totalSum"] = Convert.ToInt32(ViewData["baseSum"]) + Convert.ToInt32(ViewData["tiChengSum"]);

            //ViewData["baseRequireSum"] = this.PayRequireRecordRepo.GetPayRequireRecordsSum(_PayType.基础服务费, localYear, localMonth);
            //ViewData["tiChengRequireSum"] = this.PayRequireRecordRepo.GetPayRequireRecordsSum(_PayType.提成, localYear, localMonth);
            //ViewData["totalRequireSum"] = Convert.ToInt32(ViewData["baseRequireSum"]) + Convert.ToInt32(ViewData["tiChengRequireSum"]);
            PagedData<PayRecords> data = this.PayRecordsRepo.GetPayRecords(queryInfo,this.Users().DepartMent, name,localYear,localMonth);
            return View(data);
        }
        public ActionResult PayRequireRecordIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string Year, string Month)
        {
            string localYear;
            string localMonth;
            if (Year.IsNullOrEmpty())
            {
                localYear = System.DateTime.Now.Year.ToString().Trim();
            }
            else
            {
                localYear = Year;
            }
            if (Month.IsNullOrEmpty())
            {
                localMonth = "全部";
            }
            else
            {
                localMonth = Month;
            }
            ViewData["Year"] = localYear;
            ViewData["Month"] = localMonth;

            ViewData["DefaultDepartMent"] = this.Users().DepartMent;
          

            ViewData["baseSum"] = this.PayRequireRecordRepo.GetPayRequireRecordsSum(_PayType.基础服务费, localYear, localMonth);
            ViewData["tiChengSum"] = this.PayRequireRecordRepo.GetPayRequireRecordsSum(_PayType.提成, localYear, localMonth);
            ViewData["totalSum"] = Convert.ToInt32(ViewData["baseSum"]) + Convert.ToInt32(ViewData["tiChengSum"]);
            PagedData<PayRequireRecords> data = this.PayRequireRecordRepo.GetPayRequireRecords(queryInfo,this.Users().DepartMent, name, localYear, localMonth);
            return View(data);
        }

        //deletePayRecord
        public ActionResult deletePayRecord(string id)
        {
            this.PayRecordsRepo.Delete(Convert.ToInt32(id));
            return Json(new { state = true, message = "操作成功！" });
        }
        public ActionResult AddPayBaseRecord(string id,string shopID)
        {
            //shopID不为空的时候是添加，为空的时候是修改。
            //ShopID,PayTypeID,PayDate,SaleVolume,PayNum,NextPayDate,NextPayNum,DemandUserID,ConfirmUserID
            //取出数据来做DROPDOWNLIST的传递。  
            if (id != null)
            {
                PayRecords PayRecord = this.PayRecordsRepo.GetByDatabaseID(Convert.ToInt32(id));
                ViewBag.Edit = "1";
                return View(PayRecord);
            }
            ShopRepository shopRepo=new ShopRepository();
            Shop shop=shopRepo.GetByDatabaseID(Convert.ToInt32(shopID));
            ViewData["shopID"] = shopID;
            PayRecords payRecord = new PayRecords();
            payRecord.PayDate = System.DateTime.Today;           
            switch(shop._PayCircle)
            {
                case _PayCircle.月付:
                    payRecord.NextPayDate = System.DateTime.Today.AddDays(30);
                    payRecord.PayNum = shop.PriceByMonth;
                    payRecord.NextPayNum = shop.PriceByMonth;
                    break;
                case _PayCircle.季付:
                    payRecord.NextPayDate = System.DateTime.Today.AddDays(90);
                    payRecord.PayNum = shop.PriceByMonth*3;
                    payRecord.NextPayNum = shop.PriceByMonth*3;
                    break;
                case _PayCircle.半年:
                    payRecord.NextPayDate = System.DateTime.Today.AddDays(180);
                    payRecord.PayNum = shop.PriceByMonth*6;
                    payRecord.NextPayNum = shop.PriceByMonth*6;
                    break;
                case _PayCircle.一年:
                    payRecord.NextPayDate = System.DateTime.Today.AddDays(365);
                    payRecord.PayNum = shop.PriceByMonth*12;
                    payRecord.NextPayNum = shop.PriceByMonth*12;
                    break;
            }        
            return View(payRecord);
        }
        [HttpPost]
        public ActionResult AddPayBaseRecord(string PayDate, string PayNum, string NextPayDate,string NextPayNum,string shopID, int? id)
        {
            if (Convert.ToInt32(PayNum)<=0)
                return Json(new { state = false, message = "付款金额不得小于等于0！" });
            if (Convert.ToInt32(NextPayNum) <= 0)
                return Json(new { state = false, message = " 下次付款金额不得小于等于0！" });
            if (NextPayDate.ToDateTime()<=PayDate.ToDateTime())
                return Json(new { state = false, message = " 下次时间不得早于本次付款时间！" });
            PayRecords payRecord = new PayRecords();
            if (id != null && id.Value > 0)
            {
                payRecord = this.PayRecordsRepo.GetByDatabaseID(Convert.ToInt32(id));
            }
            try
            {
                //赋值                
                
                payRecord._PayType = _PayType.基础服务费;
               
                payRecord.PayDate = PayDate.ToDateTime();
                payRecord.PayNum = PayNum.ToInt();
                payRecord.SaleVolume = 0;
                payRecord.UpdateTime = System.DateTime.Now;
                payRecord.Year = PayDate.ToDateTime().Year;
                payRecord.Month = PayDate.ToDateTime().Month;
                payRecord.NextPayNum = NextPayNum.ToInt();
                payRecord.NextPayDate = NextPayDate.ToDateTime();                
                //保存
                PayRequireRecordsRepository payRequireRecordsRepo = new PayRequireRecordsRepository();
                if (id != null && id.Value > 0)
                {
                    payRecord.ConfirmUser = payRecord._Shop.MainKfUser;
                    payRecord.DemandUser = payRecord._Shop.MainKfUser;
                    this.PayRecordsRepo.Update(payRecord);
                    //根据PayRecords更新PayRequireRecords
                    payRequireRecordsRepo.AddPayRequireRecords(payRecord);
                }
                else
                {
                    ShopRepository shopRepo = new ShopRepository();
                    payRecord._Shop = shopRepo.GetByDatabaseID(Convert.ToInt32(shopID));
                    payRecord.ConfirmUser = payRecord._Shop.MainKfUser;
                    payRecord.DemandUser = payRecord._Shop.MainKfUser;
                    this.PayRecordsRepo.Save(payRecord);
                    payRequireRecordsRepo.AddPayRequireRecords(payRecord);
                }            
                return Json(new { state = true, message = "操作成功！" });
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }            
        }

        public ActionResult AddPayTiChengRecord(string id, string shopID)
        {
            //shopID不为空的时候是添加，为空的时候是修改。
            //ShopID,PayTypeID,PayDate,SaleVolume,PayNum,NextPayDate,NextPayNum,DemandUserID,ConfirmUserID
            //取出数据来做DROPDOWNLIST的传递。  

            if (id != null)
            {
                PayRecords PayRecord = this.PayRecordsRepo.GetByDatabaseID(Convert.ToInt32(id));
                ViewBag.Edit = "1";
                return View(PayRecord);
            }
            ShopRepository shopRepo = new ShopRepository();
            Shop shop = shopRepo.GetByDatabaseID(Convert.ToInt32(shopID));
            ViewData["shopID"] = shopID;
            PayRecords payRecord = new PayRecords();
            payRecord.PayNum = 0;
            payRecord.PayDate = System.DateTime.Today;
            payRecord.NextPayNum = 0;
            payRecord.NextPayDate = System.DateTime.Today.AddDays(15);
          
            return View(payRecord);
        }
        [HttpPost]
        public ActionResult AddPayTiChengRecord(string PayDate, string SaleVolume, string PayNum, string NextPayDate, string NextPayNum, string shopID, int? id)
        {
            if (Convert.ToInt32(PayNum) <= 0)
                return Json(new { state = false, message = "付款金额不得小于等于0！" });
            if (Convert.ToInt32(SaleVolume) <= 0 )
                return Json(new { state = false, message = " 销售额不得小于等于0！" });
            if (Convert.ToInt32(NextPayNum) <= 0)
                return Json(new { state = false, message = " 下次付款金额不得小于等于0！" });
            if (NextPayDate.ToDateTime() <= PayDate.ToDateTime())
                return Json(new { state = false, message = " 下次时间不得早于本次付款时间！" });
            PayRecords payRecord = new PayRecords();
            if (id != null && id.Value > 0)
            {
                payRecord = this.PayRecordsRepo.GetByDatabaseID(Convert.ToInt32(id));
            }
            try
            {
                //赋值                

                payRecord._PayType = _PayType.提成;

                payRecord.PayDate = PayDate.ToDateTime();
                payRecord.PayNum = PayNum.ToInt();
                payRecord.SaleVolume = SaleVolume.ToInt();
                payRecord.UpdateTime = System.DateTime.Now;
                payRecord.Year = PayDate.ToDateTime().Year;
                payRecord.Month = PayDate.ToDateTime().Month;
                payRecord.NextPayNum = NextPayNum.ToInt();
                payRecord.NextPayDate = NextPayDate.ToDateTime();
                //保存
                PayRequireRecordsRepository payRequireRecordsRepo = new PayRequireRecordsRepository();
                if (id != null && id.Value > 0)
                {
                    payRecord.ConfirmUser = payRecord._Shop.MainKfUser;
                    payRecord.DemandUser = payRecord._Shop.MainKfUser;
                    this.PayRecordsRepo.Update(payRecord);
                    //根据PayRecords更新PayRequireRecords
                    payRequireRecordsRepo.AddPayRequireRecords(payRecord);
                }
                else
                {
                    ShopRepository shopRepo = new ShopRepository();
                    payRecord._Shop = shopRepo.GetByDatabaseID(Convert.ToInt32(shopID));
                    payRecord.ConfirmUser = payRecord._Shop.MainKfUser;
                    payRecord.DemandUser = payRecord._Shop.MainKfUser;
                    this.PayRecordsRepo.Save(payRecord);
                    payRequireRecordsRepo.AddPayRequireRecords(payRecord);
                }
                return Json(new { state = true, message = "操作成功！" });
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }


        //public ActionResult AddShopGroup(string ShopGroupName, string WorkDayOrNight, string _Kf_DepartMent, int? id)
        //{
        //    //Kf_DepartMentRepository kf_DepartRepo = new Kf_DepartMentRepository();
        //    //IEnumerable<SelectListItem> listDepartMent = kf_DepartRepo.GetData(this.Users().DepartMent.ID)
        //    //    .Select(p => new SelectListItem
        //    //    {
        //    //        Text = p.DepartMentName,
        //    //        Value = p.ID.ToString().Trim(),
        //    //    });
        //    //ViewData["listDepartMent"] = listDepartMent;
        //    if (ShopGroupName.IsNullOrEmpty())
        //        return Json(new { state = false, message = "请填写分组名称！" });
        //    if (WorkDayOrNight.IsNullOrEmpty())
        //        return Json(new { state = false, message = "请选择值班类型！" });
        //    if (_Kf_DepartMent.IsNullOrEmpty())
        //        return Json(new { state = false, message = "请选择部门！" });

        //    ShopGroups shopGroup = new ShopGroups();
        //    if (id != null && id.Value > 0)
        //    {
        //        shopGroup = this.ShopGroupRepo.GetByDatabaseID(id.Value);
        //    }

        //    try
        //    {
        //        //赋值
        //        shopGroup.ShopGroupName = ShopGroupName;
        //        shopGroup.WorkDayOrNight = WorkDayOrNight.ToEnum<DayOrNight>();
        //        shopGroup._Kf_DepartMent = new Kf_DepartMent { ID = _Kf_DepartMent.ToInt() };

        //        //保存
        //        if (id != null && id.Value > 0)
        //        {
        //            shopGroup.UpdateTime = System.DateTime.Today;
        //            ShopGroupRepo.Update(shopGroup);
        //        }
        //        else
        //        {
        //            if (ShopGroupRepo.ExistShopGroupsName(shopGroup.ShopGroupName))
        //            {
        //                return Json(new { state = false, message = "添加失败 店铺已存在！" });
        //            }
        //            shopGroup.ContentShops = "无";
        //            shopGroup.UpdateTime = System.DateTime.Today;
        //            ShopGroupRepo.Save(shopGroup);
        //        }

        //        //alertMessage = "操作成功！";
        //        //ViewData["alertMessage"] = alertMessage;
        //        //return RedirectToAction("ShopGroupIndex");
        //        return Json(new { state = true, message = "操作成功！" });
        //    }
        //    catch (RuleException ex)
        //    {
        //        throw new RuleException(ex.Message, ex);
        //    }
        //}
    }
}
