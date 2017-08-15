using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.Common.Cache
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
    public class CacheTools
    {
        ICache _cache;

        //单例实现

        public CacheTools(ICache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key) where T : class
        {
            return _cache.Get<T>(key);
        }

        public void Add(string key,object value)
        {
            _cache.Add(key, value);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public List<KeyValuePair<string, object>> GetList()
        {
            return _cache.GetList();
        }
    }
}
