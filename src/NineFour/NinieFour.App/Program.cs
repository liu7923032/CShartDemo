using NineFour.Common.Attributes;
using NineFour.Common.Extension;
using System;

namespace NinieFour.App
{
    class Program
    {
        static void Main(string[] args)
        {

            string strAndroid = SystemEnum.Android.GetRemark();
            string strIOS = SystemEnum.Android.GetRemark();
            Console.WriteLine($"android的备注:{strAndroid},ios的备注:{strIOS}");

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }



    public enum SystemEnum
    {
        [Remark("安卓")]
        Android=0,
      
        IOS=1
    }
}