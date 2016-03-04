using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
   
        [DisplayName("客服工资表")]
        [Class(Table = "UserSalary", NameType = typeof(Models.UserSalary))]
        public class UserSalary : DomainModel
        {
            //ID,UserID,Year,Month,DayNum,NightNum,TotalNum,zhiBanSalary,TotalSalary

            [Id(Name = "ID", Column = "ID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("客服")]
            [ManyToOne(Column = "UserID")]
            virtual public User User { get; set; }

            [DisplayName("年")]
            [Property]
            virtual public int Year { get; set; }

            [DisplayName("月")]
            [Property]
            virtual public int Month { get; set; }

            [DisplayName("白班数量")]
            [Property]
            virtual public int DayNum { get; set; }

            [DisplayName("晚班数量")]
            [Property]
            virtual public int NightNum { get; set; }

            [DisplayName("值班数量")]
            [Property]
            virtual public int TotalNum { get; set; }

            [DisplayName("值班工资")]
            [Property]
            virtual public int zhiBanSalary { get; set; }

            [DisplayName("总工资")]
            [Property]
            virtual public int TotalSalary { get; set; }           


            public override string ToString()
            {
                return "";//User.userName;
            }

        } 


    
  
}
