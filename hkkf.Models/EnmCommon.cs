using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models;

using NHibernate.Mapping.Attributes;
using I = Iesi.Collections.Generic;
using N = NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    [DisplayName("枚举")]
    [Class(Table = "Enm_Commons", NameType = typeof(EnmCommon))]
    public class EnmCommon : DomainModel
    {
        [Id(Name = "ID", Column = "CommonID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("名称")]
        [Property]
        virtual public string Name { get; set; }

        [DisplayName("英文名")]
        [Property]
        virtual public string EnglishName { get; set; }

        [DisplayName("值")]
        [Set(Table = "Enm_CommonValues", Cascade = "all-delete-orphan", Lazy = CollectionLazy.False, Fetch = CollectionFetchMode.Subselect)]
        [N.Key(1, Column = "CommonID")]
        [OneToMany(2, ClassType = typeof(EnmCommonValue))]
        virtual public I.ISet<EnmCommonValue> Values { get; set; }

        [DisplayName("是否为系统枚举表")]
        [Property]
        virtual public bool IsSystem { get; set; }

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
