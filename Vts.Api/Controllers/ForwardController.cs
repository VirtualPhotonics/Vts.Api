using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ForwardController : ControllerBase
    {
        private readonly IForwardSolverService _forwardSolverService;

        public ForwardController(IForwardSolverService forwardSolverService)
        {
            _forwardSolverService = forwardSolverService;
        }

        // GET: api/v1/Forward
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Controller", "Forward" };
        }

        // POST: api/v1/Forward
        [HttpPost]
        [Authorize(Policy = "ApiKeyPolicy")]
        public dynamic Post([FromBody] SolutionDomainPlotParameters plotParameters)
        {
            var result = plotParameters.ValidateData();
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
            return _forwardSolverService.GetPlotData(plotParameters);
        }
    }
}
