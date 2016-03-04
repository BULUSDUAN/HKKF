using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Common;
using hkkf.Models;
using JieNuo.Data;
using NHibernate.Linq;

namespace hkkf.Repositories
{

    public class PBDateTempletRepository : NHibernateRepository<PBDateTemplet, int>
    {
        //(4)进行系统排班,清空PBDateTemplet,PersonShopPBs，PersonPBs之前的记录。插入PBDateTemplet,PersonShopPBs，PersonPBs自动插入。
        //（5）手工调整PBDateTemplet和值班人员，调整时PersonShopPBs，PersonPBs自动更新。
        public void PaiBan(DateTime localStartDate, DateTime localEndDate,Kf_DepartMent kf_DepartMent)
        {
            WorkDateRepository workDateRepo=new WorkDateRepository();
            List<UserWorkDate> listUserWorkDate=workDateRepo.PaiBan(localStartDate,localEndDate);
            ShopTempletRepository shopTempletRepo = new ShopTempletRepository();          
            
            //把PBDateTemplet中的记录删除掉，再重新                     
            List<PBDateTemplet>listPBDateTemplet=this.GetAll()
                    .Where(it => it._UserWorkDate.WorkDate.Date <= localEndDate.Date)
                    .Where(it => it._UserWorkDate.WorkDate.Date >= localStartDate.Date)
                    .WhereIf(it => it._ShopTemplet._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                    .ToList();
            //删除，重新排
            if (listPBDateTemplet != null && listPBDateTemplet.Count != 0)
            {
                foreach (var pb in listPBDateTemplet)
                {
                    this.Delete(pb);
                }
            }

            List<ShopTemplet> listShopTemplet = shopTempletRepo.GetAll()
              .Where(it => it.isExpire == isExpire.未过期)
              .Where(it => it.isValid == isValid.有效)
              .WhereIf(it => it._Kf_DepartMent == kf_DepartMent,kf_DepartMent.ID!=1)
              .ToList();
            for (DateTime Date = localStartDate; Date <= localEndDate; Date = Date.AddDays(1))
            {
                UserWorkDate workDate=listUserWorkDate.Where(it=>it.WorkDate==Date).FirstOrDefault();                         
             
                //循环
                ShopTemplet shopTempletOrdinary=listShopTemplet.Where(it=>it.ShopTempletTypeID==_ShopTempletType.平时).ToList().FirstOrDefault();
                ShopTemplet shopTempletWeekEnd=listShopTemplet.Where(it=>it.ShopTempletTypeID==_ShopTempletType.周末).ToList().FirstOrDefault();
                ShopTemplet shopTempletHoliday=listShopTemplet.Where(it=>it.ShopTempletTypeID==_ShopTempletType.假期).ToList().FirstOrDefault();
                ShopTemplet shopTempletSpecailDate=listShopTemplet.Where(it=>it.ShopTempletTypeID==_ShopTempletType.特定日期).ToList().FirstOrDefault();              

                PBDateTemplet pbDateTemplet = new PBDateTemplet();
                if((Date.DayOfWeek==DayOfWeek.Saturday||Date.DayOfWeek==DayOfWeek.Sunday)&&shopTempletWeekEnd!=null)
                {
                   pbDateTemplet._ShopTemplet=shopTempletWeekEnd;     
                }
                else if (shopTempletSpecailDate!= null&&Date.Date==shopTempletSpecailDate.SpecialDate.Date)
                {
                    pbDateTemplet._ShopTemplet = shopTempletSpecailDate;    
                }
                else if (false)//假期还没完善上去。
                {

                }
                else
                {
                    pbDateTemplet._ShopTemplet = shopTempletOrdinary;  
                }
                pbDateTemplet._UserWorkDate=workDate;
                pbDateTemplet.UpdateTime=System.DateTime.Now.Date;      
                this.Save(pbDateTemplet);
            }              

        }

        public PagedData<PBDateTemplet> GetData(QueryInfo queyInfo, DateTime localStartDate, DateTime localEndDate, Kf_DepartMent kf_DepartMent)
        {
            return GetSession().Linq<PBDateTemplet>()
                .Where(it=>it._UserWorkDate.WorkDate>=localStartDate)
                 .Where(it => it._UserWorkDate.WorkDate <= localEndDate)
                .WhereIf(u => u._ShopTemplet._Kf_DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .OrderBy("_UserWorkDate", false)
                .Page(queyInfo);
        }
    }
}
