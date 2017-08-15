using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFour.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NumberAttribute : Attribute, IValidate
    {
        public string ErrorMsg { get; set; } = "非数字";

        public bool IsValidate(object value)
        {
            int i;
            return int.TryParse(value.ToString(), out i);
        }
    }
}
