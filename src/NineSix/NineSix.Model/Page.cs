using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.Model
{
    //分页的类
    public class Page
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页的数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序规则
        /// </summary>
        public string order { get; set; } = "ASC";

        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
    }
}
