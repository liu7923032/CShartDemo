using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NineEight.Common
{
    public class JsonHelper
    {
        #region Json
        /// <summary>
        /// JsonConvert.SerializeObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// JsonConvert.DeserializeObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T ToObject<T>(string content) where T : class
        {
            return JsonConvert.DeserializeObject<T>(content);
        }


        public static List<T> ToObjList<T>(string strList) where T : class
        {
            return JsonConvert.DeserializeObject<List<T>>(strList);
        }

        #endregion Json
    }
}
