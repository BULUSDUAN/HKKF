using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
//using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
    [DisplayName("付款周期")]
    [Class(Table = "PayRequireRecords", NameType = typeof(PayRequireRecords))]
    public class PayRequireRecords : DomainModel
    {
        //ShopID,PayTypeID,PayDate,SaleVolume,PayNum,NextPayDate,NextPayNum,DemandUserID,ConfirmUserID

        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("店铺")]
        [ManyToOne(Column = "ShopID")]
        virtual public Shop _Shop { get; set; }

        [DisplayName("付款类型")]
        [Property(Name = "_PayType", Column = "PayTypeID")]
        virtual public _PayType _PayType { get; set; }

        [DisplayName("应付款日期")]
        [Property]
        virtual public DateTime PayRequireDate { get; set; }
   

        [DisplayName("付款金额")]
        [Property]
        virtual public int PayRequireNum { get; set; }

        [DisplayName("年")]
        [Property]
        virtual public int Year { get; set; }

        [DisplayName("月")]
        [Property]
        virtual public int Month { get; set; }    
      
        [DisplayName("更新日期")]
        [Property]
        virtual public DateTime UpdateTime { get; set; }       
       
        public override string ToString()
        {
            return _Shop.Name;
        }
    } 
}
