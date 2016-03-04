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
    [DisplayName("合同表")]
    [Class(Table = "Contacts", NameType = typeof(Contact))]
    public class Contact : DomainModel
    {
        [Id(Name = "ID", Column = "ContactID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("合同名")]
        [Property]
        virtual public string CName { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺")]
        [ManyToOne(Column = "ShopID")]
        virtual public Shop shop { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("文件地址")]
        [Property]
        virtual public string FileUrl { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("用户")]
        [ManyToOne(Column = "userID")]
        virtual public User _user { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("上传时间")]
        [Property]
        virtual public DateTime? ContacTime { get; set; }


        
    }
}
