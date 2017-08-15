using NineEight.Common.Attibutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.Model
{
    public class GM_Commodity:BaseModel
    {
        [DocField]
        [Analyzed]
        public string Title { get; set; }


        public string SkuId { get; set; }
        public string ProductId { get; set; }

        [DocField]
        public decimal Price { get; set; }

        [DocField]
        public string Url { get; set; }

        public string ImageUrl { get; set; }
    }
}
