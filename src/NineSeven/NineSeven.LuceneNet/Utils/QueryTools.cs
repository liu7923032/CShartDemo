using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NineSeven.LuceneNet.Utils
{
    public class QueryTools
    {
        /// <summary>
        /// 通过属性类型获取该排序的类型
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public static int GetSortField(Type propertyType)
        {
            if (propertyType == typeof(double) || propertyType == typeof(decimal))
            {
                return SortField.DOUBLE;
            }
            else if (propertyType == typeof(float))
            {
                return SortField.FLOAT;
            }
            else if (propertyType == typeof(int))
            {
                return SortField.INT;
            }
            else
            {
                return SortField.STRING;
            }
        }

        /// <summary>
        /// 通过类型来获取
        /// </summary>
        /// <param name="property"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Filter GetNumberFilter(PropertyInfo property, double min, double max)
        {

            if (property.PropertyType == typeof(double) || property.PropertyType == typeof(decimal))
            {
                return NumericRangeFilter.NewDoubleRange(property.Name, min, max, true, true);
            }
            else if (property.PropertyType == typeof(float))
            {
                return NumericRangeFilter.NewFloatRange(property.Name, (float)min, (float)max, true, true);
            }
            else if (property.PropertyType == typeof(int))
            {
                return NumericRangeFilter.NewIntRange(property.Name, (int)min, (int)max, true, true);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过正则表达式获取范围
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public static Tuple<double, double> GetMinAndMax(string strFilter)
        {
            string strRegex = @"(\d+(\.\d)?)";
            Regex regex = new Regex(strRegex, RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(strFilter);
            double minDouble = 0, maxDouble = 0;
            bool isOk = false;
            isOk = double.TryParse(matches[0].Groups[0].Value, out minDouble);
            isOk = double.TryParse(matches[1].Groups[0].Value, out maxDouble);
            if (isOk)
            {
                return Tuple.Create<double, double>(minDouble, maxDouble);
            }
            else
            {
                return null;
            }

        }
    }
}
