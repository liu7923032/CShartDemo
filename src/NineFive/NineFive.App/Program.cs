using NineFive.App.AbstractFactory;
using NineFive.App.Factory;
using NineFive.App.MethodFactory;
using NineFive.Model;
using NineFive.Model.OrderSystem;
using NineFive.OrderSystem;
using NineFive.OrderSystem.Decorate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.App
{
    class Program
    {
        private static string prefix = "*************************";
        static void Main(string[] args)
        {
            try
            {
                #region 1.0 普通的展示菜的方法
                {
                    Console.WriteLine($"{prefix}普通方法{prefix}");
                    NineFive.Model.Food.BeefFood beefFood = new NineFive.Model.Food.BeefFood();
                    beefFood.Show();

                    NineFive.Model.Food.FishFood fishFood = new NineFive.Model.Food.FishFood();
                    fishFood.Show();

                    NineFive.Model.Food.ChopFood chopFood = new NineFive.Model.Food.ChopFood();
                    chopFood.Show();
                }
                #endregion

                #region 2.0 简单工厂点菜
                {
                    //简单工厂：如果要加一类修改case 条件，添加类别
                    //长处: 简单，有效，类型创建剥离出来了
                    //短处：添加或者删除类，需要变更DLL,还需要该case或者if 条件
                    Console.WriteLine($"{prefix}简单工厂{prefix}");
                    BaseFood beefFood = SimpleFoodFactory.Create(FoodType.Beef);
                    beefFood.Show();
                    BaseFood fishFood = SimpleFoodFactory.Create(FoodType.Fish);
                    fishFood.Show();
                    BaseFood chopFood = SimpleFoodFactory.Create(FoodType.Chop);
                    chopFood.Show();
                }
                #endregion

                #region 3.0 工厂方法
                {
                    Console.WriteLine($"{prefix}工厂方法{prefix}");
                    //工厂方法：如果要加一类，只需添加工厂就可以
                    //长处: 创建单一产品的时候只需结合反射和配置文件可以完美的实现对象的创建
                    //短处：增加和修改产品依然需要变更DLL文件
                    IFoodFactory beefFactory = new BeefFactory();
                    beefFactory.Create().Show();

                    IFoodFactory fishFactory = new FishFactory();
                    fishFactory.Create().Show();

                    IFoodFactory chopFactory = new ChopFactory();
                    chopFactory.Create().Show();

                }
                #endregion

                #region 4.0 抽象工厂
                {
                    Console.WriteLine($"{prefix}抽象工厂{prefix}");
                    //抽象工厂：如果要加一类，只需添加工厂实现父类就可
                    //长处: 在创建多产品的时候非常适用
                    //短处：因为父类中有多产品，和父类耦合度过高
                    AbsFoodFactory sFactory = new SouthFoodFactory();
                    sFactory.CreateRice().Show();
                    sFactory.CreateSoup().Show();
                    sFactory.CreateBeef().Show();
                    sFactory.CreateFish().Show();
                    sFactory.CreateChop().Show();
                    AbsFoodFactory nFactory = new NorthFoodFactory();
                    nFactory.CreateRice().Show();
                    nFactory.CreateSoup().Show();
                    nFactory.CreateBeef().Show();
                    nFactory.CreateFish().Show();
                    nFactory.CreateChop().Show();

                }
                #endregion

                #region 5.0 点菜系统
                {
                    bool isExist = true;
                    OrderMenu.GetInstance();
                    while (isExist)
                    {
                        //1：显示菜单
                        OrderMenu.GetInstance().ShowMenu();
                        //2：等待用户输入要点的菜
                        string foods = Console.ReadLine();
                        if (foods.Trim().Length == 0)
                        {
                            //new OrderSystem.OrderMenu().ShowMenu();
                            continue;
                        }
                        //3:显示我点的菜名称
                        string[] strList = foods.Split(',');
                        if (strList.Contains("0"))
                        {
                            isExist = false;
                            continue;
                        }
                        List<int> foodIds = new List<int>();
                        foreach (var item in strList)
                        {
                            foodIds.Add(Convert.ToInt32(item));
                        }
                        OrderMenu.GetInstance().AllFoods.Where(u => foodIds.Contains(u.Id)).OrderBy(u => u.Id).ToList().ForEach(u =>
                            {
                                Console.WriteLine($"我点的菜是：{u.FoodName},描述:{u.Describe}");
                            });
                    }
                }
                #endregion

                #region 6.0 顾客点菜
                {

                    OrderMenu orderMenu = OrderMenu.GetInstance();
                    //1:创建客人
                    List<Customer> customers = new List<Customer>();
                    Customer aCustomer = new Customer("甲");
                    Customer bCustomer = new Customer("乙");
                    Customer cCustomer = new Customer("丙");
                    customers.Add(aCustomer);
                    customers.Add(bCustomer);
                    customers.Add(cCustomer);

                    //2:客人开始点菜
                    Console.WriteLine($"{prefix}三个客人开始点菜了{prefix}");
                    //2.1 加载程序集，通过反射来加载菜的程序集
                    Assembly assembly = Assembly.Load("NineFive.Model");
                    //3：创建多线程
                    TaskFactory taskFactory = new TaskFactory();
                    List<Task> tasks = new List<Task>();
                    tasks.Add(taskFactory.StartNew(() =>
                   {
                       //1:随机点3个菜
                       List<AbstractFood> foods = GetFoods();
                       foods.ForEach(u =>
                       {
                           //通过反射来创建菜的对象
                           AbstractFood newFood = u.Clone() as AbstractFood;
                           aCustomer.Foods.Add(newFood);
                       });
                       //2:开始品菜
                       aCustomer.Foods.ForEach(u =>
                      {
                          //装饰器模式实现
                          AbstractFood food = new BaseFoodDecorate(u);
                          food = new BeforeCookDecorate(food);
                          food = new AfterCookDecorate(food);
                          //做菜
                          food.Cook();
                          //品尝
                          food.Taste();
                          //点评
                          food.Comment();
                      });
                   }));
                    tasks.Add(taskFactory.StartNew(() =>
                    {

                        //1:随机点3个菜
                        List<AbstractFood> foods = GetFoods();
                        foods.ForEach(u =>
                        {
                            //通过反射来创建菜的对象
                            AbstractFood newFood = u.Clone() as AbstractFood;
                            bCustomer.Foods.Add(newFood);
                        });
                        //2:开始品菜
                        bCustomer.Foods.ForEach(u =>
                        {
                            //做菜
                            u.Cook();
                            //品尝
                            u.Taste();
                            //点评
                            u.Comment();
                        });
                    }));
                    tasks.Add(taskFactory.StartNew(() =>
                    {

                        //1:随机点3个菜
                        List<AbstractFood> foods = GetFoods();
                        foods.ForEach(u =>
                        {
                            //通过反射来创建菜的对象
                            AbstractFood newFood = u.Clone() as AbstractFood;
                            cCustomer.Foods.Add(newFood);
                        });
                        //2:开始品菜
                        cCustomer.Foods.ForEach(u =>
                        {
                            //做菜
                            u.Cook();
                            //品尝
                            u.Taste();
                            //点评
                            u.Comment();
                        });
                    }));
                    //4：三个客人都吃完了后,开始进行点评了
                    taskFactory.ContinueWhenAll(tasks.ToArray(), (ts) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("开始显示评选得分最高的菜");
                        customers.ForEach(u =>
                        {
                            u.Foods.OrderByDescending(iu => iu.Score).ToList().ForEach(h =>
                             {
                                 Console.WriteLine($"{u.Name}：菜名:{h.FoodName},描述:{h.Describe},得分:{h.Score}");
                             });
                        });
                    });
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常信息:{ex.Message}");
            }

            Console.ReadLine();
        }

        /// <summary>
        /// 随机获取三个
        /// </summary>
        /// <returns></returns>
        public static List<AbstractFood> GetFoods()
        {
            List<AbstractFood> foods = OrderMenu.GetInstance().AllFoods;
            List<int> foodIds = new List<int>();
            while (foodIds.Count != 3)
            {
                int id = new Random().Next(1, 4);
                if (!foodIds.Contains(id))
                {
                    foodIds.Add(id);
                }
            }
            return foods.Where(u => foodIds.Contains(u.Id)).ToList();
        }
    }



}
