using NineFive.Model;
using NineFive.Model.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.App.MethodFactory
{
    public class FishFactory : IFoodFactory
    {
        public BaseFood Create()
        {
            return new FishFood();
        }
    }
}
