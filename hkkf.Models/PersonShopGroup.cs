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
    [TypeInfo("ID", "UpdateTime")]
    [DisplayName("班组客服表")]
    [Class(Table = "PersonShopGroup", NameType = typeof(PersonShopGroup))]
    public class PersonShopGroup:DomainModel
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
      

        [DisplayName("更新时间")]
        [Property]
        virtual public DateTime UpdateTime { get; set; }

        public override string ToString()
        {
            return _ShopGroups.ToString();
        }
    }
}
