using NineTwo.Perform.Base;
using NineTwo.Perform.Common;
using NineTwo.Perform.Interface;
using NineTwo.Perform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NineTwo.Perform
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("*口技表演大舞台,请各位门派大师一次上场*");
                Console.WriteLine("");

                #region 1.2 北派的表演
                var north = Init<NorthPerform>();
                north.Show(1001);
                north.Charge(300);

                #endregion

                #region 1.3 南派的表演
                var south = Init<SouthPerform>();
                south.Show(801);
                south.Charge(300);
                #endregion

                #region 1.4 东派的表演
                var east = Init<EastPerform>();
                east.Show(401);
                east.Charge(300);
                #endregion

                #region 1.5 西派的表演
                var west = Init<WestPerform>();
                west.Show(401);
                west.Charge(300);
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine($"系统异常:{ex.Message}");
            }

            Console.ReadLine();
        }



        /// <summary>
        /// 通过泛型来初始化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static T Init<T>() where T : AbsPerform, ICharge
        {
            //1:实例化
            T obj = (T)Activator.CreateInstance(typeof(T));

            //2:通过XML文档来初始化对象,给对象赋值
            XMLTools.InitByXML<T>(obj);
            Console.WriteLine($"=================口技大师：{obj.User}来了==================");
            //3:打印该对象的属性和方法
            foreach (var item in typeof(T).GetProperties())
            {
                Console.WriteLine($"*属性名:{item.Name},属性值:{item.GetValue(obj)}*");
            }
            foreach (var item in typeof(T).GetFields())
            {
                Console.WriteLine($"*字段名:{item.Name},字段值:{item.GetValue(obj)}*");
            }
            return obj;
        }
    }
}
