using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
//    CREATE TABLE [dbo].[ShopTemplet](
//    [ShopTempletID] [int] IDENTITY(1,1) NOT NULL,
//    [ShopTempletName] [nvarchar](50) NULL,
//    [DepartMentID] [int] NULL,
//    [ShopTempletTypeID] [int] NULL,
//    [SpecialDate] [datetime] NULL,
//    [ifHistory] [bit] NULL,
// CONSTRAINT [PK_ShopTemplet] PRIMARY KEY CLUSTERED 
//(
//    [ShopTempletID] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
//) ON [PRIMARY]
        [DisplayName("店铺组")]
        [Class(Table = "ShopTemplet", NameType = typeof(Models.ShopTemplet))]
    public class ShopTemplet : DomainModel
        {
            [Id(Name = "ID", Column = "ShopTempletID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("模板名")]
            [Property]
            virtual public string ShopTempletName { get; set; }

            [DisplayName("部门")]
            [ManyToOne(Column = "DepartMentID")]
            virtual public Kf_DepartMent _Kf_DepartMent { get; set; }

            [DisplayName("模板类型")]
            [Property]
            virtual public _ShopTempletType ShopTempletTypeID { get; set; }

            [DisplayName("使用时间")]
            [Property]
            virtual public DateTime SpecialDate { get; set; }

            [DisplayName("是否过期")]
            [Property]
            virtual public isExpire isExpire { get; set; }

            [DisplayName("是否有效")]
            [Property]
            virtual public isValid isValid { get; set; }

            [DisplayName("更新时间")]
            [Property]
            virtual public DateTime UpdateTime { get; set; }


            public override string ToString()
            {
                return ShopTempletName;
           }
        } 


    
  
}
