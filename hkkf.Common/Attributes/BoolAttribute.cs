using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hkkf.Common.Attributes
{
     [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
     public class BoolAttribute: Attribute
    {
        public string TextForTrue { get; private set; }
        public string TextForFalse { get; private set; }

        public string TextForNull { get; set; }

        public bool? Default { get; set; }

        public string Message { get; set; }

        public BoolAttribute(string textForTrue, string textForFalse)
        {
            this.TextForTrue = textForTrue;
            this.TextForFalse = textForFalse;
            Default = null;
        }
    }
}
