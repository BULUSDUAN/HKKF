using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel;

namespace hkkf.Common.Validations
{
    public class DateBeforeOtherAttribute
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
        public sealed class ComparisonDateAttribute : ValidationAttribute
        {
            private const string _defaultErrorMessage = "'{0}' 不能晚于 '{1}'  .";
            private readonly object _typeId = new object();

            public ComparisonDateAttribute(string originalProperty, string confirmProperty)
                : base(_defaultErrorMessage)
            {
                OriginalProperty = originalProperty;
                ConfirmProperty = confirmProperty;
            }

            public string ConfirmProperty { get; private set; }
            public string OriginalProperty { get; private set; }

            public override object TypeId
            {
                get
                {
                    return _typeId;
                }
            }

            public override string FormatErrorMessage(string name)
            {
                return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                    OriginalProperty, ConfirmProperty);
            }

            public override bool IsValid(object value)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
                if (value == null)
                {
                    return true;
                }
                object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
                object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
                if (originalValue == null || confirmValue == null)
                {
                    return true;
                }
                return (DateTime)originalValue >= (DateTime)confirmValue;
            }
        }
    }
}
