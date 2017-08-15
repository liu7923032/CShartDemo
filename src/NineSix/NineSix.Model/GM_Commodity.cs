﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.Model
{
    public class GM_Commodity:BaseModel
    {
        public string Title { get; set; }
        public string SkuId { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}
