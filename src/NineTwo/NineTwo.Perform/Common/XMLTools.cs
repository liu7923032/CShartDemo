using NineTwo.Perform.Base;
using NineTwo.Perform.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NineTwo.Perform.Common
{
    //读取配置文件
    public class XMLTools
    {
        public static void InitByXML<T>(T perform) where T : AbsPerform, ICharge
        {
            //1.0 读取配置文件
            string fileName = typeof(T).Name;
            string baseURL = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = baseURL + "\\CfgFiles\\" + fileName + ".xml";
            //2.0 加载XML文档
            XElement xe = XElement.Load(filePath);
            //3.0 给对象赋值
            foreach (var item in typeof(T).GetProperties())
            {
                var xItem = xe.Element(item.Name);
                if (xItem != null)
                {
                    item.SetValue(perform, Convert.ChangeType(xItem.Value, item.PropertyType));
                }
            }
            //4.0 注册事件
            var elements = xe.Element("Events").Elements("Event");
            foreach (var item in elements)
            {
                perform.FireEvent += () =>
                {
                    //打印到控制台
                    Console.WriteLine($"*{perform.User}:{ item.Value}*");
                    //记录到日志
                    LogHelper.WriteLog($"{perform.User}:起火了:{ item.Value}");
                };
            }


        }
    }
}
