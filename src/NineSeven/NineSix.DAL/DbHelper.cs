using NineSeven.IDAL;
using NineSeven.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NineSeven.DAL
{
    public class DbHelper : IDbHelper
    {
        public DbHelper() { }

        #region 0.0 数据库链接字符串
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static readonly string conStr = ConfigurationManager.ConnectionStrings["ConnectStr"].ConnectionString;
        #endregion

        #region 1.0 添加和批量添加

        /// <summary>
        /// 添加,返回产生的Id值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<T>(T entity, string tableName = "") where T : BaseModel
        {
            //1：构建插入SQL
            Type type = typeof(T);
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();

            List<SqlParameter> parameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = type.Name;
            }
            //排除掉Id
            foreach (var item in type.GetProperties())
            {
                if (item.Name == "Id")
                {
                    continue;
                }

                var propValue = item.GetValue(entity);
                if (propValue != null)
                {
                    sbColumns.Append($"{item.Name},");
                    //构建参数化配置
                    sbValues.Append($"@{item.Name},");
                    parameters.Add(new SqlParameter($"@{item.Name}", propValue));
                }
            }

            string strSQL = $"Insert into DBO.[{tableName}] ({sbColumns.ToString().Substring(0, sbColumns.Length - 1)}) Values({sbValues.ToString().Substring(0, sbValues.Length - 1)});select @@IDENTITY";
            //2:执行SQL,并返回id
            string result = DbCommand<string>(strSQL, (cmd) =>
               {
                   return cmd.ExecuteScalar().ToString();
               }, parameters);
            int res = 0;
            int.TryParse(result, out res);
            return res;
        }

        /// <summary>
        /// 批量生成插入SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetInsertSql<T>(T model, string tableName = "") where T : BaseModel
        {
            StringBuilder sbSql = new StringBuilder();

            StringBuilder sbFields = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();

            Type type = model.GetType();
            var properties = type.GetProperties();
            foreach (PropertyInfo p in properties)
            {
                string name = p.Name;
                if (!name.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    //1:拼接字段
                    sbFields.AppendFormat("[{0}],", name);
                    string sValue = null;
                    object oValue = p.GetValue(model);
                    //2:拼接值
                    if (oValue != null)
                        sValue = oValue.ToString().Replace("'", "");
                    sbValues.AppendFormat("'{0}',", sValue);
                }
            }
            sbSql.AppendFormat("INSERT INTO {0} ({1}) VALUES ({2});", string.IsNullOrEmpty(tableName) ? type.Name : tableName, sbFields.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
            return sbSql.ToString();
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="insertList"></param>
        public void InsertList<T>(List<T> insertList, string tableName = "") where T : BaseModel
        {
            string sql = string.Join(" ", insertList.Select(t => GetInsertSql<T>(t, tableName)));
            ExecuteNonQueryWithTrans(sql);
        }
        #endregion

        #region 2.0 删除和批量删除
        /// <summary>
        /// 通过Id来删除指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        public void Delete<T>(int? Id) where T : BaseModel
        {
            Type type = typeof(T);

            //1：构建参数命令和where条件
            List<SqlParameter> parameters = null;
            string strWhere = string.Empty;
            //2：检查Id是否为空
            if (Id == null)
            {
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Id",Id.Value)
                };
                strWhere = Id == null ? "" : "WHERE Id = @Id";
            }

            //3：构建SQL
            string strSQL = $"DELETE FROM DBO.[{type.Name}] {strWhere} ";
            //4：执行SQL
            DbCommand<int>(strSQL, (cmd) =>
              {
                  return cmd.ExecuteNonQuery();
              }, parameters);
        }

        #endregion

        #region 3.0 查询 （id查询/查询所有/分页查询）
        /// <summary>
        /// 通过Id主键来获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //1.构建SQL
            string strSql = $"SELECT * FROM DBO.[{type.Name}] WHERE Id=@Id";
            //2.构建参数列表
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Id",id)
            };
            //3：执行SQL
            return DbCommand<T>(strSql, (cmd) =>
            {
                //3：创建读取对象
                IDataReader reader = cmd.ExecuteReader();

                PropertyInfo[] props = type.GetProperties();
                //4：实例化model
                T obj = Activator.CreateInstance(typeof(T)) as T;
                //5：循环读取数据
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        //5.1:找到该字段名称
                        var fieldName = reader.GetName(i);
                        //5.1:通过字段名称来获取属性
                        PropertyInfo prop = props.Where(u => u.Name.Equals(fieldName)).FirstOrDefault();
                        if (prop != null && !reader.IsDBNull(i))
                        {
                            prop.SetValue(obj, reader.GetValue(i));
                        }
                    }

                }
                //5：读取完关闭对象
                reader.Close();
                return obj;
            }, parameters);
        }
        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string strSql = "") where T : BaseModel
        {
            var type = typeof(T);
            //1：构建SQL
            if (string.IsNullOrEmpty(strSql))
            {
                strSql = $"SELECT * FROM DBO.[{type.Name}]";
            }
            //2：执行SQL
            return DbCommand<List<T>>(strSql, (cmd) =>
              {
                  //1：创建读取对象
                  IDataReader reader = cmd.ExecuteReader();
                  PropertyInfo[] props = type.GetProperties();
                  List<T> list = new List<T>();

                  //2：循环读取数据
                  while (reader.Read())
                  {
                      //4.1：实例化model
                      T obj = Activator.CreateInstance(type) as T;
                      foreach (PropertyInfo p in type.GetProperties())
                      {
                          var value = reader[p.Name];
                          if (value is DBNull)
                          {
                              continue;
                          }
                          p.SetValue(obj, Convert.ChangeType(value, p.PropertyType));
                      }
                      //4.3：添加对象到集合
                      list.Add(obj);
                  }
                  //5：读取完关闭对象
                  reader.Close();
                  //6：返回list集合
                  return list;
              });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByPage<T>(Page page) where T : BaseModel
        {
            Type type = typeof(T);
            //1.构建SQL
            string strSQL = $"SELECT * FROM dbo.[{type.Name}]  ORDER BY {page.Sort} offset {page.PageSize * (page.PageIndex - 1)} rows fetch next {page.PageSize} rows only;";
            //2：执行SQL
            return DbCommand<List<T>>(strSQL, (cmd) =>
           {
               //1：创建读取对象
               IDataReader reader = cmd.ExecuteReader();
               PropertyInfo[] props = type.GetProperties();
               List<T> list = new List<T>();
               //2：循环读取数据
               while (reader.Read())
               {
                   //4.1：实例化model
                   T obj = Activator.CreateInstance(typeof(T)) as T;
                   for (int i = 0; i < reader.FieldCount; i++)
                   {
                       //5.1:找到该字段名称
                       var fieldName = reader.GetName(i);
                       //5.1:通过字段名称来获取属性
                       PropertyInfo prop = props.Where(u => u.Name.Equals(fieldName)).FirstOrDefault();
                       if (prop != null && !reader.IsDBNull(i))
                       {
                           prop.SetValue(obj, reader.GetValue(i));
                       }
                   }
                   //4.3：添加对象到集合
                   list.Add(obj);
               }
               //5：读取完关闭对象
               reader.Close();
               //6：返回list集合
               return list;
           });

        }

        #endregion

        #region 4.0 更新
        /// <summary>
        /// 通过自定对象来变更对象的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="changeData">要变更的数据 变更的字段和对于变更的值</param>
        public void Update<T>(T entity, Dictionary<string, object> changeData) where T : BaseModel
        {
            Type type = typeof(T);
            StringBuilder sb = new StringBuilder();
            //构建命令参数
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Id",entity.Id)
            };

            foreach (var item in type.GetProperties())
            {
                if (changeData.ContainsKey(item.Name))
                {
                    sb.Append($"{item.Name} = @{item.Name},");
                    parameters.Add(new SqlParameter($"@{item.Name}", changeData[item.Name]));
                }
            }

            string strSQL = $"UPDATE DBO.[{type.Name}] SET {sb.ToString().Substring(0, sb.Length - 1)} WHERE Id=@Id";

            DbCommand<int>(strSQL, (cmd) =>
             {
                 return cmd.ExecuteNonQuery();
             }, parameters);
        }


        #endregion

        #region 5.0 执行SQL 和 执行事务
        public int ExecuteNonQuery(string strSQL, params SqlParameter[] parameters)
        {
            return DbCommand<int>(strSQL, (cmd) =>
              {
                  return cmd.ExecuteNonQuery();
              }, parameters.ToList());
        }

        /// <summary>
        /// 执行事务SQL
        /// </summary>
        /// <param name="strSQL"></param>
        public void ExecuteNonQueryWithTrans(string strSQL)
        {
            SqlTransaction trans = null;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(conStr))
                {
                    sqlConn.Open();
                    trans = sqlConn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand(strSQL, sqlConn, trans);
                    cmd.ExecuteNonQuery();//.ExecuteNonQueryAsync();//
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("ExecuteNonQueryWithTrans出现异常，sql={0}", sql), ex);
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                throw ex;
            }
            finally
            {
            }
        }
        #endregion

        #region 6.0 通用SQL链接

        /// <summary>
        /// 执行SQL 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private W DbCommand<W>(string strSql, Func<IDbCommand, W> action, List<SqlParameter> parameters = null)
        {
            using (IDbConnection conn = new SqlConnection(conStr))
            {
                //1：打开数据库连接
                conn.Open();
                //2：创建数据库命令
                IDbCommand cmd = conn.CreateCommand();
                //3：构建SQL 参数
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                cmd.CommandText = strSql;
                cmd.CommandType = CommandType.Text;
                //4：执行链接后的方法
                return action(cmd);
            }
        }
        #endregion
    }
}
