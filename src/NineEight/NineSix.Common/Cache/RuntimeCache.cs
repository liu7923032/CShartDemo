using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.Common.Cache
{
    public class RuntimeCache : ICache
    {
        public MemoryCache CacheContext { get; set; }

        public RuntimeCache()
        {
            this.CacheContext = MemoryCache.Default;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            CacheContext.AddOrGetExisting(key, value, DateTimeOffset.Now.AddHours(5));
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            T obj = CacheContext.Get(key) as T;
            return obj;
        }

        public void Remove(string key)
        {
            if (CacheContext.Contains(key))
            {
                CacheContext.Remove(key);
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            KeyValuePair<string, object>[] keyPairs = CacheContext.ToArray();
            foreach (var item in keyPairs)
            {
                CacheContext.Remove(item.Key);
            }
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<KeyValuePair<string, object>> GetList()
        {
            return CacheContext.ToArray().ToList();
        }
    }
}
