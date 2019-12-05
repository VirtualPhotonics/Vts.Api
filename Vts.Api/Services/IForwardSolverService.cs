using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface IForwardSolverService
    {
        string GetPlotData(SolutionDomainPlotParameters plotParameters);
    }
}
