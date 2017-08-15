using NineEight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.IBLL
{
    public interface ICategoryService:IAppService
    {
        void AddCategory(List<Category> categoryList);
        List<Category> GetCategoryList();

        void InsertCommodity(List<GM_Commodity> commodityList);
        void InitDbCatetory();
    }
}
