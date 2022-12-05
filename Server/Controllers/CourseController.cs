using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/lucene")]
    public class CourseController : ControllerBase
    {
        private static readonly ServiceLogic logic = new ServiceLogic();
        private static Indexer indexer = new Indexer();


        [HttpPost]
        [Route("Index")]
        public ActionResult CreateTheIndex() 
        {
            lock (indexer)
            {
                var res = logic.CreateIndexLogic(indexer);
                return Ok(res);
            }
        }

        [HttpGet]
        [Route("Query")]
        public ActionResult<IEnumerable<Course>> QueryingSpecific([FromQuery] int hpp, [FromQuery] string queryForLucene, [FromQuery] string header)
        {
            lock (indexer) {
                var result = logic.QueryProcessSpecific(hpp, queryForLucene, header, indexer);
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("QueryEverything")]
        public ActionResult<IEnumerable<Course>> QueryingGeneral([FromQuery] int hpp, [FromQuery] string queryForLucene)
        {
            lock (indexer) {
                var result = logic.QueryProcessEverything(hpp, indexer, queryForLucene);
                return Ok(result);
            }
        }
    }
}
