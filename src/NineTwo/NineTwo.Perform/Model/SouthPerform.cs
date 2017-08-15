using NineTwo.Perform.Base;
using NineTwo.Perform.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineTwo.Perform.Model
{
    public class SouthPerform:AbsPerform, ICharge
    {

        public string South { get; set; } = $"南派传人属性";
        public string filedSouth = $"南派传人字段";

        

        public override void DogVoice()
        {
            Console.WriteLine($"{SYMBOL}{base.User}开始学狗叫了{SYMBOL}");
        }

        public override void PersonVoice()
        {
            Console.WriteLine($"{SYMBOL}{base.User}开始人声叫了{SYMBOL}");
        }

        public override void WindVoice()
        {
            Console.WriteLine($"{SYMBOL}{base.User}开始风声叫了{SYMBOL}");
        }

        public override void BeginPerform()
        {
            Console.WriteLine($"{SYMBOL}开场白:{base.User}:这回请听我分解{SYMBOL}");
        }

        public  void Charge(decimal money)
        {
            Console.WriteLine($"{SYMBOL}{base.User}:谢谢大爷的赏钱,共:{money}{SYMBOL}");
        }
    }
}
