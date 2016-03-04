using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("记录日期表")]
    [Class(Table = "DayDatas", NameType = typeof(DaysData))]
    public class DaysData : DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("开始日期")]
        [Property]
        virtual public DateTime BeginDateTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("结束日期")]
        [Property]
        virtual public DateTime EndDateTime { get; set; }



    }
}
