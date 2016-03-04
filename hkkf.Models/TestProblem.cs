using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;
using Spring.Validation;

namespace hkkf.Models
{
    
    [DisplayName("客服考试表")]
    [Class(Table = "TestProblem", NameType = typeof(TestProblem))]
    public class TestProblem : DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("用户")]
        [ManyToOne(Column = "userid")]
        virtual public User userid { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("开始日期")]
        [Property]
        virtual public DateTime startTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("结束日期")]
        [Property]
        virtual public DateTime endTime { get; set; }

        [DisplayName("问题")]
        [ManyToOne(Column = "problemid")]
        virtual public ExamPages problemid { get; set; }
        
     
        [DisplayName("答案记录")]
        [Property]
        virtual public string thisanswer { get; set; }

        [DisplayName("题号")]
        [Property]
        virtual public int tihao { get; set; }

    }
}
