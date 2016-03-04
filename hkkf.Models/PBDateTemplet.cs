using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using hkkf.Common.Attributes;
using NHibernate.Mapping.Attributes;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
    [DisplayName("排班日期模板表")]
    [Class(Table = "PBDateTemplet", NameType = typeof(PBDateTemplet))]
    public class PBDateTemplet : DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("日期")]
        [ManyToOne(Column = "UserWorkDateID")]
        virtual public UserWorkDate _UserWorkDate { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("排班模板")]
        [ManyToOne(Column = "ShopTempletID")]
        virtual public ShopTemplet _ShopTemplet { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("更新日期")]
        [Property]
        virtual public DateTime UpdateTime { get; set; }       
    }
}
