using Vts.Api.Data;
using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface IInverseSolverService
    {
        Plots GetPlotData(SolutionDomainPlotParameters plotParameters);
    }
}
