using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SpectralController : ControllerBase
    {
        private readonly ISpectralService _spectralService;

        public SpectralController(ISpectralService spectralService)
        {
            _spectralService = spectralService;
        }

        // GET: api/v1/Spectral
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Controller", "Spectral" };
        }

        // POST: api/v1/Spectral
        [HttpPost]
        [Authorize(Policy = "ApiKeyPolicy")]
        public string Post([FromBody] SpectralPlotParameters plotParameters)
        {
            return _spectralService.GetPlotData(plotParameters);
        }
    }
}
