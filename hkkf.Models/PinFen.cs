using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
    [DisplayName("店铺评分表")]
    [Class(Table = "PinFens", NameType = typeof(PinFen))]
   public  class PinFen:DomainModel
    {
        
        [Id(Name = "ID", Column = "Id")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("客服")]
        [ManyToOne(Column = "UserID")]
        virtual public User _user { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺")]
        [ManyToOne(Column = "ShopId")]
        virtual public Shop _shop { get; set; }

        [DisplayName("更新时间")]
        [Property]
        virtual public DateTime UpdateTime { get; set; }
    }
}
