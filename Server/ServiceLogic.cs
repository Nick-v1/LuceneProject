using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class ServiceLogic
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public string CreateIndexLogic(Indexer indexer) {
            return indexer.CreateIndex();
        }

        public IEnumerable<Course> QueryProcessSpecific(int hpp, string queryToSearch, string header, Indexer indexer) {
            var courses = indexer.SearchIndexSpecific(hpp, queryToSearch, header);
            log.Info("Processing...");
            
            
            return courses;
        }
    }
}
