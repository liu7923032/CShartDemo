using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.Food
{
    /// <summary>
    /// 牛肉
    /// </summary>
    public class BeefFood : BaseFood
    {
        public override void Show()
        {
            Console.WriteLine("这是烤牛肉");
        }
    }
}
