using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vts.Api.Data;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Api.Tools;
using Vts.Factories;

namespace Vts.Api.Services
{
    public class InverseSolverService : IInverseSolverService
    {
        private readonly ILogger<InverseSolverService> _logger;
        private readonly IPlotFactory _plotFactory;
        private readonly IParameterTools _parameterTools;

        public InverseSolverService(ILogger<InverseSolverService> logger, IPlotFactory plotFactory, IParameterTools parameterTools)
        {
            _logger = logger;
            _plotFactory = plotFactory;
            _parameterTools = parameterTools;
        }
        public Plots GetPlotData(SolutionDomainPlotParameters plotParameters)
        {
            try
            {
                var inverseSolver = plotParameters.InverseSolverType;
                var initialGuessParams = _parameterTools.GetParametersInOrder(
                    _parameterTools.GetOpticalPropertiesObject(plotParameters.OpticalProperties, plotParameters.WavelengthOpticalPropertyList),
                    plotParameters.SolutionDomain,
                    plotParameters.XAxis,
                    plotParameters.IndependentAxis,
                    plotParameters.SecondIndependentAxis);
                var initialGuessParamsConvert = initialGuessParams.Values.ToArray();
                // get measured data from inverse solver analysis component
                var measuredPoints = plotParameters.MeasuredData;
                var dependentValues = measuredPoints.Select(p => p.Last()).ToArray(); // get y value
                var lowerBounds = new double[] {0, 0, 0, 0};
                var upperBounds = new []
                {
                    double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity
                };
                var fit = ComputationFactory.SolveInverse(
                    plotParameters.InverseSolverType,
                    plotParameters.OptimizerType,
                    plotParameters.SolutionDomain,
                    dependentValues,
                    dependentValues, // set standard deviation to measured to match WPF
                    plotParameters.OptimizationParameters,
                    initialGuessParamsConvert,
                    lowerBounds,
                    upperBounds);
                var fitops = ComputationFactory.UnFlattenOpticalProperties(fit);
                //var fitparms =
                //    GetParametersInOrder(fitops, independentValues, sd, independentAxis, independentAxisValue);
                plotParameters.ForwardSolverType = inverseSolver;
                plotParameters.OpticalProperties = fitops[0]; // not sure [0] is always going to work here
                plotParameters.NoiseValue = 0;
                return _plotFactory.GetPlot(PlotType.SolutionDomain, plotParameters);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred: {Message}", e.Message);
                throw;
            }
        }

    }
}
