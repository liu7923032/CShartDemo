using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSeven.Redis
{
    public class RedisManage
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisCfg RedisConfigInfo = new RedisCfg();

        /// <summary>
        /// Redis客户端池化管理
        /// </summary>
        private static PooledRedisClientManager prcManager;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManage()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] WriteServerConStr = RedisConfigInfo.MasterServer.Split(',');
            string[] ReadServerConStr = RedisConfigInfo.SlaveServer.Split(',');
            prcManager = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = RedisConfigInfo.MaxWritePoolSize,
                                 MaxReadPoolSize = RedisConfigInfo.MaxReadPoolSize,
                                 AutoStart = RedisConfigInfo.AutoStart,
                             });
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            return prcManager.GetClient();
        }
    }
}
