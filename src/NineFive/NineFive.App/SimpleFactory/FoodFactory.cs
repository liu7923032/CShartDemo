using NineFive.Model;
using NineFive.Model.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.App.Factory
{
    /// <summary>
    ///  1:如果要增加一个菜,那么需要增加个类别和添加菜品类
    /// </summary>
    public class SimpleFoodFactory
    {
        public static BaseFood Create(FoodType foodType)
        {
            
            BaseFood food = null;
            switch (foodType)
            {
                case FoodType.Fish:
                    food = new FishFood();
                    break;
                case FoodType.Beef:
                    food = new BeefFood();
                    break;
                case FoodType.Chop:
                    food = new ChopFood();
                    break;
                default:
                    break;
            }
            return food;
        }
    }

    
}
