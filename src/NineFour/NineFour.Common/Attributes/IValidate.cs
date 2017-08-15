using System;
using System.Collections.Generic;
using System.Text;

namespace NineFour.Common.Attributes
{
    public interface IValidate
    {
        bool IsValidate(object value);
        string ErrorMsg { get; set; }
    }
}
