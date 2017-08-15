using System;
using System.Collections.Generic;
using System.Text;

namespace NineFour.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredAttribute : Attribute, IValidate
    {
        public bool IsValidate(object value)
        {
            //如果对象不为空并且值不为空字符串
            return value != null && !string.IsNullOrEmpty(value.ToString().Trim());
        }

        public string ErrorMsg { get; set; } = "字段为必填项";
    }
}
