using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using NHibernate.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;
using hkkf.Common.Attributes;

namespace hkkf.Models
{

    [DisplayName("排班表")]
    [Class(Table = "PersonPBs", NameType = typeof(PersonPB))]
    public class PersonPB:DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("用户")]
        [ManyToOne(Column = "UserID")]
        virtual public User _user { get; set; }


        [DisplayName("店铺")]
        [ManyToOne(Column = "ShopID")]
        virtual public Shop _Shop { get; set; }
 
        [DisplayName("排班日期")]
        [ManyToOne(Column = "UserWorkDateID")]
        virtual public UserWorkDate UserWorkDate { get; set; }

         //[Bool("白班", "晚班")]
        //[DisplayName("白班/晚班")]
        //[Property(Name = "DayOrNight", Column = "DayOrNight")]
        //virtual public Ban DayOrNight { get; set; }

        [DisplayName("值班类型")]
        [Property(Name = "WorkDayOrNight", Column = "DayOrNightID")]
        virtual public DayOrNight WorkDayOrNight { get; set; }


        public virtual string DayShopName { get; set; }
        public virtual string NightShopName { get; set; }
    }
}
