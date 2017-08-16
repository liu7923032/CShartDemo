using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using LuceneIO = Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using NineEight.LuceneNet.Interface;
using NineEight.Common;
using NineEight.Model;
using NineEight.Common.Extend;
using NineEight.Common.Attibutes;
using System.Reflection;
using System.Text;
using NineEight.LuceneNet.Utils;
using System.Text.RegularExpressions;

namespace NineEight.LuceneNet.Service
{
    /// <summary>
    /// 多线程的问题 ：多文件写，然后合并
    /// 延时：异步队列
    /// 
    /// </summary>
    public class LuceneBulid<T> : ILuceneBulid<T> where T : BaseModel
    {
        #region 1.0 设置索引目录
        public LuceneBulid(string indexPath)
        {
            this.IndexPath = indexPath;

            //初始化该类的属性
            Type type = typeof(T);
            //找到该类的属性集合
            this.Properties = type.GetProperties();
        }
        #endregion

        #region 2.0 公共属性

        private Logger logger = new Logger(typeof(LuceneBulid<T>));
        /// <summary>
        /// 索引目录
        /// </summary>
        private string IndexPath { get; set; }

        private PropertyInfo[] Properties { get; set; }

        #endregion Identity

        #region 3.0 新增,批量新增Index 索引合并
        /// <summary>
        /// 新增一条数据的索引
        /// </summary>
        /// <param name="ci"></param>
        public void Insert(T entity)
        {
            IndexWriter writer = null;
            try
            {
                if (entity == null) return;
                //1：找到目录
                DirectoryInfo dirInfo = CreateDirPath();
                //2：检查目录需要新建索引
                bool isCreate = dirInfo.GetFiles().Count() == 0;//下面没有文件则为新建索引 
                //3：创建对象
                writer = CreateWriter(this.IndexPath, isCreate);
                //4：添加索引
                writer.AddDocument(ParseEntityToDoc(entity));
            }
            catch (Exception ex)
            {
                logger.Error("InsertIndex异常", ex);
                throw ex;
            }
            finally
            {
                if (writer != null)
                {
                    //if (fileNum > 50)
                    //    writer.Optimize();
                    writer.Dispose();
                }
            }
        }


        /// <summary>
        /// 批量创建索引(要求是统一的sourceflag，即目录是一致的)
        /// </summary>
        /// <param name="ciList">sourceflag统一的</param>
        /// <param name="pathSuffix">索引目录后缀，加在电商的路径后面，为空则为根目录.如sa\1</param>
        /// <param name="isCreate">默认为false 增量索引  true的时候删除原有索引</param>
        public void InsertList(List<T> ciList, string pathSuffix = "", bool isCreate = false)
        {
            IndexWriter writer = null;
            try
            {
                //1.检查list集合是否为空
                if (ciList == null || ciList.Count == 0)
                {
                    return;
                }
                //2.获取索引目录
                string indexPath = string.IsNullOrWhiteSpace(pathSuffix) ? IndexPath : string.Format("{0}\\{1}", IndexPath, pathSuffix);
                writer = CreateWriter(indexPath, isCreate);
                ciList.ForEach(c => writer.AddDocument(ParseEntityToDoc(c)));
            }
            finally
            {
                if (writer != null)
                {
                    //writer.Optimize(); 创建索引的时候不做合并  merge的时候处理
                    writer?.Dispose();
                }
            }
        }

        /// <summary>
        /// 将索引合并到上级目录
        /// </summary>
        /// <param name="sourceDir">子文件夹名</param>
        public void Merge(string[] childDirs)
        {
            Console.WriteLine("MergeIndex Start");
            IndexWriter writer = null;
            try
            {
                if (childDirs == null || childDirs.Length == 0) return;
                Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                DirectoryInfo dirInfo = CreateDirPath();
                LuceneIO.Directory directory = LuceneIO.FSDirectory.Open(dirInfo);
                writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);//删除原有的
                LuceneIO.Directory[] dirNo = childDirs.Select(dir => LuceneIO.FSDirectory.Open(Directory.CreateDirectory(string.Format("{0}\\{1}", this.IndexPath, dir)))).ToArray();
                writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
                writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量
                writer.AddIndexesNoOptimize(dirNo);
            }
            finally
            {
                writer?.Optimize();
                writer?.Dispose();
                Console.WriteLine("MergeIndex End");
            }
        }

        //Field.Store.YES:存储字段值（未分词前的字段值）        
        //Field.Store.NO:不存储,存储与索引没有关系         
        //Field.Store.COMPRESS:压缩存储,用于长文本或二进制，但性能受损         
        //Field.Index.ANALYZED:分词建索引         
        //Field.Index.ANALYZED_NO_NORMS:分词建索引，但是Field的值不像通常那样被保存，而是只取一个byte，这样节约存储空间         
        //Field.Index.NOT_ANALYZED:不分词且索引         
        //Field.Index.NOT_ANALYZED_NO_NORMS:不分词建索引，Field的值去一个byte保存         
        //TermVector表示文档的条目（由一个Document和Field定位）和它们在当前文档中所出现的次数         
        //Field.TermVector.YES:为每个文档（Document）存储该字段的TermVector         
        //Field.TermVector.NO:不存储TermVector         
        // Field.TermVector.WITH_POSITIONS:存储位置        
        //Field.TermVector.WITH_OFFSETS:存储偏移量         
        //Field.TermVector.WITH_POSITIONS_OFFSETS:存储位置和偏移量
        #endregion 批量BuildIndex 索引合并

        #region 4.0 删除索引
        /// <summary>
        /// 删除多条数据的索引
        /// </summary>
        /// <param name="ci"></param>
        public void Delete(T entity)
        {
            IndexReader reader = null;
            try
            {
                if (entity == null) return;

                reader = CreateReader();

                reader.DeleteDocuments(new Term("Id", entity.Id.ToString()));
            }
            catch (Exception ex)
            {

                logger.Error("DeleteIndex异常", ex);
                throw ex;
            }
            finally
            {
                reader?.Dispose();
            }
        }

        /// <summary>
        /// 批量删除数据的索引
        /// </summary>
        /// <param name="ciList"></param>
        public void DeleteList(List<T> ciList)
        {
            IndexReader reader = null;
            try
            {
                //1：检查数据是否存在
                if (ciList == null || ciList.Count == 0) return;
                //2：创建IndexReader对象
                reader = CreateReader();
                //3：删除索引
                ciList.ForEach(u => reader.DeleteDocuments(new Term("Id", u.Id.ToString())));
            }
            catch (Exception ex)
            {
                logger.Error("DeleteIndex异常", ex);
                throw ex;
            }
            finally
            {
                reader?.Dispose();
            }
        }


        #endregion

        #region 5.0 更新索引
        /// <summary>
        /// 更新一条数据的索引
        /// </summary>
        /// <param name="ci"></param>
        public void Update(T enity)
        {
            IndexWriter writer = null;
            try
            {
                if (enity == null) return;
                writer = CreateWriter();
                writer.UpdateDocument(new Term("Id", enity.Id.ToString()), ParseEntityToDoc(enity));
            }
            catch (Exception ex)
            {
                logger.Error("InsertIndex异常", ex);
                throw ex;
            }
            finally
            {
                writer?.Dispose();
            }
        }

        /// <summary>
        /// 批量更新数据的索引
        /// </summary>
        /// <param name="ciList">sourceflag统一的</param>
        public void UpdateList(List<T> ciList)
        {
            IndexWriter writer = null;
            try
            {
                if (ciList == null || ciList.Count == 0) return;
                writer = CreateWriter();
                ciList.ForEach(u => writer.UpdateDocument(new Term("Id", u.Id.ToString()), ParseEntityToDoc(u)));

            }
            catch (Exception ex)
            {
                logger.Error("InsertIndex异常", ex);
                throw ex;
            }
            finally
            {
                writer?.Dispose();
            }
        }
        #endregion

        #region 6.0 查询
        /// <summary>
        /// 查询单个数据 
        /// </summary>
        /// <param name="strQueryCon">field1:'xxx' field2:'dddd'</param>
        /// <returns></returns>
        public List<T> Query(string strCondition)
        {
            IndexSearcher searcher = null;
            try
            {
                List<T> ciList = new List<T>();

                Analyzer analyzer = new PanGuAnalyzer();
                searcher = CreateSearcher();

                //--------------------这里配置搜索条件----------
                StringBuilder sb = new StringBuilder();
                //找到需要进行分词的属性字段
                PropertyInfo property = Properties.Where(u => u.GetCustomAttribute<AnalyzedAttribute>() != null).FirstOrDefault();
                //构建查询器 ,设定第一个属性字段为默认搜索域
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, property.Name, analyzer);
                //给关键字进行分词
                //var splitWords = new LuceneAnalyze().AnalyzerKey(property.Name, strCondition).ToList<string>();

                //splitWords.ForEach(u =>
                //{
                //    sb.AppendFormat("{0}:{1} ", property.Name, u);
                //});

                Query query = parser.Parse(strCondition);
                TopDocs docs = searcher.Search(query, (Filter)null, 10000);
                foreach (ScoreDoc sd in docs.ScoreDocs)
                {
                    Document doc = searcher.Doc(sd.Doc);
                    ciList.Add(ParseDocToEntity(doc));
                }
                return ciList;
            }
            finally
            {
                searcher?.Dispose();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="strCondition">查询条件 lucene的查询语法</param>
        /// <param name="page">分页相关</param>
        /// <param name="strFilter">[100,3000]</param>
        /// <returns></returns>
        public List<T> QueryByPage(string strCondition, Page page, string strFilter)
        {
            IndexSearcher searcher = null;
            try
            {
                List<T> ciList = new List<T>();
                searcher = CreateSearcher();
                Analyzer analyzer = new PanGuAnalyzer();
                //--------------------------------------这里配置搜索条件
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", analyzer);
                Query query = parser.Parse(strCondition);

                int startIndex = (page.PageIndex - 1) * page.PageSize;
                int endIndex = page.PageIndex * page.PageSize;

                var property = Properties.Where(u => u.Name.Equals(page.Sort)).FirstOrDefault();

                Sort sort = null;
                if (!string.IsNullOrEmpty(page.Sort))
                {
                    //排序
                    sort = new Sort();
                    //找到该属性
                    SortField sortField = new SortField(page.Sort, SortField.DOUBLE, page.Order.Equals("asc", StringComparison.CurrentCultureIgnoreCase));
                    sort.SetSort(sortField);
                }
                Filter filter = null;
                TopDocs docs = null;
                if (sort != null && !string.IsNullOrEmpty(strFilter))
                {
                    Tuple<double, double> tuple = QueryTools.GetMinAndMax(strFilter);
                    if (tuple != null)
                    {
                        filter = QueryTools.GetNumberFilter(property, tuple.Item1, tuple.Item2);
                        docs = searcher.Search(query, filter, 10000, sort);
                    }
                }
                else
                {
                    //filter = NumericRangeFilter.NewDoubleRange("Price", 100, 400, true, true);
                    docs = searcher.Search(query, filter, 1000);
                }
             
                //获取中标的数据
                page.Total = docs.TotalHits;

                //PrintScores(docs, startIndex, endIndex, searcher);
                for (int i = startIndex; i < endIndex && i < page.Total; i++)
                {
                    Document doc = searcher.Doc(docs.ScoreDocs[i].Doc);
                    ciList.Add(ParseDocToEntity(doc));
                }
                return ciList;
            }
            finally
            {
                searcher?.Dispose();
            }
        }
        #endregion

        #region 7.0 私有的公共方法

        /// <summary>
        /// 获取索引目录
        /// </summary>
        /// <returns></returns>
        private DirectoryInfo CreateDirPath(string indexPath = "")
        {
            if (string.IsNullOrEmpty(indexPath))
            {
                indexPath = this.IndexPath;
            }
            DirectoryInfo dirInfo = null;
            if (!Directory.Exists(indexPath))
            {
                dirInfo = Directory.CreateDirectory(indexPath);
            }
            else
            {
                dirInfo = new DirectoryInfo(indexPath);
            }
            return dirInfo;
        }

        /// <summary>
        /// 创建IndexWriter 对象
        /// </summary>
        private IndexWriter CreateWriter(string indexPath = "", bool isCreate = false)
        {
            //1:声明writer对象
            IndexWriter writer = null;
            //2:创建索引目录
            DirectoryInfo dirInfo = CreateDirPath(indexPath);
            //3.得到lucene 的索引目录对象
            LuceneIO.Directory directory = LuceneIO.FSDirectory.Open(dirInfo);
            //4.创建lucene 的索引写入对象
            //writer = new IndexWriter(directory, new PanGuAnalyzer(), isCreate, IndexWriter.MaxFieldLength.LIMITED);
            writer = new IndexWriter(directory, CreateAnalyzerWrapper(), isCreate, IndexWriter.MaxFieldLength.LIMITED);
            writer.SetMaxBufferedDocs(100);
            //控制写入一个新的segent前内存中保存的doc的数量 默认10             
            writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
            writer.UseCompoundFile = true;//创建复合文件 减少索引文件数量
            return writer;
        }

        /// <summary>
        /// 创建IndexReader对象
        /// </summary>
        /// <returns></returns>
        private IndexReader CreateReader()
        {

            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            DirectoryInfo dirInfo = CreateDirPath();

            LuceneIO.Directory directory = LuceneIO.FSDirectory.Open(dirInfo);

            return IndexReader.Open(directory, false);
        }

        /// <summary>
        /// 创建 IndexSearcher
        /// </summary>
        /// <returns></returns>
        private IndexSearcher CreateSearcher()
        {
            LuceneIO.Directory dir = LuceneIO.FSDirectory.Open(this.IndexPath);
            return new IndexSearcher(dir);
        }

        /// <summary>
        /// 创建对象字段的分析器
        /// </summary>
        /// <returns></returns>
        private PerFieldAnalyzerWrapper CreateAnalyzerWrapper()
        {
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            PerFieldAnalyzerWrapper analyzerWrapper = new PerFieldAnalyzerWrapper(analyzer);
            foreach (var item in Properties)
            {
                AnalyzedAttribute attr = item.GetCustomAttribute<AnalyzedAttribute>();
                if (attr != null && attr.IsAnalyzed)
                {
                    analyzerWrapper.AddAnalyzer(item.Name, new PanGuAnalyzer());
                }
                else
                {
                    analyzerWrapper.AddAnalyzer(item.Name, analyzer);
                }
            }
            return analyzerWrapper;
        }


        /// <summary>
        /// 7.2 将对象转换Document 对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Document ParseEntityToDoc(T entity)
        {
            Document doc = new Document();
            //通过特性来定义那些字段需要进行分析
            foreach (var item in Properties)
            {
                DocFieldAttribute fieldAttr = item.GetCustomAttribute<DocFieldAttribute>();
                //如果没有说明是Document的字段那么就忽略该字段
                if (fieldAttr == null || !fieldAttr.IsDocField)
                {
                    continue;
                }
                //检查该字段是否需要进行分词
                AnalyzedAttribute analyzedAttr = item.GetCustomAttribute<AnalyzedAttribute>();
                //如果不需要进行分词的情况
                //检查该字段的类型,如果是字符串的类型的情况
                if (analyzedAttr != null && analyzedAttr.IsAnalyzed)
                {
                    doc.Add(new Field(item.Name, item.GetValue(entity).ToString(), Field.Store.YES, Field.Index.ANALYZED));
                }
                else
                {
                    //检查该字段的类型,如果是字符串的类型的情况
                    if (item.PropertyType == typeof(string) && item.GetValue(entity) != null)
                    {
                        doc.Add(new Field(item.Name, item.GetValue(entity).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }
                    //如果是数字的情况
                    else if (item.PropertyType == typeof(decimal) 
                        || item.PropertyType == typeof(double)
                        || item.PropertyType == typeof(float)
                        || item.PropertyType == typeof(int)
                        || item.PropertyType == typeof(long))
                    {

                        bool isDouble = false;
                        double doubleValue = 0;
                        isDouble = Double.TryParse(item.GetValue(entity).ToString(), out doubleValue);
                        if (isDouble)
                        {
                            doc.Add(new NumericField(item.Name, Field.Store.YES, true).SetDoubleValue(doubleValue));
                        }
                    }
                }

            }
            return doc;
        }

        /// <summary>
        /// 将Document 对象转换为 T
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public T ParseDocToEntity(Document doc)
        {
            Type type = typeof(T);
            T obj = Activator.CreateInstance(type) as T;
            foreach (var propertyItem in Properties)
            {
                var tempValue = doc.Get(propertyItem.Name);
                if (tempValue != null)
                {

                    propertyItem.SetValue(obj, Convert.ChangeType(tempValue, propertyItem.PropertyType));
                }
            }
            return obj;
        }

        #endregion PrivateMethod
    }
}
