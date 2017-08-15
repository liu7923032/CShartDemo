using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NineFour.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class MobileAttribute : Attribute, IValidate
    {
        public bool IsValidate(object value)
        {
            Regex r = new Regex(@"^1[3|4|5|8][0-9]\d{4,8}$");

            return r.IsMatch(value.ToString());
        }

        public string ErrorMsg { get; set; } = "手机号码格式不正确";
    }
}
