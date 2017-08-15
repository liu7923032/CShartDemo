using Nine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nine.IDAL
{
    public interface IDbHelper
    {
        /// <summary>
        /// 通过id来获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id) where T : BaseModel;
        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        int Add<T>(T entity) where T : BaseModel;
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity, Dictionary<string,object> changeCols) where T : BaseModel;
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        void Delete<T>(int Id) where T : BaseModel;

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllList<T>() where T : BaseModel;
        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetPageList<T>(Page page) where T : BaseModel;
    }
}
