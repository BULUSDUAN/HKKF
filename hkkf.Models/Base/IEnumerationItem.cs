using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hkkf.Models.Base
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    public interface IEnumerationItem
    {
        int ID { get; }
        string Name { get; }
    }
}
