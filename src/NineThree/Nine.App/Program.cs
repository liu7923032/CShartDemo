using Nine.Common;
using Nine.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Nine.App
{
    class Program
    {
        //用于锁
        private static object lockObj = new object();

        //用于中断人物触发的事件的全局变量
        private static CancellationTokenSource cts = new CancellationTokenSource();
        //用于中断监控线程
        private static CancellationTokenSource ctsWatch = new CancellationTokenSource();

        static void Main(string[] args)
        {
            try
            {
                //1：从配置文件中加载数据
                string strJSON = TextHelper.ReadText(CommonHelper.GetAppSetting("jsonCfg"));
                List<JSONObject> jsonData = JSONHelper.GetListByJSON<JSONObject>(strJSON);
                //2：初始化线程工厂
                List<Task> tasks = new List<Task>();
                TaskFactory taskFactory = new TaskFactory();

                //3：用于判断是否是第一个事件的标识符
                bool isFirst = false;
                //4：用于记录时间
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                //5：循环创建人物及绑定事件
                jsonData.ForEach(u =>
                {
                    Person person = new Person(u.Name);
                    if (u.Events.Count() > 0)
                    {
                        u.Events.ForEach(e =>
                        {
                            person.ChangeEvent += () =>
                            {
                                Thread.Sleep(1000);
                                //保证在第一次动作后触发
                                if (e.IsFirst)
                                {
                                    lock (lockObj)
                                    {
                                        LogHelper.WriteInfo($"{u.Name}:{e.EName}", u.Color);
                                        FisrtAction(ref isFirst);
                                    }
                                }
                                else
                                {
                                    LogHelper.WriteInfo($"{u.Name}:{e.EName}", u.Color);
                                }
                            };
                        });
                    }

                    tasks.Add(taskFactory.StartNew((obj) =>
                    {
                        //此处可以设置一个 Thread.Sleep(100)看灭世神雷
                        //Thread.Sleep(100);
                        if (!cts.IsCancellationRequested)
                        {
                            person.Change();
                        }
                    }, person, cts.Token));
                });


                //6:启动一个监控线程,创建一个随机数,如果是当前年份,那么就中断所有线程
                taskFactory.StartNew(() =>
                 {
                     while (!cts.IsCancellationRequested)
                     {
                         //停留一会
                         Thread.Sleep(100);
                         int year = new Random().Next(1, 10000);
                         if (year == DateTime.Now.Year)
                         {
                             cts.Cancel();
                             LogHelper.Warn("天降雷霆灭世，天龙八部的故事就此结束....");
                         }
                         //检查是否剧情都有结束,如果结束了,那么就退出循环
                         //停掉该线程任务
                         if (ctsWatch.IsCancellationRequested)
                         {
                             break;
                         }
                     }
                     LogHelper.Info($"-----监控线程也结束了-----");
                 }, ctsWatch.Token);


                //7：任意一个人员任务线完成后触发
                taskFactory.ContinueWhenAny(tasks.ToArray(), t =>
                 {
                     Person person = t.AsyncState as Person;
                     if (person != null)
                     {
                         Console.ForegroundColor = ConsoleColor.White;
                         Console.WriteLine($"{person.Name}:已经做好准备啦。。。。");
                     }
                 });

                //8：当所有任务都完成后
                taskFactory.ContinueWhenAll(tasks.ToArray(), ts =>
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("中原群雄大战辽兵，忠义两难一死谢天。。。");

                    //7.1 完成后计算耗时
                    stopWatch.Stop();

                    Console.WriteLine("天龙八部共计耗时{0}", stopWatch.Elapsed.TotalSeconds);
                    //结束掉监控线程
                    ctsWatch.Cancel();
                });



            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                throw;
            }


            Console.ReadLine();
        }

        /// <summary>
        /// 第一个事件触发动作
        /// </summary>
        /// <param name="isFirst"></param>
        public static void FisrtAction(ref bool isFirst)
        {
            if (!isFirst)
            {
                isFirst = true;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("天龙八部从此揭开序幕。。。");
            }
        }
    }
}
