using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Example.Controllers
{
    public enum Test
    {
        None,
        A,
        B
    }

    [ApiExplorerSettings(GroupName = "Values")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [ProducesResponseType(404, Type = typeof(bool))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public IEnumerable<string> Get([FromQuery]int gg, [FromQuery]decimal cc)
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id">dadasd</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(string id = "gg")
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value, [FromBody]Program v1, [FromBody]List<Program> v2, [FromBody]Dictionary<string,Program> v3, [FromBody] Test v4)
        {
        }

        // PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}