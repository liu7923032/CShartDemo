using HtmlAgilityPack;
using NineSeven.Common;
using NineSeven.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NineSeven.HomeWork7
{
    /// <summary>
    /// 抓取国美的数据
    /// </summary>
    public class GM_Crawler
    {

        #region 1：获取国美的所有所有类别数据
        public List<Category> GetAllCategoryURl(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
            {
                return null;
            }
            List<Category> list = new List<Category>();
            //1：获取国美的所有所有类别数据
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(strHtml);
            //2：找到二级菜单
            HtmlNodeCollection twoNodes = document.DocumentNode.SelectNodes("//*[@class='item']/ul/li");
            foreach (var twoNode in twoNodes)
            {

                var subXPath = "div[@class='nav_list']/a";
                HtmlNodeCollection subNodes = twoNode.SelectNodes(subXPath);
                foreach (var subNode in subNodes)
                {
                    //获取title 
                    var title = subNode.Attributes["title"].Value;
                    //获取href
                    var url = "http:" + subNode.Attributes["href"].Value;
                    list.Add(new Category()
                    {
                        Name = title,
                        Url = url,
                        ParentCode = twoNode.SelectSingleNode("p").InnerText,
                    });
                }
            }
            //找明细页面
            return list;
        }
        #endregion

        #region 2：获取各个类别的明细页面,并且获取明细页面类别的详细信息 code，name parentCode;
        public List<Category> GetAllCategoryList(string strHtml)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(strHtml);

            ////*[@id="category-first"]
            string oneCate = "//span[@id='category-first']",
                   twoCate = "//dl[@id='category-second']",
                   threeCate = "//*[@id='category-third']";

            HtmlNode oneNode = doc.DocumentNode.SelectSingleNode(oneCate);

            #region 1.0 添加分类集合
            List<Category> category = new List<Category>();
            if (oneNode != null)
            {

                //一级菜单
                category.Add(new Category()
                {
                    CategoryLevel = 1,
                    Code = oneNode.Attributes["data-code"].Value,
                    Name = oneNode.InnerText,
                    ParentCode = "",
                    Url = "",
                    State = 0
                });
            }
            HtmlNode twoNode = doc.DocumentNode.SelectSingleNode(twoCate);
            if (twoNode != null)
            {
                //二级菜单
                category.Add(new Category()
                {
                    CategoryLevel = 2,
                    Code = twoNode.SelectSingleNode("//dd").Attributes["modelid"].Value,
                    Name = twoNode.SelectSingleNode("//dt").InnerText,
                    ParentCode = oneNode.Attributes["data-code"].Value,
                    Url = "",
                    State = 0
                });
            }
            HtmlNode threeNode = doc.DocumentNode.SelectSingleNode(threeCate);
            if (threeNode != null)
            {
                //var threeCategory = new Category()
                //{
                //    CategoryLevel = 3,
                //    Code = threeNode.SelectSingleNode("//dd[@id='category-box-third']").Attributes["modelid"].Value,
                //    Name = threeNode.SelectSingleNode("dt").InnerText,
                //    ParentCode = twoNode.SelectSingleNode("//dd").Attributes["modelid"].Value,
                //    Url = "",
                //    State = 0
                //};

                ////三级菜单
                //category.Add(threeCategory);
                var fourNodes = threeNode.SelectNodes("//dd/a");
                foreach (var item in fourNodes)
                {
                    //四级菜单
                    category.Add(new Category()
                    {
                        CategoryLevel = 3,
                        Code = item.Attributes["data-code"].Value,
                        Name = item.InnerText,
                        ParentCode = twoNode.SelectSingleNode("//dd").Attributes["modelid"].Value,
                        Url = "http:" + item.Attributes["href"].Value,
                        State = 0
                    });
                }
            }
            #endregion

            return category;
        }
        #endregion

        #region 3：通过类别来加载所有的明细，分页
        public List<GM_Commodity> GetCategoryDetails(string url, int page = 1)
        {

            HtmlDocument doc = new HtmlDocument();
            string pageUrl = url + "?page=" + page;
            //1:加载当前URL
            Console.WriteLine($"page={page},url={pageUrl}");
            string strDoc = HttpTools.Get(pageUrl);
            doc.LoadHtml(strDoc);

            //2:检查当前页面商品的数量,如果数量大于48,那么就记载下一页
            List<GM_Commodity> commodityList = new List<GM_Commodity>();
            commodityList.AddRange(GetCommodityList(strDoc));
            if (commodityList.Count == 48)
            {
                commodityList.AddRange(GetCategoryDetails(url, page + 1));
            }
            //3:检查当前页面是否有最后一页，如果有最后一页
            return commodityList;
        }
        #endregion

        #region 4：获取每个明细页面商品数据
        private List<GM_Commodity> GetCommodityList(string strDoc)
        {
            HtmlDocument doc = new HtmlDocument();
            //通过URL加载
            string products = "//div[@class='product-box']/ul/li[@class='product-item']";
            doc.LoadHtml(strDoc);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(products);
            List<GM_Commodity> commodityList = new List<GM_Commodity>();
            if (nodes == null)
            {
                return commodityList;
            }
            foreach (var item in nodes)
            {

                HtmlNode subNode = item.SelectSingleNode("div[@class='item-tab-warp']");
                GM_Commodity commodity = new GM_Commodity();
                var urlNode = subNode.SelectSingleNode("p[1]/a");
                if (urlNode != null)
                {
                    commodity.Url = urlNode.Attributes["href"].Value;
                }

                var imgNode = subNode.SelectSingleNode("p[1]/a/img");
                if (imgNode != null)
                {
                    if (imgNode.Attributes["gome-src"] != null)
                    {
                        commodity.ImageUrl = imgNode.Attributes["gome-src"].Value;
                    }
                    else
                    {
                        commodity.ImageUrl = imgNode.Attributes["src"].Value;
                    }
                }
                var textNode = subNode.SelectSingleNode("p[2]/a");
                if (textNode != null)
                {
                    commodity.Title = textNode.InnerText;
                }
                //获取价格
                if (!string.IsNullOrEmpty(commodity.Url))
                {
                    string strTemp = Path.GetFileName(commodity.Url).Replace(".html", "");
                    commodity.ProductId = strTemp.Split('-')[0];
                    commodity.SkuId = strTemp.Split('-')[1];
                    //var priceURL = $"http://ss.gome.com.cn/search/v1/price/single/{commodity.ProductId}/{commodity.SkuId}/23060000/flag/item/fn0?callback=fn0&_=1500802980750";
                }
                commodityList.Add(commodity);
            }
            return commodityList;
        }
        #endregion

        #region 5.0 获取所有商品的价格
        public void GetCommodityPrice(GM_Commodity commodity)
        {
            if (string.IsNullOrEmpty(commodity.SkuId) || string.IsNullOrEmpty(commodity.ProductId))
            {
                commodity.Price = 0;
            }

            var priceURL = $"http://ss.gome.com.cn/search/v1/price/single/{commodity.ProductId}/{commodity.SkuId}/23060000/flag/item/fn0?callback=fn0&_=1500802980750";
            //请求URL 
            Console.WriteLine($"价格设置:商品:{commodity.Title}");
            string callback = HttpTools.Get(priceURL, "text/json");
            Regex regex = new Regex("\\d+\\.\\d+", RegexOptions.Singleline);
            if (regex.IsMatch(callback))
            {
                string strPrice = regex.Match(callback).Value.ToString();
                decimal price = 0;
                if (decimal.TryParse(strPrice, out price))
                {
                    commodity.Price = price;
                }
            }
        }
        #endregion
    }
}
