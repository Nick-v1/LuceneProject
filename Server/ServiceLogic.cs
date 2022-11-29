using NLog;
using System.Collections.Generic;

namespace Server
{
    public class ServiceLogic
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public string CreateIndexLogic(Indexer indexer) {
            return indexer.CreateIndex();
        }

        public IEnumerable<Course> QueryProcessSpecific(int hpp, string queryToSearch, string header, Indexer indexer) {
            log.Info("Processing Specific...");
            var courses = indexer.SearchIndexSpecific(hpp, queryToSearch, header);
            
            
            return courses;
        }

        public IEnumerable<Course> QueryProcessEverything(int hpp, Indexer indexer, string queryToSearch) {
            log.Info("Processing Everything...");
            var courses = indexer.SearchIndexMultiQuery(hpp, queryToSearch);



            return courses;
        }
    }
}
