using NineSix.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.IBLL
{
    public interface ICategoryService:IAppService
    {
        void AddCategory(List<Category> categoryList);
        List<Category> GetAllList();

        void InsertCommodity(List<GM_Commodity> commodityList);

        void InitDbCatetory();
        void InitDbCommodity();
    }
}
