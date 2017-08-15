using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.Common
{
    /// <summary>
    /// 通用的http方法
    /// </summary>
    public class HttpTools
    {
       

        private string GetURL(string url, string method = "GET", string contentType = "text/json", string paramData = "")
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            string resMsg = "";
            try
            {
                //设置请求超时时间 5s
                webRequest.ContinueTimeout = 5000;
                //设置请求的内容格式
                webRequest.ContentType = contentType;
                webRequest.Method = method;


                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                //webRequest.
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream(); // 获取接收到的流
                System.IO.StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true);
                resMsg = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误信息:{ex.Message.ToString()}");
            }
            return resMsg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public static string Get(string url, string contentType = "text/html")
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            string resMsg = "";
            try
            {
                //设置请求超时时间 5s
                webRequest.ContinueTimeout = 5000;
                //设置请求的内容格式
                webRequest.ContentType = contentType;
                webRequest.Method = "GET";

                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                //webRequest.
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream(); // 获取接收到的流
                System.IO.StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true);
                resMsg = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误信息:{ex.Message.ToString()}");
            }
            return resMsg;
        }



        /// <summary>
        /// Post 请求提交数据到
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strParam"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Post(string url, string strParam, string contentType = "text/json")
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            string resMsg = "";
            try
            {
                //将参数转换为二进制
                byte[] postData = Encoding.UTF8.GetBytes(strParam);
                //1:构建请求对象 设置请求超时时间 5s
                webRequest.ContinueTimeout = 5000;
                //设置请求的内容格式
                webRequest.ContentType = contentType;
                webRequest.Method = "GET";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                webRequest.ContentLength = postData.Length;
                //2:提交请求参数化数据
                Stream outputStream = webRequest.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                //3:获取返回的流
                HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                resMsg = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误信息:{ex.Message.ToString()}");
            }
            return resMsg;
        }


    }
}
