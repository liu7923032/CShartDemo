using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.OrderSystem
{
    /// <summary>
    /// 用于品尝食物
    /// </summary>
    public abstract class AbstractFood : ICloneable
    {
        /// <summary>
        /// 品尝食物
        /// </summary>
        public void Taste()
        {
            //Console.WriteLine($"这道菜不错");
        }

        /// <summary>
        /// 点评食物
        /// </summary>
        public virtual void Comment()
        {
            //Console.WriteLine($"{this.FoodName}:菜不错");
            this.Score = new Random().Next(6, 9);
        }

        /// <summary>
        /// 做菜
        /// </summary>
        public abstract void Cook();

        public object Clone()
        {
            return this.MemberwiseClone();
        }

      
        /// <summary>
        /// 标识符
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 菜名
        /// </summary>
        public string FoodName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }
    }
}
