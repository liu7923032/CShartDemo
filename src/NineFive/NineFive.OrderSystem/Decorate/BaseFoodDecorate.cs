using NineFive.Model.OrderSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.OrderSystem.Decorate
{
    public class BaseFoodDecorate : AbstractFood
    {
        private AbstractFood _food;

        public BaseFoodDecorate(AbstractFood food)
        {
            this._food = food;
        }

        public override void Cook()
        {
            this._food.Cook();
        }
    }
}
