using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using hkkf.Common.Attributes;
//using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
        //[TypeInfo("ID", "Name")]
        [DisplayName("学生")]
        [Class(Table = "Student", NameType = typeof(Models.Student))]
        public class Student : DomainModel
        {
            [Id(Name = "ID", Column = "StudentID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("学生姓名ss")]
            [Required(ErrorMessage = "{0} 是必填的")]
            [Property]
            virtual public string StudentName { get; set; }

            [DisplayName("年龄sfds")]
            [Required(ErrorMessage = "{0} 是必填的")]
            [Property]
            virtual public int StudentAge { get; set; }

            [DisplayName("地址df")]
            [Required(ErrorMessage = "{0} 是必填的")]
            [Property]
            [PasswordPropertyText]
            virtual public string Address { get; set; }

            [DisplayName("学生类别")]
            [ManyToOne(Column = "TypeID")]
            virtual public ShopType Type { get; set; }

            [DisplayName("开始日期")]
            [Property]
            virtual public DateTime StartDay { get; set; }

            [DisplayName("结束日期")]
            [Property]
            virtual public DateTime EndDay { get; set; }

            [DisplayName("性别")]
            [Property]
            virtual public Boolean Sex { get; set; }

            public override string ToString()
            {
                return StudentName;
            }

        } 


    
  
}
