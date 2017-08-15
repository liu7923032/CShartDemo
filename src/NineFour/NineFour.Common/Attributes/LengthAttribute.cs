using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFour.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class LengthAttribute:Attribute,IValidate
    {

        public LengthAttribute(int min,int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public string ErrorMsg { get; set; } = "长度不符合要求";

        public int Min { get; set; }

        public int Max { get; set; }


        public bool IsValidate(object value)
        {
            string strValue = value.ToString();
            return strValue.Length >= this.Min && strValue.Length <= this.Max;
        }
    }
}
