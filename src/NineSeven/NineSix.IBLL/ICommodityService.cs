using NineSeven.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSeven.IBLL
{
    public interface ICommodityService
    {
        void InitDbCommodity();

        List<GM_Commodity> GetCommodityList(string strSql);
    }
}
