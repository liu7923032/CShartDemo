
using Lucene.Net.Search;
using NineSeven.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NineSeven.LuceneNet.Interface
{
    public interface ILuceneBulid<T> where T : BaseModel
    {

        #region 1.0 插入索引
        /// <summary>
        /// 新增一条数据的索引
        /// </summary>
        /// <param name="ci"></param>
        void Insert(T ci);
        /// <summary>
        /// 批量创建索引
        /// </summary>
        /// <param name="ciList"></param>
        /// <param name="pathSuffix">索引目录后缀，加在电商的路径后面，为空则为根目录.如sa\1</param>
        /// <param name="isCreate">默认为false 增量索引  true的时候删除原有索引</param>
        void InsertList(List<T> ciList, string pathSuffix = "", bool isCreate = false);

        /// <summary>
        /// 将索引合并到上级目录
        /// </summary>
        /// <param name="sourceDir">子文件夹名</param>
        void Merge(string[] sourceDirs);
        #endregion

        #region 2.0 删除索引
        /// <summary>
        /// 删除一条数据的索引
        /// </summary>
        /// <param name="ci"></param>
        void Delete(T ci);

        /// <summary>
        /// 批量删除数据的索引
        /// </summary>
        /// <param name="ciList">sourceflag统一的</param>
        void DeleteList(List<T> ciList);

        #endregion

        #region 3.0 更新索引
        /// <summary>
        /// 更新一条数据的索引
        /// </summary>
        /// <param name="ci"></param>
        void Update(T ci);

        /// <summary>
        /// 批量更新数据的索引
        /// </summary>
        /// <param name="ciList">sourceflag统一的</param>
        void UpdateList(List<T> ciList);
        #endregion

        #region 4.0 查询
        /// <summary>
        /// 获取商品信息数据
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        List<T> Query(string strCondition);

        /// <summary>
        /// 分页获取商品信息数据
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="pageIndex">第一页为1</param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<T> QueryByPage(string strCondition, Page page, string strFilter);
        #endregion
    }
}
