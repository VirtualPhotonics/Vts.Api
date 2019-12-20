using System.Collections.Generic;

namespace Vts.Api.Tools
{
    public interface IParameterTools
    {
        object GetOpticalPropertiesObject(OpticalProperties opticalProperties);

        IDictionary<IndependentVariableAxis, object> GetParametersInOrder(
            object opticalProperties, double[] xs, SolutionDomainType solutionDomain, IndependentVariableAxis? independentAxis,
            double? independentValue);

    }
}