using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
   
        [DisplayName("店铺组")]
        [Class(Table = "ShopGroups", NameType = typeof(Models.ShopGroups))]
    public class ShopGroups : DomainModel
        {
            [Id(Name = "ID", Column = "ShopGroupID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("组名")]
            [Property]
            virtual public string ShopGroupName { get; set; }

            [DisplayName("值班类型")]
            [Property(Name = "WorkDayOrNight", Column = "DayOrNightID")]
            virtual public DayOrNight WorkDayOrNight { get; set; }

            [DisplayName("部门")]
            [ManyToOne(Column = "DepartMentID")]
            virtual public Kf_DepartMent _Kf_DepartMent { get; set; }

            [DisplayName("店铺")]
            [Property]
            virtual public string ContentShops { get; set; }

            [DisplayName("更新时间")]
            [Property]
            virtual public DateTime UpdateTime { get; set; }


            public override string ToString()
            {
                return ShopGroupName;
           }
        } 


    
  
}
