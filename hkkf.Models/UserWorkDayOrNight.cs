using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
   
        [DisplayName("白班晚班")]
        [Class(Table = "UserWorkDayOrNight", NameType = typeof(Models.UserWorkDayOrNight))]
    public class UserWorkDayOrNight : DomainModel
        {
            [Id(Name = "ID", Column = "ID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("等级名称")]
            [Property]
            virtual public string DayOrNight { get; set; }

            public override string ToString()
            {
                return DayOrNight.ToString();
           }
        } 


    
  
}
