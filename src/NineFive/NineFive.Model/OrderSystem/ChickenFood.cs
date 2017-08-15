using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.OrderSystem
{
    public class ChickenFood : AbstractFood
    {
        public override void Cook()
        {
            Console.WriteLine("土豆焖鸡下锅了");
        }
    }
}
