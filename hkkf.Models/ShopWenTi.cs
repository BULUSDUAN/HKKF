using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;
using JieNuo.ComponentModel;

namespace hkkf.Models
{
    [DisplayName("店铺问题表")]
    [Class(Table = "ShopWenTis", NameType = typeof(ShopWenTi))]
   public class ShopWenTi:DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺名称")]
        [ManyToOne(Column = "ShopID")]
        virtual public Shop _Shop { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("问题类别")]
        [Property(Name = "wtType", Column = "wtType")]
        virtual public WenTiType wtType { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("问题内容")]
        [Property]
        virtual public string NContent { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("问题答案")]
        [Property]
        virtual public string NAnswer { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("更新时间")]
        [Property]
        virtual public DateTime UpdateTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("更新人")]
        [ManyToOne(Column="UpdateUserID")]
        virtual public User _User { get; set; }
    }
}
