using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;
//using JieNuo.ComponentModel;

namespace hkkf.Models
{

        //[TypeInfo("ID", "userName")]
       // [TypeInfo("ID","Name")]
        [DisplayName("店铺类型")]
        [Class(Table = "Shop_Enm_Types", NameType = typeof(Models.ShopType))]
        public class ShopType : DomainModel, IEnumerationItem
        {
            [Id(Name = "ID", Column = "TypeID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("店铺类型名称")]
            [Property]
            virtual public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }  
}
