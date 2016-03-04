using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
   
        [DisplayName("店铺咨询难度等级")]
        [Class(Table = "ShopDifficultyLevel", NameType = typeof(Models.ShopDifficultyLevel))]
      public class ShopDifficultyLevel : DomainModel
        {
            [Id(Name = "ID", Column = "ShopDifficultyLevelID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("难易等级")]
            [Property]
            virtual public string ShopDifficultyLevelName { get; set; }

            [DisplayName("难易分数")]
            [Property]
            virtual public string ShopDifficultyLevelScore { get; set; }

            [DisplayName("备注")]
            [Property]
            virtual public string Memo { get; set; }

            public override string ToString()
            {
                return ShopDifficultyLevelName;
           }
        } 


    
  
}
