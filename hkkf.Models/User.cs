
namespace hkkf.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using hkkf.Common.Attributes;
    using NHibernate.Mapping.Attributes;
    using JieNuo.ComponentModel;
    
    [TypeInfo("ID", "userName")]
    [DisplayName("用户表")]
    [Class(Table = "Users", NameType = typeof(hkkf.Models.User))]
    public class User : DomainModel
    {
        [QuickQuery(false, 5)]
        [Id(Name = "ID", Column = "UserID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("用户名")]
        [Property]
        virtual public string Name { get; set; }

        [QuickQuery(true, 5)]
        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("姓名")]
        [Property]
        virtual public string userName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} 是必填的")]
        [RegularExpression(@"\w{6,16}", ErrorMessage = "{0} 只能使用数字、英文，长度为6~16位")]
        [DisplayName("密码")]
        [Property]
        virtual public string Password { get; set; }


        [DisplayName("用户类别")]
        [ManyToOne(Column = "TypeID")]
        virtual public User_Enm_Type Type { get; set; }

        [DisplayName("用户等级")]
       [ManyToOne(Column = "LevelID")]
        virtual public UserEnmLevel UserEnmLevel { get; set; }


        [DataType(DataType.EmailAddress)]
        [DisplayName("邮箱")]
        [Property]
        virtual public string Email { get; set; }

        [QuickQuery(true, 5)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("手机号")]
        [Property]
        virtual public string Tel { get; set; }

        [DisplayName("个人QQ")]
        [Property]
        virtual public string PrivateQQ { get; set; }

        [DisplayName("工作QQ")]
        [Property]
        virtual public string workQQ { get; set; }


        [DisplayName("工作QQ昵称")]
        [Property]
        virtual public string WorkQQName { get; set; }

        //[RegistrationTime] [datetime] NULL,
        //[Tel] [nvarchar](50) NULL,
        //[PrivateQQ] [nvarchar](50) NULL,
        //[workQQ] [nchar](10) NULL,
        //[WorkQQName] [nvarchar](50) NULL,
        //[WorkQQPassword] [nvarchar](50) NULL,
        //[DepartMentID] [int] NULL,
        //[PartTimeOrFullTime] [char](4) NULL,
        [DisplayName("工作QQ密码")]
        [Property]
        virtual public string WorkQQPassword { get; set; }

        [DisplayName("部门")]
        [ManyToOne(Column = "DepartMentID")]
        virtual public Kf_DepartMent  DepartMent { get; set; }

        [DisplayName("注册时间")]
       // [DataFormat("yyyy-MM-dd")]
        [Property]
        virtual public DateTime RegistrationTime { get; set; }

        [DisplayName("员工状态")]
        [Property]
        virtual public UserEnmState UserStateID { get; set; }

        virtual public string strUserName
        {
            get 
            {
                if (UserStateID == UserEnmState.兼职)
                {
                    return userName + "(兼)";
                }
                if (UserStateID == UserEnmState.离职)
                {
                    return userName + "(离职)";
                }
                 return userName;                
            }
        }

        // 把用户类型转换成中文 sjw
        //virtual public string TypeToChina
        //{
        //    get
        //    {
        //        switch (Type.ID)
        //        {
        //            case 1:
        //                return "客服";
        //            case 2:
        //                return "管理员";
        //            case 3:
        //                return "财务";
        //            case 4:
        //                return "销售";

        //        }
        //        return "";
        //    }
        //}
        public override string ToString()
        {
            return userName.ToString();
        }


    }
}