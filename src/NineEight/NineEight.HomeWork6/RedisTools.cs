using NineEight.Model;
using NineEight.Redis.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NineEight.HomeWork7
{
    /// <summary>
    /// Redis 实现异步队列
    /// </summary>
    public class RedisTools
    {

        #region 1.0 工具类的单例实现
        private static RedisTools _current = null;
        private static object lockCurrent = new object();
        public static RedisTools Current
        {
            get
            {
                if (_current == null)
                {
                    lock (lockCurrent)
                    {
                        if (_current == null)
                        {
                            _current = new RedisTools();
                        }
                    }
                }
                return _current;
            }
        }

        #endregion


        /// <summary>
        /// redis list 集合
        /// </summary>
        private static RedisListService _rListService = null;
        private static string listKey = "CommodityList";
        /// <summary>
        /// 构造函数
        /// </summary>
        private RedisTools()
        {
            //在构造函数中初始化list 服务，保证服务线程实例唯一
            _rListService = new RedisListService();
        }

        /// <summary>
        /// 销毁 _rListService
        /// </summary>
        public void CloseRedis()
        {
            _rListService?.Dispose();
        }




        /// <summary>
        /// 启动redis
        /// </summary>
        public void Start(LuceneTools luceneTools)
        {
            //开启一个异步线程
            Task.Run(() =>
            {
                while (true)
                {
                    var commodityList = _rListService.Get(listKey);
                    if (commodityList.Count > 0)
                    {
                        Console.WriteLine($"当前redis 对象的包含的list集合的数量是{commodityList.Count}");
                        var strList = _rListService.BlockingDequeueItemFromList(listKey, TimeSpan.FromHours(1));
                        //1.将值转换为List集合
                        GM_Commodity entity = Common.JsonHelper.ToObject<GM_Commodity>(strList);
                        //2.检查索引中是否有该对象,如果有那么就更新索引
                        luceneTools.UpdateIndex(entity);
                        ////3：更新成功再次查询数据
                        //luceneTools.QueryList("Title:asus", new Page() { Sort = "Price" }, "[100,200]");
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
        }


        /// <summary>
        /// 将对象添加到list集合中
        /// </summary>
        /// <param name="entity"></param>
        public void Add(GM_Commodity entity)
        {
            //一个一个对象放入队列中
            _rListService.LPush(listKey, Common.JsonHelper.ToJSON<GM_Commodity>(entity));
        }


    }
}
