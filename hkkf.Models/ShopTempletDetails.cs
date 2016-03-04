using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
 //   CREATE TABLE [dbo].[ShopTempletDetails](
 //   [ID] [int] IDENTITY(1,1) NOT NULL,
 //   [ShopTempletID] [int] NULL,
 //   [ShopGroupID] [int] NULL,
 //   [UpdateTime] [datetime] NULL,
 //CONSTRAINT [PK_ShopTempletDetails] PRIMARY KEY CLUSTERED    
        [DisplayName("店铺模板详情")]
        [Class(Table = "ShopTempletDetails", NameType = typeof(Models.ShopTempletDetails))]
    public class ShopTempletDetails : DomainModel
        {
            [Id(Name = "ID", Column = "ID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("店铺模板")]
            [ManyToOne(Column = "ShopTempletID")]
            virtual public ShopTemplet _ShopTemplet { get; set; }


            [DisplayName("班组")]
            [ManyToOne(Column="ShopGroupID")]
            virtual public ShopGroups _ShopGroup { get; set; }

            [DisplayName("更新时间")]
            [Property]
            virtual public DateTime UpdateTime { get; set; }


            public override string ToString()
            {
                return _ShopTemplet.ToString() + _ShopGroup.ToString();
           }
        }    
}
