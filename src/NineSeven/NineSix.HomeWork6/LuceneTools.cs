using NineSeven.Common;
using NineSeven.IBLL;
using NineSeven.LuceneNet.Interface;
using NineSeven.LuceneNet.Service;
using NineSeven.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NineSeven.HomeWork7
{
    public class LuceneTools
    {

        private ICommodityService _commodityService;
        private ILuceneBulid<GM_Commodity> _gmLucene;
        private CancellationTokenSource cts = null;

        private Logger logger = new Logger(typeof(LuceneTools));
        private string rootPath = string.Empty;

        public LuceneTools(ICommodityService commodityService)
        {
            this._commodityService = commodityService;
            //定义根目录
            rootPath = AppDomain.CurrentDomain.BaseDirectory + "lucene\\";
            _gmLucene = new LuceneBulid<GM_Commodity>(rootPath);
        }

        /// <summary>
        /// lucene 初始化
        /// </summary>
        public void InitLucene()
        {
            //初始化索引
            if (_gmLucene != null)
            {
                //从数据库中获取所有的数据,将数据放到索引中
                //需要进行多线程操作
                InitIndex();
            }
            //查询
            else
            {
                Console.WriteLine($"lucene 没有初始化");
            }
        }

        /// <summary>
        /// 初始化索引
        /// </summary>
        private void InitIndex()
        {
            #region index 初始化
            List<GM_Commodity> categoryList = new List<GM_Commodity>();
            TaskFactory taskFactory = new TaskFactory();
            List<Task> tasks = new List<Task>();
            cts = new CancellationTokenSource();
            string[] dirs = new string[30];
            //开启30个线程,进行索引的初始化
            for (int i = 1; i < 31; i++)
            {
                string prefix = i.ToString("000");
                dirs[i - 1] = prefix;
                string strSql = $"SELECT * FROM DBO.GM_Commodity_{prefix};";
                string[] strObj =
                {
                        strSql,
                        prefix
                    };
                tasks.Add(taskFactory.StartNew((objs) =>
                {

                    try
                    {
                        //检查是线程是否有中断,如果中断那么就不在执行
                        if (cts.IsCancellationRequested)
                        {
                            return;
                        }

                        string[] strObjs = objs as string[];

                        //1:加载数据
                        List<GM_Commodity> gmList = _commodityService.GetCommodityList(strObjs[0]).ToList();
                        //2:生成lucene.net 的索引
                        _gmLucene.InsertList(gmList, strObjs[1], true);
                        Console.WriteLine($"索引：{strObjs[1]}:建立完成");
                    }
                    catch (Exception ex)
                    {
                        cts.Cancel();
                        logger.Error("建立索引时出错:" + ex.Message.ToString());
                    }

                }, strObj, cts.Token));
            }

            //等待所有的线程完成
            tasks.Add(taskFactory.ContinueWhenAll(tasks.ToArray(), (ts) =>
            {
                Console.WriteLine($"lucene 所有的索引都已经建立完成");
            }));
            Task.WaitAll(tasks.ToArray());
            #endregion
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryCondition"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<GM_Commodity> QueryList(string queryCondition, Page page, string strFilter = "")
        {
            var list = _gmLucene.QueryByPage(queryCondition, page, strFilter);
            Console.WriteLine($"总共查询来的结果数量是:{page.Total}");
            //显示查询的结果
            list.ForEach(u =>
            {
                Console.WriteLine($"Title:{u.Title},Price:{u.Price}");
            });
            return list;
        }


        public void UpdateIndex(GM_Commodity commodity)
        {
            //
        }
    }
}
