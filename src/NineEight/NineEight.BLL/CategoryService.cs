using NineEight.Common;
using NineEight.IBLL;
using NineEight.IDAL;
using NineEight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.BLL
{
    public class CategoryService : AppService, ICategoryService
    {
        IDbHelper _dbHelper;
        public CategoryService(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        /// <summary>
        /// 将数据添加到数据库中
        /// </summary>
        /// <param name="categoryList"></param>
        public void AddCategory(List<Category> categoryList)
        {
            _dbHelper.InsertList<Category>(categoryList);
        }

        /// <summary>
        /// 将数据均分到各个表中
        /// </summary>
        /// <param name="commodityList"></param>
        public void InsertCommodity(List<GM_Commodity> commodityList)
        {
            var typeName = typeof(GM_Commodity).Name;
            var allCount = commodityList.Count;
            //1：计算每张表的数量
            var avgCount = allCount / 30;
            if (allCount % 30 != 0)
            {
                avgCount += 1;
            }
            //2：得到每张表的商品
            List<GM_Commodity> categoryList = null;
            for (int i = 1; i <= 30; i++)
            {
                categoryList = new List<GM_Commodity>();
                string tableName = $"{typeName}_{i.ToString("000")}";
                int start = (i - 1) * avgCount;
                int end = i * avgCount;
                if (end > allCount)
                {
                    end = allCount;
                }
                //2.1 将表数量添加到
                for (int j = start; j < end; j++)
                {
                    categoryList.Add(commodityList[j]);
                }
                _dbHelper.InsertList<GM_Commodity>(categoryList, tableName);
            }

        }
        /// <summary>
        /// 获取所有的商品数据
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategoryList()
        {
            return _dbHelper.GetAll<Category>("").ToList();
        }



        public void InitDbCatetory()
        {
            #region Delete
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DROP TABLE [dbo].[Category];");

                _dbHelper.ExecuteNonQuery(sb.ToString());
                logger.Info("Category 删除成功");
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("无法对 表 'dbo.Category' 执行 删除，因为它不存在，或者您没有所需的权限。"))
                {
                    logger.Warn("初始化数据库InitCategoryTable删除的时候，原表不存在");
                }
                else
                {
                    logger.Error("初始化数据库InitCategoryTable失败", ex);
                    throw ex;
                }
            }
            #endregion Delete

            #region Create
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"CREATE TABLE [dbo].[Category](
	                                    [Id] [int] IDENTITY(1,1) NOT NULL,
	                                    [Code] [varchar](100) NULL,
	                                    [ParentCode] [varchar](100) NULL,
	                                    [CategoryLevel] [int] NULL,
	                                    [Name] [nvarchar](50) NULL,
	                                    [Url] [varchar](1000) NULL,
	                                    [State] [int] NULL,
                                      CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
                                     (
                                     	[Id] ASC
                                     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                     ) ON [PRIMARY];");

                _dbHelper.ExecuteNonQuery(sb.ToString());
                logger.Info("Category 创建成功");
            }
            catch (Exception ex)
            {
                logger.Error("初始化数据库InitCategoryTable 创建失败", ex);
                throw ex;
            }
            #endregion Create
        }

      
    }
}
