using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("试卷类型")]
    [Class(Table = "ExamPages", NameType = typeof(ExamPages))]
    public class ExamPages:DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("试卷类型")]
        [ManyToOne(Column = "ETypeID")]
        virtual public ExamType ETypeID { get; set; }



        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("题目类型")]
        [Property(Name = "PTypeID", Column = "PTypeID")]
        virtual public PExamType PTypeID { get; set; }

       
        [DisplayName("店铺")]
        [ManyToOne(Column = "ShopID")]
        virtual public Shop _Shop { get; set; }



        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("题目")]
        [Property]
        virtual public string Title { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("答案Ａ")]
        [Property]
        virtual public string AnswerA { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("答案Ｂ")]
        [Property]
        virtual public string AnswerB { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("答案C")]
        [Property]
        virtual public string AnswerC { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("答案D")]
        [Property]
        virtual public string AnswerD { get; set; }

     
        [DisplayName("答案E")]
        [Property]
        public virtual string AnswerE { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("正确答案")]
        [Property]
        virtual public string TrueAnswer { get; set; }
        
        
    }
}
