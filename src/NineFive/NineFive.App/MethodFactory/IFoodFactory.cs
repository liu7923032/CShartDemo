using NineFive.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.App.MethodFactory
{
    interface IFoodFactory
    {
        BaseFood Create();
    }
}
