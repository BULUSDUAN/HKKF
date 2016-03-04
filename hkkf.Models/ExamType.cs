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
    [Class(Table = "ExamTypes", NameType = typeof(ExamType))]
   public class ExamType:DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("试卷类型名")]
        [Property]
        virtual public string EName { get; set; }

        public override string ToString()
        {
            return EName;
        }
    }
}
