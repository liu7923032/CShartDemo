using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFour.DI
{
    public class UnityContext
    {
        public UnityContext()
        {
            this.IOCContainer = new UnityContainer();
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            
            config.Configure(this.IOCContainer, "Default");
        }

        /// <summary>
        /// 属性
        /// </summary>
        public IUnityContainer IOCContainer
        {
            get; set;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return IOCContainer.Resolve<T>();
        }

        public T Resolve<T>(string name) where T : class
        {
            return IOCContainer.Resolve<T>(name);
        }
    }
}
