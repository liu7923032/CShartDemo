using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.Common.Cache
{
    public interface ICache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key) where T:class;

        void Add(string key, object value);

        void Remove(string key);

        void Clear();
        /// <summary>
        /// 查询list集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<KeyValuePair<string,object>> GetList();
    }
}
