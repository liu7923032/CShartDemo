using NineTwo.Perform.Common;
using NineTwo.Perform.Interface;
using System;


namespace NineTwo.Perform.Base
{
    /// <summary>
    /// 表演者的抽象类
    /// </summary>
    public abstract class AbsPerform
    {

        /// <summary>
        /// 起火点
        /// </summary>
        public int Temperature
        {
            get; set;
        }


        /// <summary>
        /// 默认起火的方法
        /// </summary>
        public virtual void Fire(int temperature)
        {
            //默认400℃起火
            if (temperature > Temperature)
            {
                Console.WriteLine($"{SYMBOL}起火啦:当前温度:{temperature}{SYMBOL}");
                //触发事件
                FireEvent?.Invoke();
            }
        }

        public const string SYMBOL = "*";


        /// <summary>
        /// 表演者
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 桌子
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 椅子
        /// </summary>
        public string Chair { get; set; }

        /// <summary>
        /// 尺子
        /// </summary>
        public string Ruler { get; set; }

        /// <summary>
        /// 扇子
        /// </summary>
        public string Fun { get; set; }

        /// <summary>
        /// 普通启动的方法
        /// </summary>
        public void Show(int temperature)
        {
            Console.WriteLine($"{SYMBOL}表演开始了{SYMBOL}");
            Console.WriteLine($"*{User}坐着{Chair},手里拿着{Ruler},开始表演了*");
            //启动方法
            this.BeginPerform();
            this.DogVoice();
            this.PersonVoice();
            this.WindVoice();
            this.Fire(temperature);
            this.EndPerform();
        }
        /// <summary>
        /// 狗声音
        /// </summary>
        public abstract void DogVoice();

        /// <summary>
        /// 人声音
        /// </summary>
        public abstract void PersonVoice();

        /// <summary>
        /// 风声音
        /// </summary>
        public abstract void WindVoice();

        /// <summary>
        /// 开场白
        /// </summary>
        public virtual void BeginPerform()
        {
            Console.WriteLine($"{SYMBOL}开场白:从前有座山,山里有个庙{SYMBOL}");
        }

        /// <summary>
        /// 结束语
        /// </summary>
        public virtual void EndPerform()
        {
            Console.WriteLine($"{SYMBOL}结束语:谢谢,有钱捧个钱场,没钱捧个人场,欢迎下次在来{SYMBOL}");
        }

        //定义失火的事件
        public event Action FireEvent;

    }
}
