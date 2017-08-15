using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nine.Common
{
    public class LogHelper
    {
        /// <summary>
        /// 锁住多线程,
        /// </summary>

        public static void WriteInfo(string info, string color)
        {
            Console.ForegroundColor = (ConsoleColor)(Enum.Parse(typeof(ConsoleColor), color));
            Console.WriteLine(info);

            //记录到后台
            TextHelper.WriteToTxt(info);
        }

        public static void Info(string info)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{info}");
            TextHelper.WriteToTxt(info);
        }

        public static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{msg}");
            TextHelper.WriteToTxt(msg);
        }

        public static void Error(string errMsg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{errMsg}");
            TextHelper.WriteToTxt(errMsg);
        }
    }
}
