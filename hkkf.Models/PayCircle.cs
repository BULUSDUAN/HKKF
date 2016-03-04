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
    [DisplayName("付款周期")]
    [Class(Table = "PayCircle", NameType = typeof(PayCircle))]
    public class PayCircle : DomainModel
    {
        [Id(Name = "ID", Column = "PayCircleID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("付款周期")]
        [Property]
        virtual public string PayCircleName { get; set; }
       
        public override string ToString()
        {
            return PayCircleName;
        }
    } 
}
