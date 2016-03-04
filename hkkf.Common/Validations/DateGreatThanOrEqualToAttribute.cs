using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;
using hkkf.Common.MVC;
using System.Web.Mvc;

namespace hkkf.Common.Validations
{
    /// <summary>
    /// 日期大于等于
    /// </summary>
    public class DateGreatThanOrEqualToAttribute : ValidationAttribute, IClientScriptable
    {
        public DateTime Date { get; private set; }

        public DateGreatThanOrEqualToAttribute(int year, int month = 1, int day = 1)
        {
            this.Date = new DateTime(year, month, day);
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime == false) return true;

            var v = (DateTime)value;

            return v >= Date;
        }


        public MvcHtmlString GenerateScriptForProperty(string propertyName)
        {
            var s = string.Format(
                "$('#{0}').datepicker('option', 'minDate', new Date({1}, {2} - 1, {3}));",
                propertyName,
                Date.Year,
                Date.Month,
                Date.Day
                );
            return MvcHtmlString.Create(s);
        }
    }
}
