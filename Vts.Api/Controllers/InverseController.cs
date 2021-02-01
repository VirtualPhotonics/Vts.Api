using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InverseController : ControllerBase
    {
        private readonly IInverseSolverService _inverseSolverService;

        public InverseController(IInverseSolverService inverseSolverService)
        {
            _inverseSolverService = inverseSolverService;
        }

        // GET: api/v1/Inverse
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Controller", "Inverse" };
        }

        // POST: api/v1/Inverse
        [HttpPost]
        [Authorize(Policy = "ApiKeyPolicy")]
        public dynamic Post([FromBody] SolutionDomainPlotParameters plotParameters)
        {
            return _inverseSolverService.GetPlotData(plotParameters);
        }
    }
}
