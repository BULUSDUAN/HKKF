using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("客服角色")]
    [Class(Table = "Kf_Role_Type", NameType = typeof(Kf_Role_Type))]
    public class Kf_Role_Type : DomainModel
    {
        [Id(Name = "ID", Column = "ID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("客服角色")]
        [Property]
        virtual public string RoleName { get; set; }

        public override string ToString()
        {
            return RoleName;
        }
    } 
}
