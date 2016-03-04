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
    [DisplayName("排班数据表")]
    [Class(Table = "PersonPBData", NameType = typeof(PersonPBData))]
    public class PersonPBData : DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("用户")]
        [ManyToOne(Column = "UserID")]
        virtual public User _user { get; set; }


        [DisplayName("排班日期")]
        [ManyToOne(Column = "UserWorkDateID")]
        virtual public UserWorkDate UserWorkDate { get; set; }

        [DisplayName("值班类型")]
        [Property(Name = "WorkDayOrNight", Column = "DayOrNightID")]
        virtual public DayOrNight WorkDayOrNight { get; set; }

        [DisplayName("年")]
        [Property]
        virtual public int Year { get; set; }

        [DisplayName("月")]
        [Property]
        virtual public int Month { get; set; }

        [DisplayName("客服组数")]
        [Property]
        virtual public int DayNumMonth { get; set; }

        [DisplayName("客服组数")]
        [Property]
        virtual public int NightNumMonth { get; set; }

        [DisplayName("客服组数")]
        [Property]
        virtual public int RestNumMonth { get; set; }

        [DisplayName("更新时间")]
        [Property]
        virtual public DateTime UpdateTime { get; set; }
    }
}
