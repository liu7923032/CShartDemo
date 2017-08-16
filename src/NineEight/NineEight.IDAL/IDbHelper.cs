using NineEight.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.IDAL
{
    public interface IDbHelper
    {

        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        int Insert<T>(T entity, string tableName = "") where T : BaseModel;

        void InsertList<T>(List<T> insertList, string tableName = "") where T : BaseModel;

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity, Dictionary<string, object> changeCols) where T : BaseModel;


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        void Delete<T>(int? Id) where T : BaseModel;


        /// <summary>
        /// 通过id来获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id) where T : BaseModel;
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(string strSql) where T : BaseModel;

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetByPage<T>(Page page) where T : BaseModel;

        /// <summary>
        /// 执行普通的SQL
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSQL, params SqlParameter[] parameters);

        /// <summary>
        /// 在事物中执行SQL
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        void ExecuteNonQueryWithTrans(string strSQL);
    }
}
