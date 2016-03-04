using System;
using System.ComponentModel.DataAnnotations;
using hkkf.Common.MVC;
using System.Web.Mvc;

namespace hkkf.Common.Validations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CannotAfterTodayAttribute : ValidationAttribute, IClientScriptable
    {
        public CannotAfterTodayAttribute() : base("{0} 不能晚于今天") { }

        public CannotAfterTodayAttribute(string errorMessage) : base(errorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is DateTime == false) return true;
            return ((DateTime)value).Date <= DateTime.Today;
        }

        public MvcHtmlString GenerateScriptForProperty(string propertyName)
        {
            var s = string.Format("$('#{0}').datepicker('option', 'maxDate', '+0d');", propertyName);
            return MvcHtmlString.Create(s);
        }
    }
}
