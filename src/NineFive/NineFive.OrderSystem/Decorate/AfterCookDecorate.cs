using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NineFive.Model.OrderSystem;

namespace NineFive.OrderSystem.Decorate
{
    public class AfterCookDecorate : BaseFoodDecorate
    {
        public AfterCookDecorate(AbstractFood food) : base(food)
        {
        }

        public override void Cook()
        {
            base.Cook();
            Console.WriteLine("摆盘");
            Console.WriteLine("上菜");
        }
    }
}
