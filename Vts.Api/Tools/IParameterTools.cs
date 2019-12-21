using System.Collections.Generic;
using Vts.Api.Models;

namespace Vts.Api.Tools
{
    public interface IParameterTools
    {
        IEnumerable<OpticalProperties> GetOpticalPropertiesObject(OpticalProperties opticalProperties, IEnumerable<OpticalProperties> wavelengthOpticalProperties);

        IDictionary<IndependentVariableAxis, object> GetParametersInOrder(
            object opticalProperties, SolutionDomainType solutionDomain, IndependentAxis xAxis, IndependentAxis independentAxis, IndependentAxis secondIndependentAxis);

    }
}