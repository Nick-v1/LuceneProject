using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Index;
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
        private readonly StandardAnalyzer _standardAnalyzer;
        private readonly RAMDirectory _directory;
        private readonly IndexWriterConfig _indexWriterConfig;
        private readonly IndexWriter _writer;
        private readonly CharArraySet StopWordSet;
        private readonly TokenStream tokenStream;
        private readonly IOffsetAttribute offsetAtt;

        public Indexer(string datasetfile)
        {
            _standardAnalyzer = new StandardAnalyzer(version);
            _directory = new RAMDirectory();
            _indexWriterConfig = new IndexWriterConfig(version, _standardAnalyzer);
            _writer = new IndexWriter(_directory, _indexWriterConfig);
            StopWordSet = _standardAnalyzer.StopwordSet;
            tokenStream = _standardAnalyzer.GetTokenStream("alltokens", new StringReader(File.ReadAllText(datasetfile)));
            offsetAtt = tokenStream.AddAttribute<IOffsetAttribute>();
        }

        public void testing() {
            tokenStream.Reset();
            while (tokenStream.IncrementToken()) {
                Console.WriteLine("token: " + tokenStream.ReflectAsString(true));
                Console.WriteLine("token start offset: " + offsetAtt.StartOffset);
                Console.WriteLine("  token end offset: " + offsetAtt.EndOffset);
            }
            //Console.WriteLine($"Standard analyzer stop word set: {StopWordSet}");
            tokenStream.End();
            tokenStream.Dispose();
        }
    }
}
