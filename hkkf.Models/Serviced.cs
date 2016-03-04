using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("接口定时测试表")]
    [Class(Table = "Services", NameType = typeof(Serviced))]
    public class Serviced : DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("姓名")]
        [Property]
        virtual public string name { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("描述")]
        [Property]
        virtual public string Descript { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("插入数据时间")]
        [Property]
        virtual public DateTime UpDateTime { get; set; }
    }
}
