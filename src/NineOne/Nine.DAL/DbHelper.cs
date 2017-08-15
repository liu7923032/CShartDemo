using Nine.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nine.DAL
{
    public class DbHelper : IDAL.IDbHelper
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static readonly string conStr = Common.CommonTools.GetByAppSetting("DbConnect");


        /// <summary>
        /// 添加,返回产生的Id值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add<T>(T entity) where T : BaseModel
        {
            //1：构建插入SQL
            Type type = typeof(T);
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();

            List<SqlParameter> parameters = new List<SqlParameter>();

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


            string strSQL = $"Insert into DBO.[{type.Name}] ({sbColumns.ToString().Substring(0, sbColumns.Length - 1)}) Values({sbValues.ToString().Substring(0, sbValues.Length - 1)});SELECT @@Identity;";
            //2:执行SQL,并返回id
            string result = DbCommand<T, string>(strSQL, parameters, (cmd) =>
                {
                    return cmd.ExecuteScalar().ToString();
                });
            int res = 0;
            int.TryParse(result, out res);
            return res;
        }

        /// <summary>
        /// 通过Id来删除指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        public void Delete<T>(int Id) where T : BaseModel
        {
            Type type = typeof(T);

            //1：构建SQL
            string strSQL = $"DELETE FROM DBO.[{type.Name}] WHERE Id=@Id";
            //2：构建参数命令
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Id",Id)
            };
            //3：执行SQL
            DbCommand<T, ValueType>(strSQL, parameters, (cmd) =>
              {
                  return cmd.ExecuteNonQuery();
              });
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllList<T>() where T : BaseModel
        {
            var type = typeof(T);
            //1：构建SQL
            string strSql = $"SELECT * FROM DBO.[{type.Name}]";
            //2：执行SQL
            return DbCommand<T, List<T>>(strSql, null, (cmd) =>
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
                      //4.5：找到主键
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
            return DbCommand<T, T>(strSql, parameters, (cmd) =>
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
              });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPageList<T>(Page page) where T : BaseModel
        {
            Type type = typeof(T);
            //1.构建SQL
            string strSQL = $"SELECT * FROM dbo.[{type.Name}]  ORDER BY {page.sort} offset {page.PageSize * (page.PageIndex - 1)} rows fetch next {page.PageSize} rows only;";
            //2：执行SQL
            return DbCommand<T, List<T>>(strSQL, null, (cmd) =>
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

            DbCommand<T, ValueType>(strSQL, parameters, (cmd) =>
             {
                 return cmd.ExecuteNonQuery();
             });
        }

        /// <summary>
        /// 统一的连接请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private W DbCommand<T, W>(string strSql, List<SqlParameter> parameters, Func<IDbCommand, W> action) where T : BaseModel
            where W : class
        {
            using (IDbConnection conn = new SqlConnection(conStr))
            {
                //1：打开数据库连接
                conn.Open();
                //2：创建数据库命令
                IDbCommand cmd = conn.CreateCommand();
                //3：构建SQL 参数
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Count > 0)
                {
                    parameters.ForEach(u =>
                    {
                        cmd.Parameters.Add(u);
                    });
                }
                cmd.CommandText = strSql;
                cmd.CommandType = CommandType.Text;
                //4：执行链接后的方法
                return action(cmd);
            }
        }
    }
}
