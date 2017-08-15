using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.Common.DI
{
    /// <summary>
    /// 依赖注入容器实现
    /// </summary>
    public class IOCFactory
    {
        private static IUnityContainer container = null;

        private static object lockUnity = new object();

        public static void InitContainer()
        {
            if (container == null)
            {
                lock (lockUnity)
                {
                    if (container == null)
                    {

                        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

                        fileMap.ExeConfigFilename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CfgFiles\\unity.cfg.xml");
                        //找配置文件的路径
                        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                        UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);


                        container = new UnityContainer();

                        section.Configure(container, "DefaultContainer");
                    }
                }
            }
        }

        public static T Resolve<T>() where T : class
        {
            if (container == null) InitContainer();
            return container.Resolve<T>();
        }
    }
}
