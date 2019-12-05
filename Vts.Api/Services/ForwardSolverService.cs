using System;
using Microsoft.Extensions.Logging;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;

namespace Vts.Api.Services
{
    public class ForwardSolverService : IForwardSolverService
    {
        private readonly ILogger<ForwardSolverService> _logger;
        private readonly IPlotFactory _plotFactory;

        public ForwardSolverService(ILogger<ForwardSolverService> logger, IPlotFactory plotFactory)
        {
            _logger = logger;
            _plotFactory = plotFactory;
        }

        public string GetPlotData(SolutionDomainPlotParameters plotParameters)
        {
            try
            {
                var msg = _plotFactory.GetPlot(PlotType.SolutionDomain, plotParameters);
                return msg;
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred: {Message}", e.Message);
                throw;
            }
        }
    }
}
