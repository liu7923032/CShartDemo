using NineFive.Common;
using NineFive.Model.OrderSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NineFive.OrderSystem
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class OrderMenu
    {

        private static OrderMenu instance = null;
        private static object lockObj = new object();

        public List<AbstractFood> AllFoods = new List<AbstractFood>();

        private OrderMenu()
        {
            //加载配置文件
            XMLTool xmlTool = new XMLTool("CfgFiles\\foodMenu.xml");
            XElement xRoot = xmlTool.LoadXml();
            var foodList = xRoot.Elements("Food");
            Type type = typeof(AbstractFood);
            foreach (var item in foodList)
            {
                //1：找到类的名称
                var csName = item.Element("FoodName").Value;
                //2：通过反射来实例化对象
                Assembly assembly = Assembly.Load("NineFive.Model");
                AbstractFood food = assembly.CreateInstance($"NineFive.Model.OrderSystem.{csName}Food") as AbstractFood;
                if (food == null)
                {
                    continue;
                }
                //3：赋值
                foreach (var propItem in type.GetProperties())
                {
                    var propValue = item.Element(propItem.Name)?.Value;
                    if (!string.IsNullOrEmpty(propValue))
                    {
                        
                        propItem.SetValue(food, Convert.ChangeType(propValue,propItem.PropertyType), null);
                    }
                }
                AllFoods.Add(food);
            }
        }


        /// <summary>
        /// 单例实现
        /// </summary>
        /// <returns></returns>
        public static OrderMenu GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new OrderMenu();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 显示菜单
        /// </summary>
        public void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("****************欢迎来到潘多拉餐厅****************");
            Console.WriteLine("****************菜单如下:****************");

            //1：找到所有的菜品
            //加载XML文档,显示菜单信息
            Console.WriteLine($"编号:0,退出");
            AllFoods.ForEach(u =>
            {
                Console.WriteLine($"编号:{u.Id},名称:{u.FoodName},价格:{u.Price},描述:{u.Describe}");
            });

            Console.WriteLine("*请输入菜名编号,用','隔开*");
            Console.ForegroundColor = ConsoleColor.White;
        }


    }
}
