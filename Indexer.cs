using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneProject
{
    public class Indexer
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;
        private static string[] StopWordList = {
            "a", "an", "and", "are", "as", "at",
            "be", "but", "by",
            "for", "from",
            "has", "he",
            "if", "in", "into", "is", "it", "its",
            "no", "not",
            "of", "on", "or",
            "such",
            "that", "the", "their", "then", "there", "these", "they", "this", "to",
            "was", "were", "will", "with"};
        private readonly string indexPath;
        private readonly string datasetFile;
        private readonly StandardAnalyzer _standardAnalyzer;
        private readonly FSDirectory _directory;
        private readonly IndexWriterConfig _indexWriterConfig;
        private readonly IndexWriter _writer;
        private TokenStream tokenStream;
        private IOffsetAttribute offsetAtt;
        private ICharTermAttribute termAtt;

        public Indexer(string datasetfile, string indexPath)
        {
            this.indexPath = indexPath;
            this.datasetFile = datasetfile;
            this._standardAnalyzer = new StandardAnalyzer(version);
            this._directory = FSDirectory.Open(indexPath);
        }

        public void myTokenizer() {
            tokenStream = _standardAnalyzer.GetTokenStream("alltokens", File.ReadAllText(datasetFile));
            offsetAtt = tokenStream.AddAttribute<IOffsetAttribute>();
            termAtt = tokenStream.AddAttribute<ICharTermAttribute>();
        }

        public void CreateIndex()
        {
            using var dir = _directory;
            //Adds a field to index;
            //document.Add(new StringField("Year", "2022", Field.Store.YES));
           /* using var dir = FSDirectory.Open(indexPath);

            var indexConfig = new IndexWriterConfig(version, _standardAnalyzer);

            using var writer = new IndexWriter(dir, indexConfig);


            var doc = new Document();
            doc.Add(new TextField("testfield", "stringvalue", Field.Store.YES));
            writer.AddDocument(doc);
            writer.Flush(triggerMerge: true, applyAllDeletes: true);*/

        }
        public void testing() {
            try
            {
                tokenStream.Reset();
                while (tokenStream.IncrementToken())
                {
                    Console.WriteLine("token: " + tokenStream.ReflectAsString(true));
                    Console.WriteLine("Term: " +termAtt.ToString());
                    Console.WriteLine("token start offset: " + offsetAtt.StartOffset);
                    Console.WriteLine("  token end offset: " + offsetAtt.EndOffset);
                }
                //Console.WriteLine($"Standard analyzer stop word set: {StopWordSet}");
                tokenStream.End();
            }
            finally
            {
                tokenStream.Dispose();
            }
        }

        public void addToIndex()
        {
            try
            {
                tokenStream.Reset();
                var document = new Document();
                while (tokenStream.IncrementToken()) {
                    document.Add(new TextField("testing_term", termAtt.ToString(),Field.Store.NO));
                }
                
            }
            finally
            {
                _writer.Commit();
                _writer.Flush(triggerMerge: true, applyAllDeletes: false);
            }
        }
    }
}
