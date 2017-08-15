using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.Food
{
    /// <summary>
    /// 鱼
    /// </summary>
    public class FishFood : BaseFood
    {
        public override void Show()
        {
            Console.WriteLine("这个生鱼片");
        }
    }
}
