using NineFour.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineFour.Model
{
    public class Person: BaseModel
    {
        [Required]
        [Length(3, 10)]
        public string Name { get; set; }

        [Number]
        [Required]
        public int Age { get; set; }

        [Email]
        public string Email { get; set; }
      
        [Length(10,20)]
        public string Address { get; set; }

        [Required]
        [Mobile]
        public string Phone { get; set; }
    }
}
