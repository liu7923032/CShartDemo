using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nine.Model
{
    public class Person
    {
        public Person(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }

        public int SleepTime { get; set; }

        public event Action ChangeEvent;

        public void Change()
        {
            ChangeEvent?.Invoke();
        }
    }
}
