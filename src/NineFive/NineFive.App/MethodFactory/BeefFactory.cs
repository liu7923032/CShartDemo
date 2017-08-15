﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NineFive.Model;
using NineFive.Model.Food;

namespace NineFive.App.MethodFactory
{
    public class BeefFactory : IFoodFactory
    {
        public BaseFood Create()
        {
            return new BeefFood();
        }
    }
}
