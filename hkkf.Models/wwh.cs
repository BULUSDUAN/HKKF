using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
//using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("旺旺号表")]
    [Class(Table = "wwhs", NameType = typeof(wwh))]
    public class wwh:DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺名称")]
        [ManyToOne(Column = "ShopID")]
        virtual public Shop _Shop { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("旺旺号")]
        [Property]
        virtual public string Name { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("密码")]
        [Property]
        virtual public string Password { get; set; }

    }
}
