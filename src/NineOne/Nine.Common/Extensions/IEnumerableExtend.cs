using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nine.Common.Extensions
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class IEnumerableExtend
    {
        /// <summary>
        /// 如果要查询的属性为空,那么就不执行查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="isOk"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> list, bool needQuery, Func<T, bool> where) where T : class
        {
            if (needQuery)
            {
                list = list.Where(where);
            }
            return list;
        }
    }
}
