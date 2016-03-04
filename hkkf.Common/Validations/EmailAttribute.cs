using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace hkkf.Common.Validations
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        static EmailAttribute()
       {
           DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(
               typeof(EmailAttribute), (metadata, context, attribute) =>
                   new RegularExpressionAttributeAdapter(metadata, context, (RegularExpressionAttribute)attribute)
           );
       }
        public EmailAttribute()
            : base(@"^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$")
        {
            ErrorMessage = "请输入正确格式的Email,如zhuce@126.com";
        }
    }
}
