using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Api.Tools;
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
            var hasIndependentAxis = parameters.SolutionDomain != SolutionDomainType.ROfFx && parameters.SolutionDomain != SolutionDomainType.ROfRho;
            var independentValues = parameters.XAxis.AxisRange.AsEnumerable().ToArray();
            try
            {
                var opticalPropertyList = _parameterTools.GetOpticalPropertiesObject(parameters.OpticalProperties,
                    parameters.WavelengthOpticalPropertyList).ToArray();
                var parametersInOrder = _parameterTools.GetParametersInOrder(
                    opticalPropertyList,
                    parameters.SolutionDomain,
                    parameters.XAxis,
                    parameters.IndependentAxis,
                    parameters.SecondIndependentAxis);
                var parametersInOrderObject = parametersInOrder.Values.ToArray();
                var reflectance = parameters.NoiseValue > 0 ? ComputationFactory.ComputeReflectance(fs, parameters.SolutionDomain, parameters.ModelAnalysis, parametersInOrderObject).AddNoise(parameters.NoiseValue) : ComputationFactory.ComputeReflectance(fs, parameters.SolutionDomain, parameters.ModelAnalysis, parametersInOrderObject);
                var isComplex = ComputationFactory.IsComplexSolver(parameters.SolutionDomain);
                // Get the number of points per plot
                var numberOfPointsPerPlot = independentValues.Length;
                var totalNumberOfValues = isComplex ? reflectance.Length / 2 : reflectance.Length;
                // Get the number of plots
                var numberOfPlots = totalNumberOfValues / numberOfPointsPerPlot;
                var plot = new Plots {
                    Id = hasIndependentAxis 
                        ? $"{parameters.SolutionDomain.ToString()}Independent{parameters.IndependentAxis.Axis}" 
                        : $"{parameters.SolutionDomain.ToString()}",
                    PlotList = new List<PlotDataJson>()
                };
                for (var i = 0; i < numberOfPlots; i++)
                {
                    if (!isComplex)
                    {
                        var offset = numberOfPointsPerPlot * i;
                        var xyPoints = independentValues.Zip(reflectance, (x, y) => 
                            new Point(x, reflectance[Array.IndexOf(reflectance, y) + offset]));
                        var plotData = new PlotData
                        {
                            Data = xyPoints,
                            Label = parameters.SolutionDomain.ToString()
                        };
                        plot.PlotList.Add(new PlotDataJson
                        {
                            Data = plotData.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                            Label = GetPlotLabel(fs, opticalPropertyList, i, parameters.XAxis,
                                parameters.IndependentAxis, parameters.SecondIndependentAxis, "")
                        });
                    }
                    else
                    {
                        var complexOffset = reflectance.Length / 2;
                        var offset = (complexOffset * i) + complexOffset;
                        var xyPointsComplex = independentValues.Zip(reflectance, (x, y) => 
                            new ComplexPoint(x, new Complex(y, reflectance[Array.IndexOf(reflectance, y) + offset]))).ToArray();
                        var xyPointsReal = xyPointsComplex.Select(item => 
                            new Point(item.X, item.Y.Real));
                        var xyPointsImaginary = xyPointsComplex.Select(item => 
                            new Point(item.X, item.Y.Imaginary));
                        var plotDataReal = new PlotData
                        {
                            Data = xyPointsReal,
                            Label = parameters.SolutionDomain.ToString()
                        };
                        var plotDataImaginary = new PlotData
                        {
                            Data = xyPointsImaginary,
                            Label = parameters.SolutionDomain.ToString()
                        };
                        plot.PlotList.Add(new PlotDataJson
                        {
                            Data = plotDataReal.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                            Label = GetPlotLabel(fs, opticalPropertyList, i, parameters.XAxis,
                                parameters.IndependentAxis, parameters.SecondIndependentAxis, "(real)")
                        });
                        plot.PlotList.Add(new PlotDataJson
                        {
                            Data = plotDataImaginary.Data.Select(item => new List<double> {item.X, item.Y})
                                .ToList(),
                            Label = GetPlotLabel(fs, opticalPropertyList, i, parameters.XAxis,
                                parameters.IndependentAxis, parameters.SecondIndependentAxis, "(imag)")
                        });
                    }
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

        internal string GetPlotLabel(ForwardSolverType fs, OpticalProperties[] opticalPropertyList, int i, IndependentAxis xAxis, IndependentAxis independentAxis, IndependentAxis secondIndependentAxis, string additionalLabel)
        {
            if (xAxis.Axis == IndependentVariableAxis.Wavelength)
            {
                return independentAxis == null
                    ? $"{fs}{additionalLabel}"
                    : $"{fs} {independentAxis.Axis}={independentAxis.AxisValue}{additionalLabel}";

            }
            if (opticalPropertyList.Length == 1)
            {
                return independentAxis == null
                    ? $"{fs} μa={opticalPropertyList[0].Mua} μs'={opticalPropertyList[0].Musp}{additionalLabel}"
                    : $"{fs} μa={opticalPropertyList[0].Mua} μs'={opticalPropertyList[0].Musp} {independentAxis.Axis}={independentAxis.AxisValue}{additionalLabel}";
            }
            return independentAxis == null
                ? $"{fs} μa={opticalPropertyList[i].Mua} μs'={opticalPropertyList[i].Musp}{additionalLabel}"
                : $"{fs} μa={opticalPropertyList[i].Mua} μs'={opticalPropertyList[i].Musp} {independentAxis.Axis}={independentAxis.AxisValue}{additionalLabel}";
        }
    }
}
