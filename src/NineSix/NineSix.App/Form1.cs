using NineSix.Common;
using NineSix.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineSix.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string prefix = "**********************";
        CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// 1.0 加载所有的类别list集合
        /// </summary>
        private List<Category> LoadCategories()
        {
            //0：加载国美的数据的所有分类
            string url = "http://list.gome.com.cn/";
            //1：找到所有的类别对应的URL
            string docHtml = HttpTools.Get(url);
            GM_Crawler center = new GM_Crawler();
            List<Category> tempCateList = center.GetAllCategoryURl(docHtml);
            //2：获取各个类别的父级菜单
            List<Category> cateList = new List<Category>();
            //3：用于排除同一个二级菜单多次请求的问题
            List<string> twoCategory = new List<string>();
            foreach (var item in tempCateList)
            {
                if (!twoCategory.Contains(item.ParentCode))
                {
                    twoCategory.Add(item.ParentCode);
                    var strDoc = HttpTools.Get(item.Url);
                    cateList.AddRange(center.GetAllCategoryList(strDoc));
                }
            }
            Console.WriteLine("{0}国美所有类别已经都加载完了{1}", prefix, prefix);
            return cateList;
        }

        private void LoadCommodities(List<Category> cateList)
        {
            List<GM_Commodity> commodityList = new List<GM_Commodity>();
            //1:获取有URL的类别集合
            List<Category> newList = cateList.Where(u => !string.IsNullOrEmpty(u.Url) && u.Name.Equals("手机")).ToList();
            //2:开始十个线程和一个监控线程，用来检查是否有异常 
            Task[] tasks = new Task[11];
            TaskFactory taskFactory = new TaskFactory();
            tasks[0] = taskFactory.StartNew(() =>
             {
                 while (!cts.IsCancellationRequested)
                 {
                     Thread.Sleep(100);
                     Console.WriteLine("由于抓取数据异常,所以线程中断了");
                 }
             }, cts.Token);

            //3:将所有类别的数据分为10等份，开启10个线程各自抓取数据
            var cateLen = newList.Count;
            var avgCount = cateLen % 10 == 0 ? cateLen / 10 : (cateLen / 10 + 1);
            GM_Crawler crawler = new GM_Crawler();

            for (int i = 1; i < 11; i++)
            {
                tasks[i] = taskFactory.StartNew((num) =>
                 {
                     var startCate = (Convert.ToInt16(num) - 1) * avgCount;
                     var endCate = Convert.ToInt16(num) * avgCount;
                     if (endCate > cateLen)
                     {
                         endCate = cateLen;
                     }
                     for (int j = startCate; j < endCate; j++)
                     {
                         Category category = newList[j];
                         //commodityList.AddRange(crawler.GetCategoryDetails(category));
                     }
                 }, i, cts.Token);
            }

            taskFactory.ContinueWhenAll(tasks, (ts) =>
            {
                Console.WriteLine("所有的类别明细都已经加载完成");
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            try
            {

                GM_Crawler crawler = new GM_Crawler();
                Category category = new Category();
                category.Url = "http://list.gome.com.cn/cat10000070.html";

                List<GM_Commodity> list = crawler.GetCategoryDetails(category.Url, 1);

                ////1：加载分类
                //List<Category> cateList = LoadCategories();
                ////2：开启多线程，加载明细
                //LoadCommodities(cateList);
            }
            catch (Exception ex)
            {
                //出现异常中断其他运行的线程
                cts.Cancel();
                Console.WriteLine("页面抓取出现异常:{0}", ex.Message);
            }

        }
    }
}
