using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineTwo.Perform.Common
{
    public class LogHelper
    {
        private static string FilePath = "log.txt";


        #region 2.0 记录日志信息
        public static void WriteLog(string msg)
        {
            //检查
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
            }

            using (FileStream fs = new FileStream(FilePath, FileMode.Append))
            {
                string newMsg = $"\r\n{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{msg}";
                byte[] bytes = Encoding.UTF8.GetBytes(newMsg);
                //读取最后一个位置
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        #endregion
    }
}
