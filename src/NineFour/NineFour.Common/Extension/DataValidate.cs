using NineFour.Common.Attributes;
using System;

using System.Collections.Generic;
using System.Text;

namespace NineFour.Common.Extentision
{
    /// <summary>
    /// 用于检查类的所有属性是否都满足条件
    /// </summary>
    public static class DataValidate
    {
        private static LogHelper logger = new LogHelper(typeof(DataValidate));
        public static bool CheckValidate<T>(this Type type, T obj) where T:class
        {
            //Type type = typeof(T);
            var props = type.GetProperties();
            //用于记录错误信息
            StringBuilder sb = new StringBuilder();
            foreach (var item in props)
            {
                //1:获取item的验证特性
                object[] customs = item.GetCustomAttributes(true);
                if (customs.Length == 0)
                {
                    continue;
                }
                //2:检查特性是否满足
                foreach (var attr in customs)
                {
                    IValidate validate = attr as IValidate;
                    if (validate != null)
                    {
                        bool isValidate = validate.IsValidate(item.GetValue(obj));
                        if (!isValidate)
                        {
                            sb.AppendLine($"属性:{item.Name} 验证失败,错误信息:{validate.ErrorMsg}");
                        }
                    }
                }
            }

            if (sb.ToString().Length > 0)
            {
                logger.Error(sb.ToString());
                return false;
            }
            return true;
        }
    }
}
