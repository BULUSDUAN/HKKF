using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hkkf.Common.Attributes
{

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class EnumerationAttribute : Attribute
    {
        public string EnglishName { get; private set; }

        public string DefaultText { get; set; }

        public EnumerationAttribute(string englishName)
        {
            this.EnglishName = englishName;
        }
    }
}
