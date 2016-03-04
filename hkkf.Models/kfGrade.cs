using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("客服考试成绩表")]
    [Class(Table = "kfGrade", NameType = typeof(kfGrade))]
    public class kfGrade
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("用户")]
        [ManyToOne(Column = "userid")]
        virtual public User userid { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("日期")]
        [Property]
        virtual public DateTime time { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("成绩")]
        [Property]
        virtual public string grade { get; set; }

       

    }
}
