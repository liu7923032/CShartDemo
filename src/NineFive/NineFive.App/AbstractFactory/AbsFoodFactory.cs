using NineFive.Model;
using NineFive.Model.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.App.AbstractFactory
{
    public abstract class AbsFoodFactory
    {
        public abstract BaseFood CreateSoup();

        public abstract BaseFood CreateRice();

        public abstract BaseFood CreateBeef();

        public abstract BaseFood CreateFish();

        public abstract BaseFood CreateChop();
    }
}
