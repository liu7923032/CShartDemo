using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nine.Common
{
    public class CommonTools
    {
        public static string GetByAppSetting(string appName)
        {
            return ConfigurationManager.AppSettings[appName];
        }
       
    }
}
