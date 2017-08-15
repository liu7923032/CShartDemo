using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NineFour.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class EmailAttribute:Attribute,IValidate
    {
       
        public string ErrorMsg { get; set; } = "邮箱账号格式不符合要求";

        public bool IsValidate(object value)
        {
            Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            if (r.IsMatch(value.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
