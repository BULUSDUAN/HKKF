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

        //[TypeInfo("ID", "userName")]
       // [TypeInfo("ID","Name")]
        [DisplayName("店铺模板类型")]
        [Class(Table = "ShopTempletType", NameType = typeof(Models.ShopTempletType))]
    public class ShopTempletType : DomainModel
        {
            [Id(Name = "ID", Column = "ShopTempletTypeID")]
            [Generator(1, Class = "native")]
            virtual public int ID { get; set; }

            [DisplayName("店铺模板类型")]
            [Property]
            virtual public string ShopTempletTypeName { get; set; }

            public override string ToString()
            {
                return ShopTempletTypeName;
            }
        }  
}
