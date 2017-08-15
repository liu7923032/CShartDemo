using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSeven.Model
{
    //分页的类
    public class Page
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页的数量
        /// </summary>
        public int PageSize { get; set; } = 15;

        /// <summary>
        /// 排序规则
        /// </summary>
        public string Order { get; set; } = "ASC";

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Total { get; set; }
    }
}
