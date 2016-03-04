using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("枚举值")]
    [Class(Table = "Enm_CommonValues", NameType = typeof(EnmCommonValue))]
    public class EnmCommonValue : DomainModel
    {
        [Id(Name = "ID", Column = "CommonValueID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        //[QuickQuery(true, 1)]
        [DisplayName("枚举编号")]
        [ManyToOne(Column = "CommonID")]
        virtual public EnmCommon CommonID { get; set; }

        //[QuickQuery(true, 20)]
        [DisplayName("枚举值")]
        [Property(Column = "EnmTextValue")]
        virtual public string EnmTextValue { get; set; }

        //[QuickQuery(true, 10)]
        [DisplayName("枚举值编号")]
        [Property]
        virtual public int EnmValue { get; set; }

        [DisplayName("数据类型")]
        [Property]
        virtual public string DataType { get; set; }

        public override string ToString()
        {
            return EnmTextValue.ToString();
        }
    }
}
