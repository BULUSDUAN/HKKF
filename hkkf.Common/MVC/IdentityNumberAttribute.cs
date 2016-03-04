using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace hkkf.Common.MVC
{
    public class IdentityNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //如果不是字符串，认为是合法的
            if ((value is string) == false) return true;

            string id = value as string;
            if (id.Length != 17) return false;
            return true;
        }
        
    }
}
