using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.Common.Extend
{
    /// <summary>
    /// 属性的扩展方法
    /// </summary>
    public static class PropertyInfoExtend
    {
        public static T GetCustomAttribute<T>(this PropertyInfo property) where T : System.Attribute
        {
            //接着检查该字段是否需要进行存放到索引中
            object[] objArr = property.GetCustomAttributes(typeof(T), false);
            if (objArr == null || objArr.Length == 0)
            {
                return null;
            }

            T fieldAttr = objArr[0] as T;
            if (fieldAttr == null)
            {
                return null;

            }
            return fieldAttr;
        }
    }
}
