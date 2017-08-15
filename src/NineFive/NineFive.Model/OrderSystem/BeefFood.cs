using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.OrderSystem
{
    /// <summary>
    /// 牛肉
    /// </summary>
    public class BeefFood : AbstractFood
    {
        public override void Cook()
        {
            Console.WriteLine("牛肉下锅了");
        }
    }
}
