using Microsoft.AspNetCore.Mvc;

namespace Vts.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        // GET: api/Live
        [HttpGet]
        public string Get()
        {
            HttpContext.Response.StatusCode = 200;
            return "200 OK";
        }
    }
}
