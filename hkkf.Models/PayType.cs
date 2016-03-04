using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
//using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("付款类型")]
    [Class(Table = "PayType", NameType = typeof(PayType))]
    public class PayType : DomainModel
    {
        [Id(Name = "ID", Column = "PayTypeID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("付款类型")]
        [Property]
        virtual public string PayTypeName { get; set; }
       
        public override string ToString()
        {
            return PayTypeName;
        }
    } 
}
