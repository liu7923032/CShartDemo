using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFour.IDAL
{
    public interface ISQLHelper
    {
        void Insert<T>(T value) where T : class;
    }
}
