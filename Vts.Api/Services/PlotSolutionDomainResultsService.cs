using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Extensions.Logging;
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

        public Plots Plot(IPlotParameters plotParameters)
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
                Plots plot;
                if (!isComplex)
                {
                    plot = ConstructPlots(numberOfPointsPerPlot, numberOfPlots, independentValues, reflectance,
                        parameters, opticalPropertyList, hasIndependentAxis);
                }
                else
                {
                    plot = ConstructComplexPlots(numberOfPointsPerPlot, numberOfPlots, independentValues, reflectance, parameters, opticalPropertyList, hasIndependentAxis);
                }
                return plot;
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred: {Message}", e.Message);
                throw;
            }
        }

        internal Plots ConstructPlots(int numberOfPointsPerPlot, int numberOfPlots, double[] independentValues, double[] reflectance, SolutionDomainPlotParameters parameters, OpticalProperties[] opticalPropertyList, bool hasIndependentAxis)
        {
            var plot = new Plots {
                Id = hasIndependentAxis
                    ? $"{parameters.SolutionDomain.ToString()}Independent{parameters.IndependentAxis.Axis}"
                    : $"{parameters.SolutionDomain.ToString()}",
                PlotList = new List<PlotDataJson>()
            };
            for (var i = 0; i < numberOfPlots; i++)
            {
                IEnumerable<Point> xyPoints;
                if (opticalPropertyList.Length > 1 && parameters.XAxis.Axis == IndependentVariableAxis.Wavelength)
                {
                    IList<Point> newPoints = new List<Point>();
                    for (var j = 0; j < numberOfPointsPerPlot; j++)
                    {
                        var offset = i + (numberOfPlots * j);
                        newPoints.Add(new Point(independentValues[j], reflectance[offset]));
                    }
                    xyPoints = newPoints.ToArray();
                }
                else
                {
                    var offset = numberOfPointsPerPlot * i;
                    xyPoints = independentValues.Zip(reflectance, (x, y) =>
                        new Point(x, reflectance[Array.IndexOf(reflectance, y) + offset]));
                }
                var plotData = new PlotData
                {
                    Data = xyPoints,
                    Label = parameters.SolutionDomain.ToString()
                };
                plot.PlotList.Add(new PlotDataJson
                {
                    Data = plotData.Data.Select(item => new List<double> { item.X, item.Y }).ToList(),
                    Label = GetPlotLabel(parameters.ForwardSolverType, opticalPropertyList, i, parameters.XAxis,
                        parameters.IndependentAxis, parameters.SecondIndependentAxis, "")
                });
            }
            return plot;
        }

        internal Plots ConstructComplexPlots(int numberOfPointsPerPlot, int numberOfPlots, double[] independentValues, double[] reflectance, SolutionDomainPlotParameters parameters, OpticalProperties[] opticalPropertyList, bool hasIndependentAxis)
        {
            var plot = new Plots {
                Id = hasIndependentAxis
                    ? $"{parameters.SolutionDomain.ToString()}Independent{parameters.IndependentAxis.Axis}"
                    : $"{parameters.SolutionDomain.ToString()}",
                PlotList = new List<PlotDataJson>()
            };
            var complexOffset = reflectance.Length / 2;
            for (var i = 0; i < numberOfPlots; i++)
            {
                IEnumerable<Point> xyPointsReal;
                IEnumerable<Point> xyPointsImaginary;
                if (opticalPropertyList.Length > 1 && parameters.XAxis.Axis == IndependentVariableAxis.Wavelength)
                {
                    IList<Point> newPointsReal = new List<Point>();
                    IList<Point> newPointsImaginary = new List<Point>();
                    for (var j = 0; j < numberOfPointsPerPlot; j++)
                    {
                        var offset = i + (numberOfPlots * j);
                        newPointsReal.Add(new Point(independentValues[j], reflectance[offset]));
                        newPointsImaginary.Add(new Point(independentValues[j], reflectance[offset + complexOffset]));
                    }
                    xyPointsReal = newPointsReal.ToArray();
                    xyPointsImaginary = newPointsImaginary.ToArray();
                }
                else
                {
                    var offset = numberOfPointsPerPlot * i;
                    var xyPointsComplex = independentValues.Zip(reflectance, (x, y) =>
                        new ComplexPoint(x, new Complex(reflectance[Array.IndexOf(reflectance, y) + offset], reflectance[Array.IndexOf(reflectance, y) + offset + complexOffset]))).ToArray();
                    xyPointsReal = xyPointsComplex.Select(item =>
                        new Point(item.X, item.Y.Real));
                    xyPointsImaginary = xyPointsComplex.Select(item =>
                        new Point(item.X, item.Y.Imaginary));
                }
                var plotDataReal = new PlotData {
                    Data = xyPointsReal,
                    Label = parameters.SolutionDomain.ToString()
                };
                var plotDataImaginary = new PlotData {
                    Data = xyPointsImaginary,
                    Label = parameters.SolutionDomain.ToString()
                };
                plot.PlotList.Add(new PlotDataJson {
                    Data = plotDataReal.Data.Select(item => new List<double> { item.X, item.Y }).ToList(),
                    Label = GetPlotLabel(parameters.ForwardSolverType, opticalPropertyList, i, parameters.XAxis,
                        parameters.IndependentAxis, parameters.SecondIndependentAxis, "(real)")
                });
                plot.PlotList.Add(new PlotDataJson {
                    Data = plotDataImaginary.Data.Select(item => new List<double> { item.X, item.Y })
                        .ToList(),
                    Label = GetPlotLabel(parameters.ForwardSolverType, opticalPropertyList, i, parameters.XAxis,
                        parameters.IndependentAxis, parameters.SecondIndependentAxis, "(imag)")
                });
            }
            return plot;
        }

        internal string GetPlotLabel(ForwardSolverType fs, OpticalProperties[] opticalPropertyList, int i, IndependentAxis xAxis, IndependentAxis independentAxis, IndependentAxis secondIndependentAxis, string additionalLabel)
        {
            var independentLabel = "";
            var secondIndependentLabel = "";
            if (independentAxis != null)
            {
                var independentValue = independentAxis.AxisValue;
                if (independentAxis.AxisRange != null)
                {
                    var independentArray = independentAxis.AxisRange.AsEnumerable().ToArray();
                    independentValue = independentArray[i];
                }
                independentLabel = $" {independentAxis.Axis}={independentValue}";
            }

            if (secondIndependentAxis != null)
            {
                var secondIndependentValue = secondIndependentAxis.AxisValue;
                secondIndependentLabel = $" {secondIndependentAxis.Axis}={secondIndependentValue}";
            }

            if (xAxis.Axis == IndependentVariableAxis.Wavelength)
            {
                return $"{fs}{independentLabel}{secondIndependentLabel}{additionalLabel}";
            }

            return opticalPropertyList.Length == 1 ? $"{fs} μa={opticalPropertyList[0].Mua} μs'={opticalPropertyList[0].Musp}{independentLabel}{secondIndependentLabel}{additionalLabel}" : $"{fs} μa={opticalPropertyList[i].Mua} μs'={opticalPropertyList[i].Musp}{independentLabel}{secondIndependentLabel}{additionalLabel}";
        }
    }
}
