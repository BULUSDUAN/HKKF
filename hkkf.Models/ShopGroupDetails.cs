using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
   
        [DisplayName("店铺组店铺对应表")]
        [Class(Table = "ShopGroupDetails", NameType = typeof(Models.ShopGroupDetails))]
    public class ShopGroupDetails : DomainModel
        {
            [Id(Name = "ID", Column = "DetailID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("店铺组")]
            [ManyToOne(Column = "ShopGroupID")]
            virtual public ShopGroups _ShopGroup { get; set; }


            [DisplayName("店铺")]
            [ManyToOne(Column="ShopID")]
            virtual public Shop _Shop { get; set; }

            [DisplayName("更新时间")]
            [Property]
            virtual public DateTime UpdateTime { get; set; }


            public override string ToString()
            {
                return _ShopGroup.ToString()+_Shop.ToString();
           }
        } 


    
  
}
