using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.Common.Attibutes
{
    /// <summary>
    /// 定义该字段是否是lucene 的Document对象的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DocFieldAttribute:System.Attribute
    {
        public bool IsDocField { get; set; } = true;
    }
}
