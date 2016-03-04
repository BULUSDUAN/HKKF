using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using NHibernate.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;
using hkkf.Common.Attributes;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
   // [TypeInfo("ID", "_User")]
    [DisplayName("班组排班表")]
    [Class(Table = "PersonShopGroupPBs", NameType = typeof(PersonShopGroupPBs))]
    public class PersonShopGroupPBs:DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("用户")]
        [ManyToOne(Column = "UserID")]
        virtual public User _User { get; set; }

        [DisplayName("店铺")]
        [ManyToOne(Column = "ShopGroupID")]
        virtual public ShopGroups _ShopGroups { get; set; }
 
        [DisplayName("排班日期")]
        [ManyToOne(Column = "UserWorkDateID")]
        virtual public UserWorkDate UserWorkDate { get; set; }

        [DisplayName("值班类型")]
        [Property(Name = "WorkDayOrNight", Column = "DayOrNightID")]
        virtual public DayOrNight WorkDayOrNight { get; set; }

        [DisplayName("更新时间")]
        [Property]
        virtual public DateTime UpdateTime { get; set; }

        public override string ToString()
        {
            return _ShopGroups.ToString();
        }
    }
}
