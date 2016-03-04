using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
   
        [DisplayName("店铺咨询量等级")]
        [Class(Table = "ShopCountLevel", NameType = typeof(Models.ShopCountLevel))]
        public class ShopCountLevel : DomainModel
        {
            [Id(Name = "ID", Column = "CountLevelID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("等级名称")]
            [Property]
            virtual public string CountLevel { get; set; }

            [DisplayName("等级分数")]
            [Property]
            virtual public string CountLevelScore { get; set; }

            [DisplayName("备注")]
            [Property]
            virtual public string Memo { get; set; }

            public override string ToString()
            {
                return Memo;
           }
        } 


    
  
}
