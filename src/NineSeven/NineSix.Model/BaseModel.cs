using NineSeven.Common.Attibutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSeven.Model
{
    public class BaseModel 
    {
        [PrimaryKey]
        [DocField]
        public int Id { get; set; }
    }
}
