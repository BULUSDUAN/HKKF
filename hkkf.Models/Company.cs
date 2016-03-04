using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;

namespace hkkf.Models
{
    [TypeInfo("ID", "Name")]
    [DisplayName("单位信息表")]
    [Class(Table = "Companys", NameType = typeof (Company))]
    public class Company : DomainModel
    {

        //[QuickQuery(false, 10)]
        //[Id(Name = "ID", Column = "CompanyID")]
        //[Generator(1, Class = "native")]
        //virtual public int ID { get; set; }

       // [QuickQuery(false, 5)]
        [Id(Name = "ID", Column = "CompanyID")]
        [Generator(1, Class = "foreign")]
        [Param(2, Name = "property", Content = "User")]
        public virtual int ID { get; set; }

        

//        [QuickQuery(true, 20)]
        [Required]
        [DisplayName("单位名称")]
        [Property]
        public virtual string Name { get; set; }
    }
}