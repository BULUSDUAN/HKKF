using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;

namespace hkkf.Models
{
   
        [DisplayName("学生")]
        [Class(Table = "User_Enm_Levels", NameType = typeof(Models.UserEnmLevel))]
        public class UserEnmLevel : DomainModel
        {
            //ID,UserID,Year,Month,DayNum,NightNum,TotalNum,zhiBanSalary,TotalSalary
    //        [User_Enm_Levels](
    //[ID] [int] IDENTITY(1,1) NOT NULL,
    //[UserEnmTypeID] [int] NULL,
    //[UserLevelName] [nchar](10) NULL,
    //[UserLevelSalary] [int] NULL,
    //[BaseSalary] [int] NULL,
    ////[Memo] [nvarchar](50) NULL,
    //        [Id(Name = "ID", Column = "TypeID")]
    //        [Generator(1, Class = "native")]
    //        virtual public int ID { get; set; }


            [Id(Name = "ID", Column = "LevelID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("用户类型")]
            [ManyToOne(Column = "UserEnmTypeID")]
            virtual public User_Enm_Type UserEnmType { get; set; }

            [DisplayName("用户等级名称")]
            [Property]
            virtual public string UserLevelName { get; set; }

            [DisplayName("等级工资")]
            [Property]
            virtual public int UserLevelSalary { get; set; }

            [DisplayName("基础工资")]
            [Property]
            virtual public int BaseSalary { get; set; }

            [DisplayName("备注")]
            [Property]
            virtual public string Memo { get; set; }    

            public override string ToString()
            {
                return UserLevelName;
            }

        } 
}
