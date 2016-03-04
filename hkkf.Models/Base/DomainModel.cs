using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hkkf.Models
{
    abstract public class DomainModel
    {
        public override string ToString()
        {
            return string.Format("请在类 {0} 中重写 ToString 方法", this.GetType());
        }
    }
}
