using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("客服考试错题表")]
    [Class(Table = "Wrong", NameType = typeof(Models.Wrong))]
    public class Wrong
    {       
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        public virtual int ID { get; set; }

        [DisplayName("用户")]
        [ManyToOne(Column = "userid")]
        public virtual User userid { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("日期")]
        [Property]
        public virtual DateTime time { get; set; }


        [DisplayName("错误问题")]
        [ManyToOne(Column = "wrong")]
        public virtual ExamPages wrong { get; set; }

    }
}