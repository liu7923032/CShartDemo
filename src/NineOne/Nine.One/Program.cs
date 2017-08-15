using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nine.Model;
using Nine.Common.Extensions;

namespace Nine.One
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Console.WriteLine("******************第九期 第一节课程 第二次更新******************");
                Console.WriteLine("******************查询所有数据******************");
                //1:动态加载实现出现后dbHelper
                string strCfg = Nine.Common.CommonTools.GetByAppSetting("DbFactory");
                string[] cftArray = strCfg.Split(',');
                Assembly assembly = Assembly.Load(cftArray[1]);
                IDAL.IDbHelper dbHelper = assembly.CreateInstance(cftArray[0]) as IDAL.IDbHelper;

                //2:加载所有User数据
                ShowUsers(dbHelper);

                //3:通过user的id来获取user对象
                Console.WriteLine("******************查询Id=1的User数据******************");
                User user = dbHelper.GetById<User>(1);
                Console.WriteLine($"Id={user.Id},Name={user.Name},Email={user.Email}");

                //4:新增user对象
                Console.WriteLine("******************添加小张******************");
                User uEntity = new User();
                uEntity.Name = "小张";
                uEntity.Mobile = "shouji";
                uEntity.Email = "zhang.x@sina.com";
                uEntity.Password = "mima";
                uEntity.State = 1;
                uEntity.UserType = 1;
                uEntity.LastLoginTime = DateTime.Now;
                uEntity.CreateTime = DateTime.Now;
                uEntity.CreatorId = 1;
                uEntity.CompanyId = 1;
                uEntity.CompanyName = "公司";
                uEntity.Account = "账号";
                dbHelper.Add<User>(uEntity);
                ShowUsers(dbHelper);
                //5:更新user对象
                Console.WriteLine("******************更新Id=1的User数据的名称是dark_liu******************");
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["Name"] = "dark_Liu";
                dbHelper.Update<User>(user, dict);
                ShowUsers(dbHelper);

                Console.WriteLine("******************删除Id=2的User数据******************");

                dbHelper.Delete<User>(2);
                ShowUsers(dbHelper);

                //2:加载所有的Company数据
                Console.WriteLine("******************查询所有Company数据******************");
                List<Company> companys = dbHelper.GetAllList<Company>().ToList();
                companys.ForEach(u =>
                {
                    Console.WriteLine($"Id={u.Id},Name={u.Name}");
                });
                //3:Linq 扩展demo
                Console.WriteLine("******************判断name是否为空,如果不为空,那么就查询,如果为空部查询******************");
                string name = "新";
                IEnumerable<User> userTwos = dbHelper.GetAllList<User>();
                //3.1 检查name是否为空,如果为空,那么就执行结果
                //if (!string.IsNullOrEmpty(name))
                //{
                //    userTwos = userTwos.Where(u => u.Name.Contains(name)).ToList();
                //}
                //if (!string.IsNullOrEmpty(Id))
                //{
                //    userTwos = userTwos.Where(u => u.Name.Contains(Id)).ToList();
                //}
                //if (!string.IsNullOrEmpty(company))
                //{
                //    userTwos = userTwos.Where(u => u.Name.Contains(company)).ToList();
                //}
                //if (!string.IsNullOrEmpty(xx))
                //{
                //    userTwos = userTwos.Where(u => u.xxx.Contains(xx)).ToList();
                //}
                //如果使用上面的方法的话,会有很多的判断,而使用扩展方法WhereIf的话,只需要使用一行袋面就能解决
                //有利也有弊端,不熟悉的话，会看不懂，如果熟悉的情况，感觉还是挺好的
                //userTwos=userTwos.WhereIf(!string.IsNullOrEmpty(name), u => u.Name.Contains(name));
                //userTwos=userTwos.WhereIf(!string.IsNullOrEmpty(xxx), u => u.Name.Contains(xxx));
                //userTwos=userTwos.WhereIf(!string.IsNullOrEmpty(yyyy), u => u.Name.Contains(yyyy));
                var finalData = userTwos.WhereIf(!string.IsNullOrEmpty(name), u => u.Name.Contains(name)).ToList();

                finalData.ForEach(u =>
                {
                    Console.WriteLine($"Id={u.Id},Name={u.Name},Email={u.Email}");
                });

                Console.WriteLine("******************获取用户表的,最大的id******************");
                List<User> userLinq = dbHelper.GetAllList<User>().ToList();
                var maxUserId = userLinq.Max(u => u.Id);
                Console.WriteLine("User表中id的最大值是:{0}", maxUserId);

                //3.2 执行分页查询
                Console.WriteLine("******************执行分页查询测试,只支持SQL SERVER 2012以上******************");
                var pageRows = dbHelper.GetPageList<User>(new Page() { PageIndex = 2, PageSize = 5, sort = "Id" }).ToList();
                pageRows.ForEach(u =>
                {
                    Console.WriteLine($"Id={u.Id},Name={u.Name},Email={u.Email}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("/***************错误信息==================*/");
                Console.WriteLine("{0}", ex.Message);
            }

            Console.WriteLine("******************************测试完成了******************************");
            Console.Read();
        }

        //加载所有的用户数据
        public static void ShowUsers(IDAL.IDbHelper dbHelper)
        {
            //2:加载所有的User及Company数据0
            Console.WriteLine("******************查询所有User数据******************");
            List<User> users = dbHelper.GetAllList<User>().ToList();
            users = users.OrderBy(u => u.Id).ToList();

            users.ForEach(u =>
            {
                Console.WriteLine($"Id={u.Id},Name={u.Name},Email={u.Email}");
            });
        }
    }
}
