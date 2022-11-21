using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;


namespace LuceneProject
{
    public class Indexer
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;

        
        private readonly string IndexPath;
        private readonly string DatasetFile;
        

        public Indexer(string datasetfile, string indexPath)
        {
            this.IndexPath = indexPath;
            this.DatasetFile = datasetfile;
        }

        public void CreateIndex() {
            var AllTextList = File.ReadAllLines(DatasetFile);
            using var dir = FSDirectory.Open(IndexPath);

            var Analyzer = new StandardAnalyzer(version);

            var IndexConfig = new IndexWriterConfig(version, Analyzer);
            using var writer = new IndexWriter(dir, IndexConfig);

            foreach (var item in AllTextList)
            {
                var doc = new Document();

                doc.Add(new TextField("Everything", item, Field.Store.YES));
                writer.AddDocument(doc);
            }
            writer.Commit();
            //writer.Dispose();

        }

        public void SearchIndex(int hpp, string QueryToSearch) {
            int hitsPerPage = hpp;

            var Analyzer = new StandardAnalyzer(version);

            using var dir = FSDirectory.Open(IndexPath);
            using DirectoryReader ireader = DirectoryReader.Open(dir);

            IndexSearcher isearcher = new IndexSearcher(ireader);
            //query to parse
            Query query = new QueryParser(version, "Everything", Analyzer).Parse(QueryToSearch);
            
            var collector = TopScoreDocCollector.Create(hitsPerPage, true);
            isearcher.Search(query, collector);
            ScoreDoc[] hits = collector.GetTopDocs().ScoreDocs;

            float meanScoreSum = 0.0f;
            Console.WriteLine("Top Results\n-----------------------------------------------");
            foreach (var hit in hits)
            {
                int docId = hit.Doc;
                Document d = isearcher.Doc(docId);
                Console.WriteLine("Doc Id: " + hit.Doc + ". Score: " + hit.Score);
                meanScoreSum += hit.Score;
            }
            Console.WriteLine($"\nTotal hits: {hits.Length}, Max Score: {isearcher.Search(query, hitsPerPage).MaxScore}");
            Console.WriteLine("Average relevance: " + meanScoreSum/hits.Length);
        }
        
    }
}
