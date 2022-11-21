using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
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
            //writer.Dispose();
            writer.Commit();



            /*using DirectoryReader ireader = DirectoryReader.Open(dir);
            IndexSearcher isearcher = new IndexSearcher(ireader);

            QueryParser parser = new QueryParser(version, "everything", Analyzer);
            Query query = parser.Parse("interdisciplinary");
            ScoreDoc[] hits = isearcher.Search(query, null, 10).ScoreDocs;



            foreach (var hit in hits)
            {
                Console.WriteLine("");
                Console.WriteLine(isearcher.Doc(hit.Doc).Fields);
            }*/

        }


    }
}
