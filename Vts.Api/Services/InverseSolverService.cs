using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Extensions;
using Vts.Factories;

namespace Vts.Api.Services
{
    public class InverseSolverService : IInverseSolverService
    {
        private readonly ILogger<InverseSolverService> _logger;
        private readonly IPlotFactory _plotFactory;

        public InverseSolverService(ILogger<InverseSolverService> logger, IPlotFactory plotFactory)
        {
            _logger = logger;
            _plotFactory = plotFactory;
        }
        public string GetPlotData(SolutionDomainPlotParameters plotParameters)
        {
            try
            {
                var inverseSolver = plotParameters.InverseSolverType;
                var initialGuessParams = GetParametersInOrder(
                    GetInitialGuessOpticalProperties(plotParameters.OpticalProperties),
                    plotParameters.XAxis.AsEnumerable().ToArray(), 
                    plotParameters.SolutionDomain, 
                    plotParameters.IndependentAxes.Label,
                    plotParameters.IndependentAxes.Value);
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
                var msg = _plotFactory.GetPlot(PlotType.SolutionDomain, plotParameters);
                return msg;
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred: {Message}", e.Message);
                throw;
            }
        }

        // this needs further development when add in wavelength refer to WPF code
        public static object GetInitialGuessOpticalProperties(OpticalProperties initialGuessOpticalProperties)
        {
            return new[] { initialGuessOpticalProperties };
        }

        // the following needs to change when Wavelength is added into independent variable list
        public static IDictionary<IndependentVariableAxis, object> GetParametersInOrder(
            object opticalProperties, double[] xs, SolutionDomainType solutionDomain, string independentAxis, double independentValue)
        {
            // make list of independent vars with independent first then constant
            var listIndependentVariableAxes = new List<IndependentVariableAxis>();
            var isConstant = "";
            switch (solutionDomain)
            {
                case SolutionDomainType.ROfRho:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Rho);
                break;
                case SolutionDomainType.ROfRhoAndTime:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Rho);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Time);
                isConstant = independentAxis == "t" ? "t" : "rho";
                break;
                case SolutionDomainType.ROfRhoAndFt:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Ft);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Rho);
                isConstant = independentAxis == "ft" ? "ft" : "rho";
                break;
                case SolutionDomainType.ROfFx:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Fx);
                break;
                case SolutionDomainType.ROfFxAndTime:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Time);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Fx);
                isConstant = independentAxis == "t" ? "t" : "fx";
                break;
                case SolutionDomainType.ROfFxAndFt:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Ft);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Fx);
                isConstant = independentAxis == "ft" ? "ft" : "fx";
                break;
                default:
                throw new InvalidEnumArgumentException("There is no Enum of this type");
            }

            // get all parameters in order
            var allParameters =
                from iva in listIndependentVariableAxes
                where iva != IndependentVariableAxis.Wavelength
                orderby GetParameterOrder(iva)
                select new KeyValuePair<IndependentVariableAxis, object>(iva,
                    GetParameterValues(iva, isConstant, independentValue, xs));
            // OPs are always first in the list
            return
                new KeyValuePair<IndependentVariableAxis, object>(IndependentVariableAxis.Wavelength,
                        opticalProperties)
                    .AsEnumerable()
                    .Concat(allParameters).ToDictionary();
        }

        public static int GetParameterOrder(IndependentVariableAxis axis)
        {
            switch (axis)
            {
                case IndependentVariableAxis.Wavelength:
                return 0;
                case IndependentVariableAxis.Rho:
                return 1;
                case IndependentVariableAxis.Fx:
                return 1;
                case IndependentVariableAxis.Time:
                return 2;
                case IndependentVariableAxis.Ft:
                return 2;
                case IndependentVariableAxis.Z:
                return 3;
                default:
                throw new InvalidEnumArgumentException("There is no Enum of this type");
            }
        }

        // this has commented out code that might come into play when we add wavelength as axis
        public static double[] GetParameterValues(IndependentVariableAxis axis, string isConstant, double independentValue,
            double[] xs)
        {
            if (((axis == IndependentVariableAxis.Rho) && (isConstant == "rho")) ||
                ((axis == IndependentVariableAxis.Time) && (isConstant == "t")) ||
                ((axis == IndependentVariableAxis.Fx) && (isConstant == "fx")) ||
                ((axis == IndependentVariableAxis.Ft) && (isConstant == "ft")))
            {
                return new[] { independentValue };
            }
            return xs.ToArray();
            //else
            //{
            //    if (axis != IndependentVariableAxis.Time)
            //    {
            //        return xs.ToArray();
            //    }
            //    else
            //    {
            //        return xs.ToArray();
            //    }
            //}
            //{
            //    var positionIndex = 0; //hard-coded for now
            //    switch (positionIndex)
            //    {
            //        case 0:
            //        default:
            //            return new[] {independentValue};
            //case 1:
            //return new[] { SolutionDomainTypeOptionVM.ConstantAxesVMs[1].AxisValue };
            //case 2:
            //    return new[] { SolutionDomainTypeOptionVM.ConstantAxisThreeValue };
            //}
            //}
            //else
            //{
            //    //var numAxes = axis.Count();
            //    var numAxes = 1;
            //    var positionIndex = 0; //hard-coded for now
            //    //var positionIndex = SolutionDomainTypeOptionVM.IndependentVariableAxisOptionVM.SelectedValues.IndexOf(axis);
            //    switch (numAxes)
            //    {
            //        case 1:
            //        default:
            //            //return AllRangeVMs[0].Values.ToArray();
            //            return xs.ToArray();
            //case 2:
            //    switch (positionIndex)
            //    {
            //        case 0:
            //        default:
            //            return AllRangeVMs[1].Values.ToArray();
            //        case 1:
            //            return AllRangeVMs[0].Values.ToArray();
            //    }
            //case 3:
            //    switch (positionIndex)
            //    {
            //        case 0:
            //        default:
            //            return AllRangeVMs[2].Values.ToArray();
            //        case 1:
            //            return AllRangeVMs[1].Values.ToArray();
            //        case 2:
            //            return AllRangeVMs[0].Values.ToArray();
            //    }
            //}
            //}
        }
    }
}
