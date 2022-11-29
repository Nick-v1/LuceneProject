using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost]
        [Route("Query")]
        public ActionResult<IEnumerable<Course>> QueryingSpecific([FromQuery] int hpp, [FromQuery] string queryForLucene, [FromQuery] string header)
        {
            lock (indexer) {
                var result = logic.QueryProcessSpecific(hpp, queryForLucene, header, indexer);
                return Ok(result);
            }
        }


    }
}
