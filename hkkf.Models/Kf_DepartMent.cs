using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
   
        [DisplayName("客服部门")]
        [Class(Table = "Kf_DepartMent", NameType = typeof(Models.Kf_DepartMent))]
    public class Kf_DepartMent : DomainModel
        {
            [Id(Name = "ID", Column = "DepartMentID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("部门名称")]
            [Property]
            virtual public string DepartMentName { get; set; }

            public override string ToString()
            {
                return DepartMentName;
           }
        } 


    
  
}
