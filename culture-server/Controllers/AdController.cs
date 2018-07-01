using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace culture_server.Controllers
{
    public class AdController : ApiController
    {
        // GET api/ad
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/ad/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/ad
        public void Post([FromBody]string value)
        {
        }

        // PUT api/ad/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ad/5
        public void Delete(int id)
        {
        }
    }
}
