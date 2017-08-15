using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.IOC
{
    public class IOCFactory
    {
       
        //安全锁
        private static object lockObj = new object();

        private static IUnityContainer container = null;

        public static void InitContainer()
        {
            if (container == null)
            {
                lock (lockObj)
                {
                    if (container == null)
                    {
                        string containerName = "DefaultContainer";

                        container = new UnityContainer();
                        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                        fileMap.ExeConfigFilename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CfgFiles\\unity.cfg.xml");//找配置文件的路径
                        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                        UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);

                        section.Configure(container, containerName);
                    }
                }
            }
        }


        public static IUnityContainer GetInstance()
        {
            if (container == null) InitContainer();
            return container;
        }

        #region 通过unity容器获取实例+GetInstance<T>()
        /// <summary>
        ///       通过unity容器获取实例
        /// </summary>
        /// <typeparam name="T">抽象类或者接口</typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
        #endregion

      
    }
}
