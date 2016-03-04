using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
   
        [DisplayName("客服排班日期")]
        [Class(Table = "UserWorkDate", NameType = typeof(Models.UserWorkDate))]
    public class UserWorkDate : DomainModel
        {
            [Id(Name = "ID", Column = "ID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("排班日期")]
            [Property]
            virtual public DateTime WorkDate { get; set; }

            [DisplayName("年")]
            [Property]
            virtual public int Year { get; set; }

            [DisplayName("月")]
            [Property]
            virtual public int Month { get; set; }

            [DisplayName("更新时间")]
            [Property]
            virtual public DateTime UpdateTime { get; set; }

            //[DisplayName("更新人")]
            //[Property]
            //[ManyToOne(Column="UpdateUserID")]
            //virtual public User UpdateUser { get; set; }

            public override string ToString()
            {
                return WorkDate.ToShortDateString();
           }
        } 


    
  
}
