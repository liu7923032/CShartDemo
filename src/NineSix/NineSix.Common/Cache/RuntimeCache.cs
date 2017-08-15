using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.Common.Cache
{
    public class RuntimeCache : ICache
    {
        public MemoryCache Cache { get; set; }



        public RuntimeCache()
        {
            this.Cache = MemoryCache.Default;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            Cache.AddOrGetExisting(key, value, DateTimeOffset.Now.AddHours(5));
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            T obj = Cache.Get(key) as T;
            return obj;
        }

        public void Remove(string key)
        {
            if (Cache.Contains(key))
            {
                Cache.Remove(key);
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            KeyValuePair<string, object>[] keyPairs = Cache.ToArray();
            foreach (var item in keyPairs)
            {
                Cache.Remove(item.Key);
            }
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<KeyValuePair<string, object>> GetList()
        {
            return Cache.ToArray().ToList();
        }
    }
}
