using NineFour.Common.Attributes;
using NineFour.Common.Extension;
using NineFour.IDAL;
using NineFour.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.IOC;

namespace Nine.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region 1.0 特性封装
                Console.WriteLine("*************特性封装********************");
                string strAndroid = MobileEnum.Android.GetRemark();
                string strIOS = MobileEnum.IOS.GetRemark();

                Console.WriteLine($"android备注:{strAndroid},ios的备注{strIOS}");
                #endregion


                #region 2.0 封装方法
                Console.WriteLine("*************Unity 使用********************");
                //初始化unity
                IOCFactory.InitContainer();

                Person person = new Person();
                person.Address = "哈哈哈11111111";
                person.Name = "dark";
                person.Email = "11@qq.com";
                person.Phone = "18652428020";

                ISQLHelper sqlHelper = IOCFactory.Resolve<ISQLHelper>();
                sqlHelper.Insert<Person>(person);

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine($"系统异常:{ex.Message.ToString()}");
                throw;
            }

            Console.ReadLine();
        }
    }


    public enum MobileEnum
    {
        [Remark("安卓")]
        Android,
        IOS
    }
}
