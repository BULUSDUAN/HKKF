using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hkkf.Models
{
    //店铺付款类型
    public enum _PayType
    {
        基础服务费 = 1,
        提成 = 2,
    }
    //店铺付款周期
    public enum _PayCircle
    {
        月付 = 1,
        季付 = 2,
        半年 = 3,
        一年=  4,
    }
    //店铺状态
    public enum ShopStates
    {
        正常服务 = 1,
        停止续费 = 2,
        暂停 = 3,
    }
    //是否过期
    public enum isExpire
    {
        过期 = 1,
        未过期 = 0,
    }

    //是否有效
    public enum isValid
    {
        有效 = 1,
        无效 = 0,
    }

    //婚姻状况
    public enum MarryStates
    {
        未婚 = 1,
        已婚 = 2,
        离异 = 3,
    }

    //证件类型
    public enum IdentityTypes
    {
        身份证 = 1,
        护照 = 2,
        港澳台居民往来大陆通行证 = 3,
        户口薄 = 4,
    }

    //学历类型
    public enum EduTypes
    {
        本科 = 1,
        专科 = 2,
        中专 = 3,
        研究生 = 4,
        博士 = 5,
        硕士 = 6,
    }

    //职业类型
    public enum ZYTypes
    {
        自营业主 = 1,
        公司职员 = 2,
        公务员 = 3,
        离退休人士 = 4,
        自由职业 = 5,
        其他 = 6,
    }

    //单位规模类型
    public enum CompanyGMTypes
    {
        一百人以下 = 1,
        一百至一百五十人 = 2,
        一百五至一千人 = 3,
        一千至三千人 = 4,
        三千人以上 = 5,
    }

    //您希望通过哪种方式接收债权方式类型
    public enum FileRequestTypes
    {
        信件 = 1,
        电子邮件 = 2,
        两者都选 = 3,
    }

    //拜访客户类型
    public enum PostType
    {
        无 = 0,
        电话 = 1,
        上门 = 2,
        到访 = 3,

    }

    //本利回收方式
    public enum HSTpype
    {
        每月返息百分之一到期返本及剩余收益 = 1,
        到期返本付息 = 2,
    }



    public enum Years
    {
        二零零六年度 = 2006,
        二零零七年度 = 2007,
        二零零八年度 = 2008,
        二零零九年度 = 2009,
        二零一零年度 = 2010,
        二零一一年度 = 2011,
        二零一二年度 = 2012,
        二零一三年度 = 2013,
        二零一四年度 = 2014,
        二零一五年度 = 2015,
        二零一六年度 = 2016,
        二零一七年度 = 2017,
        二零一八年度 = 2018,
        二零一九年度 = 2019,
        二零二零年度 = 2020,
        二零二一年度 = 2021,
        二零二二年度 = 2022,
    }


    public enum DayOrNight
    {
        白班 = 1,
        晚班 = 2,
        全天 = 3,
        休班 = 4,
    }

    public enum ServiceType
    {
        售前客服 = 1,
        售后客服 = 2,
    }

    public enum _ShopTempletType
    {
        平时 = 1,
        周末 = 2,
        假期 = 3,
        特定日期 = 4,
    }

    public enum ZhiBanType
    {
        全托=1,
        仅白班=2,
        仅夜班=3,
        周末=4,
        夜班加周末=5,
    }


    public enum WenTiType
    {
        客服要求 = 1,
        库存 = 2,
        发货 = 3,
        物流 = 4,
        付款方式 = 5,
        促销活动 =6,
        售后 = 7,
        其它 = 8,
    }


    public enum PExamType
    {
        单选题=1,
        多选题=2,
    }
   
}
