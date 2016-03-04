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
    [DisplayName("��ͬ��")]
    [Class(Table = "Contacts", NameType = typeof(Contact))]
    public class Contact : DomainModel
    {
        [Id(Name = "ID", Column = "ContactID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} �Ǳ����")]
        [DisplayName("��ͬ��")]
        [Property]
        virtual public string CName { get; set; }


        [Required(ErrorMessage = "{0} �Ǳ����")]
        [DisplayName("����")]
        [ManyToOne(Column = "ShopID")]
        virtual public Shop shop { get; set; }

        [Required(ErrorMessage = "{0} �Ǳ����")]
        [DisplayName("�ļ���ַ")]
        [Property]
        virtual public string FileUrl { get; set; }

        [Required(ErrorMessage = "{0} �Ǳ����")]
        [DisplayName("�û�")]
        [ManyToOne(Column = "userID")]
        virtual public User _user { get; set; }

        [Required(ErrorMessage = "{0} �Ǳ����")]
        [DisplayName("�ϴ�ʱ��")]
        [Property]
        virtual public DateTime? ContacTime { get; set; }


        
    }
}
