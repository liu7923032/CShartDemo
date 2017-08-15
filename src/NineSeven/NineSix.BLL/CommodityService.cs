using NineSeven.IBLL;
using NineSeven.IDAL;
using NineSeven.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSeven.BLL
{
    public class CommodityService : AppService, ICommodityService
    {
        public IDbHelper _dbHelper;
        public CommodityService(IDbHelper dbHelper)
        {
            this._dbHelper = dbHelper;
        }

        public List<GM_Commodity> GetCommodityList(string strSql)
        {
            return _dbHelper.GetAll<GM_Commodity>(strSql).ToList();
          
        }


        public int Insert(GM_Commodity entity)
        {
           return _dbHelper.Insert<GM_Commodity>(entity, "GM_Commodity_030");
        }

        public void InitDbCommodity()
        {
            #region Delete
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < 31; i++)
                {
                    sb.AppendFormat("IF  EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='GM_Commodity_{0}') DROP TABLE GM_Commodity_{1};", i.ToString("000"), i.ToString("000"));
                }
                _dbHelper.ExecuteNonQuery(sb.ToString());
                logger.Info("Commodity 创建成功");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("因为它不存在，或者您没有所需的权限。"))
                {
                    logger.Warn("初始化数据库InitCommodityTable删除的时候，原表不存在");
                }
                else
                {
                    logger.Error("初始化数据库InitCommodityTable失败", ex);
                    throw ex;
                }
            }
            #endregion Delete

            #region Create
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < 31; i++)
                {
                    sb.AppendFormat(@"CREATE TABLE [dbo].[GM_Commodity_{0}](
                                        [Id] [INT] IDENTITY(1,1) NOT NULL,
	                                    [Title] [NVARCHAR](500) NULL,
                                        [SkuId] [NVARCHAR](50) NULL,
                                        [ProductId] [NVARCHAR](50) NULL,
                                        [Price] [DECIMAL](18, 2) NULL,
                                        [Url] [NVARCHAR](100) NULL,
                                        [ImageUrl] [NVARCHAR](500) NULL,
                             CONSTRAINT [PK_GM_Commodity_{0}] PRIMARY KEY CLUSTERED 
                            (
                            	[Id] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY];", i.ToString("000"));
                }
                _dbHelper.ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("InitCommodityTable创建异常", ex);
                throw ex;
            }
            #endregion Create
        }
    }
}
