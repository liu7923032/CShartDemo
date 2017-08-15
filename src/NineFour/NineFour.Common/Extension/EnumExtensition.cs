using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NineFour.Common.Attributes;

namespace NineFour.Common.Extension
{
    public static class EnumExtension
    {
        public static string GetRemark(this Enum enumValue)
        {
            Type type = enumValue.GetType();
            //1：获取字段的特性
            RemarkAttribute attr = type.GetField(enumValue.ToString()).GetCustomAttribute(typeof(RemarkAttribute), true) as RemarkAttribute;
            if (attr == null)
            {
                return enumValue.ToString();
            }

            return attr.Remark;
        }
    }
}
