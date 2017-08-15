using System;
using System.Collections.Generic;
using System.Text;

namespace NineFour.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property,AllowMultiple =true,Inherited =true)]
    public class RemarkAttribute:Attribute
    {
        public RemarkAttribute(string remark)
        {
            this.Remark = remark;
        }

        public string Remark { get; set; }
    }
}
