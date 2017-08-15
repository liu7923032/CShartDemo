using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NineFive.Common
{
    /// <summary>
    /// XML帮助类,将对象保存成XML文件,操作XML文件
    /// Linq to XML
    /// </summary>
    public class XMLTool
    {
        private string _xmlPath = string.Empty;

        private XElement rootElement = null;

        /// <summary>
        /// 初始化XML工具
        /// </summary>
        /// <param name="xmlPath"></param>
        public XMLTool(string xmlPath)
        {
            this._xmlPath = xmlPath;
        }

        /// <summary>
        /// 加载XML文档
        /// </summary>
        /// <returns></returns>
        public XElement LoadXml()
        {
            rootElement = XElement.Load(this._xmlPath);
            return rootElement;
        }

        /// <summary>
        /// 通过xml节点,来找到对应的XML数据，并将数据转换为T类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ObjFromXML<T>(string nodeName = "") where T : new()
        {
            T obj = new T();
            Type type = typeof(T);
            //1：检查根节点是否存在
            if (rootElement == null)
            {
                LoadXml();
            }
            if (string.IsNullOrEmpty(nodeName))
            {
                nodeName = type.Name;
            }
            //2：通过根节点来找到对应的子节点
            var xElement = rootElement.Element(nodeName);
            //3：通过反射找到属性
            foreach (var item in type.GetProperties())
            {
                var proValue = xElement.Element(item.Name).Value;
                item.SetValue(obj, proValue);
            }
            return obj;
        }

        /// <summary>
        /// 将对象转换为XML文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string ToXML<T>(T entity, string rootName = "") where T : class
        {
            Type type = entity.GetType();
            if (string.IsNullOrEmpty(rootName))
            {
                rootName = type.Name;
            }
            //1:定义根节点
            XElement xRoot = new XElement(rootName);
            //2:添加属性
            foreach (var item in type.GetProperties())
            {
                var xe = new XElement(item.Name);
                //检查属性类型,如果属性的泛型
                if (item.PropertyType.IsGenericType)
                {

                }
                xRoot.Add(xe);
            }
            return xRoot.ToString();
        }

    }
}
