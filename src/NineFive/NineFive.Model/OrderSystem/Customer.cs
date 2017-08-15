using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFive.Model.OrderSystem
{
    public class Customer
    {
        public Customer(string name)
        {
            this.Name = name;
            this.Foods = new List<AbstractFood>();
        }

        public string Name { get; set; }

        public List<AbstractFood> Foods { get; set; }
    }
}
