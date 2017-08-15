using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.Common.Attibutes
{
    /// <summary>
    /// 方法执行完成后自动完成提交的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method )]
    public class AutoCommitAttribute : Attribute
    {
        //默认自动提交
        public bool Auto { get; set; } = true;
    }
}
