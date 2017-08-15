using NineFour.Common;
using NineFour.Common.Extentision;
using NineFour.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFour.DAL
{
    public class SQLHelper : ISQLHelper
    {
        private static LogHelper logger = new LogHelper(typeof(SQLHelper));

        public void Insert<T>(T value) where T : class
        {
            ////检查该对象是否都满足条件
            //string errMsg = value.CheckValidate<T>(); 

            //if (!string.IsNullOrEmpty(errMsg))
            //{
            //    logger.Error(errMsg);
            //    return;
            //}

            logger.Info("*********处理数据业务逻辑*********");
        }
    }
}
