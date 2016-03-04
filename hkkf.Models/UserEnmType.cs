using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using hkkf.Models.Base;
using JieNuo.ComponentModel;
using NHibernate.Mapping.Attributes;

namespace hkkf.Models
{
    public  enum UserEnmType
    {
        [KeyValueInfo("HomePageUrl", "Service")]
        Person = 1,
        [KeyValueInfo("HomePageUrl", "Sale")]
        Sale = 2,
        [KeyValueInfo("HomePageUrl", "Finance")]
        Finance = 3,
        [KeyValueInfo("HomePageUrl", "Admin")]
        Admin = 4,
    }
    public static class UserTypeExtension
    {
        public static string HomePageUrl(this UserEnmType userType)
        {
            return AttributeHelper.GetNoInherit<KeyValueInfoAttribute>(userType)
                .Where(kv => kv.Key == "HomePageUrl")
                .Select(kv => kv.Value as string)
                .FirstOrDefault();
        }
    }

    [DisplayName("用户类型")]
    [Class(Table = "User_Enm_Types", NameType = typeof(User_Enm_Type))]
    public class User_Enm_Type : DomainModel
    {
        [Id(Name = "ID", Column = "TypeID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [DisplayName("用户类型名称")]
        [Property]
        virtual public string Name { get; set; }

        [DisplayName("链接")]
        [Property]
        virtual public string HomePageUrl { get; set; }

        public override string ToString()
        {
            return Name;
        }
    } 
}
