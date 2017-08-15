using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.OrderSystem
{
    /// <summary>
    /// 排骨
    /// </summary>
    public class ChopFood : AbstractFood
    {
        public override void Cook()
        {
            Console.WriteLine("排骨下锅了");
        }

        public override void Comment()
        {
            this.Score = new Random().Next(1, 9);
        }
    }
}
