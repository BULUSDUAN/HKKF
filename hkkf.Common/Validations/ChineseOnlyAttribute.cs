using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace hkkf.Common.Validations
{
   public class ChineseOnlyAttribute : RegularExpressionAttribute 
    {
       static ChineseOnlyAttribute()
       {
           DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(
               typeof(ChineseOnlyAttribute), (metadata, context, attribute) =>
                   new RegularExpressionAttributeAdapter(metadata, context, (RegularExpressionAttribute)attribute)
           );
       }

       public ChineseOnlyAttribute() : base(@"^[\u4e00-\u9fa5]+$")
       {
           ErrorMessage = "只允许中文";
       }
    }
}
