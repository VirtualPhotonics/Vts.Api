using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vts.Extensions;

namespace Vts.Api.Tools
{
    public class ParameterTools : IParameterTools
    {
        public ParameterTools()
        {

        }

        // this needs further development when add in wavelength refer to WPF code
        public object GetOpticalPropertiesObject(OpticalProperties opticalProperties)
        {
            return new[] { opticalProperties };
        }

        // the following needs to change when Wavelength is added into independent variable list
        public IDictionary<IndependentVariableAxis, object> GetParametersInOrder(
            object opticalProperties, double[] xs, SolutionDomainType solutionDomain, IndependentVariableAxis? independentAxis, double? independentValue)
        {
            // make list of independent vars with independent first then constant
            var listIndependentVariableAxes = new List<IndependentVariableAxis>();
            var isConstant = independentAxis;
            switch (solutionDomain)
            {
                case SolutionDomainType.ROfRho:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Rho);
                break;
                case SolutionDomainType.ROfRhoAndTime:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Rho);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Time);
                break;
                case SolutionDomainType.ROfRhoAndFt:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Ft);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Rho);
                break;
                case SolutionDomainType.ROfFx:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Fx);
                break;
                case SolutionDomainType.ROfFxAndTime:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Time);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Fx);
                break;
                case SolutionDomainType.ROfFxAndFt:
                listIndependentVariableAxes.Add(IndependentVariableAxis.Ft);
                listIndependentVariableAxes.Add(IndependentVariableAxis.Fx);
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

        internal int GetParameterOrder(IndependentVariableAxis axis)
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
        internal double[] GetParameterValues(IndependentVariableAxis axis, IndependentVariableAxis? isConstant, double? independentValue,
            double[] xs)
        {
            if (((axis == IndependentVariableAxis.Rho) && (isConstant == IndependentVariableAxis.Rho)) ||
                ((axis == IndependentVariableAxis.Time) && (isConstant == IndependentVariableAxis.Time)) ||
                ((axis == IndependentVariableAxis.Fx) && (isConstant == IndependentVariableAxis.Fx)) ||
                ((axis == IndependentVariableAxis.Ft) && (isConstant == IndependentVariableAxis.Ft)))
            {
                if (independentValue == null)
                    return new double[0];
                return new[] { (double)independentValue };
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
