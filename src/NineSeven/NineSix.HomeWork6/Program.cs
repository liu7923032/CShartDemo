using NineSeven.BLL;
using NineSeven.Common;
using NineSeven.Common.DI;
using NineSeven.IBLL;
using NineSeven.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NineSeven.HomeWork7
{
    class Program
    {
        //加载所有产品信息
        private static CancellationTokenSource cts = null;
        //加载价格
        private static CancellationTokenSource ctsPrice = null;
        static void Main(string[] args)
        {
            try
            {
                #region 1.0 爬虫
                //1:初始化容器和服务
                IOCFactory.InitContainer();
                ICategoryService categoryService = IOCFactory.Resolve<ICategoryService>();
                ICommodityService commodityService = IOCFactory.Resolve<ICommodityService>();
                //这个是爬虫的demo
                //CrawlerDemo(categoryService, commodityService);
                #endregion

                #region 2.0 lucene.net 
                Console.WriteLine($"*****************lucene.net demo*****************");
                LuceneTools luceneTools = new LuceneTools(commodityService);
                luceneTools.InitLucene();
                luceneTools.QueryList("Title:asus", new Page() { Sort = "Price" }, "[100,200]");
                #endregion

                #region 3.0 redis 异步队列
                //

                #endregion
            }
            catch (Exception ex)
            {
                //如果其中一个线程出现异常，那么就终止所有其他线程
                cts?.Cancel();
                ctsPrice?.Cancel();
                //出现异常中断其他运行的线程
                Console.WriteLine("页面抓取出现异常:{0}", ex.Message);
            }

            Console.ReadLine();
        }


        #region 1.0 爬虫
        private static void CrawlerDemo(ICategoryService categoryService, ICommodityService commodityService)
        {
            //2:初始化数据库
            InitDb(categoryService, commodityService);
            //3:插入类别数据
            LoadCategory(categoryService);
            //4:加载明细数据并将数据插入到db中
            LoadCommodty(categoryService);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="categoryService"></param>
        private static void InitDb(ICategoryService categoryService, ICommodityService commodityService)
        {
            //1:初始化数据库
            categoryService.InitDbCatetory();
            commodityService.InitDbCommodity();
        }

        /// <summary>
        /// 加载所有分类
        /// </summary>
        /// <param name="categoryService"></param>
        private static void LoadCategory(ICategoryService categoryService)
        {
            //2:初始化爬虫
            GM_Crawler crawler = new GM_Crawler();

            string allCateURL = ConfigurationManager.AppSettings["GM_AllCategory"];
            string strCategory = HttpTools.Get(allCateURL);
            //2.1 加载当前页面所有的分类
            List<Category> tempCateList = crawler.GetAllCategoryURl(strCategory);
            //2.2 加载所有分类明细详情
            List<Category> allCagoryList = new List<Category>();
            //2.3 记录统一类别的情况不在发起请求
            List<string> typeList = new List<string>();
            tempCateList.ForEach(u =>
            {
                if (!typeList.Contains(u.ParentCode))
                {
                    Console.WriteLine($"类别:{u.Name},Url={u.Url}");

                    string strHtml = HttpTools.Get(u.Url);
                    allCagoryList.AddRange(crawler.GetAllCategoryList(strHtml));
                    typeList.Add(u.ParentCode);
                }
            });
            Console.WriteLine($"所有的类别数据都已经下载完成");
            //2.3 将类别数据插入到数据库中
            categoryService.AddCategory(allCagoryList);
            Console.WriteLine($"Category数据插入完成");
        }


        private static void LoadCommodty(ICategoryService categoryService)
        {
            //1:加载有URL请求的类别
            var categoryList = categoryService.GetCategoryList().Where(u => !string.IsNullOrEmpty(u.Url)).ToList();

            if (categoryList.Count == 0)
            {
                return;
            }
            GM_Crawler crawler = new GM_Crawler();
            List<GM_Commodity> commodityList = new List<GM_Commodity>();
            //2：创建多个线程
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            cts = new CancellationTokenSource();
            //3：开启一个监控线程
            foreach (Category category in categoryList)
            {
                //searcher.Crawler();
                taskList.Add(taskFactory.StartNew((url) =>
                {
                    if (!cts.IsCancellationRequested)
                    {
                        string categoryURL = url as string;
                        commodityList.AddRange(crawler.GetCategoryDetails(categoryURL));
                    }
                   
                }, category.Url, cts.Token));
                //如果线程开启的数量超过30个的情况下,那么就给线程等待一下
                if (taskList.Count > 30)
                {
                    //Console.WriteLine($"当前线程数已经到达30个");
                    taskList = taskList.Where(t => !t.IsCompleted && !t.IsCanceled && !t.IsFaulted).ToList();
                    //等待任意一个线程结束
                    Task.WaitAny(taskList.ToArray());
                }
            }
            //等待所有的线程的任务完结
            taskFactory.ContinueWhenAll(taskList.ToArray(), (ts) =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("所有抓取上面的线程都已经完结");
                Console.WriteLine($"共有商品数量:{commodityList.Count}");


                Console.ForegroundColor = ConsoleColor.White;
                //加载价格并将商品存入到数据库中
                LoadPriceAndInsertDB(commodityList, categoryService);
            });
        }

        /// <summary>
        /// 加载所有产品的价格
        /// </summary>
        /// <param name="list"></param>
        private static void LoadPriceAndInsertDB(List<GM_Commodity> commodityList, ICategoryService categoryService)
        {
            Console.WriteLine($"*****************开始加载该产品的价格信息**********************");
            if (commodityList.Count == 0)
            {
                return;
            }
            GM_Crawler crawler = new GM_Crawler();
            //2：创建多个线程
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            ctsPrice = new CancellationTokenSource();
            //3：开启一个监控线程
            foreach (GM_Commodity category in commodityList)
            {
                //searcher.Crawler();
                taskList.Add(taskFactory.StartNew((obj) =>
                {
                    if (!ctsPrice.IsCancellationRequested)
                    {
                        GM_Commodity commodity = obj as GM_Commodity;
                        crawler.GetCommodityPrice(commodity);
                    }

                }, category, ctsPrice.Token));
                //如果线程开启的数量超过30个的情况下,那么就给线程等待一下
                if (taskList.Count > 30)
                {
                    taskList = taskList.Where(t => !t.IsCompleted && !t.IsCanceled && !t.IsFaulted).ToList();
                    //等待任意一个线程结束
                    Task.WaitAny(taskList.ToArray());
                }
            }
            //等待所有的线程的任务完结
            taskFactory.ContinueWhenAll(taskList.ToArray(), (ts) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("设置商品价格的所有线程都已经完结");
                //将所有数据保存到数据库中
                categoryService.InsertCommodity(commodityList);
                Console.ForegroundColor = ConsoleColor.White;
            });
        }
        #endregion

    }



}
