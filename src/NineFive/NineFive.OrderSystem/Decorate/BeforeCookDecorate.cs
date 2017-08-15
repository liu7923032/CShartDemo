using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NineFive.Model.OrderSystem;

namespace NineFive.OrderSystem.Decorate
{
    public class BeforeCookDecorate : BaseFoodDecorate
    {
        public BeforeCookDecorate(AbstractFood food) : base(food)
        {
            
        }

        public override void Cook()
        {
            Console.WriteLine("买菜");
            Console.WriteLine("洗菜");
            Console.WriteLine("切菜");
            base.Cook();
        }
    }
}
