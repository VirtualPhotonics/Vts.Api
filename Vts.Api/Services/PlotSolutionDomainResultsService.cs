using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Api.Tools;
using Vts.Common;
using Vts.Extensions;
using Vts.Factories;

namespace Vts.Api.Services
{
    public class PlotSolutionDomainResultsService : IPlotResultsService
    {
        private readonly ILogger<PlotSolutionDomainResultsService> _logger;
        private readonly IParameterTools _parameterTools;

        public PlotSolutionDomainResultsService(ILogger<PlotSolutionDomainResultsService> logger, IParameterTools parameterTools)
        {
            _logger = logger;
            _parameterTools = parameterTools;
        }

        public string Plot(IPlotParameters plotParameters)
        {
            var parameters = (SolutionDomainPlotParameters) plotParameters;
            var fs = parameters.ForwardSolverType;
            var op = parameters.OpticalProperties;
            var independentValue = parameters.IndependentAxes.Value;
            var independentValues = parameters.XAxis.AsEnumerable().ToArray();
            try
            {
                Plots plot;
                var parametersInOrder = _parameterTools.GetParametersInOrder(
                    _parameterTools.GetOpticalPropertiesObject(parameters.OpticalProperties),
                    plotParameters.XAxis.AsEnumerable().ToArray(),
                    parameters.SolutionDomain,
                    parameters.IndependentAxes.Label,
                    parameters.IndependentAxes.Value);
                var parametersInOrderObject = parametersInOrder.Values.ToArray();
                var reflectance = parameters.NoiseValue > 0 ? ComputationFactory.ComputeReflectance(fs, parameters.SolutionDomain, parameters.ModelAnalysis, parametersInOrderObject).AddNoise(parameters.NoiseValue) : ComputationFactory.ComputeReflectance(fs, parameters.SolutionDomain, parameters.ModelAnalysis, parametersInOrderObject);
                var isComplex = ComputationFactory.IsComplexSolver(parameters.SolutionDomain);
                var hasIndependentAxis = parameters.SolutionDomain != SolutionDomainType.ROfFx && parameters.SolutionDomain != SolutionDomainType.ROfRho;
                if (!isComplex)
                {
                    var xyPoints = independentValues.Zip(reflectance, (x, y) => new Point(x, y));
                    var plotData = new PlotData { Data = xyPoints, Label = parameters.SolutionDomain.ToString() };
                    plot = new Plots {
                        Id = hasIndependentAxis ? $"{parameters.SolutionDomain.ToString()}Fixed{parameters.IndependentAxes.Label}" : $"{parameters.SolutionDomain.ToString()}",
                        Detector = hasIndependentAxis ? $"R({parameters.IndependentAxes.First},{parameters.IndependentAxes.Second})" : $"R({parameters.IndependentAxes.First})",
                        Legend = hasIndependentAxis ? $"R({parameters.IndependentAxes.First},{parameters.IndependentAxes.Second})" : $"R({parameters.IndependentAxes.First})",
                        XAxis = hasIndependentAxis && parameters.IndependentAxes.Label == parameters.IndependentAxes.First ? parameters.IndependentAxes.Second : parameters.IndependentAxes.First,
                        YAxis = "Reflectance",
                        PlotList = new List<PlotDataJson>()
                    };
                    plot.PlotList.Add(new PlotDataJson {
                        Data = plotData.Data.Select(item => new List<double> { item.X, item.Y }).ToList(),
                        Label = hasIndependentAxis ? $"{fs} μa={op.Mua} μs'={op.Musp} {parameters.IndependentAxes.Label}={parameters.IndependentAxes.Value}" : $"{fs} μa={op.Mua} μs'={op.Musp}"
                    });
                }
                else
                {
                    var offset = reflectance.Length/2;
                    IEnumerable<ComplexPoint> xyPointsComplex = independentValues.Zip(reflectance, (x, y) => new ComplexPoint(x, new Complex(y, reflectance[Array.IndexOf(reflectance, y)+offset]))).ToArray();

                    var xyPointsReal = xyPointsComplex.Select(item => new Point(item.X, item.Y.Real));
                    var xyPointsImaginary = xyPointsComplex.Select(item => new Point(item.X, item.Y.Imaginary));
                    var plotDataReal = new PlotData { Data = xyPointsReal, Label = parameters.SolutionDomain.ToString() };
                    var plotDataImaginary = new PlotData { Data = xyPointsImaginary, Label = parameters.SolutionDomain.ToString() };
                    plot = new Plots {
                        Id = $"{parameters.SolutionDomain.ToString()}Fixed{parameters.IndependentAxes.Label}",
                        Detector = $"R({parameters.IndependentAxes.First},{parameters.IndependentAxes.Second})",
                        Legend = $"R({parameters.IndependentAxes.First},{parameters.IndependentAxes.Second})",
                        XAxis = parameters.IndependentAxes.Label == parameters.IndependentAxes.First ? $"{parameters.IndependentAxes.Second}" : parameters.IndependentAxes.First,
                        YAxis = "Reflectance",
                        PlotList = new List<PlotDataJson>()
                    };
                    plot.PlotList.Add(new PlotDataJson {
                        Data = plotDataReal.Data.Select(item => new List<double> { item.X, item.Y }).ToList(),
                        Label = $"{fs} μa={op.Mua} μs'={op.Musp} {parameters.IndependentAxes.Label}={independentValue}(real)"
                    });
                    plot.PlotList.Add(new PlotDataJson {
                        Data = plotDataImaginary.Data.Select(item => new List<double> { item.X, item.Y }).ToList(),
                        Label = $"{fs} μa={op.Mua} μs'={op.Musp} {parameters.IndependentAxes.Label}={independentValue}(imag)"
                    });
                }
                var msg = JsonConvert.SerializeObject(plot);
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
