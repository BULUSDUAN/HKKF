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
    [TypeInfo("ID", "Name")]
    [DisplayName("店铺表")]
    [Class(Table = "Shops", NameType = typeof(Shop))]
    public class Shop : DomainModel
    {
    //[ShopID] [int] IDENTITY(1,1) NOT NULL,
    //[Name] [nvarchar](50) NULL,
    //[Tel] [nvarchar](50) NULL,
    //[QQ] [nvarchar](50) NULL,
    //[ElseTel] [nvarchar](50) NULL,
    //[TypeID] [int] NULL,  
    
    //[JiaoJiePerson] [nchar](10) NULL,
        [Id(Name = "ID", Column = "ShopID")]
        [Generator(1, Class = "native")]
        virtual public int ID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺名")]
        [Property]
        virtual public string Name { get; set; }
       

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店长姓名")]
        [Property]
        virtual public string ContractPerson { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("手机")]
        [Property]
        virtual public string Tel { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("QQ")]
        [Property]
        virtual public string QQ { get; set; }

        
        [DisplayName("其他联系方式")]
        [Property]
        virtual public string ElseTel { get; set; }

        [DisplayName("店铺类别")]
        [ManyToOne(Column = "TypeID")]
        virtual public ShopType Type { get; set; }


        [DisplayName("客服组数")]
        [Property]
        virtual public int GroupCount { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("服务类型")]
        [Property(Name = "ServiceTypeID", Column = "ServiceTypeID")]
        virtual public ServiceType ServiceTypeID { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺链接")]
        [Property]
        virtual public string Link { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("值班类型")]
        [Property(Name = "ZhiBanTypeID", Column = "ZhiBanTypeID")]
        virtual public ZhiBanType ZhiBanTypeID { get; set; }

        //[GroupCount] [int] NULL,
        //[ServiceTypeID] [int] NULL,
        //[Link] [nvarchar](200) NULL,
        //[ZhiBanTypeID] [int] NULL,
        [DisplayName("月度费用")]
        [Property]
        virtual public int PriceByMonth { get; set; }



        //[PriceByMonth] [int] NULL,
        //[PayCircleID] [int] NULL,

        //[IsSpecialTime] [bit] NULL,
        //[SeviceDate] [int] NULL,
        //[Beizhu] [nvarchar](300) NULL,
       
        [DisplayName("付款周期")]
        [Property(Name = "_PayCircle", Column = "PayCircleID")]
        virtual public _PayCircle _PayCircle { get; set; }

        [DisplayName("提成百分比")]
        [Property]
        virtual public int TiChengRate { get; set; }

        [DisplayName("固定提成")]
        [Property]
        virtual public int FixedTiCheng { get; set; }

        [Bool("是", "否")]
        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("是否有特殊的服务时间")]
        [Property]
        virtual public bool? IsSpecialTime { get; set; }


        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("服务期限(按月)")]
        [Property]
        virtual public int SeviceDate { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("备注")]
        [Property]
        virtual public string Beizhu { get; set; }


        [DisplayName("所属部门")]
        [ManyToOne(Column = "DepartMentID")]
        virtual public Kf_DepartMent _Kf_DepartMent { get; set; }

        [DisplayName("销售人员")]
        [ManyToOne(Column = "SaleUserID")]
        virtual public User SaleUser { get; set; }

        [DisplayName("管理人员")]
        [ManyToOne(Column = "DemandUserID")]
        virtual public User DemandUser { get; set; }

        [DisplayName("主客服")]
        [ManyToOne(Column = "MainKfUserID")]
         virtual public User MainKfUser { get; set; }

        [DisplayName("用户")]
        [ManyToOne(Column = "UserID")]
        virtual public User _User { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("交接时间")]
        [Property]
        virtual public DateTime? JiaojieTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("合同签订日期")]
        [Property]
        virtual public DateTime? ContactQianTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺成立时间")]
        [Property]
        virtual public DateTime? ShopSelfTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("服务终止时间")]
        [Property]
        virtual public DateTime? HezuoEndTime { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("合作上号正式开始时间")]
        [Property]
        virtual public DateTime? HezuoStartTime { get; set; }

        [DisplayName("咨询量")]
        [ManyToOne(Column = "CountLevelID")]
        virtual public ShopCountLevel ShopCountLevel { get; set; }

        [DisplayName("难易复杂度")]
        [ManyToOne(Column = "DifficutyLevelID")]
        virtual public ShopDifficultyLevel DifficutyLevel { get; set; }

        [DisplayName("总分值")]
        [Property]
        virtual public int TotalScore { get; set; }

     
        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("交接人")]
        [Property]
        virtual public string JiaojiePerson { get; set; }

        [Required(ErrorMessage = "{0} 是必填的")]
        [DisplayName("店铺状态")]
        [Property(Name = "ShopStateID", Column = "ShopStateID")]
        virtual public ShopStates ShopStateID { get; set; }
      
        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
