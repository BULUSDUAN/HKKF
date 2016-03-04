using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using hkkf.Common.Attributes;
using NHibernate.Mapping.Attributes;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
    [DisplayName("人员分配店铺表")]
    [Class(Table = "PersonShops", NameType = typeof(PersonShop))]
    public class PersonShop:DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("用户")]
        [ManyToOne(Column = "userID")]
        virtual public User _user { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("所拥有店铺")]
        [Property]
        virtual public string shopID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("日期")]
        [Property]
        virtual public DateTime? _DateTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("星期几")]
        [Property]
        virtual public string weeks { get; set; }


        [Bool("白班", "晚班")]
        [DisplayName("白班/晚班")]
        [Property(Name = "DayOrNight", Column = "DayOrNight")]
        virtual public DayOrNight DayOrNight { get; set; }

        

    }
}
