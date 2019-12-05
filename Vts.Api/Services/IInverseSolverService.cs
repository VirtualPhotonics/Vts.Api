using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface IInverseSolverService
    {
        string GetPlotData(SolutionDomainPlotParameters plotParameters);
    }
}
