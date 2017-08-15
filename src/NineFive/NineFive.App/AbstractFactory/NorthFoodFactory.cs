using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NineFive.Model.Food;
using NineFive.Model;

namespace NineFive.App.AbstractFactory
{
    public class NorthFoodFactory : AbsFoodFactory
    {
        public override BaseFood CreateBeef()
        {
            return new BeefFood();
        }

        public override BaseFood CreateChop()
        {
            return new ChopFood();
        }

        public override BaseFood CreateFish()
        {
            return new FishFood();
        }

        public override BaseFood CreateRice()
        {
            return new NRiceFood();
        }

        public override BaseFood CreateSoup()
        {
            return new NSoupFood();
        }
    }
}
