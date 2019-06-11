using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Vts.Api.Services;

namespace Vts.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/v1/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/v1/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/v1/values
        [HttpPost]
        public string Post([FromBody] dynamic value)
        {
            var forwardSolverService = new ForwardSolverService();
            return forwardSolverService.GetPlotData(value);
        }

        // PUT api/v1/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/v1/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
