using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace hkkf.Common.Validations
{
    /// <summary>
    /// 禁止全角
    /// </summary>
    public class ForbidSBCAttribute: RegularExpressionAttribute
    {
        static ForbidSBCAttribute()
       {
           DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(
               typeof(ForbidSBCAttribute), (metadata, context, attribute) =>
                   new RegularExpressionAttributeAdapter(metadata, context, (RegularExpressionAttribute)attribute)
           );
       }
        public ForbidSBCAttribute()
            : base(@"[^\uFF00-\uFFFF]*")
        {
            ErrorMessage = "请使用半角字符";
        }
    }
}
