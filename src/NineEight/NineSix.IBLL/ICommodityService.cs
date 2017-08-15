using NineEight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.IBLL
{
    public interface ICommodityService
    {
        void InitDbCommodity();

        List<GM_Commodity> GetCommodityList(string strSql);

        int Insert(GM_Commodity entity);
    }
}
