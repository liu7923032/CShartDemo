using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nine.Common
{
    public class JSONHelper
    {
      
        //返回list集合
        public static List<T> GetListByJSON<T>(string strData) where T : class
        {
           return JsonConvert.DeserializeObject<List<T>>(strData);
        }
        
    }
}
