using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.OrderSystem
{
    public class FishFood : AbstractFood
    {
        public override void Cook()
        {
            Console.WriteLine("开始煮酸菜鱼了");
        }

        public override void Comment()
        {
            this.Score = new Random().Next(1, 9);
        }
    }
}
