using NineEight.WCF.Interface;
using NineEight.WCF.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.WCF
{
    public class WCFInit
    {
        private static WCFInit current = null;

        public static WCFInit Current
        {
            get
            {
                current = new WCFInit();
                return current;
            }
        }

        public void StartService()
        {
            List<ServiceHost> hostList = new List<ServiceHost>()
            {
                new ServiceHost(typeof(MathService)),
                //new ServiceHost(typeof(Searcher))
            };
            foreach (ServiceHost host in hostList)
            {
                host.Open();
                Console.WriteLine("{0}已经启动！", host.Description);
            }
            Console.WriteLine("输入任何字符串停止服务");
            Console.ReadKey();

            foreach (ServiceHost host in hostList)
            {
                host.Abort();
            }
            Console.WriteLine("服务已关闭。。。。");
            Console.Read();

            //using (ServiceHost host = new ServiceHost())
            //{
            //    #region 程序配置
            //    //host.AddServiceEndpoint(typeof(IMathService), new WSHttpBinding(), "http://127.0.0.1:9999/calculatorservice");
            //    //if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            //    //{
            //    //    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            //    //    behavior.HttpGetEnabled = true;
            //    //    behavior.HttpGetUrl = new Uri("http://127.0.0.1:9999/calculatorservice/metadata");
            //    //    host.Description.Behaviors.Add(behavior);
            //    //}
            //    #endregion 程序配置
            //    host.Opened += ()=>
            //    {
            //        Console.WriteLine("MathService已经启动，按任意键终止服务！");
            //    };

            //    host.Open();
            //}
        }
    }
}
