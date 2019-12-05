using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadyController : ControllerBase
    {
        // GET: api/ready
        [HttpGet]
        public string Get()
        {
            HttpContext.Response.StatusCode = 200;
            return "200 OK";
        }

        // POST api/ready
        [HttpPost]
        [Authorize(Policy = "ApiKeyPolicy")]
        public dynamic Post([FromBody] dynamic value)
        {
            HttpContext.Response.StatusCode = 200;
            return "200 OK";
        }
    }
}
