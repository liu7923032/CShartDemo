﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NineEight.LuceneNet.Interface
{
    public interface ILuceneAnalyze
    {
        /// <summary>
        /// 根据查询的field将keyword分词
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        string[] AnalyzerKey(string field,string keyword);
    }
}
