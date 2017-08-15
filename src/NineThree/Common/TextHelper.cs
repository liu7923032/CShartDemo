using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nine.Common
{

    public class TextHelper
    {
        public static object lockObj = new object();

        public static string logPath = "log.txt";

        static TextHelper()
        {
            //1:检查是否存在log文件
            if (!File.Exists(logPath))
            {
                File.Create(logPath);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="resMsg"></param>
        public static void WriteToTxt(string resMsg)
        {
            lock (lockObj)
            {
                using (FileStream fs = new FileStream(logPath, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(resMsg);
                        sw.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// 读取文本信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadText(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath,Encoding.Default))
            {
                string line = string.Empty;
                StringBuilder sb = new StringBuilder();
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line);
                }
                return sb.ToString();
            }
        }
    }
}
